using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyzedManager : MonoBehaviour
{
	public UIGrid m_ItemGrid;
	public UIScrollView m_ItemSCV;
	public LaboratoryItem m_ItemOrigin;
	public LaboratoryItem m_DescItem;
	public UILabel m_RemainTimeLabel;
	public LaboratoryItem m_NowSelectedItem;

	public GameObject m_HasNoItemPanel;

	public LaboratoryItem[] m_ReserveItemList;
	public List<LaboratoryItem> m_ItemList;

	public GameObject m_ParentTr;
	public GameObject m_DescParent;
	public GameObject m_SelectedMessage;
	public GameObject m_ItemListParentTr;
	public UILabel m_AlarmEmptyLabel;
	public Transform m_CheckerTr;

	public UILabel m_AnalzyzedIconLabel;
	public UISprite m_AnalyzedIcon, m_AnalyzedButtonIcon;
	private readonly static string m_IconAnalysis = "analysis";
	private readonly static string m_IconAnalyzing = "analysis_check";
	private readonly static string m_IconWaiting = "analysis_waiting";
	private readonly static string m_IconDisable = "analysis_disable";

	public void SetActivePanel(bool isOpen)
	{
		if (m_ItemList.Count == 0)
		{
			m_HasNoItemPanel.SetActive(isOpen);
		}
		else
		{
			m_HasNoItemPanel.SetActive(false);
		}

		m_ParentTr.SetActive(isOpen);
		m_ItemListParentTr.SetActive(isOpen);

		for (int i = 0; i < m_ReserveItemList.Length; i++)
		{
			if (LaboratoryDataManager.ReturnAnalyzedIIndexList(i) != string.Empty)
				continue;

			m_ReserveItemList[i].Reset();
		}

		bool isShowDesc = !(LaboratoryDataManager.ReturnAnalyzedIIndexList(0) == string.Empty);

		if (m_ItemList.Count == 0)
		{
			m_AlarmEmptyLabel.gameObject.SetActive(true);
			m_DescParent.SetActive(false);
			m_SelectedMessage.SetActive(false);
		}
		else
		{
			m_AlarmEmptyLabel.gameObject.SetActive(false);
			m_DescParent.SetActive(isShowDesc);
			m_SelectedMessage.SetActive(!isShowDesc);
			if (isShowDesc)
			{
				SelectedItem(m_ReserveItemList[0]);
				ShowAnalyedIcon(m_ReserveItemList[0].GetItemCode());
			}
		}

		this.RepositionItems();
	}

	public void AddItem(string itemCode)
	{
		LaboratoryItem item = Instantiate<LaboratoryItem>(m_ItemOrigin);
		item.Setting(itemCode);
		item.transform.parent = m_ItemGrid.transform;
		item.transform.localScale = Vector3.one;

		m_ItemList.Add(item);
		item.gameObject.SetActive(true);

		item = null;
		this.RepositionItems();
	}

	public void RemoveItem(string itemCode)
	{
		for (int i = 0; i < m_ItemList.Count; i++)
		{
			if (LaboratoryDataManager.ReturnAnalyzedIIndexList(i) != itemCode)
				continue;

			string name = GetItemName(itemCode);
			string s = string.Format(Localization.Get("System_Text_StartAnalzed"), name);
			SystemTextManager.instance.InputText(s);

			m_ItemList.RemoveAt(i);
			break;
		}
	}

	public void ShowAnalyedIcon(string itemCode)
	{
		m_AnalyzedIcon.gameObject.SetActive(true);
		m_AnalyzedButtonIcon.spriteName = m_IconDisable;

		if (LaboratoryDataManager.ReturnAnalyzedIIndexList(0) == itemCode)
		{
			m_AnalyzedIcon.spriteName = m_IconAnalyzing;
			m_AnalzyzedIconLabel.text = "분석 중 ";
		}
		else if (LaboratoryDataManager.ContainAnalyzedIndexList(itemCode))
		{
			m_AnalyzedIcon.spriteName = m_IconWaiting;
			m_AnalzyzedIconLabel.text = "분석대기";
		}
		else
		{
			m_AnalyzedIcon.spriteName = m_IconAnalysis;
			m_AnalzyzedIconLabel.text = "분석가능";

			m_AnalyzedButtonIcon.spriteName = m_IconAnalysis;
		}
	}

    public void SelectedItem(string itemCode)
    {
        for(int i = 0; i < m_ItemList.Count; i++)
        {
            if(m_ItemList[i].GetItemCode() == itemCode)
            {
                SelectedItem(m_ItemList[i]);
            }
        }
    }

	public void SelectedItem(LaboratoryItem selectedItem)
	{
		m_NowSelectedItem = selectedItem;
		m_DescItem.ShowInfomation(selectedItem);

		m_DescParent.SetActive(true);
		m_SelectedMessage.SetActive(false);

		for (int i = 0; i < m_ItemList.Count; i++)
		{
			if (m_ItemList[i].GetItemCode() == selectedItem.GetItemCode())
			{
				m_ItemList[i].Selected(true);
				m_CheckerTr.SetParent(m_ItemList[i].transform);
				m_CheckerTr.localPosition = Vector3.zero;
				m_CheckerTr.gameObject.SetActive(true);
			}
			else
			{
				m_ItemList[i].Selected(false);
			}
		}

		ShowAnalyedIcon(selectedItem.GetItemCode());
	}

	public void ShowInfomation(LaboratoryItem targetItem)
	{
		m_DescItem.ShowInfomation(targetItem);
	}


	// 분석하기를 눌렀을 때 호출됨
	public void OnClickedAnalyzed()
	{
		ReadyAnalyzed(m_NowSelectedItem.GetItemCode());
	}

	public void ReadyAnalyzed(string itemCode)
	{
		if (LaboratoryDataManager.ReturnAnalyzedIIndexList(0) == string.Empty)
		{
			StartAnalyzed(itemCode);
		}
		else
		{
			AddReserveItem(itemCode);
		}

		ShowAnalyedIcon(itemCode);
	}

	private string StageName
	{
		get
		{
			if (PlayDataManager.instance == null)
			{
				print("!! PlayDataManager is NULL !!");
				return string.Empty;
			}
			return PlayDataManager.instance.m_StageName;
		}
	}
	private string CriminalCode
	{
		get
		{
			if (StageDataManager.instance == null)
			{
				print("!! StageDataManager is NULL !!");
				return string.Empty;
			}
			return StageDataManager.instance.m_CriminalCode.ToString();
		}
	}
	private LaboratoryDataManager LaboratoryDataManager
	{
		get
		{
			if (LaboratoryDataManager.instance == null)
			{
				print("!! LaboratoryDataManager is NULL !!");
				return null;
			}
			return LaboratoryDataManager.instance;
		}
	}

	private readonly static string ItemNameTemplet = "Evidence_{0}_{1}_{2}_Title";
	private readonly static string ItemDescTemplet = "Evidence_{0}_{1}_{2}_Content";
	private readonly static string ItemExtraDescTemplet = "Evidence_{0}_{1}_{2}_Add";

	private string m_AnalyzedItemName = string.Empty;
	public void StartAnalyzed(string itemCode)
	{
		m_AnalyzedItemName = GetItemName(itemCode);
		string s = string.Format(Localization.Get("System_Text_StartAnalzed"), m_AnalyzedItemName);
		SystemTextManager.instance.InputText(s);

		LaboratoryDataManager.SetAnalyzedIndexList(0, itemCode);
		m_ReserveItemList[0].Setting(itemCode);
		LaboratoryDataManager.m_AnalyzedRemainTime
			= EvidenceDataManager.instance.ReturnCoolTime(itemCode, "AnalyzedTime");

		int h = LaboratoryDataManager.m_AnalyzedRemainTime / 60;
		int m = LaboratoryDataManager.m_AnalyzedRemainTime % 60;
		string str = string.Empty;

		if (m == 0)
		{
			str = string.Format(Localization.Get("Analzed_RemainHour"), h);
		}
		else
		{
			str = string.Format(Localization.Get("Analzed_RemainHourMinute"), h, m);
		}

		// 분석 남은 시간
		m_RemainTimeLabel.text = str;
	}

	public void AddReserveItem(string itemCode)
	{
		bool b = false;
		for (int i = 1; i < m_ReserveItemList.Length; i++)
		{
			if (b == true)
				break;

			if (LaboratoryDataManager.ReturnAnalyzedIIndexList(i) == itemCode)
			{
				b = true;
				break;
			}

			if (LaboratoryDataManager.ReturnAnalyzedIIndexList(i) == string.Empty)
			{
				b = true;

				LaboratoryDataManager.SetAnalyzedIndexList(i, itemCode);
				m_ReserveItemList[i].Setting(itemCode);

				string name = GetItemName(itemCode);
				string s = string.Format(Localization.Get("System_Text_AddAnalzedReservation"), name);
				SystemTextManager.instance.InputText(s);

				m_ReserveItemList[i].gameObject.SetActive(true);
			}
		}
	}

	public void RemoveReserveItem(int k)
	{
		LaboratoryDataManager.SetAnalyzedIndexList(k, string.Empty);

		for (int i = 0; i < m_ReserveItemList.Length; i++)
		{
			if (m_ReserveItemList.Length - 1 <= i)
				continue;

			if (LaboratoryDataManager.ReturnAnalyzedIIndexList(i) == string.Empty)
			{
				LaboratoryDataManager.SetAnalyzedIndexList(i, LaboratoryDataManager.ReturnAnalyzedIIndexList(i + 1));
				LaboratoryDataManager.SetAnalyzedIndexList(i + 1, string.Empty);
				m_ReserveItemList[i + 1].Reset();
			}
		}

		if (k == 0
			&& LaboratoryDataManager.ReturnAnalyzedIIndexList(k) != "")
		{
			StartAnalyzed(LaboratoryDataManager.ReturnAnalyzedIIndexList(k));
		}
	}

	public void CheckAnalyzed(int time)
	{
		print("CheckAnalyzed : " + time + ", index : " + LaboratoryDataManager.instance.ReturnAnalyzedIIndexList(0));

		if (LaboratoryDataManager.instance.ReturnAnalyzedIIndexList(0) != "")
		{
			// 경과한 시간이 남은 연구 시간보다 크거나 같을 경우
			print("time : " + time + " / " + LaboratoryDataManager.instance.m_AnalyzedRemainTime);
			if (time >= LaboratoryDataManager.instance.m_AnalyzedRemainTime)
			{
				int remaintime = time - LaboratoryDataManager.instance.m_AnalyzedRemainTime;
				LaboratoryDataManager.instance.m_AnalyzedRemainTime = 0;

				string item = LaboratoryDataManager.instance.ReturnAnalyzedIIndexList(0);
				print("item : " + item + " / remainTime : " + remaintime +"/ Alter : " 
					+ EvidenceDataManager.instance.ReturnEvidenceItem(item).m_IsChanged);
				if (item == null || item == string.Empty)
					return;

				string str = string.Format(Localization.Get("System_Text_CompleteAnalzed"), m_AnalyzedItemName);
				SystemTextManager.instance.InputText(str);

				EvidenceDataItem data = EvidenceDataManager.instance.ReturnEvidenceItem(item);
				if (data.m_IsChanged == true)
				{
					print("IsSwaped : " + data.m_IsChanged);
					EventManager.instance.SetEventTemp("SwapEvidence-" + data.m_ItemCode + "_0");
				}
				else
				{
					EvidenceDataManager.instance.CompleteAnalyzed(LaboratoryDataManager.instance.ReturnAnalyzedIIndexList(0), true);
				}

				for (int i = 0; i < m_ItemList.Count; i++)
				{
					if (m_ItemList[i].GetItemCode() == item)
					{
						m_ItemList[i].gameObject.SetActive(false);
						m_ItemList.RemoveAt(i);
						break;
					}
				}

				m_ReserveItemList[0].Reset();

				RemoveReserveItem(0);
				CheckAnalyzed(remaintime);
			}
			else
			{
				LaboratoryDataManager.instance.m_AnalyzedRemainTime -= time;
				int h = LaboratoryDataManager.instance.m_AnalyzedRemainTime / 60;
				int m = LaboratoryDataManager.instance.m_AnalyzedRemainTime % 60;
				string str = "";
				if (m == 0)
				{
					str = string.Format(Localization.Get("Analzed_RemainHour"), h);
				}
				else
				{
					str = string.Format(Localization.Get("Analzed_RemainHourMinute"), h, m);
				}

				// 분석 남은 시간
				//m_RemainTimeLabel.text = str;
				m_RemainTimeLabel.text = string.Format("{0:00}:{1:00}", h, m);
			}
			#region Unknown Data
			/*
			LaboratoryDataManager.instance.m_AnalyzedRemainTime -= time;

			if (LaboratoryDataManager.instance.m_AnalyzedRemainTime > 0)
			{
					int h = LaboratoryDataManager.instance.m_AnalyzedRemainTime / 60;
					int m = LaboratoryDataManager.instance.m_AnalyzedRemainTime % 60;
					string str = "";
					if (m == 0)
					{
							str = string.Format(Localization.Get("Analzed_RemainHour"), h);
					}
					else
					{
							str = string.Format(Localization.Get("Analzed_RemainHourMinute"),h,m);
					}
					m_AnalyzedRemainTimeLabel.text = str;
					print("ttttttttttt ");
			}
			else
			{
					print("aaaaaaa");
					string item = LaboratoryDataManager.instance.ReturnAnalyzedIIndexList(0);
					print("item : " + item);
					if (EvidenceDataManager.instance.ReturnEvidenceItem(item).m_Alter == true)
					{
						 // EvidenceDataManager.instance.SwapEvidence(EvidenceDataManager.instance.ReturnEvidenceItem(item).m_EvidenceName,EvidenceDataManager.instance.ReturnEvidenceItem(item).m_AlterName);
							EvidenceDataManager.instance.InputEvidence(EvidenceDataManager.instance.ReturnEvidenceItem(item).m_AlterName);
							EvidenceDataManager.instance.RemoveEvidence(item);
					}
					else
					{
							EvidenceDataManager.instance.CompleteAnalyzed(LaboratoryDataManager.instance.ReturnAnalyzedIIndexList(0), true);
					}

					for (int i = 0; i < m_AnalyzedItemList.Count; i++)
					{
							if (m_AnalyzedItemList[i].ReturnItemCode() == item)
							{
									Destroy(m_AnalyzedItemList[i].gameObject);
									m_AnalyzedItemList.RemoveAt(i);
									break;
							}
					}



					string str = string.Format(Localization.Get("System_Text_CompleteAnalzed"), m_AnalyzedItemName);
					SystemTextManager.instance.InputText(str);

					m_AnalyzedItemReservationList[0].Reset();

					RemoveAnalyzedReservationItemList(0);
			}
			*/

			#endregion
		}
	}

	public string GetItemName(string itemCode)
	{
		return Localization.Get(string.Format(ItemNameTemplet, StageName, CriminalCode, itemCode));
	}

	public void RepositionItems()
	{
		m_ItemGrid.Reposition();
		m_ItemSCV.ResetPosition();
	}
}
