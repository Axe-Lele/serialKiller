using UnityEngine;
using System.Collections;

using Extension.NGUI;

public class FaceControl : MonoBehaviour
{
	public UISprite m_Face;
	public UISprite m_Cover;
	public GameObject m_NameTag;
	public UILabel m_NpcName;
	public UISprite m_BackGround;

	/// <summary>
	/// 이전 캐릭터 ItemCode
	/// </summary>
	private string m_PrevCharacter = "";

	// 감정은 나중에
	public void FaceSetting(string spriteName)
	{
		FaceSetting(spriteName, string.Empty);
		return;
	}

	/// <summary>
	/// 캐릭터 일러스트 교체
	/// </summary>
	/// <param name="itemCode">Npc ItemCode</param>
	/// <param name="emotion"></param>
	public void FaceSetting(string itemCode, string emotion)
	{
		if (string.IsNullOrEmpty(itemCode))
		{
			m_Cover.ResetSprite();
			m_Face.ResetSprite();
		}

		Debug.Log("FaceSetting - itemCode : " + itemCode + " / emotion : " + emotion);

		string spriteName = itemCode;
		if (emotion.Length != 0 && emotion != null)
			spriteName = string.Format("{0}_{1}", spriteName, emotion);

		/// 이전 캐릭터와 다른 캐릭터라면
		/// PrevCharacter와 Atlas를 갱신한다.
		if (m_PrevCharacter != itemCode)
		{
			m_PrevCharacter = itemCode;
			m_Cover.atlas = AtlasManager.instance.GetAtlas(AtlasManager.SpriteType.Character.ToString(),
														   spriteName);
			m_Face.atlas = AtlasManager.instance.GetAtlas(AtlasManager.SpriteType.Character.ToString(),
																	   spriteName);
		}

		m_Cover.spriteName = spriteName;
		m_Face.spriteName = spriteName;

		if (m_NpcName != null)
			m_NpcName.text = NpcDataManager.instance.ReturnName(itemCode);
	}

	// 말을 하지 않는 캐릭터 위에 어둡게 처리하는 부분
	public void TalkerActive(bool b)
	{
		if (b == true)
		{
			m_Face.depth = m_BackGround.depth + 1;
			m_Cover.gameObject.SetActive(false);
			if (m_NameTag != null)
				m_NameTag.SetActive(true);
		}
		else
		{
			m_Face.depth = m_BackGround.depth - 1;
			m_Cover.gameObject.SetActive(true);
			if (m_NameTag != null)
				m_NameTag.SetActive(false);
		}
	}

	#region non-used
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
	#endregion
}
