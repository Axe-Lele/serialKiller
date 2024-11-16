using UnityEngine;
using System.Collections.Generic;
using System;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using SimpleJSON;

namespace DetectiveBoard
{
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

        [SerializeField]
        private DetectiveData data;

        public List<DetectiveDataItem> m_Items;

        #endregion

        public Dictionary<string, float> m_Complishes;

        public void Initialized()
        {
            DontDestroyOnLoad(this);
            m_FilePath = Application.persistentDataPath + "/dataDT.bin";

            if (GlobalMethod.instance.ReturnFileExist(m_FilePath))
            {
                ReadData();
            }
            else
            {
                DataInitialize();
            }
        }

        private DetectiveDataItem FindItem(string key)
        {
            DetectiveDataItem item = null;
            for (int i = 0; i < m_Items.Count; i++)
            {
                if (m_Items[i].Index.Equals(key))
                {
                    item = m_Items[i];
                    break;
                }
            }

            return item;
        }

        /// <summary>
        /// 디텍티브 아이템을 언락 이벤트 대기 상태로 전환합니다.
        /// </summary>
        /// <param name="key">아이템 PK</param>
        public void UnlockItem(string key)
        {
            DetectiveDataItem item = FindItem(key);
            if (item == null)
                return;

            item.m_IsFirst = true;
        }

        /// <summary>
        /// 언락 이벤트 대기 상태인 아이템을 언락 합니다(디텍티브 보드 - 이미지 표시 조건)
        /// </summary>
        /// <param name="key">아이템 PK</param>
        public void OpenItem(string key)
        {
            DetectiveDataItem item = FindItem(key);
            if (item == null)
                return;

            if (item.m_IsFirst == false)
                return;

            item.m_IsFirst = false;
            item.m_IsOpen = true;
        }

        #region Data Serialize
        private string m_FilePath;

        private JSONNode DB;
        [SerializeField] private TextAsset OriginTextAsset;
        public void ReadData()
        {
            BinaryDeserialize();

            m_Items = new List<DetectiveDataItem>();
            DetectiveDataItem item = null;

            for (int i = 0; i < data.Items.Count; i++)
            {
                item = new DetectiveDataItem()
                {
                    m_Stage = data.Items[i].m_Stage,
                    m_Case = data.Items[i].m_Case,

                    Index = data.Items[i].Index,
                    m_SpriteInfo = data.Items[i].m_SpriteInfo,
                    m_GroupIndex = data.Items[i].m_GroupIndex,
                    m_NodeIndex = data.Items[i].m_NodeIndex,

                    m_Type = data.Items[i].m_Type,
                    m_IsFirst = data.Items[i].m_IsFirst,
                    m_IsOpen = data.Items[i].m_IsOpen
                };

                m_Items.Add(item);
                item = null;
            }

            m_Complishes = new Dictionary<string, float>();

            string key = m_Items[0].m_GroupIndex;
            float count = 0, unlockCount = 0;

            for (int i = 0; i < m_Items.Count; i++)
            {
                if (key != m_Items[i].m_GroupIndex)
                {
                    m_Complishes.Add(key, unlockCount / count);

                    key = m_Items[i].m_GroupIndex;
                    count = 0;
                }

                if (m_Items[i].m_IsOpen)
                    unlockCount++;

                count++;
            }
        }

        /// <summary>
        /// Load Json
        /// </summary>
        public void DataInitialize()
        {
            DB = JSONNode.Parse(OriginTextAsset.text);

            data = new DetectiveData();
            data.Items = new List<DetectiveDataItem>();

            DetectiveDataItem item;

            int idx = 0;
            for (int cityCount = 0; cityCount < DB.Count; cityCount++)
            {
                for (int caseCount = 0; caseCount < DB[cityCount].Count; caseCount++)
                {
                    // Murder
                    string[] temp = DB[cityCount][caseCount][1].Value.Split(':');
                    item = new DetectiveDataItem()
                    {
                        Index = temp[0],
                        m_SpriteInfo = temp[1],
                        m_GroupIndex = DB[cityCount][caseCount]["Index"],
                        m_NodeIndex = 0,

                        m_Type = DetectiveItemType.Murder,
                        m_IsFirst = false,
                        m_IsOpen = false
                    };
                    temp = item.m_GroupIndex.Split('_');
                    item.m_Stage = temp[0];
                    item.m_Case = temp[1];

                    item.SetSpriteInfo();
                    data.Items.Add(item);
                    item = null;

                    for (int typeCount = 2; typeCount < DB[cityCount][caseCount].Count; typeCount++)
                    {
                        // Another
                        for (int i = 0; i < DB[cityCount][caseCount][typeCount].Count; i++)
                        {
                            temp = DB[cityCount][caseCount][typeCount][i].Value.Split(':');
                            item = new DetectiveDataItem()
                            {
                                Index = temp[0],
                                m_SpriteInfo = temp[1],
                                m_GroupIndex = DB[cityCount][caseCount]["Index"],
                                m_NodeIndex = i,

                                m_Type = (DetectiveItemType)(typeCount - 1),
                                m_IsFirst = false,
                                m_IsOpen = false
                            };

                            temp = item.m_GroupIndex.Split('_');
                            item.m_Stage = temp[0];
                            item.m_Case = temp[1];
                            Debug.Log("Detective Item Index : " + item.Index);

                            item.SetSpriteInfo();
                            data.Items.Add(item);
                            item = null;
                        }
                    }
                }
            }

            m_Items = new List<DetectiveDataItem>();
            for (int i = 0; i < data.Items.Count; i++)
            {
                item = new DetectiveDataItem();
                item.m_Stage = data.Items[i].m_Stage;
                item.m_Case = data.Items[i].m_Case;

                item.Index = data.Items[i].Index;
                item.m_SpriteInfo = data.Items[i].m_SpriteInfo;
                item.m_GroupIndex = data.Items[i].m_GroupIndex;
                item.m_NodeIndex = data.Items[i].m_NodeIndex;

                item.m_Type = data.Items[i].m_Type;
                item.m_IsFirst = data.Items[i].m_IsFirst;
                item.m_IsOpen = data.Items[i].m_IsOpen;

                Debug.Log("Detective Item Index : " + item.Index + " / SpriteInfo : " + item.m_SpriteInfo);

                item.SetSpriteInfo();
                m_Items.Add(item);
                item = null;
            }

            for (int i = 0; i < m_Items.Count; i++)
            {
                Debug.Log("Detective Item Index : " + m_Items[i].Index + " / SpriteInfo : " + m_Items[i].m_SpriteInfo);
            }

            Save();
        }

        public void BinaryDeserialize()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(m_FilePath, FileMode.Open);
            DetectiveData d = (DetectiveData)formatter.Deserialize(stream);
            data = d;
            stream.Close();
        }

        public void Save()
        {
            this.WriteData();
        }

        public void WriteData()
        {
            data = new DetectiveData();
            data.Items = new List<DetectiveDataItem>();

            DetectiveDataItem item = null;

            for (int i = 0; i < m_Items.Count; i++)
            {
                item = new DetectiveDataItem();
                item.Index = m_Items[i].Index;
                item.m_SpriteInfo = m_Items[i].m_SpriteInfo;
                item.m_Case = m_Items[i].m_Case;
                item.m_GroupIndex = m_Items[i].m_GroupIndex;
                item.m_IsFirst = m_Items[i].m_IsFirst;
                item.m_IsOpen = m_Items[i].m_IsOpen;
                item.m_NodeIndex = m_Items[i].m_NodeIndex;
                item.m_Stage = m_Items[i].m_Stage;
                item.m_Type = m_Items[i].m_Type;

                Debug.Log("Detective Item Index : " + item.Index + " / SpriteInfo : " + item.m_SpriteInfo);

                item.SetSpriteInfo();
                data.Items.Add(item);
                //item = null;
            }

            BinarySerialize(data);
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
