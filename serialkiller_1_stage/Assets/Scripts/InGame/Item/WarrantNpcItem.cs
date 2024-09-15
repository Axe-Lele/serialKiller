using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarrantNpcItem : MonoBehaviour {

    private NpcItem m_Item;
    public UILabel m_NameLabel;

    public string ReturnNpcName()
    {
        return m_Item.Name;
    }
    public void Setting(NpcItem item)
    {
        m_Item = item;
        m_NameLabel.text = Localization.Get( m_Item.Name);
    }

    public void OnClickEvent()
    {
        WarrantManager.instance.SelectNpc(m_Item);
    }
}
