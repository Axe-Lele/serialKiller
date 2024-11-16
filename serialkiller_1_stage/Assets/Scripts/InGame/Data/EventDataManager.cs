using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleJSON;
using System.Security.Cryptography;
public class EventDataManager : Singleton<EventDataManager>
{

	private EventData data;
	private string filePath;

	#region AddCase
	public List<EventDataItem> m_AddCaseEvents;
	public void InputAddCaseEventData(int mode, string caseIndex)
	{
		EventDataItem item = new EventDataItem();
		// EventCode : {CaseMode}-{CaseIndex}
		// GameEventType : EventType

		item.m_EventCode = string.Format("{0}-{1}", mode.ToString(), caseIndex);
		item.m_GameEventType = GameEventType.AddCase;

		m_AddCaseEvents.Add(item);

		Debug.Log(string.Format("Input Event - AddCase - EventCode : {0}", item.m_EventCode));
		item = null;
	}

	public bool HasSeenAddCaseEvent(int mode, string caseIndex)
	{
		string eventCode = string.Format("{0}-{1}", mode, caseIndex);

		for (int i = 0; i < m_AddCaseEvents.Count; i++)
		{
			if (m_AddCaseEvents[i].m_EventCode.Equals(eventCode))
				return true;
		}

		return false;
	}

	private void InputAddCaseEventData(string eventCode)
	{
		//Debug.Log(string.Format("Input Event - AddCase - EventCode : {0}", eventCode));

		string[] temp = eventCode.Split('-');
		// temp[0] : {CaseMode}
		// temp[1] : {CaseIndex}

		int mode = temp[0].ToInt();

		this.InputAddCaseEventData(mode, temp[1]);
	}

	private void ReadAddCaseEvents()
	{
		for (int i = 0; i < data.m_AddCaseEvents.Count; i++)
		{
			// EventCode : {CaseMode}-{CaseIndex}
			InputAddCaseEventData(data.m_AddCaseEvents[i].m_EventCode);
			string[] temp = data.m_AddCaseEvents[i].m_EventCode.Split('-');
			NoteManager.instance.ForcedInputCase(temp[0].ToInt(), temp[1]);
		}
	}
	private void WriteAddCaseEventData()
	{
		data.m_AddCaseEvents = new List<EventDataItem>();

		for (int i = 0; i < m_AddCaseEvents.Count; i++)
		{
			EventDataItem item = new EventDataItem();

			item.m_EventCode = m_AddCaseEvents[i].m_EventCode;
			item.m_GameEventType = GameEventType.AddCase;

			data.m_AddCaseEvents.Add(item);

			Debug.Log(string.Format("Save Event - AddCase - EventCode : {0}", item.m_EventCode));
			item = null;
		}

		Debug.Log("Save Event-AddCase Data.");
	}
	#endregion

	#region StartCase
	public List<EventDataItem> m_StartCaseEvents;
	public void InputStartCaseEvent(string mode, string caseIndex)
	{
		EventDataItem item = new EventDataItem();
		// EventCode : {CaseMode}-{CaseIndex}
		// GameEventType : EventType

		item.m_EventCode = string.Format("{0}-{1}", mode.ToString(), caseIndex);
		item.m_GameEventType = GameEventType.StartCase;

		m_StartCaseEvents.Add(item);

		Debug.Log(string.Format("Input Event - {0} - EventCode : {1}", GameEventType.StartCase, item.m_EventCode));
		item = null;
	}

	public bool HasSeenStartCaseEvent(string mode, string caseIndex)
	{
		string eventCode = string.Format("{0}-{1}", mode.ToString(), caseIndex);

		for (int i = 0; i < m_StartCaseEvents.Count; i++)
		{
			if (m_StartCaseEvents[i].m_EventCode.Equals(eventCode))
				return true;
		}

		return false;
	}

	private void InputStartCaseEvent(string eventCode)
	{
		Debug.Log(string.Format("Input Event - {0} - EventCode : {1}", GameEventType.StartCase, eventCode));

		string[] temp = eventCode.Split('-');
		// temp[0] : {CaseMode}
		// temp[1] : {CaseIndex}

		this.InputStartCaseEvent(temp[0], temp[1]);
	}

	private void ReadStartCaseEvents()
	{
		for (int i = 0; i < data.m_StartCaseEvents.Count; i++)
		{
			// EventCode : {CaseMode}-{CaseIndex}
			InputStartCaseEvent(data.m_StartCaseEvents[i].m_EventCode);
		}
	}
	private void WriteStartCaseEventDatas()
	{
		data.m_StartCaseEvents = new List<EventDataItem>();

		for (int i = 0; i < m_AddCaseEvents.Count; i++)
		{
			EventDataItem item = new EventDataItem();

			item.m_EventCode = m_StartCaseEvents[i].m_EventCode;
			item.m_GameEventType = GameEventType.AddCase;

			data.m_StartCaseEvents.Add(item);

			Debug.Log(string.Format("Input Event - {0} - EventCode : {1}", GameEventType.StartCase, item.m_EventCode));
			item = null;
		}

		Debug.Log("Save Event-StartCase Data.");
	}
	#endregion

	public List<EventDataItem> m_CaseEventList;
	public List<EventDataItem> m_AreaEventList;
	public List<EventDataItem> m_NewsEventList;
	public List<EventDataItem> m_NpcEventList;
	public List<EventDataItem> m_DialogEventList;
	public List<EventDataItem> m_EndDialogEventList;
	public List<EventDataItem> m_SearchEventList;
	public List<EventDataItem> m_StartDialogEventList;
	public List<EventDataItem> m_SetEvidenceList;
	public List<EventDataItem> m_SystemMessageList;

	public List<EventDataItem> m_CasePlaceEventList;
	public void ChangeCasePlaceEventData(string code)
	{
		bool b = false;
		if (m_CasePlaceEventList.Count > 0)
		{
			for (int i = 0; i < m_CasePlaceEventList.Count; i++)
			{
				if (m_CasePlaceEventList[i].m_EventCode == code)
				{
					b = true;
					break;
				}
			}
		}

		if (b == false)
		{
			EventDataItem item = new EventDataItem();
			item.m_EventCode = code;
			item.m_IsStart = true;

			EventManager.instance.SetEvent("CasePlace", StageDataManager.instance.m_CriminalCode.ToString(), code);

			m_CasePlaceEventList.Add(item);
			item = null;
		}
	}


	public void LoadEventData()
	{
		filePath = Application.persistentDataPath + "/data06.bin";
		m_AreaEventList = new List<EventDataItem>();
		m_NewsEventList = new List<EventDataItem>();
		m_NpcEventList = new List<EventDataItem>();

		m_CaseEventList = new List<EventDataItem>();
		m_DialogEventList = new List<EventDataItem>();
		m_EndDialogEventList = new List<EventDataItem>();
		m_CasePlaceEventList = new List<EventDataItem>();
		m_SearchEventList = new List<EventDataItem>();
		m_StartDialogEventList = new List<EventDataItem>();
		m_SetEvidenceList = new List<EventDataItem>();

		m_SystemMessageList = new List<EventDataItem>();


		m_AddCaseEvents = new List<EventDataItem>();


		if (GlobalMethod.instance.ReturnFileExist(filePath))
		{
			BinaryDeserialize();
			ReadData();
		}
	}

	// 매일 9시에 발생하는 이벤트 !
	public void StartEvent(int date)
	{

		for (int i = 0; i < m_CaseEventList.Count; i++)
		{
			if (m_CaseEventList[i].m_Date > date)
			{
				break;
			}
			else if (m_CaseEventList[i].m_IsStart == true)
			{
				continue;
			}
			else if (m_CaseEventList[i].m_Date == date && m_CaseEventList[i].m_IsStart == false)
			{
				print("Case Date : " + m_CaseEventList[i].m_Date + ", IsStart : " + m_CaseEventList[i].m_IsStart
						+ ", Order : " + m_CaseEventList[i].m_Place);
				m_CaseEventList[i].m_IsStart = true;
				EventManager.instance.SetEvent("Case", StageDataManager.instance.m_CriminalCode.ToString()
						, m_CaseEventList[i].m_Place.ToString());
				//PlaceDataManager.instance.ControlPlace(PlaceType.Case, m_CaseEventList[i].m_Place, true);
				//CaseManager.instance.OpenCase(i);
				//StartEvent(i);
			}
		}


		for (int i = 0; i < m_AreaEventList.Count; i++)
		{
			if (m_AreaEventList[i].m_Date > date)
			{
				break;
			}
			else if (m_AreaEventList[i].m_IsStart == true)
			{
				continue;
			}
			else if (m_AreaEventList[i].m_Date == date && m_AreaEventList[i].m_IsStart == false)
			{
				m_AreaEventList[i].m_IsStart = true;
				PlaceManager.instance.ControlPlaceButton((PlaceType)m_AreaEventList[i].m_PlaceType, m_AreaEventList[i].m_Place, m_AreaEventList[i].m_IsOpen);
			}
		}

		for (int i = 0; i < m_NewsEventList.Count; i++)
		{
			//print("i : " + i + " / date : " + m_NewsEventList[i].m_Date + " code : " + m_NewsEventList[i].m_EventCode);
			if (m_NewsEventList[i].m_Date > date)
			{
				continue;
			}
			else if (m_NewsEventList[i].m_IsStart == true)
			{
				continue;
			}
			else if (m_NewsEventList[i].m_Date == date && m_NewsEventList[i].m_IsStart == false)
			{
				m_NewsEventList[i].m_IsStart = true;
				NewsManager.instance.AddUnreadNewsList(m_NewsEventList[i].m_Date, m_NewsEventList[i].m_EventCode);
				NewsDataManager.instance.InputNewsItem(m_NewsEventList[i].m_Date, m_NewsEventList[i].m_EventCode);
			}
		}

		for (int i = 0; i < m_NpcEventList.Count; i++)
		{
			if (m_NpcEventList[i].m_Date > date)
			{
				break;
			}
			else if (m_NpcEventList[i].m_IsStart == true)
			{
				continue;
			}
			else if (m_NpcEventList[i].m_Date == date && m_NpcEventList[i].m_IsStart == false)
			{
				m_NpcEventList[i].m_IsStart = true;
			}
		}

		for (int i = 0; i < m_DialogEventList.Count; i++)
		{
			if (m_DialogEventList[i].m_Date > date)
			{
				break;
			}
			else if (m_DialogEventList[i].m_IsStart == true)
			{
				continue;
			}
			else if (m_DialogEventList[i].m_Date == date && m_DialogEventList[i].m_IsStart == false)
			{
				m_DialogEventList[i].m_IsStart = true;
			}
		}
	}

	public int ReturnCaseLocation(int i)
	{
		return m_CaseEventList[i].m_Place;
	}

	public string ReturnCaseCode(int i)
	{
		return m_CaseEventList[i].m_EventCode;
	}

	public int ReturnCaseDate(int i)
	{
		//print("i : " + i);
		return m_CaseEventList[i].m_Date;
	}


	public void InputAreaEventData(int date, string eventcode, int placetype, int place, bool b)
	{
		EventDataItem item = new EventDataItem();
		item.m_Date = date;
		item.m_EventCode = eventcode;
		item.m_IsOpen = b;
		item.m_IsStart = false;
		item.m_PlaceType = placetype;
		item.m_Place = place;

		m_AreaEventList.Add(item);
		item = null;
	}

	public void InputNewsEventData(int date, string eventcode)
	{
		EventDataItem item = new EventDataItem();
		item.m_Date = date;
		item.m_EventCode = eventcode;
		//print("eventcode : " + eventcode);
		item.m_IsStart = false;

		m_NewsEventList.Add(item);
		item = null;
	}

	public void InputCaseEventData(int date, string eventcode, int location, bool b)
	{
		EventDataItem item = new EventDataItem();
		item.m_Date = date;
		item.m_EventCode = eventcode;
		item.m_IsStart = b;
		item.m_Place = location;
		m_CaseEventList.Add(item);
		item = null;
	}

	public void InputNpcEventData(int date, string eventcode)
	{
		EventDataItem item = new EventDataItem();
		item.m_Date = date;
		item.m_EventCode = eventcode;
		item.m_IsStart = false;

		m_NpcEventList.Add(item);
		item = null;
	}

	public void InputDialogEventData(int date, string who, string eventcode)
	{
		EventDataItem item = new EventDataItem();
		item.m_Date = date;
		item.m_Who = who;
		item.m_EventCode = eventcode;
		item.m_IsStart = false;

		m_DialogEventList.Add(item);
		item = null;
	}

	public void InputEndDialogEventData(string who, string eventcode)
	{
		EventDataItem item = new EventDataItem();
		item.m_Who = who;
		item.m_EventCode = eventcode;
		item.m_IsStart = true;

		m_EndDialogEventList.Add(item);
		item = null;
	}

	public void InputSearchEventData(string code)
	{
		EventDataItem item = new EventDataItem();
		item.m_EventCode = code;
		item.m_IsStart = false;
		m_SearchEventList.Add(item);
		item = null;

		PlaceDataManager.instance.ControlPlaceIsSearched(code, false);
		//print(code + " / " + eventcode);
		//eventcode = string.Format("AddEvidence-{0}", eventcode);
		//PlaceManager.instance.AddEventCoddeOnPlace(code, eventcode);
	}

	public void InputSetEvidence(string code, bool b)
	{
		EventDataItem item = new EventDataItem();
		item.m_EventCode = code;
		item.m_IsStart = b;

		m_SetEvidenceList.Add(item);
		item = null;
	}

	public void InputNpcEvent(string code, bool b)
	{
		EventDataItem item = new EventDataItem();
		item.m_EventCode = code;
		item.m_IsStart = b;

		m_NpcEventList.Add(item);
		item = null;
	}

	public void InputSystemMessage(string code)
	{
		for (int i = 0; i < m_SystemMessageList.Count; i++)
		{
			if (m_SystemMessageList[i].m_EventCode == code)
				return;
		}

		EventDataItem item = new EventDataItem();
		item.m_EventCode = code;
		item.m_IsStart = false;

		m_SystemMessageList.Add(item);

		item = null;
	}

	public bool ReturnSearchEventData(string code)
	{
		bool b = false;
		for (int i = 0; i < m_SearchEventList.Count; i++)
		{
			if (m_SearchEventList[i].m_EventCode == code)
			{
				b = m_SearchEventList[i].m_IsStart;
				break;
			}
		}

		return b;
	}

	public void SwapSearchEventData(string code, bool b)
	{
		for (int i = 0; i < m_SearchEventList.Count; i++)
		{
			if (m_SearchEventList[i].m_EventCode == code)
			{
				m_SearchEventList[i].m_IsStart = b;
				break;
			}
		}
	}

	public void RemoveSearchEventData(string code)
	{
		if (m_SearchEventList.Count > 0)
		{
			for (int i = 0; i < m_SearchEventList.Count; i++)
			{
				if (m_SearchEventList[i].m_EventCode == code)
				{
					m_SearchEventList.RemoveAt(i);
					break;
				}
			}
		}
	}

	public void InputStartDialogEventData(string who, string code)
	{
		if (ReturnStartDialogEventData(who) == -1)
		{
			EventDataItem item = new EventDataItem();
			item.m_Who = who;
			item.m_EventCode = code;

			m_StartDialogEventList.Add(item);
			item = null;
		}
	}

	public void ChangeStartDialogEventData(string who, string code)
	{
		for (int i = 0; i < m_StartDialogEventList.Count; i++)
		{
			if (m_StartDialogEventList[i].m_Who == who)
			{
				m_StartDialogEventList[i].m_EventCode = code;
			}
		}
	}

	public int ReturnStartDialogEventData(string who)
	{
		int index = -1;

		for (int i = 0; i < m_StartDialogEventList.Count; i++)
		{
			if (m_StartDialogEventList[i].m_Who == who)
			{
				index = i;
				break;
			}
		}

		return index;
	}

	public bool IsFirstShowSystemMessage(string code)
	{
		if (m_SystemMessageList.Count <= 0)
			return true;

		for (int i = 0; i < m_SystemMessageList.Count; i++)
		{
			if (string.Equals(m_SystemMessageList[i].m_EventCode, code))
				return false;
		}

		return true;
	}


	public void InputCasePlaceEventData(string code)
	{
		EventDataItem item = new EventDataItem();
		item.m_EventCode = code;
		item.m_IsStart = true;

		m_CasePlaceEventList.Add(item);
		item = null;
	}

	public bool HasCaseEventData(string code)
	{
		for (int i = 0; i < m_CaseEventList.Count; i++)
		{
			if (string.Equals(m_CaseEventList[i], code))
				return true;
		}

		return false;
	}

	public bool HasSeenCasePlaceEventData(string code)
	{
		for (int i = 0; i < m_CasePlaceEventList.Count; i++)
		{
			if (string.Equals(m_CasePlaceEventList[i], code))
				return true;
		}

		return false;
	}


	private void WriteData()
	{
		data = new EventData();
		data.m_NewsEventList = new List<EventDataItem>();
		data.m_NpcEventList = new List<EventDataItem>();
		data.m_AreaEventList = new List<EventDataItem>();
		data.m_CaseEventList = new List<EventDataItem>();
		data.m_DialogEventList = new List<EventDataItem>();
		data.m_EndDialogEventList = new List<EventDataItem>();
		data.m_CasePlaceEventList = new List<EventDataItem>();
		data.m_SearchEventList = new List<EventDataItem>();
		data.m_StartDialogEventList = new List<EventDataItem>();
		data.m_SystemMessageList = new List<EventDataItem>();

		for (int i = 0; i < m_NewsEventList.Count; i++)
		{
			EventDataItem item = new EventDataItem();
			item.m_Date = m_NewsEventList[i].m_Date;
			item.m_EventCode = m_NewsEventList[i].m_EventCode;
			item.m_IsStart = m_NewsEventList[i].m_IsStart;

			data.m_NewsEventList.Add(item);
			item = null;
		}

		for (int i = 0; i < m_AreaEventList.Count; i++)
		{
			EventDataItem item = new EventDataItem();
			item.m_Date = m_AreaEventList[i].m_Date;
			item.m_EventCode = m_AreaEventList[i].m_EventCode;
			item.m_IsOpen = m_AreaEventList[i].m_IsOpen;
			item.m_IsStart = m_AreaEventList[i].m_IsStart;
			item.m_PlaceType = m_AreaEventList[i].m_PlaceType;
			item.m_Place = m_AreaEventList[i].m_Place;

			data.m_AreaEventList.Add(item);
			item = null;
		}

		for (int i = 0; i < m_CaseEventList.Count; i++)
		{
			EventDataItem item = new EventDataItem();
			item.m_Date = m_CaseEventList[i].m_Date;
			item.m_EventCode = m_CaseEventList[i].m_EventCode;
			item.m_IsStart = m_CaseEventList[i].m_IsStart;
			item.m_Place = m_CaseEventList[i].m_Place;

			data.m_CaseEventList.Add(item);
			item = null;
		}

		for (int i = 0; i < m_NpcEventList.Count; i++)
		{
			EventDataItem item = new EventDataItem();
			item.m_Date = m_NpcEventList[i].m_Date;
			item.m_EventCode = m_NpcEventList[i].m_EventCode;
			item.m_IsStart = m_NpcEventList[i].m_IsStart;

			data.m_NpcEventList.Add(item);
			item = null;
		}

		for (int i = 0; i < m_DialogEventList.Count; i++)
		{
			EventDataItem item = new EventDataItem();
			item.m_Date = m_DialogEventList[i].m_Date;
			item.m_Who = m_DialogEventList[i].m_Who;
			item.m_EventCode = m_DialogEventList[i].m_EventCode;
			item.m_IsStart = m_DialogEventList[i].m_IsStart;

			data.m_DialogEventList.Add(item);
			item = null;
		}

		for (int i = 0; i < m_EndDialogEventList.Count; i++)
		{
			EventDataItem item = new EventDataItem();
			item.m_Date = m_EndDialogEventList[i].m_Date;
			item.m_Who = m_EndDialogEventList[i].m_Who;
			item.m_EventCode = m_EndDialogEventList[i].m_EventCode;
			item.m_IsStart = m_EndDialogEventList[i].m_IsStart;

			data.m_EndDialogEventList.Add(item);
			item = null;
		}

		for (int i = 0; i < m_CasePlaceEventList.Count; i++)
		{
			EventDataItem item = new EventDataItem();
			item.m_EventCode = m_CasePlaceEventList[i].m_EventCode;
			item.m_IsStart = m_CasePlaceEventList[i].m_IsStart;

			data.m_CasePlaceEventList.Add(item);
			item = null;
		}

		for (int i = 0; i < m_SearchEventList.Count; i++)
		{
			EventDataItem item = new EventDataItem();
			item.m_EventCode = m_SearchEventList[i].m_EventCode;

			data.m_SearchEventList.Add(item);
			item = null;
		}

		for (int i = 0; i < m_StartDialogEventList.Count; i++)
		{
			EventDataItem item = new EventDataItem();
			item.m_Who = m_StartDialogEventList[i].m_Who;
			item.m_EventCode = m_StartDialogEventList[i].m_EventCode;

			data.m_StartDialogEventList.Add(item);
			item = null;
		}

		for (int i = 0; i < m_SystemMessageList.Count; i++)
		{
			EventDataItem item = new EventDataItem();
			item.m_EventCode = m_SystemMessageList[i].m_EventCode;
			item.m_IsStart = true;

			data.m_SystemMessageList.Add(item);
			item = null;
		}

		WriteAddCaseEventData();

		BinarySerialize(data);
	}

	private void ReadData()
	{

		//print("news : " + m_NewsEventList.Count);
		for (int i = 0; i < data.m_NewsEventList.Count; i++)
		{
			InputNewsEventData(data.m_NewsEventList[i].m_Date, data.m_NewsEventList[i].m_EventCode);
		}

		//print("m_AreaEventList : " + m_AreaEventList.Count);
		for (int i = 0; i < data.m_AreaEventList.Count; i++)
		{
			InputAreaEventData(data.m_AreaEventList[i].m_Date, data.m_AreaEventList[i].m_EventCode, data.m_AreaEventList[i].m_PlaceType, data.m_AreaEventList[i].m_Place, data.m_AreaEventList[i].m_IsOpen);
		}

		//print("m_CaseEventList : " + m_CaseEventList.Count);
		for (int i = 0; i < data.m_CaseEventList.Count; i++)
		{
			InputCaseEventData(data.m_CaseEventList[i].m_Date, data.m_CaseEventList[i].m_EventCode, data.m_CaseEventList[i].m_Place, data.m_CaseEventList[i].m_IsStart);
		}

		//print("m_NpcEventList : " + m_NpcEventList.Count);
		for (int i = 0; i < data.m_NpcEventList.Count; i++)
		{
			InputNpcEventData(data.m_NpcEventList[i].m_Date, data.m_NpcEventList[i].m_EventCode);
		}

		//print("m_DialogEventList : " + m_DialogEventList.Count);
		for (int i = 0; i < data.m_DialogEventList.Count; i++)
		{
			InputDialogEventData(data.m_DialogEventList[i].m_Date, data.m_DialogEventList[i].m_Who, data.m_DialogEventList[i].m_EventCode);
		}

		//print("m_EndDialogEventList : " + m_EndDialogEventList.Count);
		for (int i = 0; i < data.m_EndDialogEventList.Count; i++)
		{
			InputEndDialogEventData(data.m_EndDialogEventList[i].m_Who, data.m_EndDialogEventList[i].m_EventCode);
		}

		for (int i = 0; i < data.m_CasePlaceEventList.Count; i++)
		{
			InputCasePlaceEventData(data.m_CasePlaceEventList[i].m_EventCode);
		}

		for (int i = 0; i < data.m_SearchEventList.Count; i++)
		{
			InputSearchEventData(data.m_SearchEventList[i].m_EventCode);
		}

		for (int i = 0; i < data.m_StartDialogEventList.Count; i++)
		{
			InputStartDialogEventData(data.m_StartDialogEventList[i].m_Who, data.m_StartDialogEventList[i].m_EventCode);
		}

		for (int i = 0; i < m_SystemMessageList.Count; i++)
		{
			InputSystemMessage(data.m_SystemMessageList[i].m_EventCode);
		}

		ReadAddCaseEvents();
	}

	public void BinarySerialize(EventData data)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream;
		if (GlobalMethod.instance.ReturnFileExist(filePath))
		{
			stream = new FileStream(filePath, FileMode.Open);
		}
		else
		{
			stream = new FileStream(filePath, FileMode.Create);
		}
		formatter.Serialize(stream, data);
		stream.Close();
	}

	public void BinaryDeserialize()
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(filePath, FileMode.Open);
		EventData d = (EventData)formatter.Deserialize(stream);
		data = d;
		stream.Close();
	}

	public void Save()
	{
		print("EventDataManager Save");
		WriteData();
	}
}
