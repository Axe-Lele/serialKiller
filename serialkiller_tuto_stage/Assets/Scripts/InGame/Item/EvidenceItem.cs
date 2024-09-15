using UnityEngine;
using System.Collections;

public class EvidenceItem : MonoBehaviour
{

    public UILabel m_TitleLabel;
    public UISprite m_ImageIcon;

    private EvidenceDataItem item;
    public string m_EveidenceName
    {
        get
        {
            if (item == null)
                return string.Empty;
            return item.m_EvidenceName;
        }
    }

    public UISprite m_BG;
    private string m_BGName;

    public void Init(EvidenceDataItem i)
    {
        m_BGName = "Note_Evidence_ListBtn";

        item = i;
        m_TitleLabel.text = Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + item.m_EvidenceName + "_Title");
        m_ImageIcon.spriteName = "Evidence_" + PlayDataManager.instance.m_StageName + "_" + item.m_EvidenceName;
    }

    public void UnSelect()
    {
        m_BG.spriteName = "button_character_off";
    }

    public void ActivateButton()
    {
        m_BG.spriteName = "button_character_on";
    }

    /// <summary>
    /// OnClick 이벤트와 같이 사용됨. 망함.
    /// </summary>
    public void Select()
    {
        if (GameManager.instance.m_NoteMode == NoteMode.Suggest)
        {
            SuggestManager.instance.Setting("Evidence", item.m_EvidenceName);
        }
        else if (GameManager.instance.m_NoteMode == NoteMode.Warrant)
        {
            WarrantManager.instance.SettingMode("Evidence", item.m_EvidenceName);
        }

        NoteManager.instance.ChangeEvidenceInfoUI(item);

        ActivateButton();
    }

    public EvidenceDataItem ReturnItem()
    {
        return item;
    }

}
