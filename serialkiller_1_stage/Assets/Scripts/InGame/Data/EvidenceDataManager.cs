using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class EvidenceDataManager : Singleton<EvidenceDataManager>
{

	public List<EvidenceDataItem> m_EvidenceList;


	//public List<string> m_EvidenceList;


	EvidenceData evidencedata;
	private string filePath;

	private JSONNode EvidenceNode;
	public TextAsset EvidenceTextAsset;

	private void Awake()
	{
		filePath = Application.persistentDataPath + "/data04.bin";

		m_EvidenceList = new List<EvidenceDataItem>();
		EvidenceNode = JSONNode.Parse(EvidenceTextAsset.text);
	}

	public List<UIAtlas> m_AtlasList;

	private UIAtlas GetAtlas(string code)
	{
		int p = 0;
		for (int i = 0; i < m_EvidenceList.Count; i++)
		{
			if (m_EvidenceList[i].m_ItemCode == code)
			{
				p = m_EvidenceList[i].m_AtlasIndex;
				break;
			}
		}
		return m_AtlasList[p];
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="sprite"></param>
	/// <param name="spriteType">sprite Type : Evidence, Character, etc...</param>
	/// <param name="spriteName">spritename</param>
	public void SetSprite(ref UISprite sprite, string spriteType, string spriteName)
	{
		sprite.atlas = AtlasManager.instance.GetAtlas(spriteType, spriteName);
		sprite.spriteName = spriteName;
	}

	public void GetAllEvidence()
	{
		for (int i = 0; i < 16; i++)
		{
			InputEvidence(string.Format("A{0}", i), true);
			EventManager.instance.SetEvent("GetItem", string.Format("A{0}", i));
		}
		for (int i = 0; i < 3; i++)
		{
			InputEvidence(string.Format("0{0}", i), true);
			EventManager.instance.SetEvent("GetItem", string.Format("0{0}", i));
		}

		InputEvidence("T0", true);
		EventManager.instance.SetEvent("GetItem", "T0");
	}

	public void LoadEvidenceData()
	{
		if (GlobalMethod.instance.ReturnFileExist(filePath))
		{
			BinaryDeserialize();
			ReadData();
		}
	}

	private void WriteData()
	{
		evidencedata = new EvidenceData();

		evidencedata.m_EvidenceList = new EvidenceDataItem[m_EvidenceList.Count];

		for (int i = 0; i < m_EvidenceList.Count; i++)
		{
			EvidenceDataItem item = new EvidenceDataItem();
			item.m_AtlasIndex = m_EvidenceList[i].m_AtlasIndex;
			item.m_ItemCode = m_EvidenceList[i].m_ItemCode;
			item.m_EvidenceState = m_EvidenceList[i].m_EvidenceState;
			item.m_StartTime = m_EvidenceList[i].m_StartTime;
			item.m_IsAnalyzed = m_EvidenceList[i].m_IsAnalyzed;
			item.m_ResultAnalyzed = m_EvidenceList[i].m_ResultAnalyzed;
			item.m_IsMathced = m_EvidenceList[i].m_IsMathced;
			item.m_MatchTarget = m_EvidenceList[i].m_MatchTarget;
			item.m_IsChanged = m_EvidenceList[i].m_IsChanged;
			item.m_SwapItemCode = m_EvidenceList[i].m_SwapItemCode;
			item.m_MatchedEvidenceName = new List<string>();//  string[m_EvidenceList[i].m_MatchedEvidenceName.Count];
			item.m_ResultMatched = new List<bool>(); //bool[m_EvidenceList[i].m_ResultMatched.Length];

			for (int k = 0; k < item.m_MatchedEvidenceName.Count; k++)
			{
				item.m_MatchedEvidenceName[k] = m_EvidenceList[i].m_MatchedEvidenceName[k];
			}

			for (int k = 0; k < item.m_ResultMatched.Count; k++)
			{
				item.m_ResultMatched[k] = m_EvidenceList[i].m_ResultMatched[k];
			}

			evidencedata.m_EvidenceList[i] = item;
		}

		BinarySerialize(evidencedata);
	}

	private void ReadData()
	{

		for (int i = 0; i < evidencedata.m_EvidenceList.Length; i++)
		{

			EvidenceDataItem item = new EvidenceDataItem();
			item.m_AtlasIndex = evidencedata.m_EvidenceList[i].m_AtlasIndex;
			item.m_ItemCode = evidencedata.m_EvidenceList[i].m_ItemCode;
			item.m_EvidenceState = evidencedata.m_EvidenceList[i].m_EvidenceState;
			item.m_StartTime = evidencedata.m_EvidenceList[i].m_StartTime;
			item.m_IsAnalyzed = evidencedata.m_EvidenceList[i].m_IsAnalyzed;
			item.m_ResultAnalyzed = evidencedata.m_EvidenceList[i].m_ResultAnalyzed;
			item.m_IsMathced = evidencedata.m_EvidenceList[i].m_IsMathced;
			item.m_MatchTarget = evidencedata.m_EvidenceList[i].m_MatchTarget;
			item.m_IsChanged = evidencedata.m_EvidenceList[i].m_IsChanged;
			item.m_SwapItemCode = evidencedata.m_EvidenceList[i].m_SwapItemCode;
			item.m_MatchedEvidenceName = new List<string>();// string[evidencedata.m_EvidenceList[i].m_MatchedEvidenceName.Length];
			item.m_ResultMatched = new List<bool>();// bool[evidencedata.m_EvidenceList[i].m_ResultMatched.Length];

			for (int k = 0; k < item.m_MatchedEvidenceName.Count; k++)
			{
				item.m_MatchedEvidenceName[k] = evidencedata.m_EvidenceList[i].m_MatchedEvidenceName[k];
			}

			for (int k = 0; k < item.m_ResultMatched.Count; k++)
			{
				item.m_ResultMatched[k] = evidencedata.m_EvidenceList[i].m_ResultMatched[k];
			}

			m_EvidenceList.Add(item);

			NoteManager.instance.InputEvidence(item);

			// 이벤트 처리
			//     EventManager.instance.SetEvent("GetItem"
			//, StageDataManager.instance.m_CriminalCode.ToString(), item.m_EvidenceName);
			/*if (item.m_EvidenceName == "A10")
			{
					NoteManager.instance.InputCase("3");
			}*/

			if (item.m_IsAnalyzed && item.m_ResultAnalyzed == false)
			{
				LaboratoryManager.instance.AddAnalyzedItemList(item.m_ItemCode);
			}

			if (item.m_IsMathced)
			{
				LaboratoryManager.instance.AddMatchedItemList(item.m_ItemCode);
			}

			item = null;
		}

	}

	public void InputEvidence(string evidence, bool isTest)
	{
		if (ReturnHaveEvidence(evidence) == false)
		{
			EvidenceDataItem item = new EvidenceDataItem();
			item.m_AtlasIndex = EvidenceNode["Evidence"][evidence]["AtlasIndex"].AsInt;
			item.m_ItemCode = evidence;
			item.m_EvidenceState = "None";
			item.m_StartTime = new DateTime();
			item.m_IsAnalyzed = EvidenceNode["Evidence"][evidence]["IsAnalyzed"].AsBool;
			item.m_ResultAnalyzed = false;
			item.m_IsMathced = EvidenceNode["Evidence"][evidence]["IsMatched"].AsBool;
			item.m_IsChanged = EvidenceNode["Evidence"][evidence]["IsChange"].AsBool;
			item.m_IsEnable = true;
			if (item.m_IsChanged == true)
			{
				if (EvidenceNode["Evidence"][evidence]["Swap"] == null)
				{
					item.m_SwapItemCode = string.Empty;
				}
				else
				{
					item.m_SwapItemCode = EvidenceNode["Evidence"][evidence]["Swap"];
				}
			}
			item.m_MatchTarget = "";

			item.m_MatchedEvidenceName = new List<string>();
			item.m_ResultMatched = new List<bool>();

			m_EvidenceList.Add(item);
			NoteManager.instance.InputEvidence(item);

			string evidencename = Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + evidence + "_Title");
			SystemTextManager.instance.InputText(string.Format(Localization.Get("System_Text_AddEvidence"), evidencename));
			//print("ev : " + evidencename);

			/*if (evidence == "A10")
			{
					NoteManager.instance.InputCase("3");
			}*/
			if (item.m_IsAnalyzed && item.m_ResultAnalyzed == false)
			{
				LaboratoryManager.instance.AddAnalyzedItemList(evidence);
			}

			if (item.m_IsMathced)
			{
				LaboratoryManager.instance.AddMatchedItemList(evidence);
			}

		}
		else
		{
			print("already have evidence " + evidence);
		}

	}

	public void InputEvidence(string evidence)
	{
		if (ReturnHaveEvidence(evidence) == false)
		{
			EvidenceDataItem item = new EvidenceDataItem()
			{
				m_AtlasIndex = EvidenceNode["Evidence"][evidence]["AtlasIndex"].AsInt,
				m_ItemCode = evidence,
				m_EvidenceState = "None",

				m_StartTime = new DateTime(),

				m_IsAnalyzed = EvidenceNode["Evidence"][evidence]["IsAnalyzed"].AsBool,
				m_ResultAnalyzed = false,

				m_IsMathced = EvidenceNode["Evidence"][evidence]["IsMatched"].AsBool,

				m_IsChanged = EvidenceNode["Evidence"][evidence]["IsChange"].AsBool,
				m_SwapItemCode = string.Empty,

				m_IsEnable = true,

				m_MatchTarget = string.Empty,
				m_MatchedEvidenceName = new List<string>(),
				m_ResultMatched = new List<bool>()
			};

			if(EvidenceNode["Evidence"][evidence]["IsChange"] != null)
			{
				item.m_IsChanged = EvidenceNode["Evidence"][evidence]["IsChange"].AsBool;
			}

			if (item.m_IsChanged == true)
			{
				item.m_SwapItemCode = EvidenceNode["Evidence"][evidence]["Swap"];
			}

			m_EvidenceList.Add(item);

			NoteManager.instance.InputEvidence(item);
			string evidencename = Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + evidence + "_Title");
			SystemTextManager.instance.InputText(string.Format(Localization.Get("System_Text_AddEvidence"), evidencename));

			EventManager.instance.InsertEvent("GetItem", evidence);

			if (item.m_IsAnalyzed && item.m_ResultAnalyzed == false)
			{
				LaboratoryManager.instance.AddAnalyzedItemList(evidence);
			}

			if (item.m_IsMathced)
			{
				LaboratoryManager.instance.AddMatchedItemList(evidence);
			}

		}
		else
		{
			print("already have evidence " + evidence);
		}
	}

	public void RemoveEvidence(string evidence)
	{
		if (ReturnHaveEvidence(evidence) == true)
		{
			m_EvidenceList.RemoveAt(ReturnEvidenceIndex(evidence));
			NoteManager.instance.RemoveEvidence(evidence);
		}
		else
		{
			print("yoy have not evidence " + evidence);
		}
	}

	public void CompleteMatched(string evidence, string target)
	{
		if (ReturnHaveEvidence(evidence) == true)
		{

			bool b = false;
			print("////////// + " + EvidenceNode["Match"][evidence].Count);
			for (int i = 0; i < EvidenceNode["Match"][evidence].Count; i++)
			{
				if (EvidenceNode["Match"][evidence][i]["Index"].ToString().Replace("\"", "") == target)
				{
					InputEvidence(EvidenceNode["Match"][evidence][i]["Evidence"]);
					b = true;
					break;
				}
			}
			LaboratoryDataManager.instance.InputMatchResult(evidence, target, b);
			LaboratoryManager.instance.InputMatchResult(evidence, target, b);
		}
		else
		{
			print("yoy have not evidence " + evidence);
		}
	}

	public void CompleteAnalyzed(string evidence, bool b)
	{
		if (ReturnHaveEvidence(evidence) == true)
		{
			m_EvidenceList[ReturnEvidenceIndex(evidence)].m_ResultAnalyzed = b;
			//EventManager.instance.SetDelayEvent("Analysis", StageDataManager.instance.m_CriminalCode.ToString(), evidence);
			//EventManager.instance.SetEvent("Analysis",StageDataManager.instance.m_CriminalCode.ToString(), evidence);
		}
		else
		{
			print("yoy have not evidence " + evidence);
		}
	}

	public void SwapEvidence(string evidence1, int swapIdx)
	{
		//print("Swap Evidence : " + evidence1 + " <> " + evidence1 + "_" + swapIdx);
		string evidence2 = EvidenceNode["Evidence"][evidence1]["Swap"][swapIdx];
		SwapEvidence(evidence1, evidence2);
	}

	public void SwapEvidence(string evidence1, string evidence2)
	{
		if (ReturnHaveEvidence(evidence1) == true)
		{
			print("Swap Evidence : " + evidence1 + " <> " + evidence2);
			InputEvidence(evidence2);
			BannedEvidence(evidence1);

			EventManager.instance.InsertEvent("GetItem", evidence2);
		}
		else
		{
			print("yoy have not evidence " + evidence1);
		}

	}

	public void BannedEvidence(string itemCode)
	{
		EvidenceDataItem data = ReturnEvidenceItem(itemCode);

		data.m_IsEnable = false;

		NoteManager.instance.BannedEvidence(itemCode);
	}

	public bool ReturnHaveEvidence(string evidence)
	{
		bool b = false;

		for (int i = 0; i < m_EvidenceList.Count; i++)
		{
			if (m_EvidenceList[i].m_ItemCode == evidence)
			{
				b = true;
				break;
			}
		}
		return b;
	}

	public int ReturnEvidenceIndex(string evidence)
	{
		int index = -1;
		for (int i = 0; i < m_EvidenceList.Count; i++)
		{
			if (m_EvidenceList[i].m_ItemCode == evidence)
			{
				index = i;
				break;
			}
		}
		if (index == -1)
		{
			Debug.LogWarning("return value is -1");
		}

		return index;
	}

	public int ReturnCoolTime(string evidence, string act)
	{
		return EvidenceNode["Evidence"][evidence][act].AsInt;
	}

	public bool ReturnRequest(string evidence, string act)
	{
		return EvidenceNode["Evidence"][evidence][act].AsBool;
	}

	public EvidenceDataItem ReturnEvidenceItem(string evidence)
	{
		EvidenceDataItem item = null;
		for (int i = 0; i < m_EvidenceList.Count; i++)
		{
			if (m_EvidenceList[i].m_ItemCode == evidence)
			{
				item = m_EvidenceList[i];
				break;
			}
		}
		return item;
	}

	public void BinarySerialize(EvidenceData data)
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
		EvidenceData d = (EvidenceData)formatter.Deserialize(stream);
		evidencedata = d;
		stream.Close();
	}

	public void Save()
	{
		print("EvidenceDataManager Save");
		WriteData();
	}

}
