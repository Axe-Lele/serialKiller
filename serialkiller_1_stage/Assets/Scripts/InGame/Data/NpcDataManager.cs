using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleJSON;
using System.Security.Cryptography;

public class NpcDataManager : Singleton<NpcDataManager>
{
	enum EventType
	{
		Common,
		Criminal
	}

	enum ScheduleDataKey
	{
		StartHour,
		StartMinute,
		EndHour,
		EndMinute,
		Position
	}

	public NpcData data;
	private string filePath;

	private JSONNode NpcScheduleNode;
	public TextAsset m_NpcScheduleText;

	private JSONNode NpcNode;
	public TextAsset m_NpcData;
	public List<NpcItem> m_NpcItemList;
	public UIAtlas[] NpcAtlas;

	/// <summary>
	/// NPC 행동 리스트를 가진 NPC 리스트
	/// </summary>
	public List<List<NpcActItem>> m_NpcActList;

	private void Awake()
	{
		filePath = Application.persistentDataPath + "/data07.bin";
		m_NpcItemList = new List<NpcItem>();
		NpcScheduleNode = JSONNode.Parse(m_NpcScheduleText.text);
		NpcNode = JSONNode.Parse(m_NpcData.text);
	}

	public void Init()
	{
		m_NpcActList = new List<List<NpcActItem>>();

		for (int i = 0; i < NpcDataManager.instance.m_NpcItemList.Count; i++)
		{
			List<NpcActItem> m_ActItem = new List<NpcActItem>();
			m_NpcActList.Add(m_ActItem);
		}
	}

	/*public void ActNpc()
	{
			string time = (GameManager.instance.ReturnNowTime() / 60).ToString();
			//print("m_NpcItemList.Count : " + m_NpcItemList.Count + " / time : " + time);
			for (int i = 0; i < m_NpcItemList.Count; i++)
			{
					 //print("i : " + i + " / name : " + m_NpcItemList[i].m_Index + " / state : " + m_NpcItemList[i].m_Company);
					if (m_NpcItemList[i].NpcState == NpcState.Dead || m_NpcItemList[i].NpcState == NpcState.Missing)
					{
					}
					else if (m_NpcItemList[i].NpcState == NpcState.Alive)
					{
							// 이벤트가 있을 때 어떻게 한다는 현재 없다.
							string str = NpcScheduleNode[m_NpcItemList[i].m_Index]["Common"][GameManager.instance.ReturnDayOfWeek()][time];


							if (str != null)
							{
									if (str.Contains("_"))
									{
											ChangeNpcPosition(i, str);
									}
									else if (str.Contains("Home"))
									{
											if (m_NpcItemList[i].m_Home != -1)
											{
													string temp = "0_" + m_NpcItemList[i].m_Home;
													ChangeNpcPosition(i, temp);
											}
									}
									else if (str.Contains("Company"))
									{
											if (m_NpcItemList[i].m_Company != -1)
											{
													string temp = "1_" + m_NpcItemList[i].m_Company;
													ChangeNpcPosition(i, temp);
											}
									}
									else if (str.Contains("Missing"))
									{
											//print("i :" + i);
											ChangeNpcPosition(i, str);
									}
							}

							//    print("npc : " + m_NpcItemList[i].m_Index + " / act : " + str);
					}
			}
	}*/

	public void ActNpc()
	{
		List<int> m_ActList = new List<int>();
		int m_CurrentTime = (GameManager.instance.ReturnNowTime());

		for (int i = 0; i < m_NpcItemList.Count; i++)
		{
			if (m_NpcItemList[i].NpcState == NpcState.Dead || m_NpcItemList[i].NpcState == NpcState.Missing)
			{
			}
			else if (m_NpcItemList[i].NpcState == NpcState.Alive && m_NpcItemList[i].m_Index.Contains("N") == false && m_NpcItemList[i].m_Index.Contains("P") == false)
			{
				m_ActList.Clear();

				for (int k = 0; k < m_NpcActList[i].Count; k++)
				{
					int m_StartTime = m_NpcActList[i][k].m_StartHour * 60 + m_NpcActList[i][k].m_StartMinute;
					int m_EndTime = m_NpcActList[i][k].m_EndHour * 60 + m_NpcActList[i][k].m_EndMinute;

					print("i : " + i + " / start : " + m_StartTime + " / end : " + m_EndTime + " / current : " + m_CurrentTime);

					if (m_StartTime <= m_CurrentTime && m_EndTime >= m_CurrentTime)
					{
						m_ActList.Add(k);
					}
				}

				string str = null;

				if (m_ActList.Count == 1)
				{
					print(" list count is 1");
					str = m_NpcActList[i][m_ActList[0]].m_Position;
				}
				else if (m_ActList.Count == 0)
				{
					print(" list count is 0");
				}
				else if (m_ActList.Count > 1)
				{
					print(" list count is 1 over");
					int index = 0;
					int order = m_NpcActList[i][m_ActList[0]].m_Order;
					for (int k = 1; k < m_ActList.Count; k++)
					{
						if (m_NpcActList[i][m_ActList[k]].m_Order > order)
						{
							index = m_ActList[k];
						}
					}
					print("count : " + m_ActList.Count + " / index :  " + index + " / [] : ");

					str = m_NpcActList[i][index].m_Position;
					print("str : " + str);
				}

				if (str != null)
				{
					if (str.Contains("_"))
					{
						ChangeNpcPosition(i, str);
					}
					else if (str.Contains("Home"))
					{
						if (m_NpcItemList[i].m_Home != -1)
						{
							string temp = "0_" + m_NpcItemList[i].m_Home;
							ChangeNpcPosition(i, temp);
						}
					}
					else if (str.Contains("Company"))
					{
						if (m_NpcItemList[i].m_Company != -1)
						{
							string temp = "1_" + m_NpcItemList[i].m_Company;
							ChangeNpcPosition(i, temp);
						}
					}
					else if (str.Contains("Missing"))
					{
						//print("i :" + i);
						ChangeNpcPosition(i, str);
					}
				}

				print("npc : " + m_NpcItemList[i].m_Index + " / act : " + str);
			}
		}
	}

	public void ActSetting()
	{
		string _type;
		string _criminalcode = StageDataManager.instance.m_CriminalCode.ToString();
		string _dayOfWeek = GameManager.instance.ReturnDayOfWeek();
		string _npcIndex = string.Empty;

		for (int i = 0; i < m_NpcItemList.Count; i++)
		{
			//print("i : " + i + " / name : " + m_NpcItemList[i].m_Index + " / state : " + m_NpcItemList[i].m_Company);
			if (m_NpcItemList[i].NpcState == NpcState.Dead || m_NpcItemList[i].NpcState == NpcState.Missing)
			{
			}
			else if (m_NpcItemList[i].NpcState == NpcState.Alive && m_NpcItemList[i].m_Index.Contains("N") == false && m_NpcItemList[i].m_Index.Contains("P") == false)
			{
				List<NpcActItem> _npcActList = new List<NpcActItem>();
				_type = EventType.Common.ToString();
				_npcIndex = m_NpcItemList[i].m_Index;

				// eventCountOfDay = 하루 동안 해야할 스케쥴 갯수
				int eventCountOfDay = NpcScheduleNode[_npcIndex][_type][_dayOfWeek].Count;
				if (eventCountOfDay > 0)
				{
					for (int _scheduleIndex = 0; _scheduleIndex < eventCountOfDay; _scheduleIndex++)
					{
						NpcActItem item = new NpcActItem();

						item.m_StartHour = NpcScheduleNode[_npcIndex][_type][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.StartHour.ToString()].AsInt;
						item.m_StartMinute = NpcScheduleNode[_npcIndex][_type][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.StartMinute.ToString()].AsInt;
						item.m_EndHour = NpcScheduleNode[_npcIndex][_type][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.EndHour.ToString()].AsInt;
						item.m_EndMinute = NpcScheduleNode[_npcIndex][_type][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.EndMinute.ToString()].AsInt;
						item.m_Position = NpcScheduleNode[_npcIndex][_type][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.Position.ToString()];
						item.m_Order = -1;

						_npcActList.Add(item);
						item = null;
					}
				}
				_type = EventType.Criminal.ToString();
				string _order = string.Empty;

				// specialEventCountOfDay = 통상 이벤트를 제외한 하루 동안 해야할 스케쥴 갯수
				int specialEventCountOfDay = m_NpcItemList[i].m_NpcEventCode.Count;
				if (specialEventCountOfDay > 0)
				{
					for (int _scheduleIndex = 0; _scheduleIndex < m_NpcItemList[i].m_NpcEventCode.Count; _scheduleIndex++)
					{
						_order = m_NpcItemList[i].m_NpcEventCode[_scheduleIndex];

						NpcActItem item = new NpcActItem();

						item.m_StartHour = NpcScheduleNode[_npcIndex][_type][_criminalcode][_order][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.StartHour.ToString()].AsInt;
						item.m_StartMinute = NpcScheduleNode[_npcIndex][_type][_criminalcode][_order][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.StartMinute.ToString()].AsInt;
						item.m_EndHour = NpcScheduleNode[_npcIndex][_type][_criminalcode][_order][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.EndHour.ToString()].AsInt;
						item.m_EndMinute = NpcScheduleNode[_npcIndex][_type][_criminalcode][_order][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.EndMinute.ToString()].AsInt;
						item.m_Position = NpcScheduleNode[_npcIndex][_type][_criminalcode][_order][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.Position.ToString()];
						item.m_Order = int.Parse(_order);

						_npcActList.Add(item);
						item = null;
					}
				}
				m_NpcActList[i] = _npcActList;

				// print ("i : " + i + " / index : " + m_NpcItemList[i].m_Index + " / week : " + GameManager.instance.ReturnDayOfWeek() + " / count : " + m_NpcActList [i].Count); 
			}
		}
	}

	public void ActSetting(int i)
	{
		string _type;
		string _criminalcode = StageDataManager.instance.m_CriminalCode.ToString();
		string _dayOfWeek = GameManager.instance.ReturnDayOfWeek();
		string _npcIndex = string.Empty;

		//print("i : " + i + " / name : " + m_NpcItemList[i].m_Index + " / state : " + m_NpcItemList[i].m_Company);
		if (m_NpcItemList[i].NpcState == NpcState.Dead
				|| m_NpcItemList[i].NpcState == NpcState.Missing)
		{
		}
		else if (m_NpcItemList[i].NpcState == NpcState.Alive
				&& m_NpcItemList[i].m_Index.Contains("N") == false
				&& m_NpcItemList[i].m_Index.Contains("P") == false)
		{
			List<NpcActItem> _npcActList = new List<NpcActItem>();
			_type = EventType.Common.ToString();
			_npcIndex = m_NpcItemList[i].m_Index;

			// eventCountOfDay = 하루 동안 해야할 스케쥴 갯수
			int eventCountOfDay = NpcScheduleNode[_npcIndex][_type][_dayOfWeek].Count;
			if (eventCountOfDay > 0)
			{
				for (int _scheduleIndex = 0; _scheduleIndex < eventCountOfDay; _scheduleIndex++)
				{
					NpcActItem item = new NpcActItem();

					item.m_StartHour = NpcScheduleNode[_npcIndex][_type][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.StartHour.ToString()].AsInt;
					item.m_StartMinute = NpcScheduleNode[_npcIndex][_type][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.StartMinute.ToString()].AsInt;
					item.m_EndHour = NpcScheduleNode[_npcIndex][_type][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.EndHour.ToString()].AsInt;
					item.m_EndMinute = NpcScheduleNode[_npcIndex][_type][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.EndMinute.ToString()].AsInt;
					item.m_Position = NpcScheduleNode[_npcIndex][_type][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.Position.ToString()];
					item.m_Order = -1;

					_npcActList.Add(item);
					item = null;

				}
			}
			_type = EventType.Criminal.ToString();
			string _order = string.Empty;

			// specialEventCountOfDay = 통상 이벤트를 제외한 하루 동안 해야할 스케쥴 갯수
			int specialEventCountOfDay = m_NpcItemList[i].m_NpcEventCode.Count;
			if (specialEventCountOfDay > 0)
			{
				print("NPC : " + _npcIndex + ", special event count : " + m_NpcItemList[i].m_NpcEventCode.Count);
				for (int _scheduleIndex = 0; _scheduleIndex < m_NpcItemList[i].m_NpcEventCode.Count; _scheduleIndex++)
				{
					_order = m_NpcItemList[i].m_NpcEventCode[_scheduleIndex];

					NpcActItem item = new NpcActItem();

					print("npcIndex : " + _npcIndex + " / type : " + _type + " / Criminal : " + _criminalcode);
					item.m_StartHour = NpcScheduleNode[_npcIndex][_type][_criminalcode][_order][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.StartHour.ToString()].AsInt;
					item.m_StartMinute = NpcScheduleNode[_npcIndex][_type][_criminalcode][_order][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.StartMinute.ToString()].AsInt;
					item.m_EndHour = NpcScheduleNode[_npcIndex][_type][_criminalcode][_order][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.EndHour.ToString()].AsInt;
					item.m_EndMinute = NpcScheduleNode[_npcIndex][_type][_criminalcode][_order][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.EndMinute.ToString()].AsInt;
					item.m_Position = NpcScheduleNode[_npcIndex][_type][_criminalcode][_order][_dayOfWeek][_scheduleIndex.ToString()][ScheduleDataKey.Position.ToString()];
					item.m_Order = int.Parse(_order);

					print("criminal event :  " + _scheduleIndex + " / Time : " + item.m_StartHour + " ~ " + item.m_EndHour + " / Order : " + item.m_Order + " / m_Position : " + item.m_Position);

					_npcActList.Add(item);
					item = null;

				}
			}
			print("i : " + i + " / index : " + m_NpcItemList[i].m_Index + " / week : " + GameManager.instance.ReturnDayOfWeek() + " / count : " + m_NpcActList[i].Count);
			m_NpcActList[i] = _npcActList;
		}
	}

	public void ActSetting(string s)
	{
		ActSetting(GetNpcIndex(s));
	}

	/// <summary>
	/// ex) D0_0 (npcname = D0, eventcode = 0)
	/// </summary>
	/// <param name="npcname"></param>
	/// <param name="eventcode"></param>
	public void AddNpcEvent(string npcname, string eventcode)
	{
		print("add event / npc : " + npcname + " / evet : " + eventcode);
		int num = GetNpcIndex(npcname);
		m_NpcItemList[num].m_NpcEventCode.Add(eventcode);
		m_NpcItemList[num].m_NpcEventCode.Sort();

		ActSetting(npcname);
	}

	public void RemoveNpcEvent(string npcname, string eventcode)
	{
		print("remove event / npc : " + npcname + " / evet : " + eventcode);
		int index = -1;
		int num = GetNpcIndex(npcname);
		for (int i = 0; i < m_NpcItemList[num].m_NpcEventCode.Count; i++)
		{
			if (m_NpcItemList[num].m_NpcEventCode[i] == eventcode)
			{
				index = i;
				break;
			}
		}

		if (index != -1)
		{
			m_NpcItemList[num].m_NpcEventCode.RemoveAt(index);
			m_NpcItemList[num].m_NpcEventCode.Sort();
			print("success to delete event " + eventcode + " in " + npcname);
		}
		ActSetting(npcname);
	}

	/// <summary>
	/// NPC 상태 변경
	/// </summary>
	/// <param name="npcname"></param>
	/// <param name="state"></param>
	public void ChangeNpcState(string npcname, NpcState state) { ChangeNpcState(GetNpcIndex(npcname), state); }

	public void ChangeNpcState(int index, NpcState state) { m_NpcItemList[index].NpcState = state; }

	/// <summary>
	/// NPC 위치 변경
	/// </summary>
	/// <param name="npcname"></param>
	/// <param name="point"></param>
	public void ChangeNpcPosition(string npcname, string point) { ChangeNpcPosition(GetNpcIndex(npcname), point); }

	public void ChangeNpcPosition(int index, string point)
	{
		//print("index : " + index + " / point : " + point + " / len : " + m_NpcItemList[index].NpcPosition);
		string prevPoint;
		string[] temp;
		prevPoint = m_NpcItemList[index].NpcPosition;

		print("index : " + m_NpcItemList[index].Name + "(" + m_NpcItemList[index].m_Index + ") / point : " + point + " / prev : " + prevPoint
				+ " / isInit : " + m_NpcItemList[index].m_IsInit);

		// 최초 초기화
		if (m_NpcItemList[index].m_IsInit == false)
		{
			if (point.Contains("_"))
			{
				temp = point.Split('_');
				switch (temp[0])
				{
					case "0":
						PlaceManager.instance.m_HomeList[int.Parse(temp[1])].AddCharacter(m_NpcItemList[index].m_Index);
						break;
					case "1":
						PlaceManager.instance.m_CompanyList[int.Parse(temp[1])].AddCharacter(m_NpcItemList[index].m_Index);
						break;
					case "2":
						PlaceManager.instance.m_ExtraPlaceList[int.Parse(temp[1])].AddCharacter(m_NpcItemList[index].m_Index);
						break;
					case "3":
						PlaceManager.instance.m_CasePlaceList[int.Parse(temp[1])].AddCharacter(m_NpcItemList[index].m_Index);
						break;
				}
				m_NpcItemList[index].NpcPosition = point;
			}
			else if (point == "Missing")
			{
				print("index : " + index);
			}
			//m_NpcItemList[index].NpcPosition = point;

			if (m_NpcItemList[index].m_Index == GlobalValue.instance.m_DialogTarget)
			{
				GlobalValue.instance.m_IsNpcForcedMoveFlag = true;
			}

			m_NpcItemList[index].m_IsInit = true;
		}
		else if (prevPoint != point)
		{
			/*if (point.Contains("_"))
{
	temp = point.Split('_');
	switch (temp[0])
	{
	case "0":
		PlaceManager.instance.m_HomeList[int.Parse(temp[1])].AddCharacter(m_NpcItemList[index].m_Index);
		break;
	case "1":
		PlaceManager.instance.m_CompanyList[int.Parse(temp[1])].AddCharacter(m_NpcItemList[index].m_Index);
		break;
	case "2":
		PlaceManager.instance.m_ExtraPlaceList[int.Parse(temp[1])].AddCharacter(m_NpcItemList[index].m_Index);
		break;
	}
	m_NpcItemList[index].NpcPosition = point;
}
else if(point  == "Missing")
{
	print("index : " + index);
}
*/
			if (m_NpcItemList[index].NpcPosition != null)
			{
				temp = m_NpcItemList[index].NpcPosition.Split('_');
				switch (temp[0])
				{
					case "0":
						PlaceManager.instance.m_HomeList[int.Parse(temp[1])].RemoveCharacter(m_NpcItemList[index].m_Index);
						break;
					case "1":
						PlaceManager.instance.m_CompanyList[int.Parse(temp[1])].RemoveCharacter(m_NpcItemList[index].m_Index);
						break;
					case "2":
						PlaceManager.instance.m_ExtraPlaceList[int.Parse(temp[1])].RemoveCharacter(m_NpcItemList[index].m_Index);
						break;
					case "3":
						PlaceManager.instance.m_CasePlaceList[int.Parse(temp[1])].RemoveCharacter(m_NpcItemList[index].m_Index);
						break;
				}
			}

			if (point.Contains("_"))
			{
				temp = point.Split('_');
				switch (temp[0])
				{
					case "0":
						PlaceManager.instance.m_HomeList[int.Parse(temp[1])].AddCharacter(m_NpcItemList[index].m_Index);
						break;
					case "1":
						PlaceManager.instance.m_CompanyList[int.Parse(temp[1])].AddCharacter(m_NpcItemList[index].m_Index);
						break;
					case "2":
						PlaceManager.instance.m_ExtraPlaceList[int.Parse(temp[1])].AddCharacter(m_NpcItemList[index].m_Index);
						break;
					case "3":
						PlaceManager.instance.m_CasePlaceList[int.Parse(temp[1])].AddCharacter(m_NpcItemList[index].m_Index);
						break;
				}
				m_NpcItemList[index].NpcPosition = point;
			}
			else if (point == "Missing")
			{
				print("index : " + index);
			}
			//m_NpcItemList[index].NpcPosition = point;

			if (m_NpcItemList[index].m_Index == GlobalValue.instance.m_DialogTarget)
			{
				GlobalValue.instance.m_IsNpcForcedMoveFlag = true;
			}
		}
	}

	public int GetNpcIndex(string npcname)
	{
		int index = -1;
		for (int i = 0; i < m_NpcItemList.Count; i++)
		{
			if (m_NpcItemList[i].m_Index == npcname)
			{
				index = i;
				break;
			}
		}
		return index;
	}

	public string GetNpcPosition(string npcname) { return m_NpcItemList[GetNpcIndex(npcname)].NpcPosition; }

	public string GetNpcPosition(int index) { return m_NpcItemList[index].NpcPosition; }

	public void LoadNpcData()
	{
		if (GlobalMethod.instance.ReturnFileExist(filePath))
		{
			BinaryDeserialize();
			ReadData();
		}
		else
		{
			DataInitialize();
			//NpcManager.instance.InitializeNpcState();
		}
	}

	public UIAtlas ReturnAtlas(string code)
	{
		int p = 0;
		for (int i = 0; i < m_NpcItemList.Count; i++)
		{
			if (m_NpcItemList[i].m_Index == code)
			{
				p = m_NpcItemList[i].m_AtlasIndex;
				break;
			}
		}
		return NpcAtlas[p];
	}

	public UIAtlas ReturnAtlas(int index)
	{
		return NpcAtlas[m_NpcItemList[index].m_AtlasIndex];
	}

	public NpcItem ReturnNpc(string code)
	{
		NpcItem item = null;
		for (int i = 0; i < m_NpcItemList.Count; i++)
		{
			if (m_NpcItemList[i].m_Index == code)
			{
				item = m_NpcItemList[i];
				break;
			}
		}
		print("code : " + code + " / item : " + item.m_AtlasIndex);
		return item;
	}

	public string ReturnName(string code)
	{
		return Localization.Get("Name_" + PlayDataManager.instance.m_StageName + "_" + code);
	}

	public string ReturnHomeName(string code)
	{
		return Localization.Get("Place_" + PlayDataManager.instance.m_StageName + "_Home_" + code);
	}

	public string ReturnJobName(string code)
	{
		return Localization.Get("Job_" + PlayDataManager.instance.m_StageName + "_" + code);
	}

	public string ReturnCompanyName(string code)
	{
		return Localization.Get("Company_" + PlayDataManager.instance.m_StageName + "_" + code);
	}

	public string ReturnCompanyAreaName(string code)
	{
		return Localization.Get(PlayDataManager.instance.m_StageName + "_Area_" + ReturnCompanyName(code));
	}


	private void WriteData()
	{
		data = new NpcData();
		data.m_NpcItem = new List<NpcItem>();
		for (int i = 0; i < m_NpcItemList.Count; i++)
		{
			m_NpcItemList[i].m_IsInit = false;

			data.m_NpcItem.Add(m_NpcItemList[i]);
			data.m_NpcItem[i].m_NpcEventCode = new List<string>();
			if (m_NpcItemList[i].m_NpcEventCode.Count > 0)
			{
				for (int k = 0; k < m_NpcItemList[i].m_NpcEventCode.Count; k++)
				{
					data.m_NpcItem[i].m_NpcEventCode.Add(m_NpcItemList[i].m_NpcEventCode[k]);
				}
			}
		}

		BinarySerialize(data);
	}

	private void ReadData()
	{
		for (int i = 0; i < data.m_NpcItem.Count; i++)
		{
			m_NpcItemList.Add(data.m_NpcItem[i]);
			m_NpcItemList[i].Name = ReturnName(m_NpcItemList[i].m_Index);
			m_NpcItemList[i].Job = ReturnJobName(m_NpcItemList[i].m_Index);
			m_NpcItemList[i].CompanyName = ReturnCompanyName(m_NpcItemList[i].m_Index);

			m_NpcItemList[i].m_NpcEventCode = new List<string>();

			if (data.m_NpcItem[i].m_NpcEventCode.Count > 0)
			{
				for (int k = 0; k < data.m_NpcItem[i].m_NpcEventCode.Count; k++)
				{
					m_NpcItemList[i].m_NpcEventCode.Add(data.m_NpcItem[i].m_NpcEventCode[k]);
				}
			}
			print("i : " + i + " / com : " + m_NpcItemList[i].m_Company);
		}
	}

	public void BinarySerialize(NpcData data)
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
		NpcData d = (NpcData)formatter.Deserialize(stream);
		data = d;
		stream.Close();
	}

	public void Save()
	{
		print("NpcDataManager Save");
		WriteData();
	}

	public void DataInitialize()
	{
		int count = NpcNode.Count;
		//List<NpcActItem> m_ActItem = new List<NpcActItem>();

		for (int i = 0; i < count; i++)
		{
			NpcItem item = new NpcItem();
			item.m_Index = NpcNode[i]["m_Index"];
			//item.m_Race = NpcNode.m_NpcItem[i].m_Race;
			//item.m_Sex = NpcNode.m_NpcItem[i].m_Sex;
			//item.m_Religion = NpcNode.m_NpcItem[i].m_Religion;
			item.m_Age = NpcNode[i]["m_Age"].AsInt;
			item.m_Height = NpcNode[i]["m_Height"].AsInt;
			item.m_AtlasIndex = NpcNode[i]["m_AtlasIndex"].AsInt;
			item.m_Company = NpcNode[i]["m_Company"].AsInt;
			item.m_Home = NpcNode[i]["m_Home"].AsInt;

			item.m_NpcEventCode = new List<string>();

			item.Name = ReturnName(item.m_Index);
			item.Job = ReturnJobName(item.m_Index);
			item.CompanyName = ReturnCompanyName(item.m_Index);

			m_NpcItemList.Add(item);
		}



		/* string path = Application.dataPath + "/Resources/Data/" + PlayDataManager.instance.m_StageName + "/NpcData.bin";
		 BinaryFormatter formatter = new BinaryFormatter();
		 FileStream stream = new FileStream(path, FileMode.Open);
		 NpcData NpcDataBinary = (NpcData)formatter.Deserialize(stream);
		 stream.Close();

		 for (int i = 0; i < NpcDataBinary.m_NpcItem.Count; i++)
		 {
				 NpcItem item = new NpcItem();
				 item.m_Index = NpcDataBinary.m_NpcItem[i].m_Index;
				 item.m_Race = NpcDataBinary.m_NpcItem[i].m_Race;
				 item.m_Sex = NpcDataBinary.m_NpcItem[i].m_Sex;
				 item.m_Religion = NpcDataBinary.m_NpcItem[i].m_Religion;
				 item.m_Age = NpcDataBinary.m_NpcItem[i].m_Age;
				 item.m_Height = NpcDataBinary.m_NpcItem[i].m_Height;
				 item.m_AtlasIndex = NpcDataBinary.m_NpcItem[i].m_AtlasIndex;
				 item.m_Company = NpcDataBinary.m_NpcItem[i].m_Company;
				 item.m_Home = NpcDataBinary.m_NpcItem[i].m_Home;

				 item.m_NpcEventCode = new List<string>();

				 item.Name = ReturnName(item.m_Index);
				 item.Job = ReturnJobName(item.m_Index);
				 item.CompanyName = ReturnCompanyName(item.m_Index);

				 m_NpcItemList.Add(item);
		 }*/


	}

}

public class NpcActItem
{
	public int m_StartHour;
	public int m_StartMinute;
	public int m_EndHour;
	public int m_EndMinute;
	public string m_Position;
	public int m_Order;
}
