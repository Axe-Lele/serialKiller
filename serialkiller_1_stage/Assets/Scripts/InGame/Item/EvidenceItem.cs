using UnityEngine;
using System.Collections;

public class EvidenceItem : MonoBehaviour
{
	public bool m_IsSelected = false;

	public UILabel m_TitleLabel;
	public UISprite m_ImageIcon;

	private EvidenceDataItem item;
	public string m_EveidenceName
	{
		get
		{
			if (item == null)
				return string.Empty;
			return item.m_ItemCode;
		}
	}

	public UISprite m_BG;
	private string m_BGName;

	public void Init(EvidenceDataItem i)
	{
		m_BGName = "Note_Evidence_ListBtn";

		item = i;
		m_TitleLabel.text = Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + item.m_ItemCode + "_Title");
		i.m_IsEnable = true;
	}

	public void UnSelect()
	{
		m_IsSelected = false;
		//m_BG.spriteName = "button_character_off";
	}

	public void ActivateButton()
	{
		m_IsSelected = true;
		//m_BG.spriteName = "button_character_on";
	}

	/// <summary>
	/// OnClick 이벤트와 같이 사용됨. 망함.
	/// </summary>
	public void Select()
	{
		print(item.m_ItemCode + " / IsSelected : " + m_IsSelected);
		switch (GameManager.instance.m_NoteMode)
		{
			case NoteMode.None:
				ActivateButton();
				break;

			case NoteMode.Suggest:
				if (m_IsSelected)
				{
					SuggestManager.instance.Suggest();
					m_IsSelected = false;

				}
				else
				{
					SuggestManager.instance.Setting("Evidence", item.m_ItemCode);
					NoteManager.instance.ChangeEvidenceInfoUI(item);
					m_IsSelected = true;
				}
				break;

			case NoteMode.Warrant:
				if (m_IsSelected)
				{
					WarrantManager.instance.SubmitEvidence("Evidence", item.m_ItemCode);
					m_IsSelected = false;
				}
				else
				{
					print(item.m_ItemCode + " / IsSelected : " + m_IsSelected);
					NoteManager.instance.ChangeEvidenceInfoUI(item);
					m_IsSelected = true;
					print(item.m_ItemCode + " / IsSelected : " + m_IsSelected);
				}

				//WarrantManager.instance.SettingMode("Evidence", item.m_EvidenceName);
				break;

			case NoteMode.SelectCase:
				ActivateButton();
				break;

			default:
				break;
		}

		NoteManager.instance.ChangeEvidenceInfoUI(item);
		print(item.m_ItemCode + " / IsSelected : " + m_IsSelected);
	}

	public EvidenceDataItem ReturnItem()
	{
		return item;
	}

}
