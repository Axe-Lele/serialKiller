using UnityEngine;
using System.Collections.Generic;

public class NoteCaseManager : MonoBehaviour
{
	public GameObject m_Root;
	public GameObject m_HasNoCase;
	public GameObject m_SelectedCaseMsg;

	[Space]
	public List<CaseItemInNote> m_ItemList;
	public CaseItemInNote m_ItemOrigin;
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

	public void ShowCase(bool isOpen)
	{
		print("Show [Note] Case UI : " + isOpen);
		m_Root.SetActive(isOpen);
		m_ItemChecker.SetActive(false);

		if (m_ItemList.Count == 0)
		{
			m_HasNoCase.SetActive(true);
			m_ItemRoot.SetActive(false);
			m_DescParent.SetActive(false);
			m_SelectedCaseMsg.SetActive(false);
		}
		else
		{
			m_HasNoCase.SetActive(false);
			m_ItemRoot.SetActive(true);
			m_DescParent.SetActive(false);
			m_SelectedCaseMsg.SetActive(true);
		}

		this.Reposition();
	}

	public void SelectedCaseItem(CaseMode mode, string itemCode)
	{
		print(itemCode);

		string _title = string.Format("Case_{0}_{1}_{2}"
									, PlayDataManager.instance.m_StageName
									, mode.ToString()
									, itemCode);

		string _desc = string.Format("Case_{0}_{1}_{2}_{3}"
									, PlayDataManager.instance.m_StageName
									, mode.ToString()
									, StageDataManager.instance.m_CriminalCode
									, itemCode);

		print("title : " + _title + " / desc : " + _desc);

		switch (mode)
		{
			case CaseMode.Main:
				m_MainDescTitle.text = Localization.Get(_title);
				m_MainDesc.text = Localization.Get(_desc);
				if (m_Illust.gameObject.activeInHierarchy == false)
					m_Illust.gameObject.SetActive(true);

				//m_CaseTab[(int)CaseMode.Main].gameObject.SetActive(true);
				//m_CaseTab[(int)CaseMode.Side].gameObject.SetActive(false);
				break;

			case CaseMode.Side:
				//m_CaseSideInfoLabel.text = Localization.Get(s);
				//m_CaseTab[(int)CaseMode.Main].gameObject.SetActive(false);
				//m_CaseTab[(int)CaseMode.Side].gameObject.SetActive(true);
				break;

			default:
				break;
		}

		m_ItemChecker.SetActive(true);
		int itemIdx = this.GetItemIdx(itemCode);
		if (itemIdx != -1)
			MoveChecker(m_ItemList[itemIdx]);

		m_SelectedCaseMsg.SetActive(false);
		m_DescParent.SetActive(true);
	}

	public int InputItem(CaseMode mode, string itemCode)
	{
		if (EventDataManager.instance.HasSeenAddCaseEvent((int)mode, itemCode))
		{
			Debug.Log("Where is ?");
			return -1;
		}

		CaseItemInNote item = Instantiate(m_ItemOrigin);
		item.Setting(m_ItemList.Count, mode, itemCode);
		item.transform.parent = m_ItemGrid.transform;
		item.transform.localScale = Vector3.one;
		m_ItemList.Add(item);
		item.gameObject.SetActive(true);

		m_ItemGrid.enabled = true;
		
		this.Reposition();

		return m_ItemList.Count - 1;
	}

	private void MoveChecker(CaseItemInNote target)
	{
		if(target == null)
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
			if (m_ItemList[i].m_Index.CompareTo(itemCode) == 0)
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

	public void ForcedInputItem(int mode, string caseIndex)
	{ 
		CaseItemInNote item = Instantiate(m_ItemOrigin);
		
		item.Setting(m_ItemList.Count, (CaseMode)mode, caseIndex);
		item.transform.parent = m_ItemGrid.transform;
		item.transform.localScale = Vector3.one;

		m_ItemList.Add(item);

		item.gameObject.SetActive(true);
		m_ItemGrid.enabled = true;

		this.Reposition();
	}
}
