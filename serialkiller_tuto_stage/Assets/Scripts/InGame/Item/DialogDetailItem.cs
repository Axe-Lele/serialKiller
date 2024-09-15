using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDetailItem : MonoBehaviour
{
    public UILabel m_NameLabel;
    public UISprite m_BG;
    private string m_Target;
    private string m_Index;
    private string m_BGName;
    public void Setting(string who, string dialog)
    {
        m_BGName = "button_talk";
        m_Target = who;
        m_Index = dialog;
        m_NameLabel.text = Localization.Get("Selection_" + PlayDataManager.instance.m_StageName + "_" + who + "_" + dialog);
        //m_NameLabel.text = Localization.Get("Dialog_" + PlayDataManager.instance.m_StageName + "_" + who + "_" + dialog + "_0");
    }

    public string ReturnCharacter()
    {
        return m_Target;
    }

    public string ReturnIndex()
    {
        return m_Index;
    }

    public void UnSelect()
    {
        m_BG.spriteName = m_BGName + "_off";
    }

    public void Select()
    {
        if (GameManager.instance.m_NoteMode == NoteMode.Suggest)
        {
            SuggestManager.instance.Setting("Dialog", (m_Target + "_" + m_Index));
            m_BG.spriteName = m_BGName + "_on";
        }
        else if (GameManager.instance.m_NoteMode == NoteMode.Warrant)
        {
            WarrantManager.instance.SettingMode("Dialog", (m_Target + "_" + m_Index));
            m_BG.spriteName = m_BGName + "_on";
        }
        else
        {
            GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.InNote, m_Target, m_Index, DialogDataManager.instance.ReturnPlace(m_Target, m_Index));
        }

    }
}
