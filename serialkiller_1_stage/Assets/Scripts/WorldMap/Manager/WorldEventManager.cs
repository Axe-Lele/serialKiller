using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

public class WorldEventManager : Singleton<WorldEventManager> {

    private JSONNode EventNode;
    public TextAsset EventTextAsset;
    private List<string> EventTemp;
    private int m_EventCount;
    private void Awake()
    {
        EventNode = JSONNode.Parse(EventTextAsset.text);
        EventTemp = new List<string>();
    }

    public void StartEvent()
    {
        EventTemp.Clear();
        m_EventCount = EventNode["Start"].Count;

        for (int i = 0; i < m_EventCount; i++)
        {
            EventTemp.Add(EventNode["Start"][i]);
        }
        ApplyEvent();
    }

    public void StartEvent(string str)
    {
        EventTemp.Clear();
        EventTemp.Add(str);
    }



    public void SetEvent(string category, string s)
    {
        EventTemp.Clear();
        m_EventCount = 0;

        m_EventCount = EventNode[category][s].Count;

        if (m_EventCount > 0)
        {
            for (int i = 0; i < m_EventCount; i++)
            {
                EventTemp.Add(EventNode[category][s][i]);
            }
            ApplyEvent();
        }
        else
        {
            print("category : " + category + " / index : " + s + " has not event");
        }
    }

    public void SetEvent(string category, string s1, string s2)
    {
        EventTemp.Clear();
        m_EventCount = 0;

        m_EventCount = EventNode[category][s1][s2].Count;

        if (m_EventCount > 0)
        {
            for (int i = 0; i < m_EventCount; i++)
            {
                EventTemp.Add(EventNode[category][s1][s2][i]);
            }
            ApplyEvent();
        }
        else
        {
            print("category : " + category + " / s1 : " + s1 + " / s2 : " + s2 + " has not event");
        }
    }

    public void SetEvent(string category, string s1, string s2, string s3)
    {
        EventTemp.Clear();
        m_EventCount = 0;

        m_EventCount = EventNode[category][s1][s2][s3].Count;

        if (m_EventCount > 0)
        {
            for (int i = 0; i < m_EventCount; i++)
            {
                EventTemp.Add(EventNode[category][s1][s2][s3][i]);
            }
            ApplyEvent();
        }
        else
        {
            print("category : " + category + " / s1 : " + s1 + " / s2 : " + s2 + " / s3 : " + s3 + " has not event");
        }
    }

    private void ApplyEvent()
    {
        for (int i = 0; i < EventTemp.Count; i++)
        {
            string[] str = EventTemp[i].Split('-');
            print(i + " : " + str[0] + " / 1 : " + str[1]);
            switch ((WorldEventType)Enum.Parse(typeof(WorldEventType), str[0]))
            {
                case WorldEventType.Dialog:
                    Debug.Break();
                    string[] DialogTemp = str[1].Split('_');
                    WorldManager.instance.m_DialogManager.StartDialogInWorld(DialogType.Dialog, DialogTemp[0], DialogTemp[1]);
                    break;
                case WorldEventType.AddArea:
                    WorldDataManager.instance.ControlStage(int.Parse(str[1]), true);
                    break;
                case WorldEventType.Save:
                    WorldManager.instance.Save();
                    break;
            }

            WorldEventDataManager.instance.ChangeEventData(EventTemp[i]);
        }
    }
}
