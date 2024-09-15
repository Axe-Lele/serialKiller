using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogItem : MonoBehaviour {
    public UILabel m_NameLabel;
    public UISprite m_FaceSprite;
    public UILabel target_name;
    public UISprite m_BG;
    public GameObject names;
    private string m_BGName;

    private string m_Index;
    public void Setting(string who)
    {
        m_BGName = "Note_Dialog_ListBtn";

        m_Index = who;
        m_FaceSprite.atlas = NpcDataManager.instance.ReturnAtlas(who);
        m_NameLabel.text = Localization.Get("Name_" + PlayDataManager.instance.m_StageName + "_" +  who);
    }

    public void UnSelect()
    {
        //m_BG.spriteName = m_BGName + "off";
    }

    public string ReturnIndex()
    {
        return m_Index;
    }

    public void Select()
    {
        //NoteManager.instance.ShowDetailDialogList(m_Index);

        if (GameManager.instance.m_NoteMode == NoteMode.None)
        {
            //NoteManager.instance.ShowDetailDialogList(m_Index);
        }
        else if (GameManager.instance.m_NoteMode == NoteMode.Suggest)
        {
            SuggestManager.instance.Setting("Dialog", m_Index);
        }
        else if(GameManager.instance.m_NoteMode == NoteMode.Warrant)
        {
            WarrantManager.instance.SettingMode("Dialog", m_Index);
        }
        NoteManager.instance.ShowDetailDialogList(m_Index);
        //m_BG.spriteName = m_BGName + "on";
        m_FaceSprite.gameObject.SetActive(true);
        m_FaceSprite.spriteName = m_Index;
        names.SetActive(true);
        target_name.text = Localization.Get("Name_" + PlayDataManager.instance.m_StageName + "_" + m_Index);
    }
}
