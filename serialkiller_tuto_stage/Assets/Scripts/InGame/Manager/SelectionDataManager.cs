using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SelectionDataManager : Singleton<SelectionDataManager>
{

	public List<SelectionDataItem> m_SelectionDataItemList;

	SelectionData selectiondata;
	private string filePath;

	private JSONNode SelectionNode;
	public TextAsset SelectionTextAsset;

	public bool m_IsWorld = false;

	private void Awake()
	{
		if (m_IsWorld)
			filePath = Application.persistentDataPath + "/data03_W.bin";
		else
			filePath = Application.persistentDataPath + "/data03_S.bin";

		m_SelectionDataItemList = new List<SelectionDataItem>();
	}


	public void LoadSelectionData()
	{
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

	private void WriteData()
	{
		selectiondata = new SelectionData();

		selectiondata.m_SelectionDataItemList = new List<SelectionDataItem>();
		for (int i = 0; i < m_SelectionDataItemList.Count; i++)
		{
			SelectionDataItem item = new SelectionDataItem();
			item.m_Index = m_SelectionDataItemList[i].m_Index;

			item.m_Selection = new List<string>();
			item.m_IsReadSelection = new List<bool>();
			for (int k = 0; k < m_SelectionDataItemList[i].m_Selection.Count; k++)
			{
				item.m_Selection.Add(m_SelectionDataItemList[i].m_Selection[k]);
				item.m_IsReadSelection.Add(m_SelectionDataItemList[i].m_IsReadSelection[k]);
			}
			selectiondata.m_SelectionDataItemList.Add(item);
			item = null;
		}
		BinarySerialize(selectiondata);
	}

	private void ReadData()
	{
		for (int i = 0; i < selectiondata.m_SelectionDataItemList.Count; i++)
		{
			SelectionDataItem item = new SelectionDataItem();
			item.m_Index = selectiondata.m_SelectionDataItemList[i].m_Index;

			item.m_Selection = new List<string>();
			item.m_IsReadSelection = new List<bool>();

			for (int k = 0; k < selectiondata.m_SelectionDataItemList[i].m_Selection.Count; k++)
			{
				item.m_Selection.Add(selectiondata.m_SelectionDataItemList[i].m_Selection[k]);
				item.m_IsReadSelection.Add(selectiondata.m_SelectionDataItemList[i].m_IsReadSelection[k]);
			}

			m_SelectionDataItemList.Add(item);
			item = null;
		}
	}

	private void DataInitialize()
	{
		SelectionNode = JSONNode.Parse(SelectionTextAsset.text);

		for (int i = 0; i < SelectionNode.Count; i++)
		{
			SelectionDataItem item = new SelectionDataItem();
			item.m_Index = SelectionNode[i]["m_Index"];
			item.m_Selection = new List<string>();
			item.m_IsReadSelection = new List<bool>();

			string _criminal = string.Empty;

			if (StageDataManager.instance == null)
				_criminal = "0";
			else if (StageDataManager.instance.m_CriminalCode == -1)
				_criminal = "0";
			else
				_criminal = StageDataManager.instance.m_CriminalCode.ToString();

			for (int k = 0; k < SelectionNode[i][_criminal]["m_Selection"].Count; k++)
			{
				item.m_Selection.Add(SelectionNode[i][_criminal]["m_Selection"][k]);
				item.m_IsReadSelection.Add(false);
			}

			m_SelectionDataItemList.Add(item);

			item = null;

		}
	}

	public void InputSelection(string who, string select)
	{
		bool b = ReturnIsCharacter(who);
		print("who : " + who + " / select : " + select);
		if (b == true)
		{
			bool isHave = false;
			int index = ReturnCharacterIndex(who);

			for (int i = 0; i < m_SelectionDataItemList[index].m_Selection.Count; i++)
			{
				if (m_SelectionDataItemList[index].m_Selection[i] == select)
				{
					isHave = true;
					break;
				}
			}

			if (isHave == false)
			{
				m_SelectionDataItemList[index].m_Selection.Add(select);
				m_SelectionDataItemList[index].m_IsReadSelection.Add(false);
			}
		}
		else
		{
			SelectionDataItem item = new SelectionDataItem();
			item.m_Index = who;
			item.m_Selection = new List<string>();
			item.m_Selection.Add(select);
			item.m_IsReadSelection = new List<bool>();
			item.m_IsReadSelection.Add(false);

			m_SelectionDataItemList.Add(item);
			print("i can not find who");
			item = null;
		}
	}

	public void RemoveSelection(string who, string select)
	{
		int index = ReturnCharacterIndex(who);

		// index 값이 0보다 작으면 캐릭터 리스트에 찾는 인물이 없다는 것.
		if (index < 0)
		{
			print("error! i can not find who");
		}
		else
		{
			for (int i = 0; i < m_SelectionDataItemList[index].m_Selection.Count; i++)
			{
				if (m_SelectionDataItemList[index].m_Selection[i] == select)
				{
					m_SelectionDataItemList[index].m_Selection.RemoveAt(i);
					m_SelectionDataItemList[index].m_IsReadSelection.RemoveAt(i);
					break;
				}
			}
		}
	}

	public void SwapSelection(string who, string select1, string select2)
	{
		int index = ReturnCharacterIndex(who);

		// index 값이 0보다 작으면 캐릭터 리스트에 찾는 인물이 없다는 것.
		if (index < 0)
		{
			print("error! i can not find who");
		}
		else
		{
			for (int i = 0; i < m_SelectionDataItemList[index].m_Selection.Count; i++)
			{
				if (m_SelectionDataItemList[index].m_Selection[i] == select1)
				{
					m_SelectionDataItemList[index].m_Selection[i] = select2;
					m_SelectionDataItemList[index].m_IsReadSelection[i] = false;
					break;
				}
			}
		}
	}

	// 해당 선택지에 대해서 물어본적이 있는지
	public bool ReturnCheckRead(string who, string dialog)
	{
		bool b = false;
		int index = ReturnCharacterIndex(who);

		for (int i = 0; i < m_SelectionDataItemList.Count; i++)
		{
			if (m_SelectionDataItemList[index].m_Selection[i] == dialog)
			{
				b = m_SelectionDataItemList[index].m_IsReadSelection[i];
				break;
			}
		}
		return b;
	}

	public int ReturnReadSelectionIndex(string who, string dialog)
	{
		int index = ReturnCharacterIndex(who);
		int k = -1;
		for (int i = 0; i < m_SelectionDataItemList.Count; i++)
		{
			if (m_SelectionDataItemList[index].m_Selection[i] == dialog)
			{
				k = i;
				break;
			}
		}
		return k;
	}

	// 캐릭터가 몇 번째에 있는지
	public int ReturnCharacterIndex(string who)
	{
		int index = -1;
		for (int i = 0; i < m_SelectionDataItemList.Count; i++)
		{
			if (m_SelectionDataItemList[i].m_Index == who)
			{
				index = i;
				break;
			}
		}
		return index;
	}

	private bool ReturnIsCharacter(string who)
	{
		bool b = false;
		for (int i = 0; i < m_SelectionDataItemList.Count; i++)
		{
			if (m_SelectionDataItemList[i].m_Index == who)
			{
				b = true;
				break;
			}
		}
		return b;
	}

	public void BinarySerialize(SelectionData data)
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
		SelectionData d = (SelectionData)formatter.Deserialize(stream);
		selectiondata = d;
		stream.Close();
	}

	public void Save()
	{
		print("SelectionDataManager Save");
		WriteData();
	}
}
