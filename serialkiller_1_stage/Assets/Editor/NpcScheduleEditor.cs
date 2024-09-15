using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class NpcScheduleEditor : EditorWindow {

    private Vector2 scrollPos;
    public NpcSchedule m_NpcSchedule;

    private static string filepath;
    private static string npcdatapath;
   // [MenuItem("Window/Npc Schedule Editor")]
    private static void ShowWindow()
    {
        GetWindow<NpcScheduleEditor>("Schedule");
    }


    static void Init()
    {
        filepath = Application.dataPath + "/Resources/Data/" + PlayDataManager.instance.m_StageName + "/NpcScheduleData.bin";
        npcdatapath = Application.dataPath + "/Resources/Data/" + PlayDataManager.instance.m_StageName + "/NpcData.bin";
        GetWindow(typeof(NpcScheduleEditor)).Show();
    }

    private void OnGUI()
    {

        filepath = Application.dataPath + "/Resources/Data/" + PlayDataManager.instance.m_StageName + "/NpcScheduleData.bin";
        npcdatapath = Application.dataPath + "/Resources/Data/" + PlayDataManager.instance.m_StageName + "/NpcData.bin";
        if (GlobalMethod.instance.ReturnFileExist(npcdatapath) == false)
        {
            EditorGUILayout.HelpBox("NPC Data를 먼저 작성하고 오시오.", MessageType.Warning, true);
            return;
        }

        if (GlobalMethod.instance.ReturnFileExist(filepath) == true)
        {
            LoadGameData();
        }
        
        if (m_NpcSchedule != null)
        {
            SerializedObject serializeObject = new SerializedObject(this);

            EditorGUILayout.BeginHorizontal();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false, GUILayout.MaxHeight(1000), GUILayout.ExpandHeight(true));

            SerializedProperty serializeProperty = serializeObject.FindProperty("m_NpcSchedule");
            EditorGUILayout.PropertyField(serializeProperty, true);


            serializeObject.ApplyModifiedProperties();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("값을 변경 후 반드시 Save Data 버튼을 눌러야 적용이 됨.", MessageType.Info, true);

            if (GUILayout.Button("Save Data"))
            {
                SaveGameData();

            }


            EditorGUILayout.HelpBox("Load Data를 누르면 변경한 값이 날아가고 저장되어 있는 값이 불려지게 된다.", MessageType.Info, true);
            if (GUILayout.Button("Load Data"))
            {
                LoadGameData();

            }
        }
        else
        {
            if (GUILayout.Button("Make Data"))
            {
                MakeData();
            }
        }

    }

    private void MakeData()
    {
        filepath = Application.dataPath + "/Resources/Data/" + PlayDataManager.instance.m_StageName + "/NpcScheduleData.bin";
        npcdatapath = Application.dataPath + "/Resources/Data/" + PlayDataManager.instance.m_StageName + "/NpcData.bin";
        if (GlobalMethod.instance.ReturnFileExist(filepath) == false)
        {
            m_NpcSchedule = new NpcSchedule();
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream;
            stream = new FileStream(filepath, FileMode.Create);
            
            BinaryFormatter npcdataformatter = new BinaryFormatter();
            FileStream npcdatastream = new FileStream(npcdatapath, FileMode.Open);

            NpcData data = (NpcData)npcdataformatter.Deserialize(npcdatastream);

            m_NpcSchedule.m_ScheduleList = new List<NpcScheduleData>();

            for (int i = 0; i < data.m_NpcItem.Count; i++)
            {
                NpcScheduleData m_NpcScheduleData = new NpcScheduleData();

                m_NpcScheduleData.m_Index = data.m_NpcItem[i].m_Index;

                m_NpcScheduleData.m_Common = new List<NpcScheduleDataItem>();
                m_NpcScheduleData.m_Event0 = new List<NpcScheduleDataItem>();
                m_NpcScheduleData.m_Event1 = new List<NpcScheduleDataItem>();
                m_NpcScheduleData.m_Event2 = new List<NpcScheduleDataItem>();
                m_NpcScheduleData.m_Event3 = new List<NpcScheduleDataItem>();
                m_NpcScheduleData.m_Event4 = new List<NpcScheduleDataItem>();

                m_NpcSchedule.m_ScheduleList.Add(m_NpcScheduleData);
                //NpcScheduleDataItem item = new NpcScheduleDataItem();
                // item.m_NpcIndex = data.m_NpcItem[i].m_Index;
                //m_NpcScheduleData.m_NpcSchduleItemList.Add(item);
            }

            formatter.Serialize(stream, data);

            npcdatastream.Close();
            stream.Close();
        }
        else
        {
          //  LoadGameData();
        }
    }
      private void LoadGameData()
    {
        filepath = Application.dataPath + "/Resources/Data/" + PlayDataManager.instance.m_StageName + "/NpcScheduleData.bin";
        BinaryDeserialize();
    }

    private void SaveGameData()
    {
        filepath = Application.dataPath + "/Resources/Data/" + PlayDataManager.instance.m_StageName + "/NpcScheduleData.bin";
        BinarySerialize(m_NpcSchedule);
    }

    public void BinarySerialize(NpcSchedule data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filepath, FileMode.Open);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void BinaryDeserialize()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filepath, FileMode.Open);

        NpcSchedule d = (NpcSchedule)formatter.Deserialize(stream);
        m_NpcSchedule = d;
        stream.Close();
    }
   
}
