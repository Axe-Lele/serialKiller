using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleJSON;
using System.Security.Cryptography;
public class LaboratoryDataManager : Singleton<LaboratoryDataManager>
{
	LaboratoryData laboratoryData;
	private string filePath;

	private JSONNode LaboratoryNode;
	public TextAsset LaboratoryTextAsset;

	// 분석
	public string[] m_AnalyzedIndexList;
	public int m_AnalyzedRemainTime;

	// 매치
	public string[] m_MatchedList;
	public int m_MatchedRemainTime;
	public bool m_IsStartMatched;

	public List<string> m_MatchItemA;
	public List<string> m_MatchItemB;
	public List<bool> m_MatchResult;

	private void Awake()
	{
		filePath = Application.persistentDataPath + "/data09.bin";
		m_MatchedList = new string[2];

		m_MatchItemA = new List<string>();
		m_MatchItemB = new List<string>();
		m_MatchResult = new List<bool>();
		
		for (int i = 0; i < m_MatchedList.Length; i++)
		{
			m_MatchedList[i] = string.Empty;
		}
	}

	private void Start()
	{
		m_AnalyzedIndexList = new string[LaboratoryManager.instance.m_AnalyzedManager.m_ReserveItemList.Length];
		for (int i = 0; i < m_AnalyzedIndexList.Length; i++)
		{
			m_AnalyzedIndexList[i] = "";
		}
	}

	public bool ContainAnalyzedIndexList(string itemCode)
	{
		for(int i = 0; i < m_AnalyzedIndexList.Length; i++)
		{
			if(m_AnalyzedIndexList[i].Equals(itemCode))
			{
				return true;
			}
		}
		return false;
	}

	public string ReturnAnalyzedIIndexList(int i)
	{
		return m_AnalyzedIndexList[i];
	}

	public void SetAnalyzedIndexList(int i, string code)
	{
		m_AnalyzedIndexList[i] = code;
	}

	public string ReturnMatchedIndexList(int i)
	{
		return m_MatchedList[i];
	}

	public void SetMatchedIndexList(int i, string code)
	{
		m_MatchedList[i] = code;
	}

	public void InputMatchResult(string a, string b, bool result)
	{
		m_MatchItemA.Add(a);
		m_MatchItemB.Add(b);
		m_MatchResult.Add(result);
	}

	public void LoadLaboratoryData()
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
		laboratoryData = new LaboratoryData();

		laboratoryData.m_AnalyzedRemainTime = m_AnalyzedRemainTime;
		laboratoryData.m_MatchedRemainTime = m_MatchedRemainTime;
		laboratoryData.m_IsStartMatched = m_IsStartMatched;

		laboratoryData.m_AnalyzedIndexList = new string[m_AnalyzedIndexList.Length];
		for (int i = 0; i < m_AnalyzedIndexList.Length; i++)
		{
			laboratoryData.m_AnalyzedIndexList[i] = m_AnalyzedIndexList[i];
		}

		laboratoryData.m_MatchedList = new string[m_MatchedList.Length];
		for (int i = 0; i < m_MatchedList.Length; i++)
		{
			laboratoryData.m_MatchedList[i] = m_MatchedList[i];
		}

		laboratoryData.m_MatchItemA = new List<string>();
		laboratoryData.m_MatchItemB = new List<string>();
		laboratoryData.m_MatchResult = new List<bool>();

		for (int i = 0; i < m_MatchItemA.Count; i++)
		{
			laboratoryData.m_MatchItemA.Add(m_MatchItemA[i]);
			laboratoryData.m_MatchItemB.Add(m_MatchItemB[i]);
			laboratoryData.m_MatchResult.Add(m_MatchResult[i]);
		}

		BinarySerialize(laboratoryData);
	}

	private void ReadData()
	{
		m_AnalyzedRemainTime = laboratoryData.m_AnalyzedRemainTime;
		m_MatchedRemainTime = laboratoryData.m_MatchedRemainTime;
		m_IsStartMatched = laboratoryData.m_IsStartMatched;

		for (int i = 0; i < m_AnalyzedIndexList.Length; i++)
		{
			m_AnalyzedIndexList[i] = laboratoryData.m_AnalyzedIndexList[i];
			if (m_AnalyzedIndexList[i] != null || m_AnalyzedIndexList[i] != "")
			{
				LaboratoryManager.instance.AddAnalyzedItemList(m_AnalyzedIndexList[i]);
			}
		}

		if (m_IsStartMatched == true)
		{
			for (int i = 0; i < 2; i++)
			{
				m_MatchedList[i] = laboratoryData.m_MatchedList[i];
				LaboratoryManager.instance.AddMatchedItemList(m_MatchedList[i]);
			}
		}


		for (int i = 0; i < laboratoryData.m_MatchItemA.Count; i++)
		{
			m_MatchItemA.Add(laboratoryData.m_MatchItemA[i]);
			m_MatchItemB.Add(laboratoryData.m_MatchItemB[i]);
			m_MatchResult.Add(laboratoryData.m_MatchResult[i]);
		}

		LaboratoryManager.instance.CheckAnalyzed(GameManager.instance.ReturnNowTime());
	}

	private void DataInitialize()
	{
		LaboratoryNode = JSONNode.Parse(LaboratoryTextAsset.text);
	}


	public void BinarySerialize(LaboratoryData data)
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
		LaboratoryData d = (LaboratoryData)formatter.Deserialize(stream);
		laboratoryData = d;
		stream.Close();
	}

	public void Save()
	{
		WriteData();
	}

}
