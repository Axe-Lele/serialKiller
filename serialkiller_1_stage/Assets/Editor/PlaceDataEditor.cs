using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlaceDataManager))]
public class PlaceDataEditor : Editor
{
	PlaceDataManager m_PlaceDataManager = null;

	private void OnEnable()
	{
		m_PlaceDataManager = target as PlaceDataManager;
	}
	public override void OnInspectorGUI()
	{
		m_PlaceDataManager = target as PlaceDataManager;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("name", GUILayout.MaxWidth(100));
		EditorGUILayout.LabelField("Opened", GUILayout.MaxWidth(100));
		EditorGUILayout.LabelField("Searched", GUILayout.MaxWidth(100));
		EditorGUILayout.EndHorizontal();
		string str = null;

		if (m_PlaceDataManager.m_PlaceDatas != null)
			for (int i = 0; i < m_PlaceDataManager.m_PlaceDatas.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				str = m_PlaceDataManager.m_PlaceDatas[i].Type + "_" + m_PlaceDataManager.m_PlaceDatas[i].Index;
				EditorGUILayout.LabelField(str, GUILayout.MaxWidth(100));
				m_PlaceDataManager.m_PlaceDatas[i].IsOpened = EditorGUILayout.Toggle(m_PlaceDataManager.m_PlaceDatas[i].IsOpened, GUILayout.MaxWidth(100));
				m_PlaceDataManager.m_PlaceDatas[i].IsSearched = EditorGUILayout.Toggle(m_PlaceDataManager.m_PlaceDatas[i].IsSearched, GUILayout.MaxWidth(100));
				EditorGUILayout.EndHorizontal();
			}

		if (GUILayout.Button("Now Save Data"))
		{
			m_PlaceDataManager.Save();
		}

		if (GUI.changed)
			EditorUtility.SetDirty(target);

		base.OnInspectorGUI();
		return;
	}
}