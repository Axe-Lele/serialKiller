using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlaceDataManager : Singleton<PlaceDataManager>
{
	private string filePath;

	private PlaceSaveData m_Datas;  // 가공되지 않은 직렬화 데이터
	public List<PlaceData2> m_PlaceDatas;

	private JSONNode PlaceNode;
	public TextAsset PlaceTextAsset;

	#region Canceled FM
	//public bool[] m_IsOpenHome;
	//public bool[] m_IsOpenCompany;
	//public bool[] m_IsOpenExtraPlace;
	//public bool[] m_IsOpenCasePlace;
	#endregion

	private void Awake()
	{
		filePath = Application.persistentDataPath + "/data05.bin";
		PlaceNode = JSONNode.Parse(PlaceTextAsset.text);

		// prev
		//m_IsOpenHome = new bool[PlaceManager.instance.m_HomeList.Count];
		//m_IsOpenCompany = new bool[PlaceManager.instance.m_CompanyList.Count];
		//m_IsOpenExtraPlace = new bool[PlaceManager.instance.m_ExtraPlaceList.Count];
		//m_IsOpenCasePlace = new bool[PlaceManager.instance.m_CasePlaceList.Count];
	}

	public void LoadPlaceData()
	{
		int CC = StageDataManager.instance.m_CriminalCode;
		if (CC == -1)
		{
			print("[Test Mode] Load Place Data");
			CC = 0;
		}
		PlaceNode = PlaceNode[CC];

		m_PlaceDatas = new List<PlaceData2>();
		if (GlobalMethod.instance.ReturnFileExist(filePath))
		{
			BinaryDeserialize();
			ReadData();
		}
		else
		{
			DataInitialize();
		}
	}

	#region Private Method
	private PlaceData2 GetPlaceData(PlaceType type, int index)
	{
		PlaceData2 _place = null;

		for (int idx = 0; idx < m_PlaceDatas.Count; idx++)
		{
			if (m_PlaceDatas[idx].Type.CompareTo(type.ToString()) != 0)
				continue;

			if (m_PlaceDatas[idx].Index != index)
				continue;

			_place = m_PlaceDatas[idx];
			break;
		}

		return _place;
	}
	#endregion

	#region Public Method
	public bool IsOpened(PlaceType type, int index)
	{
		PlaceData2 _place = GetPlaceData(type, index);

		if (_place == null)
		{
			print("Can't find " + type.ToString() + "_" + index + " PlaceData.");
			return false;
		}

		return _place.IsOpened;
	}

	public bool CanSearched(PlaceType type, int index)
	{
		PlaceData2 _place = GetPlaceData(type, index);

		if (_place == null)
		{
			print("Can't find " + type.ToString() + "_" + index + " PlaceData.");
			return false;
		}

		if (_place.Evidences.Count == 0)
			return false;

		return true;
	}

	public bool CanSearch(string code)
	{
		string[] tempstr = code.Split('_');
		PlaceType type = tempstr[0].ToEnum<PlaceType>();
		int index = tempstr[1].ToInt();

		return this.CanSearched(type, index);
	}

	public void AddEvidence(string place, string evidence)
	{
		string[] tempstr = place.Split('_');
		PlaceType type = tempstr[0].ToEnum<PlaceType>();
		int index = tempstr[1].ToInt();

		this.AddEvidence(type, index, evidence);
	}

	public void AddEvidence(PlaceType type, int index, string evidence)
	{
		PlaceData2 _place = GetPlaceData(type, index);

		if (_place == null)
		{
			print("Can't find " + type.ToString() + "_" + index + " PlaceData.");
			return;
		}

		_place.Evidences.Add(evidence);
		PlaceManager.instance.GetPlace(type, index).AddEvidence(evidence);
	}

	public List<string> GetEvidences(PlaceType type, int index)
	{
		PlaceData2 _place = GetPlaceData(type, index);
		if (_place == null)
		{
			//print("Can't find " + type.ToString() + "_" + index + " PlaceData.");
			return null;
		}
		return _place.Evidences;
	}

	public void RemoveEvidence(PlaceType type, int index, string evidence)
	{
		PlaceData2 _place = GetPlaceData(type, index);
		if (_place == null)
		{
			print("Can't find " + type.ToString() + "_" + index + " PlaceData.");
			return;
		}

		_place.Evidences.Remove(evidence);
	}

	public void ControlPlaceIsOpened(string code, bool b)
	{
		string[] tempstr = code.Split('_');
		PlaceType type = tempstr[0].ToEnum<PlaceType>();
		int index = tempstr[1].ToInt();

		this.ControlPlaceIsOpened(type, index, b);
	}

	public void ControlPlaceIsOpened(PlaceType type, int index, bool b)
	{
		print("[Contorl Place IsOpened] type : " + type + " / index : " + index + " / b : " + b);

		PlaceData2 _changedPlace = GetPlaceData(type, index);

		if (_changedPlace == null)
		{
			print("Can't find " + type.ToString() + "_" + index + " PlaceData.");
			return;
		}

		_changedPlace.IsOpened = true;

		PlaceManager.instance.ControlPlaceButton(type, index, b);
		return;

		#region prev
		//switch (type)
		//{
		//	case PlaceType.Home:
		//		m_IsOpenHome[index] = b;
		//		break;
		//	case PlaceType.Company:
		//		m_IsOpenCompany[index] = b;
		//		break;
		//	case PlaceType.Extra:
		//		m_IsOpenExtraPlace[index] = b;
		//		break;
		//	case PlaceType.Case:
		//		//print("len : " + StageDataManager.instance.m_CaseList.Count + " / " + StageDataManager.instance.m_CaseList[0]);
		//		//print("index : " + index + " / b : " + StageDataManager.instance.m_CaseList[index] + " / loca : " + StageDataManager.instance.m_CaseDataItemList[index].CaseLocation);
		//		m_IsOpenCasePlace[index] = b;
		//		print("index : " + index + " / b : " + b);
		//		//m_IsOpenCasePlace[StageDataManager.instance.m_CaseDataItemList[index].CaseLocation] = b;

		//		//  PlaceManager.instance.m_CasePlaceList[].SetCase(StageDataManager.instance.m_CaseList[index]);
		//		break;
		//}
		#endregion
	}

	public void ControlPlaceIsSearched(string code, bool b)
	{
		string[] tempstr = code.Split('_');
		PlaceType type = tempstr[0].ToEnum<PlaceType>();
		int index = tempstr[1].ToInt();

		this.ControlPlaceIsSearched(type, index, b);
	}

	public void ControlPlaceIsSearched(PlaceType type, int index, bool b)
	{
		//print("[Contorl Place IsSearched] type : " + type + " / index : " + index + " / " + b);

		PlaceData2 _changedPlace = GetPlaceData(type, index);

		if (_changedPlace == null)
		{
			print("Can't find " + type.ToString() + "_" + index + " PlaceData.");
			return;
		}

		_changedPlace.IsSearched = b;
		return;
	}

	public int GetAtlasIndex(string place)
	{
		string[] temp = place.Split('_');
		return PlaceNode[temp[0]][temp[1]]["Atlas"].AsInt;
	}

	public string GetSpriteIndex(string place)
	{
		string[] temp = place.Split('_');
		return PlaceNode[temp[0]][temp[1]]["Sprite"].Value;
	}
	#endregion


	#region Data Serialized

	private void WriteData()
	{
		m_Datas = new PlaceSaveData();
		m_Datas.Places = m_PlaceDatas;

		BinarySerialize(m_Datas);
		return;

		#region prev
		//prev_placedata = new PlaceData();

		//prev_placedata.m_IsOpenHome = new bool[m_IsOpenHome.Length];
		//for (int i = 0; i < m_IsOpenHome.Length; i++)
		//{
		//	prev_placedata.m_IsOpenHome[i] = m_IsOpenHome[i];
		//}

		//prev_placedata.m_IsOpenCompany = new bool[m_IsOpenCompany.Length];
		//for (int i = 0; i < m_IsOpenCompany.Length; i++)
		//{
		//	prev_placedata.m_IsOpenCompany[i] = m_IsOpenCompany[i];
		//}

		//prev_placedata.m_IsOpenExtraPlace = new bool[m_IsOpenExtraPlace.Length];
		//for (int i = 0; i < m_IsOpenExtraPlace.Length; i++)
		//{
		//	prev_placedata.m_IsOpenExtraPlace[i] = m_IsOpenExtraPlace[i];
		//}

		//prev_placedata.m_IsOpenCasePlace = new bool[m_IsOpenCasePlace.Length];
		//for (int i = 0; i < m_IsOpenCasePlace.Length; i++)
		//{
		//	prev_placedata.m_IsOpenCasePlace[i] = m_IsOpenCasePlace[i];
		//}
		#endregion

	}

	private void ReadData()
	{
		m_PlaceDatas = m_Datas.Places;
		return;

		#region prev
		//for (int i = 0; i < m_IsOpenHome.Length; i++)
		//{
		//	m_IsOpenHome[i] = prev_placedata.m_IsOpenHome[i];
		//}
		//print("len : " + m_IsOpenCompany.Length);
		//for (int i = 0; i < m_IsOpenCompany.Length; i++)
		//{
		//	m_IsOpenCompany[i] = prev_placedata.m_IsOpenCompany[i];
		//}

		//for (int i = 0; i < m_IsOpenExtraPlace.Length; i++)
		//{
		//	m_IsOpenExtraPlace[i] = prev_placedata.m_IsOpenExtraPlace[i];
		//}

		//for (int i = 0; i < m_IsOpenCasePlace.Length; i++)
		//{
		//	m_IsOpenCasePlace[i] = prev_placedata.m_IsOpenCasePlace[i];
		//	//if (m_IsOpenCasePlace[i] == true)
		//	//{
		//	//    print("i : " + i);
		//	//    //ControlPlace(PlaceType.Case, StageDataManager.instance.m_CaseDataItemList[i].CaseLocation, true);
		//	//    ControlPlace(PlaceType.Case, EventDataManager.instance.ReturnCaseLocation(i), true);
		//	//}
		//}
		#endregion
	}

	private void DataInitialize()
	{
		PlaceData2 item = null;
		string evidence = string.Empty;
		for (int _type = 0; _type < PlaceNode.Count; _type++)
		{
			for (int _num = 0; _num < PlaceNode[_type].Count; _num++)
			{
				item = new PlaceData2();
				item.Index = PlaceNode[_type].GetKey(_num).ToInt();
				item.Type = PlaceNode.GetKey(_type);
				item.IsOpened = PlaceNode[_type][_num]["IsOpen"].AsBool;
				item.IsSearched = false;
				item.Evidences = new List<string>();
				for (int i = 0; i < PlaceNode[_type][_num]["Evidence"].Count; i++)
				{
					evidence = PlaceNode[_type][_num]["Evidence"][i];
					item.Evidences.Add(evidence);
					print("Add Evidence on Place : [" + evidence + "] on [" 
						+ item.Type + "_" + item.Index + "]");
				}
				m_PlaceDatas.Add(item);

				item = null;
			}
		}
		return;

		#region prev
		//for (int i = 0; i < m_IsOpenHome.Length; i++)
		//{
		//	m_IsOpenHome[i] = PlaceNode["Home"][i.ToString()]["IsOpen"].AsBool;
		//}

		//for (int i = 0; i < m_IsOpenCompany.Length; i++)
		//{
		//	m_IsOpenCompany[i] = PlaceNode["Company"][i.ToString()]["IsOpen"].AsBool; ;
		//}

		//for (int i = 0; i < m_IsOpenExtraPlace.Length; i++)
		//{
		//	m_IsOpenExtraPlace[i] = PlaceNode["Extra"][i.ToString()]["IsOpen"].AsBool; ;
		//}

		//for (int i = 0; i < m_IsOpenCasePlace.Length; i++)
		//{
		//	m_IsOpenCasePlace[i] = PlaceNode["Case"][i.ToString()]["IsOpen"].AsBool; ;
		//}
		#endregion
	}

	public void BinarySerialize(PlaceSaveData data)
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
		PlaceSaveData d = (PlaceSaveData)formatter.Deserialize(stream);
		m_Datas = d;
		stream.Close();
	}

	public void Save()
	{
		print("PlaceDataManager Save");
		WriteData();
	}

	#endregion

	/* public void SetCaseEvent(string caseindex)
	 {
			 print("case index : " + caseindex);
			 int len = PlaceNode["Case"][StageDataManager.instance.m_CriminalCode][caseindex].Count;
			 string[] m_Event = new string[len];

			 for (int i = 0; i < len; i++)
			 {
					 m_Event[i] = PlaceNode["Case"][StageDataManager.instance.m_CriminalCode][caseindex][i];
			 }

			 for (int i = 0; i < len; i++)
			 {
					 string[] str = m_Event[i].Split('-');
					 switch ((GameEventType)Enum.Parse(typeof(GameEventType), str[0]))
					 {
							 case GameEventType.AddArea:
									 string[] AreaTemp = str[1].Split('_');
									 ControlPlace((PlaceType)Enum.Parse(typeof(PlaceType), AreaTemp[0]), int.Parse(AreaTemp[1]), true);
									 break;
							 case GameEventType.AddNews:
									 //EventDataManager.instance.InputNewsEventData( (0, str[1], (int)GameEventType.AddNews, "");
									 break;
							 case GameEventType.AddSelection:
									 string[] SelectionTemp = str[1].Split('_');
									 SelectionDataManager.instance.InputSelection(SelectionTemp[0], SelectionTemp[1]);
									 break;
							 case GameEventType.Dialog:
									 string[] DialogTemp = str[1].Split('_');
									 GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.Dialog, DialogTemp[0], DialogTemp[1]);
									 break;
					 }
			 }
	 }*/
}
