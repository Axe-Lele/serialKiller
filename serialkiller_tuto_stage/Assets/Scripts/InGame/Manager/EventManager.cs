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
	private List<string> EventTemp;
	private List<string> m_EndEvent;
	private string m_DelayTemp = "";
	private int m_EventCount;
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
		ApplyEvent();
	}

	/// <summary>
	/// 이벤트 실행
	/// </summary>
	/// <param name="category">이벤트 카테고리</param>
	/// <param name="s">범인 인덱스</param>
	public void SetEvent(string category, string s)
	{
		//print("category : " + category + " / s : " + s);
		EventTemp.Clear();
		m_EventCount = 0;

		if (category == "GetItem")
		{
			m_EventCount = EventNode[category][StageDataManager.instance.m_CriminalCode.ToString()][s].Count;
		}
		else
		{
			m_EventCount = EventNode[category][s].Count;
		}

		if (m_EventCount > 0)
		{
			// 실행해야하는 이벤트 인덱스
			for (int i = 0; i < m_EventCount; i++)
			{
				if (category == "GetItem")
				{
					EventTemp.Add(EventNode[category][StageDataManager.instance.m_CriminalCode.ToString()][s][i]);
				}
				else
				{
					EventTemp.Add(EventNode[category][s][i]);
				}
			}
			ApplyEvent();
		}
		else
		{
			//print("category : " + category + " / index : " + s + " has not event");
		}
	}

	public void SetEvent(string category, string s1, string s2)
	{
		EventTemp.Clear();
		m_EventCount = 0;

		m_EventCount = EventNode[category][s1][s2].Count;
		//print("category : " + category + " / s1 : " + s1 + " / s2 : " + s2);
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
			//    ApplyEvent();
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

	private void ApplyEvent()
	{
		for (int i = 0; i < EventTemp.Count; i++)
		{
			if (string.Compare(EventTemp[i], string.Empty) == 0)
				continue;

			//Debug.LogWarning("app : " + EventTemp[i]);
			string[] str = EventTemp[i].Split('-');
			string[] temp;
			print(i + " : " + str[0] + " / 1 : " + str[1]);
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

				case GameEventType.AddNews:
					EventDataManager.instance.InputNewsEventData(NewsManager.instance.ReturnNewsDate(str[1]), str[1]);
					//EventDataManager.instance.InputEventData(StageDataManager.instance.m_CaseDataItemList[count].NewsDate, str[1], (int)GameEventType.AddNews, "");
					break;

				case GameEventType.AddCase:
					NoteManager.instance.InputCase(CaseMode.Main, str[1]);
					SystemTextManager.instance.InputText(Localization.Get("System_Text_RefreshCaseList"));
					NoteManager.instance.SelectCase(CaseMode.Main, str[1]);
					NoteManager.instance.SelectedNoteTab(0);
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
					GameManager.instance.m_SystemMessageManager.ShowSystemMessage(str[1]);
					break;

				case GameEventType.AddEvidence:
					EvidenceDataManager.instance.InputEvidence(str[1]);
					break;

				case GameEventType.AddSearch:
					EventDataManager.instance.InputSearchEventData(str[1]);
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
						NpcDataManager.instance.ChangeNpcState(int.Parse(temp[1]), NpcState.Missing);
						NpcDataManager.instance.ChangeNpcPosition(NpcDataManager.instance.GetNpcIndex(temp[1]), "Missing");
					}
					else
					{
						NpcDataManager.instance.ChangeNpcState(int.Parse(str[1]), NpcState.Alive);
					}

					break;

				case GameEventType.GameOver:
					temp = str[1].Split('_');
					EndingManager.instance.SetEnding("2", str[1]);
					break;

				case GameEventType.AddNpcEvent:
					temp = str[1].Split('_');
					NpcDataManager.instance.AddNpcEvent(temp[0], temp[1]);
					break;

				case GameEventType.RemoveNpcEvent:
					temp = str[1].Split('_');
					NpcDataManager.instance.RemoveNpcEvent(temp[0], temp[1]);
					break;

				case GameEventType.RemoveSelection:
					temp = str[1].Split('_');
					SelectionDataManager.instance.RemoveSelection(temp[0], temp[1]);
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