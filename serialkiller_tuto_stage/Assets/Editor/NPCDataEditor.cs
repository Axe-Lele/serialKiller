using UnityEditor;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class NpcDataEditor : EditorWindow {

    private Vector2 scrollPos;
    public NpcData m_NpcData;

    private static string filepath;
    [MenuItem("Window/Npc Data Editor")]
    static void Init()
    {
        filepath = Application.dataPath + "/Resources/Data/" + PlayDataManager.instance.m_StageName + "/NpcData.bin";
        GetWindow(typeof(NpcDataEditor)).Show();
    }

    private void OnGUI()
    {
        if (GlobalMethod.instance.ReturnFileExist(filepath) == true)
        {
            LoadGameData();
        }

        if (m_NpcData != null)
        {
            SerializedObject serializeObject = new SerializedObject(this);

            EditorGUILayout.BeginHorizontal();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false, GUILayout.MaxHeight(1000), GUILayout.ExpandHeight(true));

            SerializedProperty serializeProperty = serializeObject.FindProperty("m_NpcData");
            EditorGUILayout.PropertyField(serializeProperty, true);

            SerializedProperty p = serializeObject.FindProperty("m_NpcData");

            serializeObject.ApplyModifiedProperties();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("값을 변경 후 반드시 Save Data 버튼을 눌러야 적용이 됨.", MessageType.None, true);

            if (GUILayout.Button("Save Data"))
            {
                SaveGameData();

            }


            EditorGUILayout.HelpBox("Load Data를 누르면 변경한 값이 날아가고 저장되어 있는 값이 불려지게 된다.", MessageType.None, true);
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
        filepath = Application.dataPath + "/Resources/Data/" + PlayDataManager.instance.m_StageName + "/NpcData.bin";
        if (GlobalMethod.instance.ReturnFileExist(filepath) == false)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream;
            stream = new FileStream(filepath, FileMode.Create);
            stream.Close();

            NpcData d = new NpcData();
            m_NpcData = d;
        }
    }

    private void LoadGameData()
    {
        filepath = Application.dataPath + "/Resources/Data/" + PlayDataManager.instance.m_StageName + "/NpcData.bin";
        BinaryDeserialize();
    }

    private void SaveGameData()
    {
        filepath = Application.dataPath + "/Resources/Data/" + PlayDataManager.instance.m_StageName + "/NpcData.bin";
        BinarySerialize(m_NpcData);
    }

    public void BinarySerialize(NpcData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream;

        if (GlobalMethod.instance.ReturnFileExist(filepath))
        {
            stream = new FileStream(filepath, FileMode.Open);
        }
        else
        {
            stream = new FileStream(filepath, FileMode.Create);
        }
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void BinaryDeserialize()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filepath, FileMode.Open);
        NpcData d = (NpcData)formatter.Deserialize(stream);
        m_NpcData = d;
        stream.Close();
    }
}
