using UnityEngine;
using System.Collections;

public class CaseItemInNote : MonoBehaviour
{
	public string m_CaseIndex;
    public UILabel m_TitleLabel;
    public UILabel m_ContentLabel;
    public int m_Index;
    private CaseMode m_Mode;
    public UISprite m_BG;
    private string m_BGName;

    public Sprite[] bgSpriteArr;

    public void Setting(int i, string str)
    {
        m_BGName = "Note_Case_ListBtn";
        m_Index = i;
        m_CaseIndex = str;
        //m_TitleLabel.text = Localization.Get("CaseIndex" + i);
        
		m_ContentLabel.text = Localization.Get("CaseIndex" + m_CaseIndex);
        //m_Label.text = Localization.Get("Case_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + m_CaseIndex + "_Title");
    }

    public void UnSelect()
    {
        m_BG.spriteName = "button_character_off";
    }

    public void Selected()
    {
        m_BG.spriteName = "button_character_on";
    }

	public void Select()
	{
        NoteManager.instance.SelectCase(m_Mode, m_CaseIndex);

        if (GameManager.instance.m_NoteMode == NoteMode.Suggest)
        {
            SuggestManager.instance.CaseSetting(m_Mode, m_CaseIndex);
        }
        else if (GameManager.instance.m_NoteMode == NoteMode.SelectCase)
        {
            WarrantManager.instance.CaseSetting(m_Mode, m_CaseIndex);
        }

        //m_BG.spriteName = m_BGName + "_Selected";

        m_BG.spriteName = "button_character_on";
    }
}
