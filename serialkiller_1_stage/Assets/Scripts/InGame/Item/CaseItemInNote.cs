using UnityEngine;
using System.Collections;

public class CaseItemInNote : MonoBehaviour
{
	public bool m_IsSelected = false;
	
	public string m_Index;

	public UILabel m_ButtonLabel;
	public UISprite m_ButtonSprite;
	private CaseMode m_Mode;

	//public Sprite[] bgSpriteArr;
	//private string m_BGName;
	
	/// <param name="i"></param>
	/// <param name="mode"></param>
	/// <param name="caseItemCode">0, 1, 2</param>
	public void Setting(int i, CaseMode mode, string caseItemCode)
	{
		//m_BGName = "Note_Case_ListBtn";
		//m_TitleLabel.text = Localization.Get("CaseIndex" + i);
		m_Index = caseItemCode;
		m_Mode = mode;

		m_ButtonLabel.text
			= Localization.Get("Place_" + PlayDataManager.instance.m_StageName + "_Case_" + caseItemCode);
		//m_Label.text = Localization.Get("Case_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + m_CaseIndex + "_Title");
	}

	public void UnSelect()
	{
		m_IsSelected = false;
		m_ButtonSprite.spriteName = "button";
	}

	public void Selected()
	{
		print("selected");
		m_IsSelected = true;
		m_ButtonSprite.spriteName = "button_check";
	}

	public void Select()
	{
		switch (GameManager.instance.m_NoteMode)
		{
			case NoteMode.None:
				NoteManager.instance.SelectCase(m_Mode, m_Index);
				m_IsSelected = true;
				break;

			case NoteMode.Suggest:
				if (m_IsSelected)
				{
					SuggestManager.instance.Suggest();
					m_IsSelected = false;
				}
				else
				{
					NoteManager.instance.SelectCase(m_Mode, m_Index);
					SuggestManager.instance.CaseSetting(m_Mode, m_Index);
					m_IsSelected = true;
				}
				break;


			case NoteMode.Warrant:
				break;

			case NoteMode.SelectCase:
				if (m_IsSelected)
				{
					print("submit case");
					WarrantManager.instance.SubmitCase("Case", m_Index);
					m_IsSelected = false;
				}
				else
				{
					NoteManager.instance.SelectCase(m_Mode, m_Index);
					m_IsSelected = true;
				}
				break;

			default:
				break;
		}

		m_ButtonSprite.spriteName = "button_check";
	}
}
