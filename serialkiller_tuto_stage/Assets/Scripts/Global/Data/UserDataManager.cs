using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class UserDataManager : Singleton<UserDataManager>
{
    public string m_UserName;//유저 이름
    public string m_UserAvatarSprite;//현재 적용중인 유저의 아바타 이미지

    public List<string> m_HaveUserAvatarList; // 유저가 가지고 있는 아바타 이미지 리스트.

    public List<string> m_HaveUserCharacter; // 유저가 찍은 특성 리스트. +로 split
    public List<string> m_HaveUserCharacterAuto; // 스테이지 시작 할 시, m_UserSex, m_Fame, m_EvilFame, m_HaveUserCharacter에 따라 자동 적용. 저장하지는 않음.

    // 화폐
    public int m_Gold; // 골드
    public int m_Fame; // 각 스테이지의 명성의 합. 
    public int m_EvilFame; // 각 스테이지의 악명의 합

    public int[] m_GetFame; // 스테이지마다 얻은 명성. 배열 크기는 메인 스테이지 수.
    public int[] m_RemainFame; // 특성을 찍고 남음 명성. 배열 크기는 메인 스테이지 수.
    public int[] m_GetEvilFame; // 스테이지마다 얻은 악명. 배열 크기는 메인 스테이지 수. 


    private UserData userdata;
    private string filePath;
    // 이 오브젝트는 어떤 Scene으로 넘어가든 없어지면 안 됨
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        filePath = Application.persistentDataPath + "/" + "data0.bin";

        m_HaveUserAvatarList = new List<string>();

        m_HaveUserCharacter = new List<string>();
        m_HaveUserCharacterAuto = new List<string>();

        m_GetFame = new int[WorldDataManager.MainStageCount];
        m_RemainFame = new int[WorldDataManager.MainStageCount];
        m_GetEvilFame = new int[WorldDataManager.MainStageCount];
    }

    public  void LoadUserData()
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
        userdata = new UserData();

        userdata.m_UserName = m_UserName;
        userdata.m_UserAvatarSprite = m_UserAvatarSprite;
        userdata.m_Gold = m_Gold;

        userdata.m_HaveUserAvatarList = new string[m_HaveUserAvatarList.Count];
        userdata.m_HaveUserCharacter = new string[m_HaveUserCharacter.Count];
        userdata.m_GetFame = new int[WorldDataManager.MainStageCount];
        userdata.m_RemainFame = new int[WorldDataManager.MainStageCount];
        userdata.m_GetEvilFame = new int[WorldDataManager.MainStageCount];


        for (int i = 0; i < m_HaveUserAvatarList.Count; i++)
        {
            userdata.m_HaveUserAvatarList[i] = m_HaveUserAvatarList[i];
        }

        for (int i = 0; i < m_HaveUserCharacter.Count; i++)
        {
            userdata.m_HaveUserCharacter[i] = m_HaveUserCharacter[i];
        }

        for (int i = 0; i < WorldDataManager.MainStageCount; i++)
        {
            userdata.m_GetFame[i] = m_GetFame[i];
        }

        for (int i = 0; i < WorldDataManager.MainStageCount; i++)
        {
            userdata.m_RemainFame[i] = m_RemainFame[i];
        }

        for (int i = 0; i < WorldDataManager.MainStageCount; i++)
        {
            userdata.m_GetEvilFame[i] = m_GetEvilFame[i];
        }

        BinarySerialize(userdata);
    }

    private void ReadData()
    {
        m_UserName = userdata.m_UserName;
        //m_UserSex = userdata.m_UserSex;
        m_UserAvatarSprite = userdata.m_UserAvatarSprite;
        m_Gold = userdata.m_Gold;
        m_Fame = 0;
        m_EvilFame = 0;

        for (int i = 0; i < userdata.m_HaveUserAvatarList.Length; i++)
        {
            m_HaveUserAvatarList.Add(userdata.m_HaveUserAvatarList[i]);
        }
      
        for (int i = 0; i < userdata.m_HaveUserCharacter.Length; i++)
        {
            m_HaveUserCharacter.Add(userdata.m_HaveUserCharacter[i]);
        }

        for (int i = 0; i < WorldDataManager.MainStageCount; i++)
        {
            if (WorldDataManager.MainStageCount > userdata.m_GetFame.Length)
            {
                m_GetFame[i] = 0;
            }
            else
            {
                m_Fame += userdata.m_GetFame[i];
                m_GetFame[i] = userdata.m_GetFame[i];
            }
        }

        for (int i = 0; i < WorldDataManager.MainStageCount; i++)
        {
            if (WorldDataManager.MainStageCount > userdata.m_RemainFame.Length)
            {
                m_RemainFame[i] = 0;
            }
            else
            {
                m_RemainFame[i] = userdata.m_RemainFame[i];
            }
        }

        for (int i = 0; i < WorldDataManager.MainStageCount; i++)
        {
            if (WorldDataManager.MainStageCount > userdata.m_GetEvilFame.Length)
            {
                m_GetEvilFame[i] = 0;
            }
            else
            {
                m_EvilFame += userdata.m_GetEvilFame[i];
                m_GetEvilFame[i] = userdata.m_GetEvilFame[i];
            }
        }
    }

    private void DataInitialize()
    {
        m_UserName = "Detective";
        m_UserAvatarSprite = "Male_0";
        m_Gold = 0;
        m_Fame = 0;
        m_EvilFame = 0;

        m_HaveUserAvatarList.Add("Male_0");
        m_HaveUserAvatarList.Add("Female_0");

        for (int i = 0; i < WorldDataManager.MainStageCount; i++)
        {
            m_GetFame[i] = 0;
        }

        for (int i = 0; i < WorldDataManager.MainStageCount; i++)
        {
            m_RemainFame[i] = 0;
        }

        for (int i = 0; i < WorldDataManager.MainStageCount; i++)
        {
            m_GetEvilFame[i] = 0;
        }

        WriteData();
    }

    public void BinarySerialize(UserData data)
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
        UserData data = (UserData)formatter.Deserialize(stream);
        userdata = data;
        stream.Close();
    }

    public void Save()
    {
        WriteData();
    }
}
