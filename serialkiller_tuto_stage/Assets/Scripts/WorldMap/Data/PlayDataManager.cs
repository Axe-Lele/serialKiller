using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayDataManager : Singleton<PlayDataManager>
{
    public GameObject[] m_Managers;

    public string m_StageName;

    private string filePath;
    private PlayData playdata;
    private void Awake()
    {
        filePath = Application.persistentDataPath + "/" + "data00.bin";
    }

    public bool ReturnHavePlayData()
    {
        if (m_StageName == "")
            return false;
        else
            return true;
    }

    public void LoadPlayData()
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
        playdata = new PlayData();

        playdata.m_StageName = m_StageName;

        BinarySerialize(playdata);
    }

    private void ReadData()
    {
        m_StageName = playdata.m_StageName;
    }

    private void DataInitialize()
    {
        //m_StageName = "";
        WriteData();
    }

    public void BinarySerialize(PlayData data)
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
        PlayData data = (PlayData)formatter.Deserialize(stream);
        playdata = data;
        stream.Close();
    }

    public void Save()
    {
        for(int i = 0; i < m_Managers.Length; i++)
        {
            print(i);
            m_Managers[i].SendMessage("Save");
        }
        WriteData();
    }
}
