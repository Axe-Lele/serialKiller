using System.Collections;
using DetectiveBoard;
using System.Collections.Generic;
using UnityEngine;

public class DetectiveManager : Singleton<DetectiveManager>
{
	private DetectiveDataManager DataManager
	{
		get { return DetectiveDataManager.instance; }
	}

	public GameObject m_IconOrigin;

	public GameObject m_Camera;

	public List<DetectiveNodeItem> m_MotiveItems;
	public List<DetectiveNodeItem> m_CriminalItems;
	public List<DetectiveNodeItem> m_VictimeItems;
	public List<DetectiveNodeItem> m_ToolItems;

	public UIGrid m_MotiveGrid;
	public UIGrid m_CriminalGrid;
	public UIGrid m_VictimGrid;
	public UIGrid m_ToolGrid;

	public List<DetectiveNode> m_Nodes;

	public string m_NowCityKey = string.Empty;
	public string m_NowCaseKey = string.Empty;

	public void Init()
	{
		//DetectiveNodeItem item = null;

		m_MotiveItems = new List<DetectiveNodeItem>();
		//for (int i = 0; i < 3; i++)
		//{
		//	item = Instantiate<GameObject>(
		//		m_IconOrigin, m_MotiveGrid.transform).GetComponent<DetectiveNodeItem>();
		//	m_MotiveItems.Add(item);
		//}

		m_CriminalItems = new List<DetectiveNodeItem>();
		//for (int i = 0; i < 3; i++)
		//{
		//	item = Instantiate<GameObject>(
		//		m_IconOrigin, m_CriminalGrid.transform).GetComponent<DetectiveNodeItem>();
		//	m_CriminalItems.Add(item);
		//}

		m_VictimeItems = new List<DetectiveNodeItem>();
		//for (int i = 0; i < 3; i++)
		//{
		//	item = Instantiate<GameObject>(
		//		m_IconOrigin, m_VictimGrid.transform).GetComponent<DetectiveNodeItem>();
		//	m_VictimeItems.Add(item);
		//}

		m_ToolItems = new List<DetectiveNodeItem>();
		//for (int i = 0; i < 3; i++)
		//{
		//	item = Instantiate<GameObject>(
		//		m_IconOrigin, m_ToolGrid.transform).GetComponent<DetectiveNodeItem>();
		//	m_ToolItems.Add(item);
		//}

		m_Nodes = new List<DetectiveNode>();
	}

	public void ShowSeletedCaseList(string cityKey)
	{
		m_NowCityKey = cityKey;

		// show case list
	}

	public void ShowBoardOnClicked()
	{
		m_NowCityKey = "00";
		ShowBoard("0");
	}

	private void ShowBoard(string caseKey)
	{
		m_NowCaseKey = caseKey;

		// list reset
		for (int i = 0; i < m_Nodes.Count; i++)
		{
			// node reset
		}

		// list clear
		m_Nodes.Clear();

		// list init
		//string nodeStringKeys = string.Empty;
		//string[] nodeSplitKeys = new string[3];
		//int nodeRealKey = 0;
		for (int i = 0; i < DataManager.m_DetectiveNodes.Count; i++)
		{
			if (DataManager.m_DetectiveNodes[i].City.CompareTo(m_NowCityKey) != 0)
				continue;

			if (DataManager.m_DetectiveNodes[i].Case.CompareTo(m_NowCaseKey) != 0)
				continue;

			// change atlas & sprite

			//nodeStringKeys = DataManager.m_DetectiveNodes[i].Node;
			//nodeSplitKeys = nodeStringKeys.Split('_');// 0: node num, 1: connected num, 2: target key
			//nodeRealKey = nodeSplitKeys[0].ToInt();
			m_Nodes.Add(DataManager.m_DetectiveNodes[i]);
		}

		string nodeStringKeys = string.Empty;
		string[] nodeSplitKeys = new string[3];
		int nodeRealKey = 0;
		DetectiveNodeItem item = null;
		for (int i = 0; i < m_Nodes.Count; i++)
		{
			nodeStringKeys = m_Nodes[i].Node;
			nodeSplitKeys = nodeStringKeys.Split('_');// 0: node num, 1: connected num, 2: target key
			nodeRealKey = nodeSplitKeys[0].ToInt();

			switch (m_Nodes[i].NodeType)
			{
				case DetectiveNodeType.Motive:
					if (nodeRealKey > m_MotiveItems.Count - 1)
					{
						item = Instantiate<GameObject>(
							m_IconOrigin, m_MotiveGrid.transform).GetComponent<DetectiveNodeItem>();
						m_MotiveItems.Add(item);
					}
					m_MotiveItems[nodeRealKey].m_Index = i;
					m_MotiveItems[nodeRealKey].Set(nodeSplitKeys[2]);
					m_MotiveItems[nodeRealKey].gameObject.SetActive(true);

					item = null;
					break;

				case DetectiveNodeType.Criminal:
					if (nodeRealKey > m_CriminalItems.Count - 1)
					{
						item = Instantiate<GameObject>(
							m_IconOrigin, m_CriminalGrid.transform).GetComponent<DetectiveNodeItem>();
						m_CriminalItems.Add(item);
					}
					m_CriminalItems[nodeRealKey].m_Index = i;
					m_CriminalItems[nodeRealKey].Set(nodeSplitKeys[2]);
					m_CriminalItems[nodeRealKey].gameObject.SetActive(true);

					item = null;
					break;

				case DetectiveNodeType.Victim:
					if (nodeRealKey > m_VictimeItems.Count - 1)
					{
						item = Instantiate<GameObject>(
							m_IconOrigin, m_VictimGrid.transform).GetComponent<DetectiveNodeItem>();
						m_VictimeItems.Add(item);
					}
					m_VictimeItems[nodeRealKey].m_Index = i;
					m_VictimeItems[nodeRealKey].Set(nodeSplitKeys[2]);
					m_VictimeItems[nodeRealKey].gameObject.SetActive(true);

					item = null;
					break;

				case DetectiveNodeType.Tool:
					if (nodeRealKey > m_ToolItems.Count - 1)
					{
						item = Instantiate<GameObject>(
							m_IconOrigin, m_ToolGrid.transform).GetComponent<DetectiveNodeItem>();
						m_ToolItems.Add(item);
					}
					m_ToolItems[nodeRealKey].m_Index = i;
					m_ToolItems[nodeRealKey].Set(nodeSplitKeys[2]);
					m_ToolItems[nodeRealKey].gameObject.SetActive(true);

					item = null;
					break;

				default:
					break;
			}
		}

		m_MotiveGrid.Reposition();
		m_CriminalGrid.Reposition();
		m_VictimGrid.Reposition();
		m_ToolGrid.Reposition();

		// show board
		m_Camera.SetActive(true);
	}
}
