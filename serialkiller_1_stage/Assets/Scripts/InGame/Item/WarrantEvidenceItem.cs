using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarrantEvidenceItem : MonoBehaviour
{

	public int m_Index;
	[SerializeField] private string m_Code;
	[SerializeField] private string m_ItemIndex;
	[SerializeField] private string m_Mode;
	public UILabel m_NameLabel;
	public UISprite m_Itemicon;
	public UISprite m_PlusSprite;

	public NoteMode m_NoteMode;
	
	public void Setting(string mode, string code)
	{
		m_Mode = mode;
		m_Code = code;
		m_Itemicon.gameObject.SetActive(true);
		m_Itemicon.enabled = false;
		m_PlusSprite.gameObject.SetActive(false);

		string str;
		switch (mode)
		{
			case "Case":
				//m_Itemicon.spriteName = code;
				m_ItemIndex = string.Format("Case_{0}_{1}_{2}", PlayDataManager.instance.m_StageName, CaseMode.Main, m_Code);
				str = m_ItemIndex;
				WarrantManager.instance.SetCaseIndex(m_Code);
				break;

			case "Dialog":
				//m_Itemicon.spriteName = code;
				m_ItemIndex = "Selection_" + PlayDataManager.instance.m_StageName + "_" + m_Code;
				str = m_ItemIndex;
				WarrantManager.instance.SettingEvidence(m_Index, m_ItemIndex);
				break;

			case "Evidence":
				//m_Itemicon.spriteName = code;
				m_ItemIndex = "Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + m_Code;
				str = m_ItemIndex + "_Title";
				WarrantManager.instance.SettingEvidence(m_Index, m_ItemIndex);
				break;

			case "News":
				//m_Itemicon.spriteName = code;
				m_ItemIndex = "News_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + m_Code;
				str = m_ItemIndex + "_Title";
				WarrantManager.instance.SettingEvidence(m_Index, m_ItemIndex);
				break;

			default:
				str = "";
				print("해당 사항 없음");
				break;
		}

		m_NameLabel.text = Localization.Get(str);
	}

	public void SelectedEvidence()
	{
		if (m_Itemicon.gameObject.activeInHierarchy == true)
		{
			// 취소
			m_Mode = "";
			m_Code = "";
			m_ItemIndex = "";
			m_NameLabel.text = "";

			m_Itemicon.gameObject.SetActive(false);
			m_PlusSprite.gameObject.SetActive(true);
		}
		else
		{
			InGameUIManager.instance.ControlWarrantPopup(UIPopupFlag.Close);

			print("m_NoteMode : " + m_NoteMode);
			switch (m_NoteMode)
			{
				case NoteMode.None:
					break;

				case NoteMode.Suggest:
					break;

				case NoteMode.Warrant:
					GameManager.instance.m_NoteMode = NoteMode.Warrant;
					InGameUIManager.instance.ShowNote();
					WarrantManager.instance.ClickSelectEvidence(m_Index);
					break;

				case NoteMode.SelectCase:
					GameManager.instance.m_NoteMode = NoteMode.SelectCase;
					InGameUIManager.instance.ShowNote();
					break;

				default:
					break;
			}

			//InGameUIManager.instance.ControlNotePopup();
		}
	}

	public string ReturnMode()
	{
		return m_Mode;
	}

	public string ReturnCode()
	{
		return m_Code;
	}

	public string ReturnIndex()
	{
		return m_ItemIndex;
	}
}
