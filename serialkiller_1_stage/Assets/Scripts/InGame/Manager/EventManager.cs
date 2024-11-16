using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

public class EventManager : Singleton<EventManager>
{
	public List<EventStartItem> m_EventStartItem = new List<EventStartItem>();

	[SerializeField]
	private JSONNode EventNode;
	public TextAsset EventTextAsset;
	private string m_NowEvent;
	private List<string> EventTemp;
	private List<string> m_EndEvent;
	private string m_DelayTemp = "";
	private int m_EventCount;
	private string m_CriminalCode
	{
		get { return StageDataManager.instance.m_CriminalCode.ToString(); }
	}
	private void Awake()
	{
		EventNode = JSONNode.Parse(EventTextAsset.text);


		EventTemp = new List<string>();
		m_EndEvent = new List<string>();
	}

	public void SetStartEvent()
	{
		m_EventCount = EventNode["Start"][StageDataManager.instance.m_CriminalCode.ToString()].Count;

		for (int i = 0; i < m_EventCount; i++)
		{
			EventTemp.Add(EventNode["Start"][StageDataManager.instance.m_CriminalCode.ToString()][i]);
		}

		if (m_EventCount != 0)
			ApplyEvent();
	}

	public void SetEventTemp(List<string> eventTemps)
	{
		EventTemp.Clear();
		m_EventCount = 0;

		if (eventTemps != null && eventTemps.Count > 0)
		{
			EventTemp.AddRange(eventTemps);
			m_EventCount = eventTemps.Count;
		}

		if (m_EventCount > 0)
		{
			ApplyEvent();
		}
		else
		{
			return;
		}
	}
	public void SetEventTemp(string eventTemp)
	{
		EventTemp.Clear();
		m_EventCount = 0;

		if (!string.IsNullOrEmpty(eventTemp))
		{
			EventTemp.Add(eventTemp);
			m_EventCount++;
		}

		if (m_EventCount > 0)
		{
			ApplyEvent();
		}
		else
		{
			return;
		}
	}

	public void InsertEvent(string category, string s)
	{
		InsertEvent(m_NowEventIndex, category, s);
	}

	public void InsertEvent(int index, string category, string s)
	{
		int eventCount = 0;
		bool isGetItem = (category == "GetItem");

		if (isGetItem)
		{
			eventCount = EventNode[category][StageDataManager.instance.m_CriminalCode.ToString()][s].Count;
		}
		else
		{
			eventCount = EventNode[category][s].Count;
		}

		if (eventCount <= 0)
			return;

		if (isGetItem)
		{
			for (int i = eventCount; i > 0; i--)
				EventTemp.Insert(index + 1, EventNode[category][m_CriminalCode][s][i - 1]);
		}
		else
		{
			for (int i = eventCount; i > 0; i--)
				EventTemp.Insert(index + 1, EventNode[category][s][i - 1]);
		}
	}

	public void AddEvent(string category, params string[] s)
	{
		int len = s.Length;
		JSONNode node;

		if (len <= 0)
			return;

		node = EventNode[category][s[0]];

		if (len > 1)
			for (int i = 1; i < len; i++)
				node = node[s[i]];

		int addEventCount = node.Count;
		for (int i = 0; i < addEventCount; i++)
		{
			EventTemp.Add(node[i]);
			m_EventCount++;
		}
	}

	/// <summary>
	/// 이벤트 실행
	/// </summary>
	/// <param name="category">이벤트 카테고리</param>
	/// <param name="s">범인 인덱스</param>
	public void SetEvent(string category, string s)
	{
		bool isGetItem = (category == "GetItem");

		print("category : " + category + " / s : " + s);
		if (isGetItem)
		{
			m_EventCount = EventNode[category][StageDataManager.instance.m_CriminalCode.ToString()][s].Count;
		}
		else
		{
			m_EventCount = 0;
			EventTemp.Clear();
			m_EventCount = EventNode[category][s].Count;
		}

		if (m_EventCount <= 0)
			return;

		if (isGetItem)
			for (int i = 0; i < m_EventCount; i++)
				EventTemp.Add(EventNode[category][StageDataManager.instance.m_CriminalCode.ToString()][s][i]);
		else
			for (int i = 0; i < m_EventCount; i++)
				EventTemp.Add(EventNode[category][s][i]);

		ApplyEvent();
	}

	public void SetEvent(string category, string s1, string s2)
	{
		EventTemp.Clear();
		m_EventCount = 0;

		m_EventCount = EventNode[category][s1][s2].Count;
		print("category : " + category + " / s1 : " + s1 + " / s2 : " + s2);
		if (m_EventCount > 0)
		{
			for (int i = 0; i < m_EventCount; i++)
			{
				EventTemp.Add(EventNode[category][s1][s2][i]);
			}
			ApplyEvent();
		}
		else
		{
			//print("category : " + category + " / s1 : " + s1 + " / s2 : " + s2 + " has not event");
		}
	}

	public void SetEvent(string category, string s1, string s2, string s3)
	{
		EventTemp.Clear();
		m_EventCount = 0;

		m_EventCount = EventNode[category][s1][s2][s3].Count;

		//print("category : " + category + " / s1 : " + s1 + " / s2 : " + s2 + " / s3 : " + s3 + " / count : " + m_EventCount);

		if (m_EventCount > 0)
		{
			for (int i = 0; i < m_EventCount; i++)
			{
				EventTemp.Add(EventNode[category][s1][s2][s3][i]);
			}
			ApplyEvent();
		}
		else
		{
			//print("category : " + category + " / s1 : " + s1 + " / s2 : " + s2 + " / s3 : " + s3 + " has not event");
		}
	}

	public void SetDelayEvent(string category, string s1, string s2)
	{
		m_DelayTemp = "";
		m_EventCount = 1;

		if (m_EventCount > 0)
		{
			for (int i = 0; i < m_EventCount; i++)
			{
				m_DelayTemp = EventNode[category][s1][s2][i];
			}
			ApplyEvent();
		}
		else
		{
			print("category : " + category + " / s1 : " + s1 + " / s2 : " + s2 + " has not event");
		}
	}

	public void StartDelayEvent()
	{
		if (m_DelayTemp != "")
		{
			EventTemp.Clear();
			EventTemp.Add(m_DelayTemp);
			ApplyEvent();
			m_DelayTemp = "";
		}
	}

	public void DialogEndEvent(string who)
	{
		// 해당 인물에게 대화 종료 이벤트가 있는지 체크
		int c = EventNode["EndDialog"][who][StageDataManager.instance.m_CriminalCode.ToString()].Count;
		print("c : " + c);
		if (c > 0)
		{
			int index = DialogDataManager.instance.ReturnCharacterIndex(who);
			int count = 0;
			if (index != -1)
			{
				count = DialogDataManager.instance.m_DialogDataItemList[index].m_DialogIndex.Count;
			}

			// 해당 인물과 대화한 기록이 있는지 체크
			print("count : " + count + " / index : " + index);
			if (count > 0)
			{
				string eventcode = "";
				EventTemp.Clear();
				m_EventCount = 0;
				//m_EventCount = EventNode["EndDialog"][who][eventcode].Count;
				//tList에 해당 인물과 대화한 값들을 넣는다.
				bool b = false;
				for (int i = 0; i < count; i++)
				{
					string tempcode = DialogDataManager.instance.m_DialogDataItemList[index].m_DialogIndex[i];
					print("str : " + tempcode);
					if (EventNode["EndDialog"][who][StageDataManager.instance.m_CriminalCode.ToString()][tempcode].Count > 0)
					{
						for (int k = 0; k < EventDataManager.instance.m_EndDialogEventList.Count; k++)
						{
							if (EventDataManager.instance.m_EndDialogEventList[k].m_EventCode == tempcode && EventDataManager.instance.m_EndDialogEventList[k].m_Who == who)
							{

								b = true;
								break;
							}
						}
					}

					if (b == false)
					{
						eventcode = tempcode;
						EventDataManager.instance.InputEndDialogEventData(who, eventcode);
						print("who : " + who + " ev :" + eventcode);
					}
				}



				// 이벤트 코드가 추출되었을 시 
				if (eventcode != "")
				{
					c = EventNode["EndDialog"][who][StageDataManager.instance.m_CriminalCode.ToString()][eventcode].Count;
					for (int i = 0; i < c; i++)
					{
						EventTemp.Add(EventNode["EndDialog"][who][StageDataManager.instance.m_CriminalCode.ToString()][eventcode][i]);
					}

					ApplyEvent();
				}
			}
		}
		else
		{
			print("category : EndDialog / who : " + who + " has not event");
		}
	}

	public int m_NowEventIndex = 0;
	private void ApplyEvent()
	{
		for (m_NowEventIndex = 0; m_NowEventIndex < EventTemp.Count; m_NowEventIndex++)
		{
			if (string.Compare(EventTemp[m_NowEventIndex], string.Empty) == 0)
				continue;

			//Debug.LogWarning("app : " + EventTemp[i]);
			string[] str = EventTemp[m_NowEventIndex].Split('-');
			string[] temp;
			Debug.Log(m_NowEventIndex + " : " + (GameEventType)Enum.Parse(typeof(GameEventType), str[0]) + " / 1 : " + str[1]);
			switch ((GameEventType)Enum.Parse(typeof(GameEventType), str[0]))
			{
				case GameEventType.AddArea:
					temp = str[1].Split('_');
					print("temp 0 : " + temp[0]);

					PlaceDataManager.instance.ControlPlaceIsOpened((PlaceType)Enum.Parse(typeof(PlaceType), temp[0]), int.Parse(temp[1]), true);
					break;

				case GameEventType.RemoveArea:
					temp = str[1].Split('_');
					PlaceDataManager.instance.ControlPlaceIsOpened((PlaceType)Enum.Parse(typeof(PlaceType), temp[0]), int.Parse(temp[1]), false);
					break;

				case GameEventType.StartCase:
					// 이벤트 경험 여부 확인
					temp = str[1].Split('_');
					if (EventDataManager.instance.HasSeenStartCaseEvent(temp[0], temp[1]))
					{
						Debug.Log(string.Format("Has been this Event - {0}", EventTemp[m_NowEventIndex]));
						continue;
					}

					/// 이벤트가 즉시 발동해야하나?(InsertEvent)
					/// 아니면 나중에 발동해야하나?(AddEvent)
					/// 모든 이벤트 종료후 새롭게 발동해야하나?(DelayEvent)
					this.AddEvent("Case", StageDataManager.instance.m_CriminalCode.ToString(), temp[1]);

					// 세이브/로드용 이벤트 데이터 등록
					EventDataManager.instance.InputStartCaseEvent(temp[0], temp[1]);
					break;

				case GameEventType.AddNews:
					EventDataManager.instance.InputNewsEventData(NewsManager.instance.ReturnNewsDate(str[1]), str[1]);
					//EventDataManager.instance.InputEventData(StageDataManager.instance.m_CaseDataItemList[count].NewsDate, str[1], (int)GameEventType.AddNews, "");
					break;

				/// AddCase : 탐정수첩 - 사건정보 추가
				case GameEventType.AddCase:
					// 이벤트 경험 여부 확인
					if (EventDataManager.instance.HasSeenAddCaseEvent((int)CaseMode.Main, str[1]))
					{
						Debug.Log(string.Format("Has been this Event - {0}", EventTemp[m_NowEventIndex]));
						continue;
					}

					SystemTextManager.instance.InputText(Localization.Get("System_Text_RefreshCaseList"));
					NoteManager.instance.InputCase(CaseMode.Main, str[1]);
					NoteManager.instance.SelectCase(CaseMode.Main, str[1]);
					NoteManager.instance.SelectedNoteTab(0);

					// 세이브/로드용 이벤트 데이터 등록
					EventDataManager.instance.InputAddCaseEventData((int)CaseMode.Main, str[1]);
					break;

				case GameEventType.AddSelection:
					temp = str[1].Split('_');
					SelectionDataManager.instance.InputSelection(temp[0], temp[1]);
					break;

				case GameEventType.Dialog:
					temp = str[1].Split('_');
					GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.EventDialog, temp[0], temp[1]);
					break;

				case GameEventType.SystemMessage:
					Debug.Log("Show SystemMessage[" + str[1] + "]");
					GameManager.instance.m_SystemMessageManager.ShowSystemMessage(str[1]);
					EventDataManager.instance.InputSystemMessage(str[1]);
					break;

				case GameEventType.AddEvidence:
					EvidenceDataManager.instance.InputEvidence(str[1]);
					break;

				case GameEventType.AddSearch:
					EventDataManager.instance.InputSearchEventData(str[1]);
					break;

				case GameEventType.SetEvidence:
					temp = str[1].Split('+');

					PlaceDataManager.instance.AddEvidence(temp[0], temp[1]);
					break;

				case GameEventType.Case:
					temp = str[1].Split('_');
					if (str[0] == "Main")
					{

					}
					else if (str[0] == "Side")
					{

					}
					break;

				case GameEventType.ChangeNpcState:
					temp = str[1].Split('_');

					if (temp[1] == "Dead")
					{
						NpcDataManager.instance.ChangeNpcState(temp[0], NpcState.Dead);
						NpcDataManager.instance.ChangeNpcPosition(NpcDataManager.instance.GetNpcIndex(temp[0]), "Dead");
					}
					else if (temp[1] == "Missing")
					{
						NpcDataManager.instance.ChangeNpcState(temp[0], NpcState.Missing);
						NpcDataManager.instance.ChangeNpcPosition(NpcDataManager.instance.GetNpcIndex(temp[0]), "Missing");
					}
					else
					{
						NpcDataManager.instance.ChangeNpcState(int.Parse(str[1]), NpcState.Alive);
					}

					break;

				case GameEventType.GameOver:
					temp = str[1].Split('_');
					//EndingManager.instance.SetEnding("2", str[1]);
					break;

				case GameEventType.AddNpcEvent:
					EventDataManager.instance.InputNpcEvent(str[1], true);
					temp = str[1].Split('_');
					NpcDataManager.instance.AddNpcEvent(temp[0], temp[1]);
					NpcDataManager.instance.ActNpc();
					break;

				case GameEventType.RemoveNpcEvent:
					EventDataManager.instance.InputNpcEvent(str[1], false);
					temp = str[1].Split('_');
					NpcDataManager.instance.RemoveNpcEvent(temp[0], temp[1]);
					NpcDataManager.instance.ActNpc();
					break;

				case GameEventType.RemoveSelection:
					temp = str[1].Split('_');
					SelectionDataManager.instance.RemoveSelection(temp[0], temp[1]);
					break;

				case GameEventType.RemoveEvidence:
					EvidenceDataManager.instance.RemoveEvidence(str[1]);
					break;

				case GameEventType.SwapEvidence:
					temp = str[1].Split('_');
					EvidenceDataManager.instance.SwapEvidence(temp[0], temp[1].ToInt());
					break;

				case GameEventType.ShowCompulsion:
					temp = str[1].Split('_');
					SuggestManager.instance.ShowSelectionEvent(temp[0]
															, string.Format("Event_{0}", temp[1]));
					break;

				case GameEventType.ShowEnding:
					EndingManager.instance.SetEnding(str[1]);
					break;

				case GameEventType.UnlockDT:
					DetectiveManager.instance.UnlockItem(str[1]);
					break;
			}
		}
	}

	private void ChangeNpcState(string param)
	{
		// 이전 버전
		//temp = param.Split('_');

		//if (temp[1] == "Dead")
		//{
		//    NpcDataManager.instance.ChangeNpcState(int.Parse(str[1]), NpcState.Dead);
		//    NpcDataManager.instance.ChangeNpcPosition(NpcDataManager.instance.GetNpcIndex(str[1]), "Dead");
		//}
		//else if (temp[1] == "Missing")
		//{
		//    NpcDataManager.instance.ChangeNpcState(int.Parse(str[1]), NpcState.Missing);
		//    NpcDataManager.instance.ChangeNpcPosition(NpcDataManager.instance.GetNpcIndex(str[1]), "Missing");
		//}
		//else
		//{
		//    NpcDataManager.instance.ChangeNpcState(int.Parse(str[1]), NpcState.Alive);
		//}

		int _npcTarget = -1;
		NpcState _changedState = NpcState.Alive;

		string[] temp = param.Split('_');
		_npcTarget = temp[0].ToInt();
		_changedState = temp[1].ToEnum<NpcState>();

		NpcDataManager.instance.ChangeNpcState(_npcTarget, _changedState);
	}

	// Boxing Function
	public void SetSearchEvent(string place)
	{
		print("Search Event, Criminal : " + StageDataManager.instance.m_CriminalCode.ToString() + " / Place : " + place);
		this.SetEvent(EventCategory.Search.ToString(), StageDataManager.instance.m_CriminalCode.ToString(), place);
	}
}

public static class ExtentionMethods
{
	public static T ToEnum<T>(this string value)
	{
		if (!System.Enum.IsDefined(typeof(T), value))
			return default(T);
		return (T)System.Enum.Parse(typeof(T), value, true);
	}

	public static int ToInt(this string value)
	{
		return int.Parse(value);
	}
}