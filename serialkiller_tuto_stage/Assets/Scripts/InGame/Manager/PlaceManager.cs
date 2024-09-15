using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : Singleton<PlaceManager>
{
	public List<PlaceItem> m_HomeList;
	public List<PlaceItem> m_CompanyList;
	public List<PlaceItem> m_ExtraPlaceList;
	public List<PlaceItem> m_CasePlaceList;
	//public List<PlaceItem> m_RoadPlaceList;

	public GameObject CommandDialog;
	public GameObject CommandSearch;
	public GameObject CommandSuggest;
	public GameObject m_TouchBackground;

	private PlaceItem m_item;
	public string m_Mode;

	public GameObject CommandSelectPanel;
	public UISprite m_CommandSelectPanelBG;
	public SelectFaceItem[] m_CharacterListInCommandSelect;

	public TweenAlpha m_DialogTA;
	public TweenAlpha m_SearchTA;
	public TweenAlpha m_SuggestTA;

	public TweenPosition m_DialogTP;
	public TweenPosition m_SearchTP;
	public TweenPosition m_SuggestTP;

	private string m_Place;

	private bool m_IsShowLabel = false;
	public GameObject m_PlacePanel;

	public void DataInitialize()
	{
		for (int i = 0; i < m_HomeList.Count; i++)
		{
			m_HomeList[i].DataInitialize(i);
			if (PlaceDataManager.instance.IsOpened(PlaceType.Home, i))
			{
				m_HomeList[i].gameObject.SetActive(true);
			}
			else
			{
				m_HomeList[i].gameObject.SetActive(false);
			}
		}

		for (int i = 0; i < m_CompanyList.Count; i++)
		{
			m_CompanyList[i].DataInitialize(i);
			if (PlaceDataManager.instance.IsOpened(PlaceType.Company, i))
			{
				m_CompanyList[i].gameObject.SetActive(true);
			}
			else
			{
				m_CompanyList[i].gameObject.SetActive(false);
			}
		}

		for (int i = 0; i < m_ExtraPlaceList.Count; i++)
		{
			m_ExtraPlaceList[i].DataInitialize(i);
			if (PlaceDataManager.instance.IsOpened(PlaceType.Extra, i))
			{
				m_ExtraPlaceList[i].gameObject.SetActive(true);
			}
			else
			{
				m_ExtraPlaceList[i].gameObject.SetActive(false);
			}
		}

		for (int i = 0; i < m_CasePlaceList.Count; i++)
		{
			m_CasePlaceList[i].DataInitialize(i);

			if (PlaceDataManager.instance.IsOpened(PlaceType.Case, i))
			{
				m_CasePlaceList[i].gameObject.SetActive(true);
			}
			else
			{
				m_CasePlaceList[i].gameObject.SetActive(false);
			}
		}
	}

	public void ControlPlaceButton(PlaceType type, int index, bool b)
	{
		switch (type)
		{
			case PlaceType.Home:
				if (b)
					m_HomeList[index].gameObject.SetActive(true);
				else
					m_HomeList[index].gameObject.SetActive(false);
				break;
			case PlaceType.Company:
				if (b)
					m_CompanyList[index].gameObject.SetActive(true);
				else
					m_CompanyList[index].gameObject.SetActive(false);
				break;
			case PlaceType.Extra:
				if (b)
					m_ExtraPlaceList[index].gameObject.SetActive(true);
				else
					m_ExtraPlaceList[index].gameObject.SetActive(false);
				break;
			case PlaceType.Case:
				if (b)
					m_CasePlaceList[index].gameObject.SetActive(true);
				else
					m_CasePlaceList[index].gameObject.SetActive(false);
				break;
		}
	}

	public void ClickPlaceItem(PlaceItem item, Vector2 vec)
	{
		m_item = item;
		m_Place = m_item.m_Type.ToString() + "_" + m_item.ReturnIndex();

		m_PlacePanel.SetActive(true);
		m_PlacePanel.GetComponent<PlacePanel>().Init(m_item);

	}

	public void ClickBackground()
	{
		print("ClickBackground");
		m_PlacePanel.SetActive(false);
		CommandDialog.SetActive(false);
		CommandSearch.SetActive(false);
		CommandSuggest.SetActive(false);
		CommandSelectPanel.SetActive(false);
		//    m_TouchBackground.SetActive(false);
	}

	public void ClickCommandDialog()
	{
		m_Mode = "Dialog";
		GlobalValue.instance.isShowPersonPanel = true;
		ShowSelectCharacterList();
	}

	/// <summary>
	/// Activate 'Search Button' on PlacePanel
	/// </summary>
	public void ClickCommandSearch()
	{
		print("Place Event(" + m_Place + ") : Search / " + PlaceDataManager.instance.CanSearched(m_Place));
		if (PlaceDataManager.instance.CanSearched(m_Place))
		{
			SystemTextManager.instance.InputText(Localization.Get("System_Text_Already_Search"));
		}
		else
		{
			EventManager.instance.SetSearchEvent(m_Place);
			EventDataManager.instance.SwapSearchEventData(m_Place, true);
			PlaceDataManager.instance.ControlPlaceIsSearched(m_Place, true);
		}
	}

	public void ClickCommandSuggest()
	{
		m_Mode = "Suggest";
		GlobalValue.instance.isShowPersonPanel = true;
		ShowSelectCharacterList();
	}


	public void ShowSelectCharacterList()
	{
		int person = m_item.m_CharacterList.Count;
		CommandSelectPanel.SetActive(true);
		for (int i = 0; i < person; i++)
		{
			m_CharacterListInCommandSelect[i].Setting(m_item.m_CharacterList[i]);
			m_CharacterListInCommandSelect[i].gameObject.SetActive(true);
		}

		Vector2 vec;
		switch (person)
		{
			case 1:
				vec = new Vector2(0f, 0f);
				m_CommandSelectPanelBG.width = 280;
				break;
			case 2:
				vec = new Vector2(-45f, 0f);
				m_CommandSelectPanelBG.width = 280;
				break;
			case 3:
				vec = new Vector2(-80f, 0f);
				m_CommandSelectPanelBG.width = 280;
				break;
			case 4:
				vec = new Vector2(-155f, 0f);
				m_CommandSelectPanelBG.width = 360;
				break;
			case 5:
				vec = new Vector2(-160f, 0f);
				m_CommandSelectPanelBG.width = 440;
				break;
			default:
				vec = new Vector2(-160f, 0f);
				break;
		}

		for (int i = 0; i < m_CharacterListInCommandSelect.Length; i++)
		{
			m_CharacterListInCommandSelect[i].gameObject.SetActive(false);
		}

		for (int i = 0; i < person; i++)
		{
			m_CharacterListInCommandSelect[i].gameObject.SetActive(true);
			m_CharacterListInCommandSelect[i].transform.localPosition = new Vector2(vec.x + (i * 80f), 8f);
		}
	}

	public void ShowLabel()
	{
		m_IsShowLabel = !m_IsShowLabel;

		for (int i = 0; i < m_HomeList.Count; i++)
		{
			m_HomeList[i].ShowLabel(m_IsShowLabel);
		}

		for (int i = 0; i < m_CompanyList.Count; i++)
		{
			m_CompanyList[i].ShowLabel(m_IsShowLabel);
		}

		for (int i = 0; i < m_ExtraPlaceList.Count; i++)
		{
			m_ExtraPlaceList[i].ShowLabel(m_IsShowLabel);
		}

		for (int i = 0; i < m_CasePlaceList.Count; i++)
		{
			m_CasePlaceList[i].ShowLabel(m_IsShowLabel);
		}

	}

	public void HideSelectCharacterList()
	{
		m_PlacePanel.SetActive(false);
		CommandSelectPanel.SetActive(false);
		for (int i = 0; i < m_item.m_CharacterList.Count; i++)
		{
			m_CharacterListInCommandSelect[i].gameObject.SetActive(false);
		}
	}



	public void ClickFace(string target)
	{
		print("mode : " + m_Mode + " / index : " + target);
		switch (m_Mode)
		{
			case "Dialog":
				GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.Start, target, "0", m_Place);
				break;
			case "Search":
				break;
			case "Suggest":
				GameManager.instance.m_NoteMode = NoteMode.Suggest;
				SuggestManager.instance.SetTarget(target);
				InGameUIManager.instance.ControlNotePopup();
				break;
		}
		ClickBackground();
		HideSelectCharacterList();
	}

	public void SetSearchPossible(string code)
	{
		string[] temp = code.Split('_');

		switch ((PlaceType)System.Enum.Parse(typeof(PlaceType), temp[0]))
		{
			case PlaceType.Home:
				m_HomeList[int.Parse(temp[1])].m_SearchPossible = true;
				break;
			case PlaceType.Company:
				m_CompanyList[int.Parse(temp[1])].m_SearchPossible = true;
				break;
			case PlaceType.Extra:
				m_ExtraPlaceList[int.Parse(temp[1])].m_SearchPossible = true;
				break;
			case PlaceType.Case:
				m_CasePlaceList[int.Parse(temp[1])].m_SearchPossible = true;
				break;
		}
	}

	public string ReturnPlace()
	{
		return m_Place;
	}

	public void OnMouseOverDialogCommand()
	{
		m_DialogTA.from = 0f;
		m_DialogTA.to = 1f;

		m_DialogTA.ResetToBeginning();
		m_DialogTA.enabled = true;

		m_DialogTP.from = Vector2.zero;
		m_DialogTP.to = new Vector2(0f, 42f);

		m_DialogTP.ResetToBeginning();
		m_DialogTP.enabled = true;
	}

	public void OnMouseOverSearchCommand()
	{
		m_SearchTA.from = 0f;
		m_SearchTA.to = 1f;

		m_SearchTA.ResetToBeginning();
		m_SearchTA.enabled = true;

		m_SearchTP.from = Vector2.zero;
		m_SearchTP.to = new Vector2(0f, 42f);

		m_SearchTP.ResetToBeginning();
		m_SearchTP.enabled = true;
	}

	public void OnMouseOverSuggestCommand()
	{
		m_SuggestTA.from = 0f;
		m_SuggestTA.to = 1f;

		m_SuggestTA.ResetToBeginning();
		m_SuggestTA.enabled = true;

		m_SuggestTP.from = Vector2.zero;
		m_SuggestTP.to = new Vector2(0f, 42f);

		m_SuggestTP.ResetToBeginning();
		m_SuggestTP.enabled = true;
	}

	public void OutMouseOverDialogCommand()
	{
		m_DialogTA.from = 1f;
		m_DialogTA.to = 0f;

		m_DialogTA.ResetToBeginning();
		m_DialogTA.enabled = true;

		m_DialogTP.from = new Vector2(0f, 42f);
		m_DialogTP.to = Vector2.zero;

		m_DialogTP.ResetToBeginning();
		m_DialogTP.enabled = true;
	}

	public void OutMouseOverSearchCommand()
	{
		m_SearchTA.from = 1f;
		m_SearchTA.to = 0f;

		m_SearchTA.ResetToBeginning();
		m_SearchTA.enabled = true;

		m_SearchTP.from = new Vector2(0f, 42f);
		m_SearchTP.to = Vector2.zero;

		m_SearchTP.ResetToBeginning();
		m_SearchTP.enabled = true;
	}

	public void OutMouseOverSuggestCommand()
	{
		m_SuggestTA.from = 1f;
		m_SuggestTA.to = 0f;

		m_SuggestTA.ResetToBeginning();
		m_SuggestTA.enabled = true;

		m_SuggestTP.from = new Vector2(0f, 42f);
		m_SuggestTP.to = Vector2.zero;

		m_SuggestTP.ResetToBeginning();
		m_SuggestTP.enabled = true;
	}
}