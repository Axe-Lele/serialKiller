using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteNewsManager : MonoBehaviour
{
	public GameObject m_Root;
	public GameObject m_HasNoNews;
	public GameObject m_SelectedNewsMsg;

	[Space]
	public List<NewsItem> m_ItemList;
	public NewsItem m_ItemOrigin;
	public GameObject m_ItemChecker;

	[Space]
	public GameObject m_ItemRoot;
	public UIGrid m_ItemGrid;
	public UIScrollView m_ItemSCV;

	[Space]
	public GameObject m_DescParent;
	public UILabel m_MainDescTitle;
	public UISprite m_Illust;
	public UILabel m_MainDesc;

	public void ShowNewsPanel(bool isOpen)
	{
		print("Show [Note] News UI : " + isOpen);
		m_Root.SetActive(isOpen);
		m_ItemChecker.SetActive(false);

		if (m_ItemList.Count == 0)
		{
			m_HasNoNews.SetActive(true);
			m_ItemRoot.SetActive(false);
			m_DescParent.SetActive(false);
			m_SelectedNewsMsg.SetActive(false);
		}
		else
		{
			m_HasNoNews.SetActive(false);
			m_ItemRoot.SetActive(true);
			m_DescParent.SetActive(false);
			m_SelectedNewsMsg.SetActive(true);
		}

		this.Reposition();
	}

	public void SelectedNewsItem(int itemIdx)
	{
		this.SelectedNewsItem(m_ItemList[itemIdx].m_ItemCode);
	}

	public void SelectedNewsItem(string itemCode)
	{
		int itemIdx = GetItemIdx(itemCode);
		string _title = Localization.Get(string.Format("News_{0}_{1}_{2}_Title"
																									, PlayDataManager.instance.m_StageName
																									, StageDataManager.instance.m_CriminalCode
																									, m_ItemList[itemIdx].m_ItemCode));

		string _desc = Localization.Get(string.Format("News_{0}_{1}_{2}_Content"
																		, PlayDataManager.instance.m_StageName
																		, StageDataManager.instance.m_CriminalCode
																		, m_ItemList[itemIdx].m_ItemCode));
		if (m_Illust.gameObject.activeInHierarchy == false)
			m_Illust.gameObject.SetActive(true);

		m_MainDescTitle.text = _title;
		m_MainDesc.text = _desc;

		print("title : " + _title + " / desc : " + _desc);

		switch (GameManager.instance.m_NoteMode)
		{
			case NoteMode.None:
				break;

			case NoteMode.Suggest:
				break;

			case NoteMode.Warrant:
				break;

			case NoteMode.SelectCase:
				break;

			default:
				break;
		}

		if (itemIdx != -1)
			MoveChecker(m_ItemList[itemIdx]);

		m_SelectedNewsMsg.SetActive(false);
		m_DescParent.SetActive(true);

		this.Reposition();
	}

	public int InputItem(int date, string itemCode)
	{
		for (int i = 0; i < m_ItemList.Count; i++)
		{
			if (m_ItemList[i].m_Date == date)
				return -1;
		}

		NewsItem go = Instantiate(m_ItemOrigin) as NewsItem;
		go.gameObject.SetActive(true);
		go.transform.parent = m_ItemGrid.transform;
		go.transform.localScale = Vector2.one;
		go.transform.localPosition = Vector2.zero;
		go.Setting(date, itemCode, m_ItemList.Count, 1);
		m_ItemList.Add(go);

		m_ItemGrid.enabled = true;

		this.Reposition();

		return m_ItemList.Count - 1;
	}

	private void MoveChecker(NewsItem target)
	{
		if (target == null)
		{
			m_ItemChecker.SetActive(false);
			return;
		}

		m_ItemChecker.transform.SetParent(target.transform);
		m_ItemChecker.transform.localPosition = Vector3.zero;
		m_ItemChecker.SetActive(true);
	}
	
	private int GetItemIdx(string itemCode)
	{
		int _rvalue = -1;

		for (int i = 0; i < m_ItemList.Count; i++)
		{
			if (m_ItemList[i].m_ItemCode.CompareTo(itemCode) == 0)
			{
				_rvalue = i;
				break;
			}
		}
		return _rvalue;
	}

	private void Reposition()
	{
		m_ItemGrid.Reposition();
		m_ItemSCV.ResetPosition();
	}
}
