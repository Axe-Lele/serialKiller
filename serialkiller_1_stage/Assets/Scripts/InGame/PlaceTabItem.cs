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
        panel.characterContent.SetActive(type == 1);
        panel.infoContent.SetActive(type == 0);
        if (type == 1)
        {
            panel.profile.spriteName = content;
            panel.profileContent.text = Localization.Get("Name_" + PlayDataManager.instance.m_StageName + "_" + content);
        }
        if (panel.selected != null)
        {
            panel.selected.bg.spriteName = "button_character_1";
        }
        bg.spriteName = "button_street_1";
        panel.selected = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}