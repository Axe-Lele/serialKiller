using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTabItem : MonoBehaviour
{
    public UILabel label;
    public int type = 0;
    public string content;
    public PlacePanel panel;
    public UISprite bg;
    // Use this for initialization
    void Start()
    {

    }

    public void Init(int type, string content)
    {
        this.content = content;
        this.type = type;
        if (type == 0)
        {
            label.text = content;
        }
        else
        {
            label.text = Localization.Get("Name_" + PlayDataManager.instance.m_StageName + "_" + content);
        }
    }

    public void Select()
    {
        panel.m_NpcContentRoot.SetActive(type == 1);
        panel.m_PlaceContentRoot.SetActive(type == 0);

        panel.m_SelectedItem = this;
        panel.ShowDesc(type, content);
    }
}