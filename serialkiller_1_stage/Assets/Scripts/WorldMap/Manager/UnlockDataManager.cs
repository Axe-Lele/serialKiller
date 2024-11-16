using SimpleJSON;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class UnlockDataManager : Singleton<UnlockDataManager>
{
	[Header("Data")]
	private UnlockData data;
	private string filePath;
	private JSONNode DataNode;
	[SerializeField]
	private TextAsset UnlockTextAsset;

	public List<UnlockDataItem> m_UnlockData;

	private void Awake()
	{
		DontDestroyOnLoad(this);

		filePath = Application.persistentDataPath + "/data21.bin";

		UnlockTextAsset = Resources.Load<TextAsset>("Data/WorldMap/WorldMap_Trigger");
		DataNode = JSONNode.Parse(UnlockTextAsset.text);
	}

	public void Init()
	{
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

	#region Data
	public void ReadData()
	{
		m_UnlockData = new List<UnlockDataItem>();
		UnlockDataItem _item = null;

		for (int i = 0; i < data.Items.Count; i++)
		{
			_item = new UnlockDataItem();
			_item.Index = data.Items[i].Index;
			_item.Type = data.Items[i].Type;
			_item.IsOpen = data.Items[i].IsOpen;

			_item.Pay = data.Items[i].Pay;
			_item.UnlockStage = data.Items[i].UnlockStage;
			_item.ClearStage = data.Items[i].ClearStage;
			_item.ComplishStage = data.Items[i].ComplishStage;
			_item.Complish = data.Items[i].Complish;

			m_UnlockData.Add(_item);

			_item = null;
		}
	}

	public void CreateData()
	{
		print("Create WorldInfo Data");
		data = new UnlockData();
		data.Items = new List<UnlockDataItem>();

		UnlockDataItem _item;
		for (int i = 0; i < DataNode.Count; i++)
		{
			_item = new UnlockDataItem();
			_item.Index = DataNode[i]["Index"].Value;
			_item.Type = DataNode[i]["Type"].Value;
			_item.IsOpen = false;

			_item.ComplishStage = string.Empty;
			_item.Complish = -1;
			_item.Pay = -1;
			_item.UnlockStage = string.Empty;
			_item.ClearStage = string.Empty;

			if (_item.Type.Equals("Complish"))
			{
				_item.ComplishStage = DataNode[i]["Temp"].Value;
				_item.Complish = DataNode[i]["Temp2"].AsInt;
			}
			else if (_item.Type.Equals("Pay"))
			{
				_item.Pay = DataNode[i]["Temp"].AsInt;
			}
			else if (_item.Type.Equals("Unlock"))
			{
				_item.UnlockStage = DataNode[i]["Temp"].Value;
			}
			else if (_item.Type.Equals("Clear"))
			{
				_item.ClearStage = DataNode[i]["Temp"].Value;
			}
			else
			{
				_item = null;
				continue;
			}

			data.Items.Add(_item);

			_item = null;
		}

		BinarySerialize(data);
	}

	public void BinarySerialize(UnlockData data)
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
		UnlockData d = (UnlockData)formatter.Deserialize(stream);
		data = d;
		stream.Close();
	}
	#endregion
}
