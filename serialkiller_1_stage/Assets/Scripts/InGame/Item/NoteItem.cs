using UnityEngine;
using System.Collections;

public class NoteItem : MonoBehaviour
{
    public int itemNumber;
    public UISprite sprite;
    public string originSpriteName;
    public void Select()
    {
        NoteManager.instance.SelectedNoteTab(itemNumber);
    }

    public void ActiveSprite()
    {
        sprite.spriteName = "menu_note_" + originSpriteName + "_on";
        //sprite.spriteName = "Note_TabBtn_Active";
    }

    public void UnactiveSrptie()
    {
        sprite.spriteName = "menu_note_" + originSpriteName;
        //sprite.spriteName = "Note_TabBtn_Unactive";
    }
    
}
