using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapTool))]
public class MapToolEditor : Editor
{
    MapTool tool;
    GameObject go;
    public override void OnInspectorGUI()
    {
        tool = target as MapTool;

        go = GameObject.Find("MapTool");

        PlaceManager manager = (PlaceManager)EditorGUILayout.ObjectField("PlaceManager : ", tool.m_PlaceManager, typeof(PlaceManager), false);
        TextAsset text = (TextAsset)EditorGUILayout.ObjectField("TextAsset : ", tool.m_TextAsset, typeof(TextAsset), false);
        PlaceItem origin = (PlaceItem)EditorGUILayout.ObjectField("Sprite Original : ", tool.m_Original, typeof(PlaceItem), false);

        tool.m_XValue = EditorGUILayout.FloatField("X_Value", tool.m_XValue);
        tool.m_YValue = EditorGUILayout.FloatField("Y_Value", tool.m_YValue);
        tool.m_XIntervalValue = EditorGUILayout.FloatField("X_Interval_Value", tool.m_XIntervalValue);
        tool.m_YIntervalValue = EditorGUILayout.FloatField("Y_Interval_Value", tool.m_YIntervalValue);

        tool.m_PlaceManager = manager;
        tool.m_TextAsset = text;
        tool.m_Original = origin;

        if (GUILayout.Button("Setting"))
        {
            Setting();
        }

        if (GUILayout.Button("Draw"))
        {
            if (tool.m_TextAsset == null)
            {
                EditorUtility.DisplayDialog("Notice", "Text file is null", "OK");
                return;
            }
            Draw();
        }

        if (GUILayout.Button("Delete"))
        {
            Delete();
        }
    }

    void Setting()
    {
        tool.Setting();
    }
   
    void Delete()
    {
        tool.Delete();
    }

    void Draw()
    {
        tool.Draw();
    }
}
