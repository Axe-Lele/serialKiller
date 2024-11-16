using UnityEngine;
using System.Collections.Generic;

public class NoteDialogManager : MonoBehaviour
{
	public GameObject m_Root;
	public GameObject m_HasNoItem;
	public GameObject m_SelectedMsg;

	[Space]
	public List<DialogItem> m_DialogItemList;
	public DialogItem m_DialogItemOrigin;
	public GameObject m_DialogItemChecker;
	public GameObject m_DialogItemRoot;
	public UIGrid m_DialogItemGrid;
	public UIScrollView m_DialogItemSCV;


	[Space]
	public List<DialogDetailItem> m_DetailItemList;
	public DialogDetailItem m_DetailItemOrigin;
	public GameObject m_DetailItemChecker;
	public GameObject m_DetailItemRoot;
	public UIGrid m_DetailItemGrid;
	public UIScrollView m_DetailItemSCV;

	[Space]
	public GameObject m_PersonRoot;
	public UILabel m_Person;
	public TweenPosition m_Tweener;
	public GameObject m_Unknown;
	public UISprite m_Portrait;
	public GameObject m_NameRoot;

	public void ShowDialog(bool isOpen)
	{
		print("Show [Note] Dialog UI : " + isOpen);

		m_Root.SetActive(isOpen);
		m_Portrait.gameObject.SetActive(false);
		m_DialogItemChecker.SetActive(false);
		m_DetailItemChecker.SetActive(false);
		m_Unknown.SetActive(true);
		m_NameRoot.SetActive(false);

		if (m_DialogItemList.Count == 0)
		{
			m_HasNoItem.SetActive(true);
			m_SelectedMsg.SetActive(false);
			m_DialogItemRoot.SetActive(false);
			m_DetailItemRoot.SetActive(false);
			m_PersonRoot.SetActive(false);
		}
		else
		{
			m_HasNoItem.SetActive(false);
			m_SelectedMsg.SetActive(true);
			m_DialogItemRoot.SetActive(true);
			m_DetailItemRoot.SetActive(false);
			m_PersonRoot.SetActive(true);
		}

		this.Reposition();
	}

	public void SelectedDialogItem(string target)
	{

		int c = DialogDataManager.instance.ReturnCharacterIndex(target);
		int listcount = DialogDataManager.instance.m_DialogDataItemList[c].m_DialogIndex.Count;

		print("c : " + c + " / lc : " + listcount);
		for (int i = 0; i < m_DialogItemList.Count; i++)
		{
			m_DialogItemList[i].UnSelect();
		}

		for (int i = 0; i < m_DetailItemList.Count; i++)
		{
			m_DetailItemList[i].gameObject.SetActive(false);
		}

		if (m_DetailItemList.Count >= listcount)
		{
			for (int i = 0; i < listcount; i++)
			{
				m_DetailItemList[i].Setting(target, DialogDataManager.instance.m_DialogDataItemList[c].m_DialogIndex[i]);
				m_DetailItemList[i].gameObject.SetActive(true);
			}
		}
		else
		{
			for (int i = 0; i < m_DetailItemList.Count; i++)
			{
				m_DetailItemList[i].Setting(target, DialogDataManager.instance.m_DialogDataItemList[c].m_DialogIndex[i]);
				m_DetailItemList[i].gameObject.SetActive(true);
			}

			for (int i = m_DetailItemList.Count; i < listcount; i++)
			{
				DialogDetailItem item = Instantiate(m_DetailItemOrigin);
				item.Setting(target, DialogDataManager.instance.m_DialogDataItemList[c].m_DialogIndex[i]);
				item.transform.parent = m_DetailItemGrid.transform;
				item.transform.localScale = Vector3.one;
				m_DetailItemList.Add(item);
				item.gameObject.SetActive(true);
			}
		}

		m_NameRoot.SetActive(true);
		m_SelectedMsg.SetActive(false);
		m_DetailItemRoot.SetActive(true);
		this.MoveChecker(m_DialogItemChecker, FindTarget(target));
		m_Unknown.SetActive(false);
		m_Tweener.ResetToBeginning();
		m_Tweener.enabled = true;

		m_DetailItemGrid.enabled = true;

		//int index = 0;
		//for (int i = 0; i < m_DialogItemList.Count; i++)
		//{
		//	if (m_DialogItemList[i].ReturnIndex() == target)
		//	{
		//		index = i;
		//		break;
		//	}
		//}

		//switch (GameManager.instance.m_NoteMode)
		//{
		//	case NoteMode.None:
		//		m_NoteModeDetailIndex = index;
		//		break;
		//	case NoteMode.Suggest:
		//		m_SuggestModeDetailIndex = index;
		//		break;
		//	case NoteMode.Warrant:
		//		m_WarrantModeDetailIndex = index;
		//		break;
		//}

		m_DialogItemGrid.Reposition();
		m_DialogItemSCV.ResetPosition();
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
			DialogItem item = Instantiate(m_DialogItemOrigin);
			item.Setting(target);
			item.transform.parent = m_DialogItemGrid.transform;
			item.transform.localScale = Vector3.one;
			m_DialogItemList.Add(item);
			item.gameObject.SetActive(true);

			item = null;
		}
		m_DialogItemGrid.enabled = true;

	}

	public Transform FindTarget(string target)
	{
		Transform tr = null;
		for (int i = 0; i < m_DialogItemList.Count; i++)
		{
			if (m_DialogItemList[i].m_Index == target)
			{
				tr = m_DialogItemList[i].transform;
			}
		}
		return tr;
	}

	public void MoveChecker(DialogDetailItem item)
	{
		for (int i = 0; i < m_DetailItemList.Count; i++)
		{
			m_DetailItemList[i].m_IsSelected = (m_DetailItemList[i] == item);
		}
		this.MoveChecker(m_DetailItemChecker, item.transform);
	}

	public void MoveCheckerOnDetailItem(Transform target)
	{
		this.MoveChecker(m_DetailItemChecker, target);
	}

	private void MoveChecker(GameObject checker, Transform target)
	{
		if (target == null)
		{
			checker.SetActive(false);
			return;
		}

		checker.transform.SetParent(target);
		checker.transform.localPosition = Vector3.zero;
		checker.SetActive(true);
	}

	private void Reposition()
	{
		m_DetailItemGrid.Reposition();
		m_DetailItemSCV.ResetPosition();

		m_DialogItemGrid.Reposition();
		m_DialogItemSCV.ResetPosition();
	}
}
