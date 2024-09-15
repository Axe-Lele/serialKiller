using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using System;

public class DialogManager : Singleton<DialogManager>
{
    public TextAsset DialogTextAsset;

    public GameObject m_DialogPopup;

    public UILabel m_TextLabel;

    public GameObject LeftCharacter;
    public GameObject RightCharacter;

    private string m_Talker;
    private string m_Emotion;

    public UILabel m_LeftNameLabel;
    public UILabel m_RightNameLabel;

    public FaceControl m_LeftFaceControl;
    public FaceControl m_RightFaceControl;

    public GameObject m_CloseBtn;
    public UISprite m_BG;
    protected DialogType m_Type; // 대화 타입
    protected List<GameEventType> m_DialogAfterEventType; // 대화 종료 후 이벤트 처리
    protected List<string> m_EventList;
    protected string m_Target; // 대화 대상

    protected int m_BlockIndex; // 블록 인덱스

    // 대확가 완전히 종료된 후 일어나는 이벤트
    protected string m_DialogAfterEvent;

    protected string m_CompleteText; // 완성된 텍스트
    protected string m_TypnigText; // 타이핑 중 보여질 텍스트
    private string m_TempText; // 임시적으로 텍스트를 담는 string 값. 

    // 후에 옵션에 따라서 속도가 변하도록 변경
    protected float m_TypingSpeed = 0.010f;//0.025f
    protected string m_DialogIndex;

    protected bool m_IsTypingFlag; // true면 타자치는 중, false면 타자 종료 


    //private string initName; // 최초에 말을 시작하는 인물의 이름을 저장함. 이후부터는 이 인물이 left 다음 인물이 right가 됨

    //protected string tempName; // 임시적으로 사용할 이름을 담는 그릇
    //protected string showName; // 이름표에서 보여줄 실제 이름
    //
    protected string BG_Name;


    protected string[] NameTemp;

    protected int TalkerCount;

    protected int length; // tempString의 총 길이
    protected int count; // 타이핑 중 몇 번째 글자를 보여주고 있는지
    protected int m_TotalBlockCount; // 현 세션의 총 대화가 몇개인지  



    protected int SelectionCount;
    protected string SelectionKeyword;

    protected int SelectionParameterCount;
    protected string SelectionKeywordParameter;
    //  private int Criminal;
    private string key;

    private List<string> m_DialogKey;
     



    private void Awake()
    {
        m_DialogAfterEventType = new List<GameEventType>();
        m_EventList = new List<string>();

        m_DialogKey = new List<string>();
         

       /* if (GameManager.instance != null)
        {
            Localization.LoadCSV(GameManager.instance.LocalizationText, false);
        }
        else
        {
            Localization.LoadCSV(GlobalValue.instance.CSV, false);
        }
        */
        BG_Name = "0";
    }


    protected virtual void StartDialog(DialogType type, string target, string index, string place)
    {
        BG_Name = place;
        StartDialog(type, target, index);
    }

    protected virtual void StartDialog(DialogType type, string target, string index)//(DialogTargetType dialogTargetType, string dialogIndex, string targetNum)
    {
        print("type : " + type + " / target : " + target + " / index : " + index);

        m_LeftFaceControl.gameObject.SetActive(false);
        m_RightFaceControl.gameObject.SetActive(false);

        SelectionKeyword = null;

        StopAllCoroutines();
        m_CloseBtn.gameObject.SetActive(false);

        // 파라미터로 넘어온 값들 적용
        m_DialogIndex = index;
        //GlobalValue.instance.DialogTargetType = DialogTargetType = dialogTargetType;
        m_Type = type;
        GlobalValue.instance.m_DialogTarget = m_Target = target;

        m_BlockIndex = 0;

        m_DialogPopup.SetActive(true);

        if (BG_Name == "0")
        {
            m_BG.spriteName = "BG_0";
        }
        else
        {
            m_BG.spriteName = PlaceDataManager.instance.GetBackgroundSpriteName(BG_Name);
        }

    //    m_TotalBlockCount = DialogNode[m_Target][m_DialogIndex].Count;

        switch (m_Type)
        {
            case DialogType.Start:
                if (EventDataManager.instance.ReturnStartDialogEventData(m_Target) == -1)
                {
                    EventDataManager.instance.InputStartDialogEventData(m_Target, "Start_1");
                    m_DialogIndex = "Start_0";
                }
                else
                {
                    m_DialogIndex = "Start_1";
                }

                //m_TotalBlockCount = DialogNode[m_Target][m_DialogIndex].Count;
                break;

            case DialogType.EventDialog:
                break;

            case DialogType.End:
                m_DialogIndex = "End_" + index;
              //  m_TotalBlockCount = DialogNode[m_Target][m_DialogIndex].Count;
                break;

            default:
            //    m_TotalBlockCount = DialogNode[m_Target][m_DialogIndex].Count;
                break;
        }

        key = "Dialog_" + PlayDataManager.instance.m_StageName + "_" + m_Target + "_" + m_DialogIndex + "_";
        m_TotalBlockCount = Localization.GetKeyCount(key);

        print("Localization Key : " + key + " / Total Block Count : " + m_TotalBlockCount);

        Set();
      
    }


    // Dialog Json에서 현재 블록의 데이터들을 가져와 저장을 한다.
    public void Set()
    {
        m_CompleteText = m_TypnigText = "";
        SetText();
        ShowCharacter();
        StartCoroutine("Typing");
    }

    IEnumerator Typing()
    {
        m_IsTypingFlag = true;
        count = 0;
        length = m_CompleteText.Length;

        m_TempText = m_CompleteText + "[-]";

        while (m_IsTypingFlag)
        {
            if (count <= length)
            {
                m_TypnigText = m_TempText.Insert(count, "[55]");
                m_TextLabel.text = m_TypnigText;
                count++;
                yield return new WaitForSeconds(m_TypingSpeed);
            }
            else
            {
                m_IsTypingFlag = false;
            }
        }
    }

    // 자식 클래스에서 정의
    protected virtual void TypingEnd()
    {
    }

    public void SetSection(string i)
    {
        m_CloseBtn.gameObject.SetActive(false);
        m_DialogIndex = i;
        m_BlockIndex = 0;
        Set();
    }

    // 자식 클래스에서 정의
    protected virtual void Close()
    {
    }

    // 자식 클래스에서 정의
    protected virtual void SetText()
    {
        string s = "";

        //        m_Talker = DialogNode[m_Target][m_DialogIndex][m_BlockIndex]["Talker"];
        //      m_Emotion = DialogNode[m_Target][m_DialogIndex][m_BlockIndex]["Emotion"];
        //s =  "Dialog_" + PlayDataManager.instance.m_StageName + "_" + m_Target + "_" + m_DialogIndex + "_" + m_BlockIndex;
        print("m_BlockIndex : " + m_BlockIndex);
        s = Localization.m_Key[m_BlockIndex];
        m_CompleteText = Localization.Get(s);
        print("s : " + s + " / temp :" + m_CompleteText + " / m_Talker : " + m_Talker);
    }

    protected void SetEvent()
    {
        EventManager.instance.SetEvent("Dialog", m_Target,  m_DialogIndex);
    }

    protected void ShowCharacter()
    {
    //    print("m_Eotion : " + m_Emotion + " / m_Talker : " + m_Talker);

        // ※가 포함되어 있다는 것은 split을 해야한다는 것.
        if (Localization.m_Key[m_BlockIndex].Contains("※"))
        {
            string[] temp = Localization.m_Key[m_BlockIndex].Split('※');
            int n;
            // temp[1]이 숫자로 변환이 가능하다는 말은 플레이어블 캐릭터이며 감정을 표현함
            if (int.TryParse(temp[1], out n))
            {
                if (m_LeftFaceControl.gameObject.activeSelf == false)
                    m_LeftFaceControl.gameObject.SetActive(true);

                m_LeftFaceControl.FaceSetting("Player", n.ToString());

                m_LeftFaceControl.TalkerActive(true);
                m_RightFaceControl.TalkerActive(false);
            }
            else
            {
                // temp[1]이 숫자로 변환되지 않으므로 플레이어블 캐릭터가 아니란 말.
                if (m_RightFaceControl.gameObject.activeSelf == false)
                    m_RightFaceControl.gameObject.SetActive(true);
                // _이 포함되어 있다는 것은 감정이 있다는 말.

                if (temp[1].Contains("_"))
                {
                    string[] t = temp[1].Split('_');
                    m_RightFaceControl.FaceSetting(t[0], t[1]);
                }
                else
                {
                    m_RightFaceControl.FaceSetting(temp[1]);
                }
             
                m_LeftFaceControl.TalkerActive(false);
                m_RightFaceControl.TalkerActive(true);
            }
        }
        else
        {
            // ※가 포함되어 있지 않다는 것은 지금 대화하는 화자가 플레이어블 캐릭터라는 것.
            if (m_LeftFaceControl.gameObject.activeSelf == false)
                m_LeftFaceControl.gameObject.SetActive(true);

            m_LeftFaceControl.FaceSetting("Player");

            m_LeftFaceControl.TalkerActive(true);
            m_RightFaceControl.TalkerActive(false);
        }


        /*
        if (m_Talker == "Player")
        {
            if (m_LeftFaceControl.gameObject.activeSelf == false)
                m_LeftFaceControl.gameObject.SetActive(true);

            if (m_Emotion == null)
            {
                m_LeftFaceControl.FaceSetting("Player");
            }
            else
            {
                m_LeftFaceControl.FaceSetting("Player", m_Emotion);
            }

            m_LeftFaceControl.TalkerActive(true);
            m_RightFaceControl.TalkerActive(false);
        }
        else if (m_Talker == "Target")
        {
            if (m_RightFaceControl.gameObject.activeSelf == false)
                m_RightFaceControl.gameObject.SetActive(true);

            if (m_Emotion == null)
            {
                m_RightFaceControl.FaceSetting(m_Target);
            }
            else
            {
                m_RightFaceControl.FaceSetting(m_Target, m_Emotion);
            }
            m_LeftFaceControl.TalkerActive(false);
            m_RightFaceControl.TalkerActive(true);
        }
        else
        {
            if (m_RightFaceControl.gameObject.activeSelf == false)
                m_RightFaceControl.gameObject.SetActive(true);

            if (m_Emotion == null)
            {
                m_RightFaceControl.FaceSetting(m_Talker);
            }
            else
            {
                m_RightFaceControl.FaceSetting(m_Talker, m_Emotion);
            }
            m_LeftFaceControl.TalkerActive(false);
            m_RightFaceControl.TalkerActive(true);
        }
        /*if (NameTemp[1] == "Player" || NameTemp[1] == "Unknown")
        {
          //  m_LeftNameLabel.text = showName;

            m_LeftFaceControl.TalkerActive(false);
            m_RightFaceControl.TalkerActive(true);
        }
        else
        {
         //   m_RightNameLabel.text = showName;

            m_LeftFaceControl.TalkerActive(true);
            m_RightFaceControl.TalkerActive(false);

        }*/
    }

    public DialogType ReturnDialogType()
    {
        return m_Type;
    }

    IEnumerator DelayClose()
    {
        yield return new WaitForSeconds(.51f);
        Close();
    }

    public bool ReturnTypingFlag()
    {
        return m_IsTypingFlag;
    }
}
