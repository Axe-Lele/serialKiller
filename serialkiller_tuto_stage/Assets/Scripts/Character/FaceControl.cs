using UnityEngine;
using System.Collections;

public class FaceControl : MonoBehaviour {

    public UISprite m_Face;
    public UISprite m_Cover;

    private string m_PrevCharacter = "";

    // 감정은 나중에
    public void FaceSetting(string str)
    {
        if (m_PrevCharacter != str)
        {
            m_PrevCharacter = str;
            m_Cover.atlas = m_Face.atlas = NpcDataManager.instance.NpcAtlas[NpcDataManager.instance.ReturnNpc(str).m_AtlasIndex];
        }
        m_Cover.spriteName = m_Face.spriteName = str;
    }

    public void FaceSetting(string str, string emotion)
    {
        if (m_PrevCharacter != str)
        {
            m_PrevCharacter = str;
            m_Cover.atlas = m_Face.atlas = NpcDataManager.instance.NpcAtlas[NpcDataManager.instance.ReturnNpc(str).m_AtlasIndex];
        }

        m_Face.spriteName = m_PrevCharacter + "_" + emotion;
    }

    // 말을 하지 않는 캐릭터 위에 어둡게 처리하는 부분
    public void TalkerActive(bool b)
    {
        if (b == true)
            m_Cover.gameObject.SetActive(false);
        else
            m_Cover.gameObject.SetActive(true);
    }





    /*public void FaceSetting(int AtlasIndex, string str)
    {
        face[0].atlas = GlobalValue.instance.Character_Atlas[AtlasIndex];
        face[0].spriteName = str;
    }

    public void FaceSetting(string type, string name)
    {
        AtlasIndex = 0;
        if (type == "Common")
        {
            AtlasIndex = 0;
        }
        else if (type == "Suspect")
        {
            AtlasIndex = 1;
        }

        for (int i = 0; i < face.Length; i++)
        {
            face[i].atlas = GlobalValue.instance.Character_Atlas[AtlasIndex];
        }
        face[0].spriteName = type + "_" + name + "_Body";
        face[1].spriteName = type + "_" + name + "_Face";
        face[2].spriteName = type + "_" + name + "_Accessory";
    }

    public void SetBG(Color c)
    {
        bg.color = c;
    }

    public void Num()
    {
        //   FaceControl[] fc = Object.FindObjectsOfType<FaceControl>();// transform.Find("Face").GetComponents<FaceControl>().Length;

        // print("len : " + fc.Length);

        for (int i = 0; i < face.Length; i++)
        {
            face[i].height = face[i].width = 32;
        }
    }

    public void Select()
    {
        print("Select method in FaceControl");
        if (gameObject.transform.FindChild("CharacterBG"))
        {
            bg = gameObject.transform.FindChild("CharacterBG").GetComponent<UISprite>();
        }
    }*/
}
