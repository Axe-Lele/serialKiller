using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoteManager : Singleton<NoteManager>
{
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
		print("zczczxczcz");
		SelectedNoteTab(m_NoteTabIndex);
	}

	public void SetNote()
	{
		// 열었을 때 보일 노트
		int m_DetailIndex = 0;
		if (GameManager.instance.m_NoteMode == NoteMode.None)
		{
			m_NoteTabIndex = m_NoteModeIndex;
			m_DetailIndex = m_NoteModeDetailIndex;
		}
		else if (GameManager.instance.m_NoteMode == NoteMode.Suggest)
		{
			m_NoteTabIndex = m_SuggestModeIndex;
			m_DetailIndex = m_SuggestModeDetailIndex;
		}
		else if (GameManager.instance.m_NoteMode == NoteMode.Warrant)
		{
			m_NoteTabIndex = m_WarrantModeIndex;
			m_DetailIndex = m_WarrantModeDetailIndex;
		}
		else if (GameManager.instance.m_NoteMode == NoteMode.SelectCase)
		{
			m_NoteTabIndex = 0;
			m_DetailIndex = 0;
		}

		// 필요없는 애들은 제외
		if (GameManager.instance.m_NoteMode == NoteMode.SelectCase)
		{
			for (int i = 0; i < m_Btns.Length; i++)
			{
				m_Btns[i].gameObject.SetActive(false);
			}
		}
		else if (GameManager.instance.m_NoteMode == NoteMode.Warrant || GameManager.instance.m_NoteMode == NoteMode.Suggest)
		{
			m_Btns[0].gameObject.SetActive(false);
			for (int i = 1; i < m_Btns.Length; i++)
			{
				m_Btns[i].gameObject.SetActive(true);
			}
		}
		else
		{
			for (int i = 0; i < m_Btns.Length; i++)
			{
				m_Btns[i].gameObject.SetActive(true);
			}
		}

		// 탭 선택
		SelectedNoteTab(m_NoteTabIndex);

		switch (m_NoteTabIndex)
		{
			case (int)NoteTap.Case:
				if (m_CaseItemList.Count > 0)
				{
					SelectReset();
					SelectCase(m_DetailIndex);
					m_CaseItemList[m_DetailIndex].Select();
				}
				break;

			case (int)NoteTap.Dialog:
				if (m_DialogItemList.Count > 0)
				{
					SelectReset();
					SelectDialog(m_DetailIndex);
					m_DialogItemList[m_DetailIndex].Select();

				}
				break;

			case (int)NoteTap.Evidence:
				if (m_EvidenceItemList.Count > 0)
				{
					SelectReset();
					ChangeEvidenceInfoUI(m_DetailIndex);
					m_EvidenceItemList[m_DetailIndex].Select();
				}
				break;

			case (int)NoteTap.News:
				if (m_NewsItemList.Count > 0)
				{
					SelectReset();
					SelectNews(m_DetailIndex);
					m_NewsItemList[m_DetailIndex].Select();
				}
				break;
		}
	}

	public void SelectedNoteTab(int index)
	{
		m_NoteTabIndex = index;
		for (int i = 0; i < m_BG.Length; i++)
		{
			if (i == index)
			{
				m_Btns[i].GetComponent<NoteItem>().ActiveSprite();
				m_BG[i].SetActive(true);
				m_Tabs[i].SetActive(true);
			}
			else
			{
				m_Btns[i].GetComponent<NoteItem>().UnactiveSrptie();
				m_BG[i].SetActive(false);
				m_Tabs[i].SetActive(false);
			}
		}

		switch (GameManager.instance.m_NoteMode)
		{
			case NoteMode.None:
				m_NoteModeIndex = index;
				break;
			case NoteMode.Suggest:
				m_SuggestModeIndex = index;
				break;
			case NoteMode.Warrant:
				m_WarrantModeIndex = index;
				break;
		}

		switch (index)
		{
			case (int)NoteTap.Case:
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

			case (int)NoteTap.Dialog:
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

			case (int)NoteTap.Evidence:
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

			case (int)NoteTap.News:
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
			if (m_CaseItemList[i].m_CaseIndex.CompareTo(caseIndex) == 0)
			{
				_rvalue = i;
				break;
			}
		}
		return _rvalue;
	}

	public int InputCase(CaseMode mode, string index)
	{
		CaseItemInNote item = Instantiate(m_OriginalCaseItem);
		item.Setting(m_CaseItemList.Count, index);
		item.transform.parent = m_CaseTable.transform;
		item.transform.localScale = Vector3.one;
		m_CaseItemList.Add(item);
		item.gameObject.SetActive(true);

		m_CaseTable.enabled = true;
		m_CaseScrollBar.value = 0f;

		return m_CaseItemList.Count - 1;
	}

	public void SelectCaseItem(CaseMode mode, string caseIndex)
	{

	}

	public void SelectCase(CaseMode mode, string caseIndex)
	{
		for (int i = 0; i < m_CaseItemList.Count; i++)
		{
			m_CaseItemList[i].UnSelect();
		}

		string s = "Case_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + caseIndex;

		// 사건 라벨 변경
		switch (mode)
		{
			case CaseMode.Main:
				m_CaseMainInfoLabel.text = Localization.Get(s);
				if (m_CaseIllust.gameObject.activeInHierarchy == false)
					m_CaseIllust.gameObject.SetActive(true);

				m_CaseTab[(int)CaseMode.Main].gameObject.SetActive(true);
				m_CaseTab[(int)CaseMode.Side].gameObject.SetActive(false);
				break;

			case CaseMode.Side:
				m_CaseSideInfoLabel.text = Localization.Get(s);
				m_CaseTab[(int)CaseMode.Main].gameObject.SetActive(false);
				m_CaseTab[(int)CaseMode.Side].gameObject.SetActive(true);
				break;

			default:
				break;
		}

		int _caseItemIndex = GetCaseItemIndex(caseIndex);
		if (_caseItemIndex != -1)
			m_CaseItemList[_caseItemIndex].Selected();
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
			m_CaseItemList[i].m_BG.spriteName = "button_character_off";
		}
		m_CaseItemList[itemIndex].m_BG.spriteName = "button_character_on";

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

	public void InputDialog(string target)
	{
		bool b = false;
		for (int i = 0; i < m_DialogItemList.Count; i++)
		{
			if (m_DialogItemList[i].ReturnIndex() == target)
			{
				b = true;
				break;
			}
		}

		if (b == false)
		{
			DialogItem item = Instantiate(m_OriginalDialogItem);
			item.Setting(target);
			item.transform.parent = m_DialogTable.transform;
			item.transform.localScale = Vector3.one;
			m_DialogItemList.Add(item);
			item.gameObject.SetActive(true);

			item = null;
		}
		m_DialogTable.enabled = true;
		m_DialogScrollBar.value = 0f;
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

		int c = DialogDataManager.instance.ReturnCharacterIndex(target);
		int listcount = DialogDataManager.instance.m_DialogDataItemList[c].m_DialogIndex.Count;

		print("c : " + c + " / lc : " + listcount);
		for (int i = 0; i < m_DialogItemList.Count; i++)
		{
			m_DialogItemList[i].UnSelect();
		}

		for (int i = 0; i < m_DialogDetailItemList.Count; i++)
		{
			m_DialogDetailItemList[i].gameObject.SetActive(false);
		}

		if (m_DialogDetailItemList.Count >= listcount)
		{
			for (int i = 0; i < listcount; i++)
			{
				m_DialogDetailItemList[i].Setting(target, DialogDataManager.instance.m_DialogDataItemList[c].m_DialogIndex[i]);
				m_DialogDetailItemList[i].gameObject.SetActive(true);
			}
		}
		else
		{
			for (int i = 0; i < m_DialogDetailItemList.Count; i++)
			{
				m_DialogDetailItemList[i].Setting(target, DialogDataManager.instance.m_DialogDataItemList[c].m_DialogIndex[i]);
				m_DialogDetailItemList[i].gameObject.SetActive(true);
			}

			for (int i = m_DialogDetailItemList.Count; i < listcount; i++)
			{
				DialogDetailItem item = Instantiate(m_OriginalDialogDetailItem);
				item.Setting(target, DialogDataManager.instance.m_DialogDataItemList[c].m_DialogIndex[i]);
				item.transform.parent = m_DialogDetailTable.transform;
				item.transform.localScale = Vector3.one;
				m_DialogDetailItemList.Add(item);
				item.gameObject.SetActive(true);
			}

			m_DialogDetailTable.enabled = true;
			m_DialogDetailScrollBar.value = 0f;
		}

		int index = 0;
		for (int i = 0; i < m_DialogItemList.Count; i++)
		{
			if (m_DialogItemList[i].ReturnIndex() == target)
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
		m_DialogDetailScrollView.ResetPosition();
	}

	public void InputEvidence(EvidenceDataItem i)
	{
		EvidenceItem item = Instantiate(m_OriginalEvidenceItem);
		item.Init(i);
		item.transform.parent = m_EvidenceTable.transform;
		item.transform.localScale = Vector3.one;
		m_EvidenceItemList.Add(item);
		item.gameObject.SetActive(true);

		m_EvidenceTable.enabled = true;
		m_EvidenceScrollBar.value = 0f;
	}

	public void RemoveEvidence(string index)
	{
		print("remove item : " + index);
		for (int i = 0; i < m_EvidenceItemList.Count; i++)
		{
			print("i : " + i + " / name : " + m_EvidenceItemList[i].ReturnItem().m_EvidenceName);
			if (m_EvidenceItemList[i].ReturnItem().m_EvidenceName == index)
			{
				Destroy(m_EvidenceItemList[i].gameObject);
				m_EvidenceItemList.RemoveAt(i);
				break;
			}
		}
	}

	public void SelectedEvidenceItem(int i)
	{

	}

	public void SelectedEvidenceItem(string index)
	{

	}

	public void SwapEvidence(string index, EvidenceDataItem item)
	{
		for (int i = 0; i < m_EvidenceItemList.Count; i++)
		{
			if (m_EvidenceItemList[i].name == index)
			{
				m_EvidenceItemList[i].Init(item);
				break;
			}
		}
	}

	public void ChangeEvidenceInfoUI(int i)
	{
		ChangeEvidenceInfoUI(m_EvidenceItemList[i].ReturnItem());
	}

	public void ChangeEvidenceInfoUI(EvidenceDataItem item)
	{
		for (int i = 0; i < m_EvidenceItemList.Count; i++)
		{
			m_EvidenceItemList[i].UnSelect();
			if (m_EvidenceItemList[i].m_EveidenceName.Equals(item.m_EvidenceName))
			{
				m_EvidenceItemList[i].ActivateButton();
				print("SelectEvidence : " + item.m_EvidenceName);
			}
		}

		string title = Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + item.m_EvidenceName + "_Title");
		string content = Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + item.m_EvidenceName + "_Content");
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

				m_AnalyzedTimeLabel.text = string.Format(Localization.Get("System_Text_Note_Evidence_RemainTime"), EvidenceDataManager.instance.ReturnCoolTime(item.m_EvidenceName, "AnalyzedTime"));
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

				m_MatchedTimeLabel.text = string.Format(Localization.Get("System_Text_Note_Evidence_RemainTime"), EvidenceDataManager.instance.ReturnCoolTime(item.m_EvidenceName, "MatchedTime"));
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
				if (item.m_Alter == true)
				{
					content = Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + item.m_EvidenceName + "_Content_Alter");
				}
				else
				{
					content += Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + item.m_EvidenceName + "_Add");
				}

				print("이미 분석이 끝났다.");
			}
		}

		//m_EvidenceTitle.text = Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + item.m_EvidenceName + "_Title");
		m_EvidenceDataItem = item;
		m_EvidenceIllust.spriteName = "Evidence_" + PlayDataManager.instance.m_StageName + "_" + item.m_EvidenceName;
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
	}

	public void EvidenceAnalyzed()
	{
		InGameUIManager.instance.ControlNotePopup();
		InGameUIManager.instance.ControlLaboratoryPanel();
		LaboratoryManager.instance.ShowAnalyzedUI();
		LaboratoryManager.instance.SetAnalyzedItem(m_EvidenceDataItem.m_EvidenceName);


		print("EvidenceAnalyzed");
	}

	public void EvidenceMatched()
	{
		InGameUIManager.instance.ControlNotePopup();
		InGameUIManager.instance.ControlLaboratoryPanel();
		LaboratoryManager.instance.SelectMatchedPopup();
		LaboratoryManager.instance.InputMatchedItem(m_EvidenceDataItem.m_EvidenceName);

		print("EvidenceMatched");
	}



	public void AddReadNewsList(int date, string index)
	{
		print("date : " + date + " / index : " + index);

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
		go.Setting(date, index, m_NewsItemList.Count, 1);
		m_NewsItemList.Add(go);

		m_NewsTable.enabled = true;
		m_NewsScrollBar.value = 0f;
		//    m_NewsScrollView.ResetPosition();

	}

	public void SelectNews(int index)
	{
		print("Select News index : " + index);
		//m_NewsIllust.spriteName = "News_Illust_" + m_NewsItemList[index].m_NewsIndex;
		m_NewsTitleLabel.text = Localization.Get("News_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + m_NewsItemList[index].m_NewsIndex + "_Title");
		m_NewsContentLabel.text = Localization.Get("News_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + m_NewsItemList[index].m_NewsIndex + "_Content");
		if (m_NewsIllust.gameObject.activeInHierarchy == false)
		{
			m_NewsIllust.gameObject.SetActive(true);
		}

		for (int i = 0; i < m_NewsItemList.Count; i++)
		{
			m_NewsItemList[index].SeletedSprite.spriteName = "button_character_off";
		}
		m_NewsItemList[index].SeletedSprite.spriteName = "button_character_on";

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
