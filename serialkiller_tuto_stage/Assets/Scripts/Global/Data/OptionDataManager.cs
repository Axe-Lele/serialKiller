using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class OptionDataManager : Singleton<OptionDataManager> {

    public int m_Resolution;
    public bool m_IsWindows;
    public int m_TypingSpeed;

    private string filePath;
    private OptionData optiondata;
    private void Awake()
    {
        filePath = Application.persistentDataPath + "/" + "data2.bin";

    }

    public void LoadOptionData()
    {
        if (GlobalMethod.instance.ReturnFileExist(filePath))
        {
            ReadData();
        }
        else
        {
            DataInitialize();
        }
    }

    private void WriteData()
    {
        optiondata = new OptionData();

        optiondata.m_Resolution = m_Resolution;
        optiondata.m_IsWindows = m_IsWindows;
        optiondata.m_TypingSpeed = m_TypingSpeed;
    }

    private void ReadData()
    {
        m_Resolution = optiondata.m_Resolution;
        m_IsWindows = optiondata.m_IsWindows;
        m_TypingSpeed = optiondata.m_TypingSpeed;
    }

    private void DataInitialize()
    {
        m_IsWindows = false;
        m_Resolution = 0;
        m_TypingSpeed = 0;
        WriteData();
    }

    public void BinarySerialize(OptionData data)
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
        OptionData data = (OptionData)formatter.Deserialize(stream);
        optiondata = data;
        stream.Close();
    }
}
