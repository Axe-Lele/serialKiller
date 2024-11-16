using UnityEngine;
using System.Collections.Generic;

public class NoteEvidenceManager : MonoBehaviour
{
	public GameObject m_Root;
	public GameObject m_HasNoItem;
	public GameObject m_SelectedMsg;

	[Space]
	public EvidenceItem m_OriginalEvidenceItem;
	public GameObject m_EvidenceItemChecker;
	public GameObject m_EvidenceItemRoot;

	[Space]
	public UIGrid m_EvidenceGrid;
	public List<EvidenceItem> m_EvidenceItemList;
	public UIScrollView m_EvidenceSCV;
	public UIScrollView m_EvidenceDetailSCV;

	[Space]
	public UIButton m_AnalyzedBtn;
	public UISprite m_AnalyzedIcon;
	public BoxCollider m_AnalyzedCol;
	public UILabel m_AnalyzedLabel, m_AnalyzedTimeLabel;

	[Space]
	public UISprite m_ItemStateIcon;
	public UILabel m_ITemStateLabel;

	[Space]
	public UIButton m_MatchedBtn;
	public UISprite m_MatchedIcon;
	public BoxCollider m_MatchedCol;
	public UILabel m_MatchedLabel, m_MatchedTimeLabel;

	[Space]
	public EvidenceDataItem m_EvidenceDataItem;
	public GameObject m_DescRoot;
	public UISprite m_EvidenceIllust;
	public UILabel m_EvidenceTitle;
	public UILabel m_EvidenceContent;

	[Space]
	public int m_NoteModeDetailIndex;
	public int m_SuggestModeDetailIndex;
	public int m_WarrantModeDetailIndex;

	[Header("Suggest")]
	public GameObject m_SuggestButton;

	public void ShowEvidence(bool isOpen)
	{
		print("Show [Note] Evidence UI : " + isOpen);

		m_Root.SetActive(isOpen);
		m_EvidenceItemChecker.SetActive(false);

		m_SuggestButton.SetActive(false);
		switch (GameManager.instance.m_NoteMode)
		{
			case NoteMode.None:
				break;

			case NoteMode.Suggest:
				m_SuggestButton.SetActive(true);
				break;

			case NoteMode.Warrant:
				break;

			case NoteMode.SelectCase:
				break;
			default:
				break;
		}

		if (m_EvidenceItemList.Count == 0)
		{
			m_HasNoItem.SetActive(true);
			m_EvidenceItemRoot.SetActive(false);
			m_SelectedMsg.SetActive(false);
			m_DescRoot.SetActive(false);
		}
		else
		{
			m_HasNoItem.SetActive(false);
			m_EvidenceItemRoot.SetActive(true);
			m_SelectedMsg.SetActive(true);
			m_DescRoot.SetActive(false);
		}

		this.Reposition();
	}

	public void Suggest()
	{
		SuggestManager.instance.Suggest();
	}

	private void Reposition()
	{
		m_EvidenceGrid.Reposition();
		m_EvidenceSCV.ResetPosition();
		m_EvidenceDetailSCV.ResetPosition();
	}

	public void InputEvidence(EvidenceDataItem i)
	{
		EvidenceItem item = Instantiate(m_OriginalEvidenceItem);
		item.Init(i);
		item.transform.parent = m_EvidenceGrid.transform;
		item.transform.localScale = Vector3.one;
		m_EvidenceItemList.Add(item);
		item.gameObject.SetActive(true);

		m_EvidenceGrid.enabled = true;
	}

	public void BannedEvidence(string itemCode)
	{
		print("This item(" + itemCode + ") never get more.");
		for (int i = 0; i < m_EvidenceItemList.Count; i++)
		{
			if (m_EvidenceItemList[i].ReturnItem().m_ItemCode == itemCode)
			{
				m_EvidenceItemList[i].gameObject.SetActive(false);
				m_EvidenceItemList.RemoveAt(i);
				return;
			}
		}
	}

	public void RemoveEvidence(string index)
	{
		print("remove item : " + index);
		for (int i = 0; i < m_EvidenceItemList.Count; i++)
		{
			if (m_EvidenceItemList[i].ReturnItem().m_ItemCode == index)
			{
				m_EvidenceItemList[i].gameObject.SetActive(false);
				break;
			}
		}
	}

	private void MoveChecker(EvidenceItem target)
	{
		if (target == null)
		{
			m_EvidenceItemChecker.SetActive(false);
			return;
		}

		m_EvidenceItemChecker.transform.SetParent(target.transform);
		m_EvidenceItemChecker.transform.localPosition = Vector3.zero;
		m_EvidenceItemChecker.SetActive(true);
	}

	public void ChangeEvidenceInfoUI(EvidenceDataItem item)
	{
		for (int i = 0; i < m_EvidenceItemList.Count; i++)
		{
			m_EvidenceItemList[i].UnSelect();
			if (m_EvidenceItemList[i].m_EveidenceName.Equals(item.m_ItemCode))
			{
				m_EvidenceItemList[i].ActivateButton();
				MoveChecker(m_EvidenceItemList[i]);
				print("SelectEvidence : " + item.m_ItemCode + "(" + item.m_AtlasIndex + ")");
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

			m_ItemStateIcon.gameObject.SetActive(false);
			m_ITemStateLabel.gameObject.SetActive(false);

			int idx = LaboratoryManager.instance.ContainAnalyzedList(item.m_ItemCode);
			if (idx != -1)
			{
				m_ItemStateIcon.gameObject.SetActive(true);
				if (idx == 0)
				{
					m_ItemStateIcon.spriteName = "analysis_check";
				}
				else
				{
					m_ItemStateIcon.spriteName = "analysis_waiting";
				}
			}

			if (LaboratoryManager.instance.ContainMatchedList(item.m_ItemCode))
			{
				m_ItemStateIcon.gameObject.SetActive(true);
				m_ItemStateIcon.spriteName = "compare_check";
			}

			if (item.m_IsAnalyzed == true && item.m_ResultAnalyzed == false)
			{
				// 분석 가능
				//m_AnalyzedLabel.text = Localization.Get("System_Text_Note_Analyzed_Possible");
				m_AnalyzedLabel.color = Color.white;
				m_AnalyzedIcon.spriteName = "analysis";

				m_AnalyzedTimeLabel.text = string.Format(Localization.Get("System_Text_Note_Evidence_RemainTime"), EvidenceDataManager.instance.ReturnCoolTime(item.m_ItemCode, "AnalyzedTime"));
			}
			else if (item.m_IsAnalyzed == true && item.m_ResultAnalyzed == true)
			{
				// 분석 끝
				//m_AnalyzedLabel.text = Localization.Get("System_Text_Note_Analyzed_Complete");
				m_AnalyzedLabel.color = Color.gray;
				m_AnalyzedIcon.spriteName = "analysis_disable";

				m_AnalyzedTimeLabel.text = "";
			}
			else
			{
				// 분석 불가능
				//m_AnalyzedLabel.text = Localization.Get("System_Text_Note_Analyzed_Impossible");
				m_AnalyzedLabel.color = Color.gray;
				m_AnalyzedTimeLabel.text = "";
				m_AnalyzedIcon.spriteName = "analysis_disable";

				m_AnalyzedCol.enabled = false;
			}

			if (item.m_IsMathced)
			{
				// 비교 가능
				//m_MatchedLabel.text = Localization.Get("System_Text_Note_Matched_Possible");
				m_MatchedLabel.color = Color.white;
				m_MatchedIcon.spriteName = "compare";

				m_MatchedTimeLabel.text = string.Format(Localization.Get("System_Text_Note_Evidence_RemainTime"), EvidenceDataManager.instance.ReturnCoolTime(item.m_ItemCode, "MatchedTime"));
			}
			else
			{
				// 비교 불가능
				//m_MatchedLabel.text = Localization.Get("System_Text_Note_Matched_Impossible");
				m_MatchedLabel.color = Color.gray;
				m_MatchedTimeLabel.text = "";
				m_MatchedIcon.spriteName = "compare_disable";

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
		EvidenceDataManager.instance.SetSprite(ref m_EvidenceIllust,
											   AtlasManager.SpriteType.Evidence.ToString(),
											   item.m_ItemCode);

		m_EvidenceTitle.text = title;
		m_EvidenceContent.text = content;

		m_SelectedMsg.SetActive(false);
		m_DescRoot.SetActive(true);

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
		m_EvidenceItemChecker.transform.SetParent(m_EvidenceItemRoot.transform);
		m_EvidenceItemChecker.SetActive(false);

		InGameUIManager.instance.ControlNotePanel();
		InGameUIManager.instance.ControlLaboratoryPanel();
		LaboratoryManager.instance.ShowAnalyzedUI();
		LaboratoryManager.instance.SelectedItem(m_EvidenceDataItem.m_ItemCode);

		print("EvidenceAnalyzed");
	}

	public void EvidenceMatched()
	{
		m_EvidenceItemChecker.transform.SetParent(m_EvidenceItemRoot.transform);
		m_EvidenceItemChecker.SetActive(false);

		InGameUIManager.instance.ControlNotePanel();
		InGameUIManager.instance.ControlLaboratoryPanel();
		LaboratoryManager.instance.SelectMatchedPopup();
		LaboratoryManager.instance.InputMatchedItem(m_EvidenceDataItem.m_ItemCode);

		print("EvidenceMatched");
	}
}
