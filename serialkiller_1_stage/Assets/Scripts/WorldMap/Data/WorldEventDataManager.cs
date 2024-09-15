using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class WorldEventDataManager : Singleton<WorldEventDataManager>
{
    WorldEventData data;
    private string filePath;

    private string m_NowEventCode;


    public void LoadEventData()
    {
        filePath = Application.persistentDataPath + "/data3.bin";

        if (GlobalMethod.instance.ReturnFileExist(filePath))
        {
            BinaryDeserialize();
            ReadData();
        }
    }


   
    public void ChangeEventData(string code)
    {
        m_NowEventCode = code;
    }

    public string ReturnEventData()
    {
        return m_NowEventCode;
    }
  

    private void WriteData()
    {
        data = new WorldEventData();

        data.m_NowEventCode = m_NowEventCode;

        BinarySerialize(data);
    }

    private void ReadData()
    {
        m_NowEventCode = data.m_NowEventCode;
    }

    public void BinarySerialize(WorldEventData data)
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
        WorldEventData d = (WorldEventData)formatter.Deserialize(stream);
        data = d;
        stream.Close();
    }

    public void Save()
    {
        WriteData();
    }

}
