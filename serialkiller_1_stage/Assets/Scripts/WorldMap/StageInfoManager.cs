using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleJSON;
using System.Security.Cryptography;

public class StageInfoManager : Singleton<StageInfoManager>
{
	[Header("Binary Data")]
	private WorldStageData data;
	private string filePath;
	private JSONNode CityNode;
	public TextAsset CityTextAsset;

	[Header("Data Field")]
	public GameObject m_CityItem_Origin;
	public List<CityData> m_CityDatas;

	[Header("Ingame Param")]
	public GameObject m_CityItemRoot;
	public List<GameObject> m_CityItems;
	public List<GameObject> m_CityParents;
	public List<GameObject> m_CityChecks;
	public List<UISprite> m_CitySprites;

	private void Awake()
	{
		filePath = Application.persistentDataPath + "/data20.bin";

		CityTextAsset = Resources.Load<TextAsset>("Data/WorldMap/WorldMap_Stage");
		CityNode = JSONNode.Parse(CityTextAsset.text);

		//CreateCity();
	}

	public void LoadData()
	{
		// Load Savedata
		if (GlobalMethod.instance.ReturnFileExist(filePath))
		{
			BinaryDeserialize();
			ReadData();
		}
		else
		{
			CreateData();
			ReadData();
		}
	}

	public void Init()
	{
		CreateCity();
	}

	private void CreateCity()
	{
		if (m_CityItems == null)
			m_CityItems = new List<GameObject>();

		// Create Items
		GameObject _item = null;
		for (int i = 0; i < m_CityDatas.Count; i++)
		{
			_item = Instantiate(m_CityItem_Origin, m_CityItemRoot.transform);
			StageItem _info = _item.GetComponent<StageItem>();
			_info.m_Index = i;
			_info.m_IsUnlock = m_CityDatas[i].IsOpen;

			m_CityItems.Add(_item);
			if (m_CityParents[i] != null)
			{
				_item.transform.SetParent(m_CityParents[i].transform);
				_item.transform.localPosition = Vector3.zero;
				Vector3 pos = _item.transform.position;
				_item.transform.parent = m_CityItemRoot.transform;
				_item.transform.position = pos;
			}

			_info.Init();
		}
	}

	public string GetUnlockIndex(int index)
	{
		string rvalue = string.Empty;

		for (int i = 0; i < m_CityDatas.Count; i++)
		{
			if (m_CityDatas[i].Index != index)
				continue;

			for (int j = 0; j < m_CityDatas[i].UnlockIndexes.Length; j++)
			{
				rvalue += "\n";
				rvalue += Localization.Get(string.Format("Text_World_Lock_{0}", m_CityDatas[i].UnlockIndexes[j]));

			}
			return rvalue;
		}

		return string.Empty;
	}

	public bool GetCityIsOpen(int index)
	{
		for (int i = 0; i < m_CityDatas.Count; i++)
		{
			if (m_CityDatas[i].Index != index)
				continue;

			return m_CityDatas[i].IsOpen;
		}
		return false;
	}

	public int GetCaseCount(int index)
	{
		for (int i = 0; i < m_CityDatas.Count; i++)
		{
			if (m_CityDatas[i].Index != index)
				continue;

			return m_CityDatas[i].IsOpenStage.Length;
		}
		return 0;
	}

	#region Data
	/// <summary>
	/// ingame param -> data
	/// </summary>
	private void WriteData()
	{
		data = new WorldStageData();
		data.Cities = new List<CityData>();
		CityData _item = null;
		for (int i = 0; i < data.Cities.Count; i++)
		{
			_item = new CityData();
			_item.Index = m_CityDatas[i].Index;
			_item.IsOpen = m_CityDatas[i].IsOpen;
			_item.IsOpenStage = new bool[m_CityDatas[i].IsOpenStage.Length];

			for (int stageIdx = 0; stageIdx < m_CityDatas[i].IsOpenStage.Length; stageIdx++)
			{
				_item.IsOpenStage[stageIdx] = m_CityDatas[i].IsOpenStage[stageIdx];
			}

			_item.UnlockIndexes = new string[m_CityDatas[i].UnlockIndexes.Length];
			for (int j = 0; j < m_CityDatas[i].UnlockIndexes.Length; j++)
			{
				_item.UnlockIndexes[j] = m_CityDatas[i].UnlockIndexes[j];
			}
			data.Cities.Add(_item);
			_item = null;
		}

		BinarySerialize(data);
	}

	/// <summary>
	/// data -> ingame param
	/// </summary>
	private void ReadData()
	{
		m_CityDatas = new List<CityData>();
		CityData _item = null;
		for (int ci = 0; ci < data.Cities.Count; ci++)
		{
			_item = new CityData();
			_item.Index = data.Cities[ci].Index;
			_item.IsOpen = data.Cities[ci].IsOpen;
			_item.IsOpenStage = new bool[data.Cities[ci].IsOpenStage.Length];
			for (int si = 0; si < data.Cities[ci].IsOpenStage.Length; si++)
			{
				_item.IsOpenStage[si] = data.Cities[ci].IsOpenStage[si];
			}

			_item.UnlockIndexes = new string[data.Cities[ci].UnlockIndexes.Length];
			for (int ui = 0; ui < data.Cities[ci].UnlockIndexes.Length; ui++)
			{
				_item.UnlockIndexes[ui] = data.Cities[ci].UnlockIndexes[ui];
			}

			m_CityDatas.Add(data.Cities[ci]);
			_item = null;
		}
	}

	/// <summary>
	/// json -> data
	/// </summary>
	private void CreateData()
	{
		print("Create WorldInfo Data");
		data = new WorldStageData();
		data.Cities = new List<CityData>();

		CityData _item;
		for (int cityIdx = 0; cityIdx < CityNode.Count; cityIdx++)
		{
			_item = new CityData();
			_item.Index = cityIdx;
			_item.IsOpen = CityNode[cityIdx]["IsOpen"].AsBool;
			_item.IsOpenStage = new bool[CityNode[cityIdx].Count - 2];
			_item.IsFirstStage = new bool[CityNode[cityIdx].Count - 2];
			for (int stageIdx = 2; stageIdx < CityNode[cityIdx].Count; stageIdx++)
			{
				_item.IsOpenStage[stageIdx - 2] = CityNode[cityIdx][stageIdx]["IsOpen"].AsBool;
			}

			_item.UnlockIndexes = new string[CityNode[cityIdx]["Unlock"].Count];
			for (int ui = 0; ui < CityNode[cityIdx]["Unlock"].Count; ui++)
			{
				JSONNode node = CityNode[cityIdx]["Unlock"];
				_item.UnlockIndexes[ui] = node[ui];
			}

			data.Cities.Add(_item);
		}

		BinarySerialize(data);
	}

	public void BinarySerialize(WorldStageData data)
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

	private void BinaryDeserialize()
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(filePath, FileMode.Open);
		WorldStageData d = (WorldStageData)formatter.Deserialize(stream);
		data = d;
		stream.Close();
	}
	#endregion
}
