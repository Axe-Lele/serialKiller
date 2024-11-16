using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoteManager : Singleton<NoteManager>
{
	public GameObject m_MapParent;
	public GameObject m_NewsPanel;

	// 전체
	public NoteItem[] m_Btns;
	public GameObject[] m_BG;
	public GameObject[] m_Tabs;

	public GameObject m_SuggestBtn;
	public GameObject m_WarrantBtn;
	public GameObject m_WarrantSelectCaseBtn;

	public UILabel m_AlarmEmptyLabel;

	private int m_NoteTabIndex;

	public int m_NoteModeIndex;
	public int m_NoteModeDetailIndex;
	public int m_SuggestModeIndex;
	public int m_SuggestModeDetailIndex;
	public int m_WarrantModeIndex;
	public int m_WarrantModeDetailIndex;

	public GameObject m_Checker;

	// 사건 탭
	public List<CaseItemInNote> m_CaseItemList;
	public CaseItemInNote m_OriginalCaseItem;
	public UIGrid m_CaseTable;
	public UIScrollView m_CaseScrollView;
	public UIScrollBar m_CaseScrollBar;

	public GameObject[] m_CaseTab;

	public UISprite m_CaseIllust;
	public UILabel m_CaseMainInfoLabel;
	public UILabel m_CaseSideInfoLabel;

	public UIScrollView m_MainCaseScrollView;
	public UIScrollView m_SideCaseScrollView;

	// 대화
	// 대화 탭
	public List<DialogItem> m_DialogItemList;
	public DialogItem m_OriginalDialogItem;
	public UIGrid m_DialogTable;
	public UIScrollView m_DialogScrollView;
	public UIScrollBar m_DialogScrollBar;

	// 대화 상세

	public List<DialogDetailItem> m_DialogDetailItemList;
	public DialogDetailItem m_OriginalDialogDetailItem;
	public UIGrid m_DialogDetailTable;
	public UIScrollView m_DialogDetailScrollView;
	public UIScrollBar m_DialogDetailScrollBar;

	// 단서 탭
	// 단서 팝업
	public List<EvidenceItem> m_EvidenceItemList;
	public EvidenceItem m_OriginalEvidenceItem;
	public UIGrid m_EvidenceTable;
	public UIScrollView m_EvidenceScrollView;
	public UIScrollBar m_EvidenceScrollBar;

	// 단서 상세
	public UIScrollView m_EvidenceDetailScrollView;
	public UISprite m_EvidenceIllust;
	public UILabel m_EvidenceTitle;
	public UILabel m_EvidenceContent;
	public UIButton m_AnalyzedBtn;
	public UILabel m_AnalyzedLabel;
	public UILabel m_AnalyzedTimeLabel;
	public BoxCollider m_AnalyzedCol;
	public UIButton m_MatchedBtn;
	public UILabel m_MatchedLabel;
	public UILabel m_MatchedTimeLabel;
	public BoxCollider m_MatchedCol;

	private EvidenceDataItem m_EvidenceDataItem;

	// 뉴스 탭
	// 뉴스 팝업에서 다루는 변수와 컴포넌트
	public List<NewsItem> m_NewsItemList;
	public List<GameObject> m_DateItemList;
	//public List<int> m_Date;
	public GameObject m_NewsDateLabel;
	public NewsItem m_OriginalNewsItem;
	public UIGrid m_NewsTable;
	public UIScrollView m_NewsScrollView;
	public UIScrollBar m_NewsScrollBar;

	// 뉴스 상세
	public UIScrollView m_NewsDetailScrollView;
	public UISprite m_NewsIllust;
	public UILabel m_NewsTitleLabel;
	public UILabel m_NewsContentLabel;

	private void Awake()
	{
		m_NoteModeIndex = 0;
		m_NoteModeDetailIndex = 0;
		m_SuggestModeIndex = 1;
		m_SuggestModeDetailIndex = 0;
		m_WarrantModeIndex = 1;
		m_WarrantModeDetailIndex = 0;
	}

	public void SelectNote()
	{
		SelectedNoteTab(m_NoteTabIndex);
	}

	public void CloseNote()
	{
		m_MapParent.SetActive(true);
		m_NewsPanel.SetActive(true);
	}

	public void ShowNoteNews()
	{
		m_MapParent.SetActive(false);
		m_NewsPanel.SetActive(false);

		GameManager.instance.m_NoteMode = NoteMode.None;
		SelectedNoteTab((int)NoteTap.NEWS);

		m_NoteNewsManager.SelectedNewsItem(0);
	}

	public void ShowNote()
	{
		SetNote();
		return; 

		/*
		m_MapParent.SetActive(false);
		m_NewsPanel.SetActive(false);

		switch (GameManager.instance.m_NoteMode)
		{
			case NoteMode.None:
				break;

			case NoteMode.Suggest:

				ShowSuggestNote();
				break;

			case NoteMode.Warrant:
				ShowWarrantNote();
				break;

			case NoteMode.SelectCase:
				ShowSelectCaseNote();
				break;

			default:
				break;
		}
		*/
	}

	#region Show NoteMode
	/*
	private void ShowSuggestNote()
	{
		int m_DetailIndex = 0;

		m_NoteTabIndex = m_SuggestModeIndex;
		m_DetailIndex = m_SuggestModeDetailIndex;

		m_Btns[0].gameObject.SetActive(false);
		for (int i = 1; i < m_Btns.Length; i++)
		{
			m_Btns[i].gameObject.SetActive(true);
		}

		for (int i = 0; i < m_NoteDialogManager.m_DetailItemList.Count; i++)
		{
			m_NoteDialogManager.m_DetailItemList[i].m_IsSelected = false;
		}
		for (int i = 0; i < m_NoteEvidenceManager.m_EvidenceItemList.Count; i++)
		{
			m_NoteEvidenceManager.m_EvidenceItemList[i].m_IsSelected = false;
		}
		for (int i = 0; i < m_NoteNewsManager.m_ItemList.Count; i++)
		{
			m_NoteNewsManager.m_ItemList[i].m_IsSelected = false;
		}

		SelectedNoteTab(m_NoteTabIndex);

		if (m_DialogItemList.Count > 0)
		{
			SelectReset();
			SelectDialog(m_DetailIndex);
			m_DialogItemList[m_DetailIndex].Select();
		}
	}
	private void ShowWarrantNote()
	{
		int m_DetailIndex = 0;

		m_NoteTabIndex = m_WarrantModeIndex;
		m_DetailIndex = m_WarrantModeDetailIndex;

		m_Btns[0].gameObject.SetActive(false);
		for (int i = 1; i < m_Btns.Length; i++)
		{
			m_Btns[i].gameObject.SetActive(true);
		}

		SelectedNoteTab(m_NoteTabIndex);

		// show note-dialog
		if (m_DialogItemList.Count > 0)
		{
			SelectReset();
			SelectDialog(m_DetailIndex);
			m_DialogItemList[m_DetailIndex].Select();
		}
	}
	private void ShowSelectCaseNote()
	{
		print("show selectcase note");
		int m_DetailIndex = 0;

		m_NoteTabIndex = 0;
		m_DetailIndex = 0;

		for (int i = 0; i < m_Btns.Length; i++)
		{
			m_Btns[i].gameObject.SetActive(false);
		}
		m_Btns[0].gameObject.SetActive(true);

		SelectedNoteTab(m_NoteTabIndex);

		if (m_CaseItemList.Count > 0)
		{
			for (int i = 0; i < m_CaseItemList.Count; i++)
			{
				m_CaseItemList[i].UnSelect();
			}

			m_CaseItemList[m_DetailIndex].Select();
		}
	}
	*/
	#endregion

	public void SetNote()
	{
		m_MapParent.SetActive(false);
		m_NewsPanel.SetActive(false);

		for (int i = 0; i < m_Btns.Length; i++)
			m_Btns[i].gameObject.SetActive(false);

		switch (GameManager.instance.m_NoteMode)
		{
			case NoteMode.None:
				m_NoteTabIndex = (int)NoteTap.CASE;
				m_Btns[(int)NoteTap.CASE].gameObject.SetActive(true);
				m_Btns[(int)NoteTap.DIALOG].gameObject.SetActive(true);
				m_Btns[(int)NoteTap.EVIDENCE].gameObject.SetActive(true);
				m_Btns[(int)NoteTap.NEWS].gameObject.SetActive(true);
				m_NoteCaseManager.ShowCase(true);
				break;

			case NoteMode.Suggest:
				m_NoteTabIndex = (int)NoteTap.DIALOG;
				m_Btns[(int)NoteTap.DIALOG].gameObject.SetActive(true);
				m_Btns[(int)NoteTap.EVIDENCE].gameObject.SetActive(true);
				m_Btns[(int)NoteTap.NEWS].gameObject.SetActive(true);

				break;

			case NoteMode.Warrant:
				m_NoteTabIndex = (int)NoteTap.DIALOG;
				m_Btns[(int)NoteTap.DIALOG].gameObject.SetActive(true);
				m_Btns[(int)NoteTap.EVIDENCE].gameObject.SetActive(true);
				m_Btns[(int)NoteTap.NEWS].gameObject.SetActive(true);
				break;

			case NoteMode.SelectCase:
				m_NoteTabIndex = (int)NoteTap.CASE;
				m_Btns[(int)NoteTap.CASE].gameObject.SetActive(true);
				break;

			default:
				break;
		}
		SelectedNoteTab(m_NoteTabIndex);
	}
	
	public void SelectedNoteTab(int index)
	{
		m_NoteTabIndex = index;
		m_Checker.transform.SetParent(m_Btns[index].transform);
		m_Checker.transform.localPosition = Vector3.zero;
		m_Checker.SetActive(true);

		m_NoteCaseManager.ShowCase(false);
		m_NoteDialogManager.ShowDialog(false);
		m_NoteNewsManager.ShowNewsPanel(false);
		m_NoteEvidenceManager.ShowEvidence(false);

		#region non-used
		//for (int i = 0; i < m_BG.Length; i++)
		//{
		//	if (i == index)
		//	{
		//		m_Btns[i].GetComponent<NoteItem>().ActiveSprite();
		//		m_BG[i].SetActive(true);
		//		m_Tabs[i].SetActive(true);
		//	}
		//	else
		//	{
		//		if (i == 0)
		//			continue;

		//		m_Btns[i].GetComponent<NoteItem>().UnactiveSrptie();
		//		m_BG[i].SetActive(false);
		//		m_Tabs[i].SetActive(false);
		//	}
		//}

		//switch (GameManager.instance.m_NoteMode)
		//{
		//	case NoteMode.None:
		//		m_NoteModeIndex = index;
		//		break;
		//	case NoteMode.Suggest:
		//		m_SuggestModeIndex = index;
		//		break;
		//	case NoteMode.Warrant:
		//		m_WarrantModeIndex = index;
		//		break;
		//}
		#endregion

		switch (index)
		{
			case (int)NoteTap.CASE:
				m_NoteCaseManager.ShowCase(true);
				break;

				m_CaseScrollView.ResetPosition();
				m_MainCaseScrollView.ResetPosition();
				m_SideCaseScrollView.ResetPosition();
				if (m_CaseItemList.Count == 0)
				{
					m_AlarmEmptyLabel.gameObject.SetActive(true);
					m_AlarmEmptyLabel.text = Localization.Get("NotHaveCaseItem");
					m_CaseIllust.gameObject.SetActive(false);
				}
				else
				{
					m_CaseIllust.gameObject.SetActive(true);
					m_AlarmEmptyLabel.gameObject.SetActive(false);
				}
				break;

			case (int)NoteTap.DIALOG:
				for (int i = 0; i < m_NoteDialogManager.m_DetailItemList.Count; i++)
				{
					m_NoteDialogManager.m_DetailItemList[i].m_IsSelected = false;
				}
				m_NoteDialogManager.ShowDialog(true);
				break;

				m_DialogScrollView.ResetPosition();
				m_DialogDetailScrollView.ResetPosition();

				if (m_DialogItemList.Count == 0)
				{
					m_AlarmEmptyLabel.gameObject.SetActive(true);
					m_AlarmEmptyLabel.text = Localization.Get("NotHaveDialogItem");
				}
				else
					m_AlarmEmptyLabel.gameObject.SetActive(false);
				break;

			case (int)NoteTap.EVIDENCE:
				for (int i = 0; i < m_NoteEvidenceManager.m_EvidenceItemList.Count; i++)
				{
					m_NoteEvidenceManager.m_EvidenceItemList[i].m_IsSelected = false;
				}
				m_NoteEvidenceManager.ShowEvidence(true);
				return;

				m_EvidenceScrollView.ResetPosition();
				m_EvidenceDetailScrollView.ResetPosition();

				if (m_EvidenceItemList.Count == 0)
				{
					m_AlarmEmptyLabel.gameObject.SetActive(true);
					m_AlarmEmptyLabel.text = Localization.Get("NotHaveEvidenceItem");
					m_EvidenceIllust.spriteName = string.Empty;
					m_EvidenceTitle.text = string.Empty;
				}
				else
				{
					m_AlarmEmptyLabel.gameObject.SetActive(false);

				}

				if (m_EvidenceDataItem != null)
				{
					ChangeEvidenceInfoUI(m_EvidenceDataItem);
				}
				break;

			case (int)NoteTap.NEWS:
				for (int i = 0; i < m_NoteNewsManager.m_ItemList.Count; i++)
				{
					m_NoteNewsManager.m_ItemList[i].m_IsSelected = false;
				}
				m_NoteNewsManager.ShowNewsPanel(true);
				return;

				m_NewsScrollView.ResetPosition();
				m_NewsDetailScrollView.ResetPosition();
				if (m_NewsItemList.Count == 0)
				{
					m_AlarmEmptyLabel.gameObject.SetActive(true);
					m_AlarmEmptyLabel.text = Localization.Get("NotHaveNewsItem");
					m_NewsIllust.gameObject.SetActive(false);
				}
				else
				{
					m_AlarmEmptyLabel.gameObject.SetActive(false);
					m_NewsIllust.gameObject.SetActive(true);
					SelectNews(0);
				}
				break;
		}
	}

	private int GetCaseItemIndex(string caseIndex)
	{
		int _rvalue = -1;

		for (int i = 0; i < m_CaseItemList.Count; i++)
		{
			if (m_CaseItemList[i].m_Index.CompareTo(caseIndex) == 0)
			{
				_rvalue = i;
				break;
			}
		}
		return _rvalue;
	}

	[GetComponent]
	public NoteCaseManager m_NoteCaseManager;
	public int InputCase(CaseMode mode, string itemCode)
	{
		return m_NoteCaseManager.InputItem(mode, itemCode);
	}

	public void SelectCase(CaseMode mode, string itemCode)
	{
		m_NoteCaseManager.SelectedCaseItem(mode, itemCode);
		//m_CaseIllust.spriteName = "Case_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + index + "_Sprite";
	}

	public void SelectCase(int itemIndex)
	{
		m_CaseScrollView.ResetPosition();
		//   m_CaseIllust.spriteName = "Case_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + EventDataManager.instance.ReturnCaseCode(i) + "_Sprite";
		string s = "Case_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + EventDataManager.instance.ReturnCaseCode(itemIndex);
		m_CaseMainInfoLabel.text = Localization.Get(s);
		if (m_CaseIllust.gameObject.activeInHierarchy == false)
			m_CaseIllust.gameObject.SetActive(true);

		for (int i = 0; i < m_CaseItemList.Count; i++)
		{
			m_CaseItemList[i].m_ButtonSprite.spriteName = "button_character_off";
		}
		m_CaseItemList[itemIndex].m_ButtonSprite.spriteName = "button_character_on";

		switch (GameManager.instance.m_NoteMode)
		{
			case NoteMode.None:
				m_NoteModeDetailIndex = itemIndex;
				break;
			case NoteMode.Suggest:
				m_SuggestModeDetailIndex = itemIndex;
				break;
			case NoteMode.Warrant:
				m_WarrantModeDetailIndex = itemIndex;
				break;
		}
	}

	/// <summary>
	/// 사건정보 강제 등록. 관련 이벤트가 발생하지 않는다.
	/// </summary>
	/// <param name="mode">{CaseMode}</param>
	/// <param name="eventCode">{CaseIndex}</param>
	public void ForcedInputCase(int mode, string itemCode)
	{
		m_NoteCaseManager.ForcedInputItem(mode, itemCode);
	}

	[GetComponent]
	public NoteDialogManager m_NoteDialogManager;

	public void InputDialog(string target)
	{
		m_NoteDialogManager.InputDialog(target);
	}

	public void RemoveDialog(string target)
	{

	}

	public void SelectDialog(int i)
	{
		ShowDetailDialogList(m_DialogItemList[i].ReturnIndex());
	}

	public void ShowDetailDialogList(string target)
	{
		m_NoteDialogManager.SelectedDialogItem(target);
	}

	[GetComponent]
	public NoteEvidenceManager m_NoteEvidenceManager;

	public void InputEvidence(EvidenceDataItem i)
	{
		m_NoteEvidenceManager.InputEvidence(i);
	}

	public void RemoveEvidence(string index)
	{
		m_NoteEvidenceManager.RemoveEvidence(index);
		return;

		#region non-used
		print("remove item : " + index);
		for (int i = 0; i < m_EvidenceItemList.Count; i++)
		{
			print("i : " + i + " / name : " + m_EvidenceItemList[i].ReturnItem().m_ItemCode);
			if (m_EvidenceItemList[i].ReturnItem().m_ItemCode == index)
			{
				Destroy(m_EvidenceItemList[i].gameObject);
				m_EvidenceItemList.RemoveAt(i);
				break;
			}
		}
		#endregion
	}

	public void BannedEvidence(string itemCode)
	{
		m_NoteEvidenceManager.BannedEvidence(itemCode);
	}

	public void SelectedEvidenceItem(int i)
	{

	}

	public void SelectedEvidenceItem(string index)
	{

	}
	
	public void ChangeEvidenceInfoUI(int i)
	{
		ChangeEvidenceInfoUI(m_EvidenceItemList[i].ReturnItem());
	}

	public void ChangeEvidenceInfoUI(EvidenceDataItem item)
	{
		m_NoteEvidenceManager.ChangeEvidenceInfoUI(item);
		return;

		#region non-used
		for (int i = 0; i < m_EvidenceItemList.Count; i++)
		{
			m_EvidenceItemList[i].UnSelect();
			if (m_EvidenceItemList[i].m_EveidenceName.Equals(item.m_ItemCode))
			{
				m_EvidenceItemList[i].ActivateButton();
				print("SelectEvidence : " + item.m_ItemCode);
			}
		}

		string title = Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + item.m_ItemCode + "_Title");
		string content = Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + item.m_ItemCode + "_Content");
		if (GameManager.instance.m_NoteMode == NoteMode.Suggest)
		{
			m_AnalyzedBtn.gameObject.SetActive(false);
			m_MatchedBtn.gameObject.SetActive(false);
		}
		else if (GameManager.instance.m_NoteMode == NoteMode.Warrant)
		{
			m_AnalyzedBtn.gameObject.SetActive(false);
			m_MatchedBtn.gameObject.SetActive(false);
		}
		else
		{
			m_AnalyzedBtn.gameObject.SetActive(true);
			m_MatchedBtn.gameObject.SetActive(true);

			m_AnalyzedCol.enabled = true;
			m_MatchedBtn.enabled = true;

			if (item.m_IsAnalyzed == true && item.m_ResultAnalyzed == false)
			{
				// 분석 가능
				m_AnalyzedLabel.text = Localization.Get("System_Text_Note_Analyzed_Possible");
				m_AnalyzedLabel.color = Color.white;

				m_AnalyzedTimeLabel.text = string.Format(Localization.Get("System_Text_Note_Evidence_RemainTime"), EvidenceDataManager.instance.ReturnCoolTime(item.m_ItemCode, "AnalyzedTime"));
			}
			else if (item.m_IsAnalyzed == true && item.m_ResultAnalyzed == true)
			{
				// 분석 끝
				m_AnalyzedLabel.text = Localization.Get("System_Text_Note_Analyzed_Complete");
				m_AnalyzedLabel.color = Color.gray;

				m_AnalyzedTimeLabel.text = "";
			}
			else
			{
				// 분석 불가능
				m_AnalyzedLabel.text = Localization.Get("System_Text_Note_Analyzed_Impossible");
				m_AnalyzedLabel.color = Color.gray;
				m_AnalyzedTimeLabel.text = "";

				m_AnalyzedCol.enabled = false;
			}

			if (item.m_IsMathced)
			{
				// 비교 가능
				m_MatchedLabel.text = Localization.Get("System_Text_Note_Matched_Possible");
				m_MatchedLabel.color = Color.white;

				m_MatchedTimeLabel.text = string.Format(Localization.Get("System_Text_Note_Evidence_RemainTime"), EvidenceDataManager.instance.ReturnCoolTime(item.m_ItemCode, "MatchedTime"));
			}
			else
			{
				// 비교 불가능
				m_MatchedLabel.text = Localization.Get("System_Text_Note_Matched_Impossible");
				m_MatchedLabel.color = Color.gray;
				m_MatchedTimeLabel.text = "";

				m_MatchedBtn.enabled = false;
			}

			if (item.m_ResultAnalyzed == true)
			{
				m_AnalyzedBtn.gameObject.SetActive(false);
				if (item.m_IsChanged == true)
				{
					content = Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + item.m_ItemCode + "_Content_Alter");
				}
				else
				{
					content += Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + item.m_ItemCode + "_Add");
				}

				print("이미 분석이 끝났다.");
			}
		}

		//m_EvidenceTitle.text = Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + item.m_EvidenceName + "_Title");
		m_EvidenceDataItem = item;
		m_EvidenceIllust.spriteName = "Evidence_" + PlayDataManager.instance.m_StageName + "_" + item.m_ItemCode;
		m_EvidenceTitle.text = title;
		m_EvidenceContent.text = content;

		if (m_EvidenceIllust.gameObject.activeInHierarchy == false)
		{
			m_EvidenceIllust.gameObject.SetActive(true);
		}

		int index = 0;
		for (int i = 0; i < m_EvidenceItemList.Count; i++)
		{
			if (m_EvidenceItemList[i].ReturnItem() == item)
			{
				index = i;
				break;
			}
		}

		switch (GameManager.instance.m_NoteMode)
		{
			case NoteMode.None:
				m_NoteModeDetailIndex = index;
				break;
			case NoteMode.Suggest:
				m_SuggestModeDetailIndex = index;
				break;
			case NoteMode.Warrant:
				m_WarrantModeDetailIndex = index;
				break;
		}

		//m_AnalyzedTimeLabel.text = item.an    m_MatchedTimeLabel
		#endregion
	}

	public void EvidenceAnalyzed()
	{
		m_NoteEvidenceManager.EvidenceAnalyzed();
	}

	public void EvidenceMatched()
	{
		m_NoteEvidenceManager.EvidenceMatched();
	}

	[GetComponent]
	public NoteNewsManager m_NoteNewsManager;
	public void AddReadNewsList(int date, string itemCode)
	{
		m_NoteNewsManager.InputItem(date, itemCode);
		return;

		print("date : " + date + " / index : " + itemCode);

		bool b = false;
		for (int i = 0; i < m_NewsItemList.Count; i++)
		{
			if (m_NewsItemList[i].m_Date == date)
			{
				b = true;
			}
		}
		if (b == false)
		{
			//GameObject go1 = Instantiate(m_NewsDateLabel) as GameObject;
			//go1.gameObject.SetActive(true);
			//go1.transform.parent = m_NewsTable.transform;
			//go1.transform.localScale = Vector2.one;
			//go1.transform.localPosition = Vector2.zero;

			//go1.GetComponent<UILabel>().text = GameManager.instance.ReturnNowDate(date);
			//m_DateItemList.Add(go1);

			//  m_NewsTable.enabled = true;
			//  m_NewsScrollView.ResetPosition();
		}

		NewsItem go = Instantiate(m_OriginalNewsItem) as NewsItem;
		go.gameObject.SetActive(true);
		go.transform.parent = m_NewsTable.transform;
		go.transform.localScale = Vector2.one;
		go.transform.localPosition = Vector2.zero;
		go.Setting(date, itemCode, m_NewsItemList.Count, 1);
		m_NewsItemList.Add(go);

		m_NewsTable.enabled = true;
		m_NewsScrollBar.value = 0f;
		//    m_NewsScrollView.ResetPosition();

	}

	public void SelectNews(int index)
	{
		m_NoteNewsManager.SelectedNewsItem(index);
		return;

		print("Select News index : " + index);
		//m_NewsIllust.spriteName = "News_Illust_" + m_NewsItemList[index].m_NewsIndex;
		m_NewsTitleLabel.text = Localization.Get("News_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + m_NewsItemList[index].m_ItemCode + "_Title");
		m_NewsContentLabel.text = Localization.Get("News_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + m_NewsItemList[index].m_ItemCode + "_Content");
		if (m_NewsIllust.gameObject.activeInHierarchy == false)
		{
			m_NewsIllust.gameObject.SetActive(true);
		}

		for (int i = 0; i < m_NewsItemList.Count; i++)
		{
			m_NewsItemList[i].SeletedSprite.spriteName = "button_character_off";
		}
		m_NewsItemList[index].SeletedSprite.spriteName = "button_character_on";

		m_NewsItemList[index].m_IsSelected = true;

		switch (GameManager.instance.m_NoteMode)
		{
			case NoteMode.None:
				m_NoteModeDetailIndex = index;
				break;
			case NoteMode.Suggest:
				m_SuggestModeDetailIndex = index;
				break;
			case NoteMode.Warrant:
				m_WarrantModeDetailIndex = index;
				break;
		}

	}


	public void InputLog(string str)
	{

	}

	public void ControlNoteUI()
	{
		print("NoteMode : " + GameManager.instance.m_NoteMode);

		if (GameManager.instance.m_NoteMode == NoteMode.Suggest)
		{
			m_SuggestBtn.gameObject.SetActive(true);
			m_WarrantBtn.gameObject.SetActive(false);
			m_WarrantSelectCaseBtn.gameObject.SetActive(false);
		}
		else if (GameManager.instance.m_NoteMode == NoteMode.Warrant)
		{
			m_SuggestBtn.gameObject.SetActive(false);
			m_WarrantBtn.gameObject.SetActive(true);
			m_WarrantSelectCaseBtn.gameObject.SetActive(false);
		}
		else if (GameManager.instance.m_NoteMode == NoteMode.SelectCase)
		{
			m_SuggestBtn.gameObject.SetActive(false);
			m_WarrantBtn.gameObject.SetActive(false);
			m_WarrantSelectCaseBtn.gameObject.SetActive(true);
		}
		else
		{
			m_SuggestBtn.gameObject.SetActive(false);
			m_WarrantBtn.gameObject.SetActive(false);
			m_WarrantSelectCaseBtn.gameObject.SetActive(false);
		}
	}

	public void SelectReset()
	{
		print("Reset Select Items");
		// 사건 탭
		for (int i = 0; i < m_CaseItemList.Count; i++)
		{
			m_CaseItemList[i].UnSelect();
		}
		/*
// 사건 디테일
m_CaseIllust.spriteName = "";
m_CaseLabel.text = "";
m_SideCaseLabel.text = "";
*/
		// 대화 탭
		for (int i = 0; i < m_DialogItemList.Count; i++)
		{
			m_DialogItemList[i].UnSelect();
		}

		// 대화 디테일
		for (int i = 0; i < m_DialogDetailItemList.Count; i++)
		{
			m_DialogDetailItemList[i].UnSelect();
		}

		// 증거물 탭
		for (int i = 0; i < m_EvidenceItemList.Count; i++)
		{
			m_EvidenceItemList[i].UnSelect();
		}

		/*// 증거물 디테일
m_EvidenceContent.text = "";
m_EvidenceIllust.spriteName = "";
m_AnalyzedBtn.gameObject.SetActive(false);
m_MatchedBtn.gameObject.SetActive(false);
*/
		// 뉴스 탭
		for (int i = 0; i < m_NewsItemList.Count; i++)
		{
			m_NewsItemList[i].UnSelect();
		}
		/*
// 뉴스 디테일

m_NewsContentLabel.text = "";
m_NewsIllust.spriteName = "";
*/
	}
}
