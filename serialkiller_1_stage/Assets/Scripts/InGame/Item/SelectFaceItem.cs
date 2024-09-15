using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFaceItem : MonoBehaviour {

    private string m_Target;
    public UISprite m_CharacterSprite;
    public UILabel m_Label;
    public void Setting( string index)
    {
        m_Target = index;
        m_CharacterSprite.atlas = NpcDataManager.instance.ReturnAtlas(index);
        m_CharacterSprite.spriteName = index;
        m_Label.text = NpcDataManager.instance.ReturnNpc(index).Name;
    }

    public void ClickFace()
    {
        PlaceManager.instance.ClickFace(m_Target);
    }
}
