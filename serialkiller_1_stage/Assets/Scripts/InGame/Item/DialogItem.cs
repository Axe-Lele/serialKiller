using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Extension.NGUI;

public class DialogItem : MonoBehaviour
{
	public bool m_IsSelected = false;
	public UILabel m_NameLabel;
	public UISprite m_FaceSprite;
	public UISprite m_Shadow;
	public UILabel target_name;
	public UISprite m_BG;
	public GameObject names;
	private string m_BGName;

	public string m_Index;
	public void Setting(string who)
	{
		m_BGName = "Note_Dialog_ListBtn";

		m_Index = who;
		m_FaceSprite.atlas = NpcDataManager.instance.ReturnAtlas(who);
		m_NameLabel.text = Localization.Get("Name_" + PlayDataManager.instance.m_StageName + "_" + who);
	}

	public void UnSelect()
	{
		m_IsSelected = false;
		//m_BG.spriteName = m_BGName + "off";
	}

	public string ReturnIndex()
	{
		return m_Index;
	}

	public void Select()
	{
		switch (GameManager.instance.m_NoteMode)
		{
			case NoteMode.None:
				break;

			case NoteMode.Suggest:
				if (m_IsSelected)
				{
					SuggestManager.instance.Suggest();
					m_IsSelected = false;
				}
				else
				{
					SuggestManager.instance.Setting("Dialog", m_Index);
					NoteManager.instance.SelectReset();
					m_IsSelected = true;
				}
				break;

			case NoteMode.Warrant:
				if (m_IsSelected)
				{
					WarrantManager.instance.SettingMode("Dialog", m_Index);
					m_IsSelected = false;
				}
				else
				{
					NoteManager.instance.SelectReset();
					m_IsSelected = true;
				}
				break;

			case NoteMode.SelectCase:
				// non-used
				break;

			default:
				break;
		}

		NoteManager.instance.ShowDetailDialogList(m_Index);

		m_FaceSprite.atlas = AtlasManager.instance.GetAtlas(AtlasManager.SpriteType.Character.ToString(),
															m_Index);
		m_FaceSprite.spriteName = (m_Index);
		m_FaceSprite.gameObject.SetActive(true);

		m_Shadow.atlas = m_FaceSprite.atlas;
		m_Shadow.spriteName = m_FaceSprite.spriteName;
		m_Shadow.transform.localPosition = Vector3.zero;

		names.SetActive(true);
		target_name.text = Localization.Get("Name_" + PlayDataManager.instance.m_StageName + "_" + m_Index);
	}
}
