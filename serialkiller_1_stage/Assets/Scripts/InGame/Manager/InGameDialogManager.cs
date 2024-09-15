using UnityEngine;
using System.Collections;

public class InGameDialogManager : DialogManager
{

    public void StartDialogInGame(DialogType type, string target, string index, string place)
    {
        base.StartDialog(type, target, index, place);
    }

    public void StartDialogInGame(DialogType type, string target, string index)
    {
        base.StartDialog(type, target, index);
    }

    protected override void Close()
    {
        if (ReturnDialogType() == DialogType.Selection || ReturnDialogType() == DialogType.Start)
        {
            SelectionManager.instance.CloseSelectionPopup();
            StartDialogInGame(DialogType.End, m_Target, "0");
        }
        else if (ReturnDialogType() == DialogType.Dialog || ReturnDialogType() == DialogType.EventDialog)
        {
            m_DialogPopup.SetActive(false);
            BG_Name = "0";
            EventManager.instance.StartDelayEvent();
        }
        else if (ReturnDialogType() == DialogType.InNote)
        {
            m_DialogPopup.SetActive(false);
        }
        else
        {
            print("현재 대화 타입은 " + GameManager.instance.m_DialogManager.ReturnDialogType().ToString());
        }
    }

    public void CheckDialog(string str)
    {
        //print("b : " + Localization.LoadDictionary(str));
    }

    /// <summary>
    /// 타이핑 중이라면 타이핑을 즉시 완료.
    /// 
    /// </summary>
    protected override void TypingEnd()
    {
        if (m_IsTypingFlag)
        {
            StopAllCoroutines();

            m_TextLabel.text = m_CompleteText;

            m_IsTypingFlag = false;
        }
        else
        {
            if (m_TotalBlockCount - 1 > m_BlockIndex)
            {
                m_BlockIndex++;
                Set();
            }
            else
            {
                switch (m_Type)
                {
                    case DialogType.Start:
                        m_CloseBtn.gameObject.SetActive(true);
                        SelectionManager.instance.Show(m_Target);
                        print("Get Selection");
                        break;

                    case DialogType.Selection:
                        // 여기서 NPC의 스케쥴에 따라서 시간이 지나면 대화를 못 하게 한다.
                        //m_CloseBtn.gameObject.SetActive(true);
                        EventManager.instance.SetEvent("Dialog", m_Target, StageDataManager.instance.m_CriminalCode.ToString(), m_DialogIndex);
                        GameManager.instance.UserAction(UserActionType.Dialog);
                        DialogDataManager.instance.InputDialog(m_Target, m_DialogIndex, BG_Name);

                        if (GlobalValue.instance.m_IsNpcForcedMoveFlag == true)
                        {
                            StartDialog(DialogType.ForcedEnd, m_Target, "ForcedEnd");
                            GlobalValue.instance.m_IsNpcForcedMoveFlag = false;
                            //print("종료 대화");
                        }
                        else
                        {
                            SelectionManager.instance.Show(m_Target);
                            //print("GET SELECTIOn");
                        }
                        break;

                    case DialogType.End:
                        //print("Dialog End");
                        EventManager.instance.DialogEndEvent(m_Target);

                        m_DialogPopup.SetActive(false);
                        // ("EndDialog", m_Target, StageDataManager.instance.m_CriminalCode.ToString(), m_DialogIndex);
                        break;

                    case DialogType.ForcedEnd:
                        EventManager.instance.DialogEndEvent(m_Target);
                        m_DialogPopup.SetActive(false);
                        break;

                    case DialogType.Suggest:
                        GameManager.instance.UserAction(UserActionType.Suggest);
                        EventManager.instance.SetEvent("Dialog", m_Target, StageDataManager.instance.m_CriminalCode.ToString(), m_DialogIndex);
                        m_DialogPopup.SetActive(false);
                        break;

                    case DialogType.Dialog:
                        Close();

                        EventManager.instance.SetEvent("Dialog", m_Target, StageDataManager.instance.m_CriminalCode.ToString(), m_DialogIndex);
                        break;

                    case DialogType.EventDialog:
                        Close();

                        EventManager.instance.SetEvent("Dialog", m_Target, StageDataManager.instance.m_CriminalCode.ToString(), m_DialogIndex);
                        break;

                    case DialogType.CompulsionSelection:
                        CompulsionSelectionManager.instance.Show(m_Target, m_DialogIndex);
                        break;

                    case DialogType.InNote:
                        //EventManager.instance.SetEvent("Dialog", m_Target, StageDataManager.instance.m_CriminalCode.ToString(), m_DialogIndex);
                        Close();
                        break;
                }
            }
        }
        /*if (SuggestManager.instance.m_IsCheckSuggest)
        {
            if (m_IsTypingFlag)
            {
                StopAllCoroutines();

                m_TextLabel.text = m_CompleteText;

                m_IsTypingFlag = false;
            }
            else
            {
                if (m_TotalBlockCount - 1 > m_BlockIndex)
                {
                    m_BlockIndex++;
                    Set();
                    //StartCoroutine(SetDelay());
                }
                else
                {
                    GameManager.instance.UserAction(UserActionType.Suggest);
                    if (SuggestManager.instance.m_IsCheckSuggest == true)
                        SuggestManager.instance.m_IsCheckSuggest = false;

                    m_DialogPopup.SetActive(false);
                    print("대화 종료");
                    //SetSection(m_DialogIndex);
                     //StartCoroutine(DelayClose());
                }
            }
        }
        else
        {
            if (m_IsTypingFlag)
            {
                StopAllCoroutines();

                m_TextLabel.text = m_CompleteText;

                m_IsTypingFlag = false;
            }
            else
            {
                if (m_TotalBlockCount - 1 > m_BlockIndex)
                {
                    m_BlockIndex++;
                    Set();
                }
                else
                {
                    switch (m_Type)
                    {
                        case DialogType.Start:
                            m_CloseBtn.gameObject.SetActive(true);
                            SelectionManager.instance.Show(m_Target);
                            print("Get Selection");
                            break;
                        case DialogType.Selection:
                            // 여기서 NPC의 스케쥴에 따라서 시간이 지나면 대화를 못 하게 한다.
                            //m_CloseBtn.gameObject.SetActive(true);
                            GameManager.instance.UserAction(UserActionType.Dialog);
                            SelectionManager.instance.Show(m_Target);
                            print("GET SELECTIOn");
                            break;
                        case DialogType.End:
                            print("Dialog End");
                            m_DialogPopup.SetActive(false);
                            break;
                        case DialogType.Suggest:
                            GameManager.instance.UserAction(UserActionType.Suggest);
                            if (SuggestManager.instance.m_IsCheckSuggest == true)
                                SuggestManager.instance.m_IsCheckSuggest = false;

                            m_DialogPopup.SetActive(false);
                            break;
                    }
                }
            }
        }*/
    }
}
