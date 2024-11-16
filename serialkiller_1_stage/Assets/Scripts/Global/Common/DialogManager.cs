using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using System;

public class DialogManager : Singleton<DialogManager>
{
	public TextAsset DialogTextAsset;

	public GameObject m_DialogPopup;

	protected UILabel m_NowTextLabel;
	public UILabel[] m_TextLabel;

	public GameObject LeftCharacter;
	public GameObject RightCharacter;

	private string m_Talker;
	private string m_Emotion;

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
	protected string m_TypingText; // 타이핑 중 보여질 텍스트
	private string m_TempText; // 임시적으로 텍스트를 담는 string 값. 

	// 후에 옵션에 따라서 속도가 변하도록 변경
	protected float m_TypingSpeed = 0.010f;//0.025f
	protected string m_DialogIndex;

	protected bool m_IsTypingFlag; // true면 타자치는 중, false면 타자 종료 


	//private string initName; // 최초에 말을 시작하는 인물의 이름을 저장함. 이후부터는 이 인물이 left 다음 인물이 right가 됨

	//protected string tempName; // 임시적으로 사용할 이름을 담는 그릇
	//protected string showName; // 이름표에서 보여줄 실제 이름
	[SerializeField]
	protected string m_PlaceBackground;


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
	}

	protected virtual void StartDialog(DialogType type, string target, string index
		, int atlasIndex, string spriteIndex)
	{
		print("type : " + type + " / target : " + target + " / index : " + index + " / place : " + atlasIndex);

		if (spriteIndex == "Prev")
		{
			// 이전 배경화면 유지
		}
		else if (spriteIndex.Length == 0 || atlasIndex == -1)
		{
			m_BG.spriteName = string.Empty;
		}
		else
		{
			UIAtlas atlas = BackgroundManager.instance.GetBackgroundAtlas(atlasIndex);
			m_BG.atlas = atlas;
			m_BG.spriteName = string.Format("BG_{0}", spriteIndex);
		}

		m_Talker = target;
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

		switch (m_Type)
		{
			case DialogType.Start:
				if (EventDataManager.instance.ReturnStartDialogEventData(m_Target) == -1)
				{
					print("Dialog_Index_Start_0");

					NpcDataManager.instance.SetMeet(m_Target, true);
					EventDataManager.instance.InputStartDialogEventData(m_Target, "Start_1");
					m_DialogIndex = "Start_0";
				}
				else
				{
					print("Dialog_Index_Start_1");
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

		print("[" + m_Type + "] Localization Key : " + key + " / Total Block Count : " + m_TotalBlockCount);

		Set();

	}

	protected virtual void StartDialog(DialogType type, string target, string index)//(DialogTargetType dialogTargetType, string dialogIndex, string targetNum)
	{
		StartDialog(type, target, index, -1, string.Empty);
	}


	// Dialog Json에서 현재 블록의 데이터들을 가져와 저장을 한다.
	public void Set()
	{
		m_CompleteText = m_TypingText = "";
		SetText();
		if (m_NowTextLabel != null)
			m_NowTextLabel.text = string.Empty;

		ShowBackground();
		m_NowTextLabel = m_TextLabel[ShowCharacter()];

		SoundManager.instance.changeSFXVolume(1.0f);
		SoundManager.instance.PlaySFX("dialog_ing", true);
		StartCoroutine("Typing");
	}

	IEnumerator Typing()
	{
		m_IsTypingFlag = true;
		count = 0;
		length = m_CompleteText.Length;

		m_TempText = m_CompleteText + "[-]";
		Debug.Log(m_TempText + " / " + length);

		bool isColor = false;
		while (m_IsTypingFlag)
		{
			if (count < length)
			{
				if (m_TempText[count].Equals('['))
				{
					if(isColor == true)
					{
						for (; m_TempText[count].Equals(']') == false;)
						{
							Debug.Log(m_TempText[count]);
							count++;
						}
						count++;
						isColor = false;
					}
					else if (m_TempText[count + 1].Equals('-') == false)
					{
						isColor = true;
						for (; m_TempText[count].Equals(']') == false;)
						{
							Debug.Log(m_TempText[count]);
							count++;
						}
						count++;
					}
				}
				m_TypingText = m_TempText.Insert(count, "[55]");
				m_NowTextLabel.text = m_TypingText;
				count++;
				yield return new WaitForSeconds(m_TypingSpeed);
			}
			else
			{
				m_TypingText = m_TempText.Insert(count, "[55]");
				m_NowTextLabel.text = m_TypingText;
				m_IsTypingFlag = false;
			}
		}

		SoundManager.instance.StopSFXByName("dialog_ing");
	}

	// 자식 클래스에서 정의
	protected virtual void TypingEnd()
	{
		SoundManager.instance.StopSFXByName("dialog_ing");
	}

	public void NextTyping()
	{
		m_BlockIndex++;
		Set();
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
		SoundManager.instance.changeSFXVolume(1.0f);
		SoundManager.instance.PlaySFX("footstep");
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
		EventManager.instance.SetEvent("Dialog", m_Target, m_DialogIndex);
	}

	protected int ShowBackground()
	{
		string key = Localization.m_Key[m_BlockIndex];

		if (key.Contains("/"))
		{
			string[] values = key.Split('/');
			print("temp / 0 : " + values[0] + " / 1 : " + values[1] + " / 2 : " + values[2]);

			int atlasIndex = -1;
			string[] spriteCodes;
			string spriteName = string.Empty;

			if (values[1].Contains("_"))
			{
				spriteCodes = values[1].Split('_');
				int.TryParse(spriteCodes[0], out atlasIndex);

				spriteName = string.Format("BG_{0}", spriteCodes[1]);
			}
			else
			{
				if (string.Equals(values[1], "place"))
				{
					atlasIndex = PlaceDataManager.instance.GetAtlasIndex(m_PlaceBackground);
					string spriteIndex = PlaceDataManager.instance.GetSpriteIndex(m_PlaceBackground);

					spriteName = string.Format("BG_{0}", spriteIndex);
				}
			}

			if (atlasIndex == -1)
			{
				print("wrong atlasIndex");
				return -1;
			}

			m_BG.atlas = BackgroundManager.instance.GetBackgroundAtlas(atlasIndex);
			m_BG.spriteName = spriteName;

			return 1;
		}
		// 배경이 설정 되어 있지 않는 경우
		else
		{
			return 0;
		}
	}

	protected int ShowCharacter()
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
				m_RightFaceControl.FaceSetting(m_Talker);

				m_LeftFaceControl.TalkerActive(true);
				m_RightFaceControl.TalkerActive(false);

				return 0;
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

				return 1;
			}
		}
		else
		{
			// ※가 포함되어 있지 않다는 것은 지금 대화하는 화자가 플레이어블 캐릭터라는 것.
			if (m_LeftFaceControl.gameObject.activeSelf == false)
				m_LeftFaceControl.gameObject.SetActive(true);

			m_LeftFaceControl.FaceSetting("Player");
			m_RightFaceControl.FaceSetting(m_Talker);

			m_LeftFaceControl.TalkerActive(true);
			m_RightFaceControl.TalkerActive(false);

			return 0;
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
