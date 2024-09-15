using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System;


//[Serializable]
[CustomEditor(typeof(EventManager))]
public class EventDataEditor : Editor
{
	private Vector2 _scrollPosition;

	//[MenuItem("Window/Event Data")]
	static void ShowWindow()
	{
		//    GetWindow<EventDataEditor>("Event Data");
	}

	//public List<string> m_StartEventList;
	private ReorderableList list;
	private void OnEnable()
	{
		//list = new ReorderableList(serializedObject, serializedObject.FindProperty("m_EventStartItem"), true, true, true,true);

		//list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
		//{
		//    var element = list.serializedProperty.GetArrayElementAtIndex(index);
		//    rect.y += 2;
		//    EditorGUI.PropertyField(new Rect(rect.x, rect.y, 30, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("m_CriminalCode"), GUIContent.none);
		//    EditorGUI.PropertyField(new Rect(rect.x + 40, rect.y, 90, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("m_EventType"), GUIContent.none);
		//    EditorGUI.PropertyField(new Rect(rect.x + 150, rect.y, rect.width - 150, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("m_EventCode"), GUIContent.none);
		//};


	}

	public override void OnInspectorGUI()
	{
		//serializedObject.Update();
		//list.DoLayoutList();
		//serializedObject.ApplyModifiedProperties();
		base.OnInspectorGUI();
	}

}

/*
    private void OnGUI()
    {
    //    _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

        //ReorderableListGUI.Title("Start Event List");
        //ReorderableListGUI.ListField(m_StartEventList, StartEventDrawer, DrawEmpty);

       // ReorderableListGUI.Title("Purchased Items");
       // ReorderableListGUI.ListField(m_StartEventList, PurchasedItemDrawer, DrawEmpty, ReorderableListFlags.HideAddButton | ReorderableListFlags.DisableReordering);

        GUILayout.EndScrollView();
    }
    */
/*private string StartEventDrawer(Rect position, string itemValue)
{
		int m_CrimeCode = 0;

		Rect r = new Rect();
		r.x = 50;
		m_CrimeCode = EditorGUI.IntField(r, m_CrimeCode);
		r.width = 50;


		/*position.width -= 150;
		itemValue = EditorGUI.TextField(position, itemValue);

		position.x = position.xMax + 5;
		position.width = 45;
		if (GUI.Button(position, "Info"))
		{
				Debug.Log("rew"); 
		}

		return itemValue;
}

private string PendingItemDrawer(Rect position, string itemValue)
{
		// Text fields do not like null values!
		if (itemValue == null)
				itemValue = "";

		position.width -= 50;
		itemValue = EditorGUI.TextField(position, itemValue);

		position.x = position.xMax + 5;
		position.width = 45;
		if (GUI.Button(position, "Info"))
		{
		}

		return itemValue;
}

private string PurchasedItemDrawer(Rect position, string itemValue)
{
		position.width -= 50;
		GUI.Label(position, itemValue);

		position.x = position.xMax + 5;
		position.width = 45;
		if (GUI.Button(position, "Info"))
		{
		}

		return itemValue;
}

private void DrawEmpty()
{
		GUILayout.Label("No items in list.", EditorStyles.miniLabel);
}*/

