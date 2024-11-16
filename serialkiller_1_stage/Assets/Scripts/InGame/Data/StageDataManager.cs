using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleJSON;
using System.Security.Cryptography;

public class StageDataManager : Singleton<StageDataManager>
{

    StageData data;
    private string filePath;
    public int m_PastDay;
    public int m_Stamina;
    public int m_CriminalCode = -1;
    public int m_RemainStamina;
    public int m_RemainCheckCount;
	//public List<string> m_CaseList;
	public List<CaseDataItem> m_CaseDataItemList;
	public List<string> m_CheckCaseList;

    /// <summary>
    ///  이 밑은 저장 안 함
    /// </summary>
    public int[] m_NeedTime;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/data01.bin";
        //  m_CaseList = new List<string>();
        //  m_CaseDataItemList = new List<CaseDataItem>();
        m_CheckCaseList = new List<string>();
    }

    public void LoadGameData()
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
        data = new StageData();

        data.m_PastDay = m_PastDay;
        data.m_Stamina = m_Stamina;
        data.m_CriminalCode = m_CriminalCode;
        data.m_RemainStamina = m_RemainStamina;
        data.m_RemainCheckCount = m_RemainCheckCount;

        /*data.m_CaseList = new string[m_CaseList.Count];

        for (int i = 0; i < m_CaseList.Count; i++)
        {
            data.m_CaseList[i] = m_CaseList[i];
        }

        data.m_CaseDataItemList = new CaseDataItem[m_CaseDataItemList.Count];
        for (int i = 0; i < m_CaseDataItemList.Count; i++)
        {
            data.m_CaseDataItemList[i] = m_CaseDataItemList[i];
        }
        */
        data.m_CheckCaseList = new string[m_CheckCaseList.Count];
        for (int i = 0; i < m_CheckCaseList.Count; i++)
        {
            data.m_CheckCaseList[i] = m_CheckCaseList[i];
        }


        BinarySerialize(data);
    }

    private void ReadData()
    {
        m_PastDay = data.m_PastDay;
        m_Stamina = data.m_Stamina;
        m_CriminalCode = data.m_CriminalCode;
        m_RemainStamina = data.m_RemainStamina;
        m_RemainCheckCount = data.m_RemainCheckCount;

        /*m_CaseList = new List<string>();
        for (int i = 0; i < data.m_CaseList.Length; i++)
        {
            m_CaseList.Add(data.m_CaseList[i]);
        }

        m_CaseDataItemList = new List<CaseDataItem>();
        for (int i = 0; i < data.m_CaseDataItemList.Length; i++)
        {
            m_CaseDataItemList.Add(data.m_CaseDataItemList[i]);
        }
        */
        m_CheckCaseList = new List<string>();
        for (int i = 0; i < data.m_CheckCaseList.Length; i++)
        {
            m_CheckCaseList.Add(data.m_CheckCaseList[i]);
            NoteManager.instance.InputCase(CaseMode.Main, data.m_CheckCaseList[i]);
        }

    }

    private void DataInitialize()
    {
        m_PastDay = 0;
        m_Stamina = GameManager.instance.m_FullStamina;
        //m_CriminalCode = -1;
        m_RemainStamina = 0;
        m_RemainCheckCount = 0;
    }

    public void BinarySerialize(StageData data)
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
        StageData d = (StageData)formatter.Deserialize(stream);
        data = d;
        stream.Close();
    }

    public void Save()
    {
        print("StageDataManager Save");
        WriteData();
    }
}
