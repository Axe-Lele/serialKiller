using UnityEngine;
using System;
using System.Collections;

public class InGameUIManager : Singleton<InGameUIManager>
{
	//public UILabel DateLabel;
	//public UILabel TimeLabel;

	public TweenPosition MainUIBarPopupTweenPosition;
	public GameObject MainUIBarPopup;
	public GameObject MainUIBarBtn;

	public TweenPosition PoliceCommandTweenPosition;

	public GameObject MainUIBar_NotePopup;
	public GameObject MainUIBar_AnalysisPopup;
	public GameObject MainUIBar_PoliceCommand;
	public GameObject MainUIBar_LibraryPopup;
	public GameObject MainUIBar_DetectiveAssocation;
	public GameObject MainUIBar_Report;
	public GameObject MainUIBar_Internet;
	public GameObject MainUIBar_Mail;

	public GameObject NewsPopup;
	public GameObject WarrantPopup;
	public GameObject ExitPopup;


	public Transform PoliceCommandBtn;

	public GameObject DisplayPlaceNamePanel;
	public UISprite m_DisplayPlaceNamePanelBG;
	public UISprite[] m_CharacterListInPlace;
	public UILabel m_PlaceNameLabel;

	private Vector2 m_MainUIBarInitPosition;
	private Vector2 m_MainUIBarGoalPosition;
	private Vector2 m_PoliceCommandInitPosition;
	private Vector2 m_PoliceCommandGoalPosition;

	private bool m_IsMainUIBarMoveFlag;
	private int m_Hour, m_Minute;


	public UISprite m_WeekSrptie;
	public UISprite[] m_MonthSprite;
	public UISprite[] m_DaySprite;
	public UISprite[] m_TimeSprite;


	/* public GameObject NotePopup;
	 public GameObject EvidenceDetailPopup;
	 public GameObject CaseSearchPopup;
	 public GameObject NoticePanel;
	 public GameObject InformationPanel;
	 public GameObject ExitPopup;
	 public GameObject NewsPopup;
	 public GameObject SuggestPanel;
	 public GameObject SuspectPanel;
	 public GameObject PoliceCammandPanel;
	 public GameObject PoliceCaseInquiryPanel;
	 public GameObject PoliceSuspectInquiryPanel;
	 public GameObject PolicePatrolPanel;
	 public GameObject PoliceWarrantRequestPanel;

	 public GameObject LeftGameUIFolderBtn;
	 public TweenPosition[] LeftGameUItp;
	 public TweenPosition[] LeftMiniGameUItp;

	 public GameObject RightGameUIFolderBtn;
	 public GameObject RightGameUI;
	*/
	public TweenAlpha DayFlowTweenAlpha;
	public UILabel DayLabel1;
	public UILabel DayLabel2;
	public UISpriteAnimation DayBlock1;
	public UISpriteAnimation DayBlock2;


	private void Awake()
	{
		m_IsMainUIBarMoveFlag = false;
		m_MainUIBarInitPosition = new Vector2(MainUIBarPopup.transform.localPosition.x, MainUIBarPopup.transform.localPosition.y);
		m_MainUIBarGoalPosition = m_PoliceCommandInitPosition = new Vector2(MainUIBarBtn.transform.localPosition.x, MainUIBarBtn.transform.localPosition.y);
		//m_PoliceCommandInitPosition = new Vector2(PoliceCommandBtn.transform.localPosition.x, PoliceCommandBtn.transform.localPosition.y);
		m_PoliceCommandGoalPosition = new Vector2(m_PoliceCommandInitPosition.x + 80f, m_PoliceCommandInitPosition.y);
	}

	public void Rest()
	{
		GameManager.instance.Rest();
	}

	public void Throw()
	{
		print("UserAction");
		GameManager.instance.UserAction(UserActionType.Dialog);
	}

	public void MainUIInitialize(int month, int day, int time)
	{
		string[] months = { "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
		m_MonthSprite[0].spriteName = "clock_month_" + months[month];

		// Date Paper
		if (day >= 10)
		{
			m_DaySprite[0].spriteName = "clock_s_" + (day / 10).ToString();
			m_DaySprite[1].spriteName = "clock_s_" + (day % 10).ToString();
		}
		else
		{
			m_DaySprite[0].spriteName = "clock_s_" + "0";
			m_DaySprite[1].spriteName = "clock_s_" + day.ToString();
		}

		string week = GameManager.instance.ReturnDayOfWeek();

		m_WeekSrptie.spriteName = "clock_week_" + week.Substring(0, 3).ToLower();

		DayLabel1.text = (StageDataManager.instance.m_PastDay / 10).ToString();
		DayLabel2.text = (StageDataManager.instance.m_PastDay % 10).ToString();

		RefreshTime(time);
	}

	public void RefreshTime(int time)
	{

		if (time > 60)
		{
			m_Hour = time / 60;
			m_Minute = time % 60;
		}

		if (m_Hour < 10)
		{
			m_TimeSprite[0].spriteName = "clock_l_" + "0";
			m_TimeSprite[1].spriteName = "clock_l_" + m_Hour.ToString();
		}
		else
		{
			int h = m_Hour / 10;
			m_TimeSprite[0].spriteName = "clock_l_" + h.ToString();
			m_TimeSprite[1].spriteName = "clock_l_" + (m_Hour % 10).ToString();
		}


		if (m_Minute < 10)
		{
			m_TimeSprite[2].spriteName = "clock_l_" + "0";
			m_TimeSprite[3].spriteName = "clock_l_" + m_Minute.ToString();
		}
		else
		{
			int h = m_Minute / 10;
			if (h > 0)
			{
				m_TimeSprite[2].spriteName = "clock_l_" + h.ToString();
			}
			else
			{
				m_TimeSprite[2].spriteName = "clock_l_" + "0";
			}
			m_TimeSprite[3].spriteName = "clock_l_" + (m_Minute % 10).ToString();
		}

		//((m_Hour < 10) ? ("0" + m_Hour) : m_Hour.ToString()) + " : " + ((m_Minute < 10)?("0" + m_Minute):m_Minute.ToString());
	}

	public void IntDate(int month, int day)
	{
		string[] months = { "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
		m_MonthSprite[0].spriteName = "clock_month_" + months[month];

		if (day >= 10)
		{
			m_DaySprite[0].spriteName = "clock_l_" + "1";
			m_DaySprite[1].spriteName = "clock_l_" + (day % 10).ToString();
		}
		else
		{
			m_DaySprite[0].spriteName = "clock_s_" + "0";
			m_DaySprite[1].spriteName = "clock_s_" + day.ToString();
		}

		m_WeekSrptie.spriteName = "clock_week_" + GameManager.instance.ReturnDayOfWeek().Substring(0, 3).ToLower();
	}

	public void RefreshDate(int month, int day)
	{
		string[] months = { "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
		m_MonthSprite[0].spriteName = "clock_month_" + months[month];

		if (day >= 10)
		{
			m_DaySprite[0].spriteName = "clock_l_" + "1";
			m_DaySprite[1].spriteName = "clock_l_" + (day % 10).ToString();
		}
		else
		{
			m_DaySprite[0].spriteName = "clock_s_" + "0";
			m_DaySprite[1].spriteName = "clock_s_" + day.ToString();
		}

		m_WeekSrptie.spriteName = "clock_week_" + GameManager.instance.ReturnDayOfWeek().Substring(0, 3).ToLower();
		StartCoroutine(ChangeDate());
	}

	private IEnumerator ChangeDate()
	{
		DayBlock1.GetComponent<UISprite>().spriteName = "DayBlock_0";
		DayBlock2.GetComponent<UISprite>().spriteName = "DayBlock_0";

		DayBlock1.ResetToBeginning();
		DayBlock2.ResetToBeginning();

		DayFlowTweenAlpha.from = 0f;
		DayFlowTweenAlpha.to = 1f;
		DayFlowTweenAlpha.duration = 0.5f;
		DayFlowTweenAlpha.ResetToBeginning();
		DayFlowTweenAlpha.enabled = true;

		yield return new WaitForSeconds(0.5f);

		if (StageDataManager.instance.m_PastDay % 10 == 0)
		{
			DayLabel1.gameObject.SetActive(false);
			DayLabel1.text = (StageDataManager.instance.m_PastDay / 10).ToString();
			DayBlock1.enabled = true;

			yield return new WaitForSeconds(0.3f);
			DayLabel1.gameObject.SetActive(true);
			DayBlock1.enabled = false;
			yield return new WaitForSeconds(0.4f);
		}

		DayLabel2.gameObject.SetActive(false);
		DayLabel2.text = (StageDataManager.instance.m_PastDay % 10).ToString();
		DayBlock2.enabled = true;

		yield return new WaitForSeconds(0.3f);
		DayLabel2.gameObject.SetActive(true);
		DayBlock2.enabled = false;
		yield return new WaitForSeconds(0.4f);

		DayFlowTweenAlpha.from = 1f;
		DayFlowTweenAlpha.to = 0f;
		DayFlowTweenAlpha.duration = 1f;
		DayFlowTweenAlpha.ResetToBeginning();
		DayFlowTweenAlpha.enabled = true;

		yield return new WaitForSeconds(1f);
	}

	public void ControlMainUIBar()
	{
		if (m_IsMainUIBarMoveFlag)
			return;

		m_IsMainUIBarMoveFlag = true;

		MainUIBarPopupTweenPosition.duration = 0.2f;

		if (MainUIBarBtn.activeSelf)
		{
			StartCoroutine(ShowMainUIBar());
		}
		else
		{
			StartCoroutine(HideMainUIBar());
		}
	}

	private IEnumerator ShowMainUIBar()
	{
		MainUIBarBtn.SetActive(false);

		MainUIBarPopupTweenPosition.from = m_MainUIBarInitPosition;
		MainUIBarPopupTweenPosition.to = m_MainUIBarGoalPosition;
		MainUIBarPopupTweenPosition.ResetToBeginning();
		MainUIBarPopupTweenPosition.enabled = true;

		yield return new WaitForSeconds(0.2f);

		m_IsMainUIBarMoveFlag = false;
	}

	private IEnumerator HideMainUIBar()
	{
		MainUIBarPopupTweenPosition.from = m_MainUIBarGoalPosition;
		MainUIBarPopupTweenPosition.to = m_MainUIBarInitPosition;
		MainUIBarPopupTweenPosition.ResetToBeginning();
		MainUIBarPopupTweenPosition.enabled = true;

		yield return new WaitForSeconds(0.2f);

		MainUIBarBtn.SetActive(true);
		m_IsMainUIBarMoveFlag = false;
	}

	public void OpenNote()
	{
		GameManager.instance.m_NoteMode = NoteMode.None;

		ControlNotePopup();
	}

	public void ControlNotePopup()
	{
		if (MainUIBar_NotePopup.activeSelf)
		{
			MainUIBar_NotePopup.SetActive(false);
			if (GameManager.instance.m_NoteMode == NoteMode.Warrant || GameManager.instance.m_NoteMode == NoteMode.SelectCase)
			{
				ControlWarrantPopup();
			}
			else if (GameManager.instance.m_NoteMode == NoteMode.Suggest)
			{

			}
		}
		else
		{
			NoteManager.instance.SetNote();
			NoteManager.instance.ControlNoteUI();
			NoteManager.instance.SelectNote();
			MainUIBar_NotePopup.SetActive(true);
		}
	}

	public void ControlLaboratoryPanel()
	{
		if (MainUIBar_AnalysisPopup.activeSelf)
		{
			MainUIBar_AnalysisPopup.SetActive(false);
			LaboratoryManager.instance.ControlLaboratoryUI(false);
		}
		else
		{
			MainUIBar_AnalysisPopup.SetActive(true);
			LaboratoryManager.instance.ControlLaboratoryUI(true);
		}
	}

	public void ControlNewsPopup()
	{
		if (NewsPopup.activeSelf)
		{
			NewsPopup.SetActive(false);
		}
		else
		{
			NewsPopup.SetActive(true);
		}
	}

	public void ControlPoliceCommand()
	{
		if (m_IsMainUIBarMoveFlag)
			return;

		m_IsMainUIBarMoveFlag = true;

		PoliceCommandTweenPosition.duration = 0.2f;

		if (MainUIBar_PoliceCommand.activeSelf)
		{
			StartCoroutine(HidePoliceCommand());
		}
		else
		{
			StartCoroutine(ShowPoliceCommand());
		}
	}



	private IEnumerator ShowPoliceCommand()
	{
		MainUIBar_PoliceCommand.SetActive(true);

		PoliceCommandTweenPosition.from = m_PoliceCommandInitPosition;
		PoliceCommandTweenPosition.to = m_PoliceCommandGoalPosition;
		PoliceCommandTweenPosition.ResetToBeginning();
		PoliceCommandTweenPosition.enabled = true;

		yield return new WaitForSeconds(0.2f);

		m_IsMainUIBarMoveFlag = false;
	}

	private IEnumerator HidePoliceCommand()
	{
		PoliceCommandTweenPosition.from = m_PoliceCommandGoalPosition;
		PoliceCommandTweenPosition.to = m_PoliceCommandInitPosition;
		PoliceCommandTweenPosition.ResetToBeginning();
		PoliceCommandTweenPosition.enabled = true;

		yield return new WaitForSeconds(0.2f);

		m_IsMainUIBarMoveFlag = false;
		MainUIBar_PoliceCommand.SetActive(false);
	}

	public void ControlWarrantPopup()
	{
		if (WarrantPopup.activeSelf)
		{
			WarrantPopup.SetActive(false);
		}
		else
		{
			WarrantPopup.SetActive(true);
		}
	}

	public void ControlExitPopup()
	{
		if (ExitPopup.activeSelf)
		{
			ExitPopup.SetActive(false);
		}
		else
		{
			ExitPopup.SetActive(true);
		}
	}

	public void ControlPhoneCommand()
	{
		SystemTextManager.instance.InputText(Localization.Get("System_Text_Not_Use_Phone"));
	}

	public void ControlLibraryPopup()
	{
		if (MainUIBar_LibraryPopup.activeSelf)
		{
			MainUIBar_LibraryPopup.SetActive(false);
		}
		else
		{
			MainUIBar_LibraryPopup.SetActive(true);
		}
	}

	public void ControlDetectiveAssocation()
	{
		if (MainUIBar_DetectiveAssocation.activeSelf)
		{
			MainUIBar_DetectiveAssocation.SetActive(false);
		}
		else
		{
			MainUIBar_DetectiveAssocation.SetActive(true);
		}
	}

	public void ControlReport()
	{
		if (MainUIBar_Report.activeSelf)
		{
			MainUIBar_Report.SetActive(false);
		}
		else
		{
			MainUIBar_Report.SetActive(true);
		}
	}

	public void ControlInternet()
	{
		if (MainUIBar_Internet.activeSelf)
		{
			MainUIBar_Internet.SetActive(false);
		}
		else
		{
			MainUIBar_Internet.SetActive(true);
		}
	}

	public void ControlMail()
	{
		if (MainUIBar_Mail.activeSelf)
		{
			MainUIBar_Mail.SetActive(false);
		}
		else
		{
			MainUIBar_Mail.SetActive(true);
		}
	}

	public void ShowPlaceName(PlaceType type, int index, string name)
	{
		int person = 0;
		string[] temp;

		if (type == PlaceType.Home)
		{
			person = PlaceManager.instance.m_HomeList[index].m_CharacterList.Count;
			temp = new string[person];
			for (int i = 0; i < person; i++)
			{
				temp[i] = PlaceManager.instance.m_HomeList[index].m_CharacterList[i];
				m_CharacterListInPlace[i].atlas = NpcDataManager.instance.ReturnAtlas(temp[i]);
				m_CharacterListInPlace[i].spriteName = temp[i];
				m_CharacterListInPlace[i].gameObject.SetActive(true);
			}
		}
		else if (type == PlaceType.Company)
		{
			person = PlaceManager.instance.m_CompanyList[index].m_CharacterList.Count;
			temp = new string[person];
			for (int i = 0; i < person; i++)
			{
				temp[i] = PlaceManager.instance.m_CompanyList[index].m_CharacterList[i];
				m_CharacterListInPlace[i].atlas = NpcDataManager.instance.ReturnAtlas(temp[i]);
				m_CharacterListInPlace[i].spriteName = temp[i];
				m_CharacterListInPlace[i].gameObject.SetActive(true);
			}
		}
		else if (type == PlaceType.Extra)
		{
			person = PlaceManager.instance.m_ExtraPlaceList[index].m_CharacterList.Count;
			temp = new string[person];
			for (int i = 0; i < person; i++)
			{
				temp[i] = PlaceManager.instance.m_ExtraPlaceList[index].m_CharacterList[i];
				m_CharacterListInPlace[i].atlas = NpcDataManager.instance.ReturnAtlas(temp[i]);
				m_CharacterListInPlace[i].spriteName = temp[i];
				m_CharacterListInPlace[i].gameObject.SetActive(true);
			}
		}
		else if (type == PlaceType.Case)
		{
			person = PlaceManager.instance.m_CasePlaceList[index].m_CharacterList.Count;
			temp = new string[person];
			for (int i = 0; i < person; i++)
			{
				temp[i] = PlaceManager.instance.m_CasePlaceList[index].m_CharacterList[i];
				m_CharacterListInPlace[i].atlas = NpcDataManager.instance.ReturnAtlas(temp[i]);
				m_CharacterListInPlace[i].spriteName = temp[i];
				m_CharacterListInPlace[i].gameObject.SetActive(true);
			}
		}
		else
		{
			person = 0;
			print("사건 지역에는 캐릭터가 배정되지 않아");
		}

		Vector2 vec;
		switch (person)
		{
			case 0:
				m_DisplayPlaceNamePanelBG.width = 280;
				vec = new Vector2(0f, 8f);
				break;

			case 1:
				vec = new Vector2(0f, 8f);
				m_DisplayPlaceNamePanelBG.width = 280;
				break;
			case 2:
				vec = new Vector2(-45f, 8f);
				m_DisplayPlaceNamePanelBG.width = 280;
				break;
			case 3:
				vec = new Vector2(-80f, 8f);
				m_DisplayPlaceNamePanelBG.width = 280;
				break;
			case 4:
				vec = new Vector2(-155f, 8f);
				m_DisplayPlaceNamePanelBG.width = 360;
				break;
			case 5:
				vec = new Vector2(-160f, 8f);
				m_DisplayPlaceNamePanelBG.width = 440;
				break;
			default:
				vec = new Vector2(-160f, 8f);
				break;
		}
		for (int i = 0; i < 5; i++)
		{
			m_CharacterListInPlace[i].gameObject.SetActive(false);
		}


		for (int i = 0; i < person; i++)
		{
			//m_CharacterListInPlace[i].spriteName = "";
			m_CharacterListInPlace[i].gameObject.SetActive(true);
			m_CharacterListInPlace[i].transform.localPosition = new Vector2(vec.x + (i * 80f), 8f);
			// m_CharacterListInPlace[i].gameObject.SetActive(true);
		}

		m_PlaceNameLabel.text = name;

		DisplayPlaceNamePanel.SetActive(true);
	}

	public void HidePlaceName()
	{
		for (int i = 0; i < m_CharacterListInPlace.Length; i++)
		{
			m_CharacterListInPlace[i].gameObject.SetActive(false);
		}

		DisplayPlaceNamePanel.SetActive(false);
	}

}
