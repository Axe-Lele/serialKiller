using UnityEngine;
using System.Collections;

public class MatchedManager : MonoBehaviour
{
	public LaboratoryItem[] m_NowSelectedItem;
	public LaboratoryItem[] m_DescItem;
	public GameObject[] m_DisableItem;

	public Transform[] m_CheckerTr;

	private void Awake()
	{
		m_NowSelectedItem = new LaboratoryItem[2];
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

		if(_idx == -1)
		{
			print("!! This is NOT DescItem !!");
			return;
		}

		
	}

	public void SelectedItem(LaboratoryItem selectedItem)
	{
		int _idx = 0;
		for (int i = 0; i < m_NowSelectedItem.Length; i++)
		{
			if (m_NowSelectedItem[i] == null)
			{
				_idx = i;
				break;
			}

			// 이미 선택한 아이템
			if (m_NowSelectedItem[i] == selectedItem)
			{
				m_NowSelectedItem[i] = null;
				m_CheckerTr[i].gameObject.SetActive(false);
				m_DisableItem[i].SetActive(true);

				return;
			}
		}


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
}
