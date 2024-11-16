using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchedManager : MonoBehaviour
{
	public UIGrid m_ItemGrid;
	public UIScrollView m_ItemSCV;
	public LaboratoryItem m_ItemOrigin;
	public LaboratoryItem[] m_DescItem;
	public UILabel m_RemainTimeLabel;
	public UILabel m_ResultLabel;
	public LaboratoryItem[] m_NowSelectedItem;

	public GameObject m_HasNoItemPanel;
	public GameObject m_MatchingAlarm;

	public List<LaboratoryItem> m_ItemList;

	public GameObject[] m_SelectedItemMessages;

	public GameObject[] m_DescParent;
	public Transform[] m_CheckerTr;

	public UISprite m_MatchButtonIcon;
	public BoxCollider m_MatchButtonCol;

	public BoxCollider m_HardCoverCol;

	public bool m_IsMatchedResult = false;


	private void Awake()
	{
		m_NowSelectedItem = new LaboratoryItem[2];
		m_ItemList = new List<LaboratoryItem>();
	}

	public void RepositionItems()
	{
		m_ItemGrid.Reposition();
		m_ItemSCV.ResetPosition();
	}

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

		if (isOpen == false)
			return;

		// !! 매치 결과가 있을 경우 !!
		if (m_IsMatchedResult)
		{
			//left
			m_MatchingAlarm.SetActive(false);
			m_ItemGrid.gameObject.SetActive(true);

			//right
			m_DescParent[0].SetActive(true);
			m_DescParent[1].SetActive(true);
			m_DescItem[0].ShowInfomation(m_NowSelectedItem[0]);
			m_DescItem[1].ShowInfomation(m_NowSelectedItem[1]);
			m_SelectedItemMessages[0].SetActive(false);
			m_SelectedItemMessages[1].SetActive(false);

			CheckerOnItem(0, m_NowSelectedItem[0]);
			CheckerOnItem(1, m_NowSelectedItem[1]);

			m_IsMatchedResult = false;

			m_MatchButtonIcon.spriteName = "compare_disable";
			m_MatchButtonCol.enabled = false;
		}
		else
		{
			if (LaboratoryDataManager.instance.m_IsStartMatched == true)
			{
				//left
				m_MatchingAlarm.SetActive(true);
				m_ItemGrid.gameObject.SetActive(false);

				//right
				m_DescParent[0].SetActive(true);
				m_DescParent[1].SetActive(true);
				m_SelectedItemMessages[0].SetActive(false);
				m_SelectedItemMessages[1].SetActive(false);
			}
			else
			{
				//left
				m_MatchingAlarm.SetActive(false);
				m_ItemGrid.gameObject.SetActive(true);

				//right
				m_DescItem[0].Reset();
				m_DescItem[1].Reset();
				m_DescParent[0].SetActive(false);
				m_DescParent[1].SetActive(false);
				m_SelectedItemMessages[0].SetActive(true);
				m_SelectedItemMessages[1].SetActive(true);
				m_ResultLabel.text = string.Empty;
			}

			CheckerOnItem(0, null);
			CheckerOnItem(1, null);
		}

		this.RepositionItems();
	}

	private void CheckerOnItem(int idx, LaboratoryItem target)
	{
		if (target == null)
		{
			m_CheckerTr[idx].gameObject.SetActive(false);
		}
		else
		{
			m_CheckerTr[idx].SetParent(target.transform);
			m_CheckerTr[idx].localPosition = Vector3.zero;
			m_CheckerTr[idx].gameObject.SetActive(true);
		}
	}

	public void AddItem(string itemCode)
	{
		LaboratoryItem item = Instantiate(m_ItemOrigin);
		item.Setting(itemCode);
		item.transform.parent = m_ItemGrid.transform;
		item.transform.localScale = Vector3.one;

		m_ItemList.Add(item);
		item.gameObject.SetActive(true);
		m_ItemGrid.enabled = true;
		m_ItemSCV.ResetPosition();

		item = null;
	}

	public void RemoveDescItem(int index)
	{

	}

	public void ClearDescItem(int idx)
	{
		if (LaboratoryDataManager.instance.m_IsStartMatched == true)
		{
			// 비교 중입니다 !!
			return;
		}

		m_DescItem[idx].Reset();

		m_NowSelectedItem[idx] = null;
		m_DescParent[idx].SetActive(false);
		m_SelectedItemMessages[idx].SetActive(true);
		CheckerOnItem(idx, null);

		LaboratoryDataManager.instance.SetMatchedIndexList(idx, string.Empty);
	}

	public void SelectedItem(string itemCode)
	{
		ClearDescItem(0);
		ClearDescItem(1);

		for (int i = 0; i < m_ItemList.Count; i++)
		{
			if (m_ItemList[i].GetItemCode() == itemCode)
			{
				SelectedItem(m_ItemList[i]);
			}
		}
	}

	public void SelectedDescItem(LaboratoryItem item)
	{
		int _idx = -1;
		for (int i = 0; i < m_NowSelectedItem.Length; i++)
		{
			if (m_NowSelectedItem[i] == item)
			{
				_idx = i;
				break;
			}
		}

		if (_idx == -1)
		{
			print("!! This is NOT DescItem !!");
			return;
		}
	}


	public void SelectedItem(LaboratoryItem selectedItem)
	{
		int _idx = 0;
		string itemCode = selectedItem.GetItemCode();

		m_MatchButtonCol.enabled = false;
		m_MatchButtonIcon.spriteName = "compare_disable";
		m_ResultLabel.text = string.Empty;

		// 선택된 아이템인지 확인
		for (int i = 0; i < m_DescItem.Length; i++)
		{
			// 이미 선택한 아이템
			if (LaboratoryDataManager.instance.ReturnMatchedIndexList(i) == itemCode)
			{
				ClearDescItem(i);
				return;
			}

			if (m_DescItem[i].GetItemCode() == itemCode)
			{
				ClearDescItem(i);
				return;
			}
		}

		// 비어있는 아이템이 있는지 확인한다.
		for (int i = 0; i < m_DescItem.Length; i++)
		{
			if (LaboratoryDataManager.instance.ReturnMatchedIndexList(i) == string.Empty)
			{
				_idx = i;

				LaboratoryDataManager.instance.SetMatchedIndexList(_idx, itemCode);

				m_NowSelectedItem[i] = selectedItem;
				m_SelectedItemMessages[i].SetActive(false);

				break;
			}

			if (m_DescItem[i].GetItemCode().Length == 0)
			{
				_idx = i;

				LaboratoryDataManager.instance.SetMatchedIndexList(_idx, itemCode);

				m_NowSelectedItem[i] = selectedItem;
				m_SelectedItemMessages[i].SetActive(false);
				break;
			}
		}

		for (int i = 0; i < m_ItemList.Count; i++)
		{
			if (m_ItemList[i].GetItemCode() == selectedItem.GetItemCode())
			{
				m_DescParent[_idx].SetActive(true);
				m_DescItem[_idx].ShowInfomation(selectedItem);

				CheckerOnItem(_idx, m_ItemList[i]);

				break;
			}
		}

		int matchedIndex
			= LaboratoryDataManager.instance.HasMatched(
				m_DescItem[0].GetItemCode(),
				m_DescItem[1].GetItemCode());

		if (matchedIndex >= 0)
		{
			string a = Localization.Get(string.Format("Evidence_Stage0_Main_0_{0}_Title", m_DescItem[0].GetItemCode()));
			string b = Localization.Get(string.Format("Evidence_Stage0_Main_0_{0}_Title", m_DescItem[1].GetItemCode()));

			m_ResultLabel.text = (string.Format(Localization.Get("System_Text_Match_Result_Success"), a, b) + "\n");
			return;
		}

		// Desc에 아이템이 전부 채워질 때, 비교 버튼 활성화
		if (_idx == m_DescItem.Length - 1 && matchedIndex == -1)
		{
			m_MatchButtonCol.enabled = true;
			m_MatchButtonIcon.spriteName = "compare";
		}



		//m_NowSelectedItem = selectedItem;
		//m_DescItem.ShowInfomation(selectedItem);

		//m_DescParent.SetActive(true);
		//m_SelectedMessage.SetActive(false);

		//for (int i = 0; i < m_ItemList.Count; i++)
		//{
		//	if (m_ItemList[i].GetItemCode() == selectedItem.GetItemCode())
		//	{
		//		m_ItemList[i].Selected(true);
		//		m_CheckerTr.SetParent(m_ItemList[i].transform);
		//		m_CheckerTr.localPosition = Vector3.zero;
		//		m_CheckerTr.gameObject.SetActive(true);
		//	}
		//	else
		//	{
		//		m_ItemList[i].Selected(false);
		//	}
		//}

		//ShowAnalyedIcon(selectedItem.GetItemCode());
	}

	public void StartMatching()
	{
		m_MatchButtonIcon.spriteName = "compare_disable";

		m_MatchingAlarm.SetActive(true);
		m_ItemGrid.gameObject.SetActive(false);
	}

	public void InputMatchResult(string a, string b, bool result)
	{
		string str = string.Empty;
		if (m_ResultLabel.text != null)
		{
			str = m_ResultLabel.text;
		}

		a = Localization.Get(string.Format("Evidence_Stage0_Main_0_{0}_Title", a));
		b = Localization.Get(string.Format("Evidence_Stage0_Main_0_{0}_Title", b));

		if (result == true)
			str += (string.Format(Localization.Get("System_Text_Match_Result_Success"), a, b) + "\n");
		else
			str += (string.Format(Localization.Get("System_Text_Match_Result_Fail"), a, b) + "\n");

		m_ResultLabel.text = str;
		m_IsMatchedResult = true;

		m_MatchButtonIcon.spriteName = "compare";

		m_HardCoverCol.enabled = true;

		//LaboratoryDataManager.instance.
	}

	public void ClearAllDesc()
	{
		m_NowSelectedItem[0] = null;
		m_NowSelectedItem[1] = null;
		this.ClearDescItem(0);
		this.ClearDescItem(1);
		m_ResultLabel.text = string.Empty;

		m_HardCoverCol.enabled = false;
	}
}
