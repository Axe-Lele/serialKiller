//using System.Collections;
//using DetectiveBoard;
using System;
using System.Collections;
using System.Collections.Generic;
using DetectiveBoard;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectiveManager : Singleton<DetectiveManager>
{
	private static readonly string StringFormat = "Criminal_{0}";

	/// <summary>
	/// Index = (int)(DetectiveItemType - 1)
	/// Murder(범인)은 오직 1명 뿐이라, 리스트에 포함되지 않습니다.
	/// </summary>
	[SerializeField]
	public DetectiveNodeItem[] this[DetectiveItemType type]
	{
		get
		{
			switch (type)
			{
				case DetectiveItemType.undefined:
					return null;

				case DetectiveItemType.Murder:
					return m_Murder;

				case DetectiveItemType.Motive:
					return m_Motive;

				case DetectiveItemType.Method:
					return m_Method;

				case DetectiveItemType.Victim:
					return m_Victim;

				case DetectiveItemType.Etc:
					return m_Etc;

				default:
					return null;
			}
		}
	}


	[Space(0)]
	[Header("Node")]
	public DetectiveNodeItem[] m_Murder;
	public DetectiveNodeItem[] m_Motive;
	public DetectiveNodeItem[] m_Method;
	public DetectiveNodeItem[] m_Victim;
	public DetectiveNodeItem[] m_Etc;

	[Space(10)]
	[Header("UI")]
	public GameObject m_UiRoot;
	public GameObject m_Camera;

	[Space(5)]
	public UILabel m_Title;
	public UILabel m_Complish;

	public void Awake()
	{
		DontDestroyOnLoad(this);
	}

	[Header("City Select")]
	public int m_CityCount;
	public GameObject m_CityBoardRoot;
	public DetectiveCityItem[] m_Cities;
	public UILabel m_CityBoardTitle;
	public UILabel m_CityBoardComplish;
	public void ShowCityBoard()
	{
		m_Camera.SetActive(true);

		m_CityCount = StageInfoManager.instance.m_CityItems.Count;

		for (int i = 0; i < m_Cities.Length; i++)
		{
			if (i < m_CityCount)
			{
				m_Cities[i].Init();
			}
			else
			{
				m_Cities[i].gameObject.SetActive(false);
			}
		}

        Dictionary<string, float> complish = DetectiveDataManager.instance.m_Complishes;
        float f = 0f;

        foreach (var item in complish)
        {
            f += item.Value;
        }

        f /= complish.Count;

		m_CityBoardTitle.text = Localization.Get("Text_DetectiveBoard");
		m_CityBoardComplish.text = string.Format("{0} {1}%", Localization.Get("Text_Complish"), f.ToString());
		m_CityBoardRoot.SetActive(true);
	}

    private bool isInit = false;
    public void Init()
	{
        if(isInit == true)
        {
            print("이미 초기화 했어요");
            return;
        }

        isInit = true;
		DetectiveDataManager.instance.Initialized();

		showlist = new List<DetectiveNodeItem>();
	}

	[SerializeField]
	public int m_StageIndex = -1, m_CaseIndex = -1;

	public void SetStageIndex(int stageindex)
	{
		m_StageIndex = stageindex;
		ShowCityBoard();
	}

	public void SetCaseIndex(int caseindex)
	{
		m_CaseIndex = caseindex;

		if (m_StageIndex != -1)
			ShowDetectiveBoard();
	}

	private List<DetectiveNodeItem> showlist = null;
	private Coroutine showCoroutine;
	[Header("Board2")]
	public GameObject m_DetectiveBoardRoot;

	public float m_UnlockNodeCount, m_TotalNodeCount;

	private void ShowDetectiveBoard()
	{
		this.ShowDetectiveBoard(m_StageIndex, m_CaseIndex);
	}
	public void ShowDetectiveBoard(int stage, int caseIndex)
	{
		// 언락 연출 중
		if (showlist.Count != 0)
			return;

		if (showCoroutine != null)
			StopCoroutine(showCoroutine);

		if (m_Title != null)
		{
			m_Title.text = "";
		}

		if (m_Complish != null)
		{
			m_Complish.text = "";
		}

		m_Camera.SetActive(true);
		m_DetectiveBoardRoot.SetActive(true);

		string groupIndex = string.Format("S{0:00}_C{1:0}", stage, caseIndex);
		Debug.Log("GroupIndex : " + groupIndex);

		// 모든 노드 리셋
		ResetBoard();
		m_UnlockNodeCount = m_TotalNodeCount = 0;

		// 언락 연출 시작
		DetectiveDataItem item = null;
		for (int i = 0; i < DetectiveDataManager.instance.m_Items.Count; i++)
		{
			item = DetectiveDataManager.instance.m_Items[i];
			if (item.m_GroupIndex.Equals(groupIndex) == false)
				continue;

			if (item.m_Type == DetectiveItemType.undefined)
				continue;

			this[item.m_Type][item.m_NodeIndex].ReadyToShow(ref item);
			m_TotalNodeCount++;

			if (item.m_IsFirst || item.m_IsOpen)
				m_UnlockNodeCount++;


			if (item.m_IsFirst == true)
			{
				item.m_IsOpen = true;
				item.m_IsFirst = false;

				showlist.Add(this[item.m_Type][item.m_NodeIndex]);

				showlist[showlist.Count - 1].ChangeSprite();
				showlist[showlist.Count - 1].ChangeAlpha(1f);

			}
			else if (item.m_IsOpen == true)
			{
				this[item.m_Type][item.m_NodeIndex].ChangeSprite();
			}

            item = null;
		}

		if (m_Title != null)
		{
			m_Title.text = string.Format(StringFormat, groupIndex);
			m_Title.text = Localization.Get(m_Title.text);
		}

		if (m_Complish != null)
		{
            m_Complish.text = string.Format("{0}%", Mathf.Floor((m_UnlockNodeCount / m_TotalNodeCount) * 100).ToString());

		}

        DetectiveDataManager.instance.Save();

		showCoroutine = StartCoroutine(OpenNodes());
		m_CaseBoardRoot.SetActive(false);
	}

	[Header("CaseBoard")]
	public GameObject m_CaseBoardRoot;
	public CaseItem[] m_Cases;
	public UILabel m_CaseBoardTitle;
	private void ShowCaseBoard()
	{
		ShowCaseBoard(m_StageIndex);
	}
	public void ShowCaseBoard(int stageIndex)
	{
		m_Camera.SetActive(true);

		m_StageIndex = stageIndex;

		m_CaseBoardTitle.text = Localization.Get(string.Format("Text_World_Title_{0:00}", stageIndex));
		m_CityBoardRoot.SetActive(false);
		m_CaseBoardRoot.SetActive(true);

		int caseCount = StageInfoManager.instance.GetCaseCount(m_StageIndex);

		for (int i = 0; i < m_Cases.Length; i++)
		{
			if (i < caseCount)
			{
				m_Cases[i].gameObject.SetActive(true);
				m_Cases[i].SetLabel(stageIndex);
			}
			else
			{
				m_Cases[i].gameObject.SetActive(false);
			}
		}
	}

	private float cycleSecond = 2f;
	private WaitForSeconds waitCycle = new WaitForSeconds(0.05f);
	private IEnumerator OpenNodes()
	{
		float s = 0f;
		int showCount = 0;

		//SoundManager.instance.changeSFXVolume(2f);

		SoundManager.instance.changeSFXVolume(1.0f);
		SoundManager.instance.PlaySFX("dialog_jump");
		while (showCount < showlist.Count)
		{
			yield return waitCycle;
			s += .05f;

			showlist[showCount].ChangeAlpha(1f - s / cycleSecond);
			if (s >= cycleSecond)
			{
				s -= cycleSecond;

				showlist[showCount].ChangeAlpha(0f);

				showCount++;

				if (showCount < showlist.Count)
				{
					SoundManager.instance.changeSFXVolume(1.0f);
					SoundManager.instance.PlaySFX("dialog_jump");
				}
			}
		}

		showlist.Clear();

		if (SceneManager.GetActiveScene().Equals("WorldMap") != true)
		{
			GlobalMethod.instance.ResetGame();
			SceneManager.LoadScene("WorldMap");
		}
	}

	private void ResetBoard()
	{
		m_Murder[0].Init();
		for (int i = 0; i < m_Motive.Length; i++)
		{
			m_Motive[i].Init();
		}
		for (int i = 0; i < m_Method.Length; i++)
		{
			m_Method[i].Init();
		}
		for (int i = 0; i < m_Victim.Length; i++)
		{
			m_Victim[i].Init();
		}
		for (int i = 0; i < m_Etc.Length; i++)
		{
			m_Etc[i].Init();
		}
	}

	public void UnlockItem(string eventCode)
	{
		DetectiveDataManager.instance.UnlockItem(eventCode);
	}

	public void ShowNodes()
	{

	}

	public void CloseBoard()
	{

		if (m_CityBoardRoot.activeInHierarchy)
		{
			m_Camera.SetActive(false);
			m_CityBoardRoot.SetActive(false);

		}
		else if (m_CaseBoardRoot.activeInHierarchy)
		{
			m_CityBoardRoot.SetActive(true);
			m_CaseBoardRoot.SetActive(false);

            ShowCityBoard();
        }
		else if (m_DetectiveBoardRoot.activeInHierarchy)
		{
			m_CaseBoardRoot.SetActive(true);
			m_DetectiveBoardRoot.SetActive(false);
		}
		else
		{
			m_Camera.SetActive(false);
			m_CityBoardRoot.SetActive(false);
			m_CaseBoardRoot.SetActive(false);
			m_DetectiveBoardRoot.SetActive(false);

			ResetBoard();
		}
	}
}

//public class DetectiveManager : Singleton<DetectiveManager>
//{
//	private DetectiveDataManager DataManager
//	{
//		get { return DetectiveDataManager.instance; }
//	}

//	public GameObject m_IconOrigin;

//	public GameObject m_Camera;


//	public UIGrid m_MotiveGrid;
//	public UIGrid m_CriminalGrid;
//	public UIGrid m_VictimGrid;
//	public UIGrid m_ToolGrid;

//	public string m_NowCityKey = string.Empty;
//	public string m_NowCaseKey = string.Empty;

//	public void Init()
//	{
//		//DetectiveNodeItem item = null;

//		m_MotiveItems = new List<DetectiveNodeItem>();
//		//for (int i = 0; i < 3; i++)
//		//{
//		//	item = Instantiate<GameObject>(
//		//		m_IconOrigin, m_MotiveGrid.transform).GetComponent<DetectiveNodeItem>();
//		//	m_MotiveItems.Add(item);
//		//}

//		m_CriminalItems = new List<DetectiveNodeItem>();
//		//for (int i = 0; i < 3; i++)
//		//{
//		//	item = Instantiate<GameObject>(
//		//		m_IconOrigin, m_CriminalGrid.transform).GetComponent<DetectiveNodeItem>();
//		//	m_CriminalItems.Add(item);
//		//}

//		m_VictimeItems = new List<DetectiveNodeItem>();
//		//for (int i = 0; i < 3; i++)
//		//{
//		//	item = Instantiate<GameObject>(
//		//		m_IconOrigin, m_VictimGrid.transform).GetComponent<DetectiveNodeItem>();
//		//	m_VictimeItems.Add(item);
//		//}

//		m_ToolItems = new List<DetectiveNodeItem>();
//		//for (int i = 0; i < 3; i++)
//		//{
//		//	item = Instantiate<GameObject>(
//		//		m_IconOrigin, m_ToolGrid.transform).GetComponent<DetectiveNodeItem>();
//		//	m_ToolItems.Add(item);
//		//}

//		m_node = new List<DetectiveNode>();
//	}

//	public void ShowSeletedCaseList(string cityKey)
//	{
//		m_NowCityKey = cityKey;

//		// show case list
//	}

//	public void ShowBoardOnClicked()
//	{
//		m_NowCityKey = "00";
//		ShowBoard("0");
//	}

//	private void ShowBoard(string caseKey)
//	{
//		m_NowCaseKey = caseKey;

//		// list reset
//		for (int i = 0; i < m_node.Count; i++)
//		{
//			// node reset
//		}

//		// list clear
//		m_node.Clear();

//		// list init
//		//string nodeStringKeys = string.Empty;
//		//string[] nodeSplitKeys = new string[3];
//		//int nodeRealKey = 0;
//		for (int i = 0; i < DataManager.m_DetectiveNodes.Count; i++)
//		{
//			if (DataManager.m_DetectiveNodes[i].City.CompareTo(m_NowCityKey) != 0)
//				continue;

//			if (DataManager.m_DetectiveNodes[i].Case.CompareTo(m_NowCaseKey) != 0)
//				continue;

//			// change atlas & sprite

//			//nodeStringKeys = DataManager.m_DetectiveNodes[i].Node;
//			//nodeSplitKeys = nodeStringKeys.Split('_');// 0: node num, 1: connected num, 2: target key
//			//nodeRealKey = nodeSplitKeys[0].ToInt();
//			m_node.Add(DataManager.m_DetectiveNodes[i]);
//		}

//		string nodeStringKeys = string.Empty;
//		string[] nodeSplitKeys = new string[3];
//		int nodeRealKey = 0;
//		DetectiveNodeItem item = null;
//		for (int i = 0; i < m_node.Count; i++)
//		{
//			nodeStringKeys = m_node[i].Node;
//			nodeSplitKeys = nodeStringKeys.Split('_');// 0: node num, 1: connected num, 2: target key
//			nodeRealKey = nodeSplitKeys[0].ToInt();

//			switch (m_node[i].NodeType)
//			{
//				case DetectiveNodeType.Motive:
//					if (nodeRealKey > m_MotiveItems.Count - 1)
//					{
//						item = Instantiate<GameObject>(
//							m_IconOrigin, m_MotiveGrid.transform).GetComponent<DetectiveNodeItem>();
//						m_MotiveItems.Add(item);
//					}
//					m_MotiveItems[nodeRealKey].m_Index = i;
//					m_MotiveItems[nodeRealKey].Set(nodeSplitKeys[2]);
//					m_MotiveItems[nodeRealKey].gameObject.SetActive(true);

//					item = null;
//					break;

//				case DetectiveNodeType.Criminal:
//					if (nodeRealKey > m_CriminalItems.Count - 1)
//					{
//						item = Instantiate<GameObject>(
//							m_IconOrigin, m_CriminalGrid.transform).GetComponent<DetectiveNodeItem>();
//						m_CriminalItems.Add(item);
//					}
//					m_CriminalItems[nodeRealKey].m_Index = i;
//					m_CriminalItems[nodeRealKey].Set(nodeSplitKeys[2]);
//					m_CriminalItems[nodeRealKey].gameObject.SetActive(true);

//					item = null;
//					break;

//				case DetectiveNodeType.Victim:
//					if (nodeRealKey > m_VictimeItems.Count - 1)
//					{
//						item = Instantiate<GameObject>(
//							m_IconOrigin, m_VictimGrid.transform).GetComponent<DetectiveNodeItem>();
//						m_VictimeItems.Add(item);
//					}
//					m_VictimeItems[nodeRealKey].m_Index = i;
//					m_VictimeItems[nodeRealKey].Set(nodeSplitKeys[2]);
//					m_VictimeItems[nodeRealKey].gameObject.SetActive(true);

//					item = null;
//					break;

//				case DetectiveNodeType.Tool:
//					if (nodeRealKey > m_ToolItems.Count - 1)
//					{
//						item = Instantiate<GameObject>(
//							m_IconOrigin, m_ToolGrid.transform).GetComponent<DetectiveNodeItem>();
//						m_ToolItems.Add(item);
//					}
//					m_ToolItems[nodeRealKey].m_Index = i;
//					m_ToolItems[nodeRealKey].Set(nodeSplitKeys[2]);
//					m_ToolItems[nodeRealKey].gameObject.SetActive(true);

//					item = null;
//					break;

//				default:
//					break;
//			}
//		}

//		m_MotiveGrid.Reposition();
//		m_CriminalGrid.Reposition();
//		m_VictimGrid.Reposition();
//		m_ToolGrid.Reposition();

//		// show board
//		m_Camera.SetActive(true);
//	}
//}
