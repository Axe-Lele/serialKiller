using UnityEngine;
using System.Collections.Generic;
using System;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using SimpleJSON;

namespace DetectiveBoard
{

	public enum DetectiveNodeType
	{
		Motive,
		Criminal,
		Victim,
		Tool
	}

	[Serializable]
	public class DetectiveData
	{
		public List<DetectiveNode> Nodes;
	}

	[Serializable]
	public class DetectiveNode
	{
		public string City;
		public string Case;
		public string Node;
		public DetectiveNodeType NodeType;
		public bool IsOpen;
	}

	public interface IDataSerialize<T>
	{
		void WriteData();
		void ReadData();
		void DataInitialize();
		void BinarySerialize(T data);
		void BinaryDeserialize();
		void Save();
	}

	public class DetectiveDataManager : Singleton<DetectiveDataManager>, IDataSerialize<DetectiveData>
	{
		#region Public Fields 
		public List<DetectiveNode> m_DetectiveNodes;
		public bool m_IsInit
		{
			get { return m_DetectiveNodes != null; }
		}
		#endregion

		private void Awake()
		{
			m_FilePath = Application.persistentDataPath + "/data44.bin";
			m_DetectiveNodes = null;
		}

		public void Initialized()
		{
			m_DetectiveNodes = new List<DetectiveNode>();
			if (GlobalMethod.instance.ReturnFileExist(m_FilePath))
			{
				BinaryDeserialize();
				ReadData();
			}
			else
			{
				DataInitialize();
			}
			m_DetectiveNodes = m_Datas.Nodes;
		}



		#region Data Serialize
		private string m_FilePath;
		private DetectiveData m_Datas;

		private JSONNode DB;
		[SerializeField] private TextAsset OriginTextAsset;
		public void ReadData()
		{
			m_DetectiveNodes = new List<DetectiveNode>();
			m_DetectiveNodes = m_Datas.Nodes;
		}

		public void DataInitialize()
		{
			DB = JSONNode.Parse(OriginTextAsset.text);

			m_Datas = new DetectiveData();
			m_Datas.Nodes = new List<DetectiveNode>();

			DetectiveNode node;
			for (int cityCount = 0; cityCount < DB.Count; cityCount++)
			{
				for (int caseCount = 0; caseCount < DB[cityCount].Count; caseCount++)
				{
					for (int typeCount = 0; typeCount < DB[cityCount][caseCount].Count; typeCount++)
					{
						for(int i = 0; i < DB[cityCount][caseCount][typeCount].Count; i++)
						{
							node = new DetectiveNode();
							node.City = DB.GetKey(cityCount);
							node.Case = DB[cityCount].GetKey(caseCount);
							node.Node = DB[cityCount][caseCount][typeCount][i];
							node.NodeType = (DetectiveNodeType)typeCount;
							node.IsOpen = false;

							m_Datas.Nodes.Add(node);
							node = null;

						}
					}
				}
			}

			Save();
		}

		public void BinaryDeserialize()
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(m_FilePath, FileMode.Open);
			DetectiveData d = (DetectiveData)formatter.Deserialize(stream);
			m_Datas = d;
			stream.Close();
		}

		public void Save()
		{
			this.WriteData();
		}

		public void WriteData()
		{
			for (int i = 0; i < m_DetectiveNodes.Count; i++)
			{
				m_Datas.Nodes[i] = m_DetectiveNodes[i];
			}
		}

		public void BinarySerialize(DetectiveData data)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream;
			if (GlobalMethod.instance.ReturnFileExist(m_FilePath))
			{
				stream = new FileStream(m_FilePath, FileMode.Open);
			}
			else
			{
				stream = new FileStream(m_FilePath, FileMode.Create);
			}
			formatter.Serialize(stream, data);
			stream.Close();
		}
		#endregion

		#region Private Methods without UnityFunction
		#endregion
	}
}
