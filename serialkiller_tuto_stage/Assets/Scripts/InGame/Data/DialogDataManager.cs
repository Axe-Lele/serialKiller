using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DialogDataManager : Singleton<DialogDataManager> {

    public List<DialogDataItem> m_DialogDataItemList;

    DialogData dialogdata;
    private string filePath;

   // private JSONNode SelectionNode;
   // public TextAsset SelectionTextAsset;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/data08.bin";

        m_DialogDataItemList = new List<DialogDataItem>();
    }


    public void LoadDialogData()
    {
        if (GlobalMethod.instance.ReturnFileExist(filePath))
        {
            BinaryDeserialize();
            ReadData();
        }
    }

    private void WriteData()
    {
        dialogdata = new DialogData();

        dialogdata.m_DialogDataItemList = new List<DialogDataItem>();
        for (int i = 0; i < m_DialogDataItemList.Count; i++)
        {
            DialogDataItem item = new DialogDataItem();
            item.m_Index = m_DialogDataItemList[i].m_Index;

            item.m_DialogIndex = new List<string>();
            item.m_Place = new List<string>();
            for (int k = 0; k < m_DialogDataItemList[i].m_DialogIndex.Count; k++)
            {
                item.m_DialogIndex.Add(m_DialogDataItemList[i].m_DialogIndex[k]);
                item.m_Place.Add(m_DialogDataItemList[i].m_Place[k]);
            }
            dialogdata.m_DialogDataItemList.Add(item);
            item = null;
        }
        BinarySerialize(dialogdata);
    }

    private void ReadData()
    {
        for (int i = 0; i < dialogdata.m_DialogDataItemList.Count; i++)
        {
            DialogDataItem item = new DialogDataItem();
            item.m_Index = dialogdata.m_DialogDataItemList[i].m_Index;

            item.m_DialogIndex = new List<string>();
            item.m_Place = new List<string>();
            for (int k = 0; k < dialogdata.m_DialogDataItemList[i].m_DialogIndex.Count; k++)
            {
                item.m_DialogIndex.Add(dialogdata.m_DialogDataItemList[i].m_DialogIndex[k]);
                item.m_Place.Add(dialogdata.m_DialogDataItemList[i].m_Place[k]);
                InputDialog(item.m_Index, item.m_DialogIndex[k], item.m_Place[k]);

            }

            m_DialogDataItemList.Add(item);

            item = null;
        }
    }

    public void InputDialog(string who, string select, string place)
    {
        bool b = ReturnIsCharacter(who);

        if (b == true)
        {
            bool c = false;
            int index = ReturnCharacterIndex(who);
            for (int i = 0; i < m_DialogDataItemList[index].m_DialogIndex.Count; i++)
            {
                if (m_DialogDataItemList[index].m_DialogIndex[i] == select)
                {
                    c = true;
                    break;
                }
            }

            if (c == false)
            {
                m_DialogDataItemList[index].m_DialogIndex.Add(select);
                m_DialogDataItemList[index].m_Place.Add(place);
            }
            
        /*    for (int i = 0; i < m_DialogDataItemList[index].m_DialogIndex.Count; i++)
            {
                m_DialogDataItemList[index].m_DialogIndex.Add(select);
            }*/
        }
        else
        {
            DialogDataItem item = new DialogDataItem();
            item.m_Index = who;
            item.m_DialogIndex = new List<string>();
            item.m_Place = new List<string>();
            item.m_DialogIndex.Add(select);
            item.m_Place.Add(place);

            m_DialogDataItemList.Add(item);
            NoteManager.instance.InputDialog(who);

            print("i can not find " + who);
            item = null;
        }
    }

    public void RemoveDialog(string who, string select)
    {
        int index = ReturnCharacterIndex(who);

        // index 값이 0보다 작으면 캐릭터 리스트에 찾는 인물이 없다는 것.
        if (index < 0)
        {
            print("error! i can not find who");
        }
        else
        {
            for (int i = 0; i < m_DialogDataItemList[index].m_DialogIndex.Count; i++)
            {
                if (m_DialogDataItemList[index].m_DialogIndex[i] == select)
                {
                    m_DialogDataItemList[index].m_DialogIndex.RemoveAt(i);
                    break;
                }
            }
        }
    }
    public string ReturnPlace(string who, string select)
    {
        string p = "";
        int index = ReturnCharacterIndex(who);
        for (int i = 0; i < m_DialogDataItemList[index].m_DialogIndex.Count; i++)
        {
            if (m_DialogDataItemList[index].m_DialogIndex[i] == select)
            {
                p = m_DialogDataItemList[index].m_Place[i];
                break;
            }
        }
        return p;
    }

    public string ReturnPlace(int whoindex, string select)
    {
        string p = "";
        for (int i = 0; i < m_DialogDataItemList[whoindex].m_DialogIndex.Count; i++)
        {
            if (m_DialogDataItemList[whoindex].m_DialogIndex[i] == select)
            {
                p = m_DialogDataItemList[whoindex].m_Place[i];
                break;
            }
        }
        return p;
    }
    // 캐릭터가 몇 번째에 있는지
    public int ReturnCharacterIndex(string who)
    {
        int index = -1;
        for (int i = 0; i < m_DialogDataItemList.Count; i++)
        {
            if (m_DialogDataItemList[i].m_Index == who)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    private bool ReturnIsCharacter(string who)
    {
        bool b = false;
        for (int i = 0; i < m_DialogDataItemList.Count; i++)
        {
            if (m_DialogDataItemList[i].m_Index == who)
            {
                b = true;
                break;
            }
        }
        return b;
    }

    public void BinarySerialize(DialogData data)
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
        DialogData d = (DialogData)formatter.Deserialize(stream);
        dialogdata = d;
        stream.Close();
    }

    public void Save()
    {
        print("DialogDataManager Save");
        WriteData();
    }
}
