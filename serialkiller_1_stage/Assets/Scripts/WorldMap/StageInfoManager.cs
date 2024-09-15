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
    public UIGrid m_CityItemsGrid;
    public List<GameObject> m_CityItems;

    [SerializeField]
    private string[] m_StageIdxs;
    public string m_StageIdx
    {
        get
        {
            return m_StageIdxs[0];
        }
    }
    public string m_CriminalCode
    {
        get
        {
            return m_StageIdxs[1];
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);

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
        for (int i = 0; i < CityNode.Count; i++)
        {
            _item = Instantiate(m_CityItem_Origin, m_CityItemRoot.transform);
            StageItem _info = _item.GetComponent<StageItem>();
            _info.m_Index = i;
            _info.isUnlock = CityNode[i]["IsOpen"].AsBool;

            m_CityItems.Add(_item);
            _info.Init();
        }

        m_CityItemsGrid.Reposition();
    }

    public void SetInfo(string[] stageIdx)
    {
        m_StageIdxs = stageIdx;
    }

    public bool GetCityIsOpen()
    {
        return false;
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
        for (int cityIdx = 0; cityIdx < data.Cities.Count; cityIdx++)
        {
            _item = new CityData();
            _item.Index = m_CityDatas[cityIdx].Index;
            _item.IsOpen = m_CityDatas[cityIdx].IsOpen;
            _item.IsOpenStage = new bool[m_CityDatas[cityIdx].IsOpenStage.Length];
            for (int stageIdx = 0; stageIdx < m_CityDatas[cityIdx].IsOpenStage.Length; stageIdx++)
            {
                _item.IsOpenStage[stageIdx] = m_CityDatas[cityIdx].IsOpenStage[stageIdx];
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
        for (int cityIdx = 0; cityIdx < data.Cities.Count; cityIdx++)
        {
            _item = new CityData();
            _item.Index = data.Cities[cityIdx].Index;
            _item.IsOpen = data.Cities[cityIdx].IsOpen;
            _item.IsOpenStage = new bool[data.Cities[cityIdx].IsOpenStage.Length];
            for (int stageIdx = 0; stageIdx < data.Cities[cityIdx].IsOpenStage.Length; stageIdx++)
            {
                _item.IsOpenStage[stageIdx] = data.Cities[cityIdx].IsOpenStage[stageIdx];
            }
            m_CityDatas.Add(data.Cities[cityIdx]);
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
            _item.Index = CityNode[cityIdx].AsInt;
            _item.IsOpen = CityNode[cityIdx]["IsOpen"].AsBool;
            _item.IsOpenStage = new bool[CityNode[cityIdx].Count - 1];
            for (int stageIdx = 1; stageIdx < CityNode[cityIdx].Count; stageIdx++)
            {
                _item.IsOpenStage[stageIdx - 1] = CityNode[cityIdx][stageIdx]["IsOpen"].AsBool;
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
