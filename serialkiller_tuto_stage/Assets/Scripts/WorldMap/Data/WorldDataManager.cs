using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class WorldDataManager : Singleton<WorldDataManager>
{

    public const int MainStageCount = 1;

    private bool[] m_IsOpend;

    private string filePath;
    private WorldData worlddata;
    private void Awake()
    {
        filePath = Application.persistentDataPath + "/" + "data1.bin";
        
        m_IsOpend = new bool[MainStageCount];
    }

    public void ControlStage(int index, bool b)
    {
        m_IsOpend[index] = b;
        WorldManager.instance.ControlStage(index, b);
    }

    public void LoadWorldData()
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
        worlddata = new WorldData();

        worlddata.m_IsOpend = new bool[MainStageCount];

        for (int i = 0; i < MainStageCount; i++)
        {
            worlddata.m_IsOpend[i] = m_IsOpend[i];
        }

        BinarySerialize(worlddata);
    }

    private void ReadData()
    {
        for (int i = 0; i < MainStageCount; i++)
        {
            m_IsOpend[i] = worlddata.m_IsOpend[i];

            ControlStage(i, m_IsOpend[i]);
        }
    }

    private void DataInitialize()
    {
        WriteData();
    }

    public void BinarySerialize(WorldData data)
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
        WorldData data = (WorldData)formatter.Deserialize(stream);
        worlddata = data;
        stream.Close();
    }

    public void Save()
    {
        WriteData();
    }
}
