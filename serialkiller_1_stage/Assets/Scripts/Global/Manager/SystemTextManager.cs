using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTextManager : Singleton<SystemTextManager>
{
    public SystemTextItem m_OriginItem;
    public  List<string> m_WordList;
    public List<SystemTextItem> m_ItemList;
    public Transform m_Content;

    private int m_TextIndex;
    private int m_ItemIndex;
    private bool m_Flag = false;
    private float m_Delay = 1f;

    private void Awake()
    {
        m_TextIndex = 0;
        m_ItemIndex = 0;
        m_WordList = new List<string>();
        m_ItemList = new List<SystemTextItem>();
        //m_TextItemList = new List<SystemTextItem>();
    }

    public void InputText(string str)
    {
        m_WordList.Add(str);
        m_TextIndex++;

        if (m_Flag)
            return;

        StartCoroutine(StartShowText());
    }

    private IEnumerator StartShowText()
    {
        m_Flag = true;

        while (m_WordList.Count > m_ItemIndex)
        {
            ShowText(m_WordList[m_ItemIndex]);
            yield return new WaitForSeconds(m_Delay);
            m_ItemIndex++;
        }
        m_Flag = false;
        m_WordList.Clear();
        m_TextIndex = 0;
        m_ItemIndex = 0;
    }
    
    private void ShowText(string str)
    {
        if (m_ItemList.Count < m_TextIndex)
        {
            SystemTextItem go = Instantiate(m_OriginItem);
            go.transform.parent = m_Content;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            m_ItemList.Add(go);
        }
       

        for (int i = 0; i < m_ItemList.Count; i++)
        {
            m_ItemList[i].SystemTextChangePosition();
        }

        m_ItemList[m_ItemList.Count-1].gameObject.SetActive(true);
        m_ItemList[m_ItemList.Count-1].SystemTextShow(str);
    }

    public void ReturnItem()
    {
        if (m_ItemList.Count > 0)
        {
            m_ItemList.Clear();
        }
        else
        {
            // 암 것도 안 함
        }
    }

}
