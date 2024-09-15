using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItem : MonoBehaviour
{
	public List<string> m_CharacterList;
	public List<string> m_EvidenceList;

	//public List<string> m_ResideCharacterList;
	public PlaceType m_Type;

	public UISprite m_Sprite;
	public UISprite m_Frame;
	public TweenColor m_TC;
	public UILabel m_NameLabel;
	public PolygonCollider2D[] m_Coliider;
	private string m_PlaceName;
	private int m_Index;
	public string m_CaseIndex;

	public bool m_SearchPossible = false;

	private void Awake()
	{
		m_CharacterList = new List<string>();
		m_EvidenceList = new List<string>();
		//m_ResideCharacterList = new List<string>();
		//m_Type = PlaceType.Company;
	}

	public void DataInitialize(int index)
	{
		m_Index = index;
		GetName();
	}

	public void ShowLabel(bool b)
	{
		m_NameLabel.gameObject.SetActive(b);
	}

	public void GetName()
	{
		m_PlaceName = gameObject.name;


		if (m_Type != PlaceType.Case)
		{
			m_NameLabel.text = Localization.Get("Place_" + PlayDataManager.instance.m_StageName + "_" + m_PlaceName);
			//print(m_NameLabel.text);
		}
		else
		{
			m_NameLabel.text = Localization.Get("CaseIndex" + m_CaseIndex);
		}
	}

	public void SetCase(string str)
	{
		print("str : " + str);
		m_CaseIndex = str;
	}

	public void AddCharacter(string str)
	{
		bool b = false;
		if (m_CharacterList.Count > 0)
		{
			for (int i = 0; i < m_CharacterList.Count; i++)
			{
				if (m_CharacterList[i] == str)
				{
					b = true;
					break;
				}
			}

			if (b == false)
			{
				m_CharacterList.Add(str);
			}
		}
		else
		{
			//    print("아무도 없으니 집어넣는다");
			m_CharacterList.Add(str);
		}
	}

	public void RemoveCharacter(string str)
	{
		if (m_CharacterList.Count > 0)
		{
			for (int i = 0; i < m_CharacterList.Count; i++)
			{
				if (m_CharacterList[i] == str)
				{
					m_CharacterList.RemoveAt(i);
					break;
				}
			}
		}
		/*else
		{
			 print("아무도 없으니 어떤 일도 일어나지 않는다");
		}*/
	}

	public void ControlToggle(bool b)
	{
		m_NameLabel.gameObject.SetActive(b);
	}

	public void ClickPlace()
	{
		print("click event");
		switch (m_Type)
		{
			//case PlaceType.Case:
			//	InGameUIManager.instance.ControlNotePopup();
			//	PlaceManager.instance.ClickBackground();
			//	bool b = false;
			//	for (int i = 0; i < StageDataManager.instance.m_CheckCaseList.Count; i++)
			//	{
			//		if (StageDataManager.instance.m_CheckCaseList[i] == m_CaseIndex)
			//		{
			//			b = true;
			//			break;
			//		}
			//	}

			//	// 사건 첫 조우
			//	if (b == false)
			//	{
			//		StageDataManager.instance.m_CheckCaseList.Add(m_CaseIndex);
			//		NoteManager.instance.InputCase(CaseMode.Main, m_CaseIndex);
			//		GameManager.instance.UserAction(UserActionType.Search);
			//		SystemTextManager.instance.InputText(Localization.Get("System_Text_RefreshCaseList"));
			//		EventDataManager.instance.ChangeCasePlaceEventData(m_CaseIndex);
			//	}
			//	GameManager.instance.m_NoteMode = NoteMode.None;


			//	NoteManager.instance.SelectCase(CaseMode.Main, m_CaseIndex);
			//	NoteManager.instance.SelectedNoteTab(0);
			//	//PlaceDataManager.instance.ControlPlaceIsOpened(PlaceType.Case, m_Index, false);
			//	PlaceManager.instance.ClickBackground();
			//	OutHoverEvent();

			//	break;
			default:
				break;
		}

		PlaceManager.instance.ClickPlaceItem(this, transform.localPosition);
		print("click place item");
	}

	public int ReturnIndex()
	{
		return m_Index;
	}

	public void OnHoverEvent()
	{
		//m_TC.gameObject.SetActive(false);

		//PlaceManager.instance.
		InGameUIManager.instance.ShowPlaceName(m_Type, m_Index, m_NameLabel.text);
	}

	public void OutHoverEvent()
	{
		//m_TC.gameObject.SetActive(true);
		//print("OutHoverEvent " + m_PlaceName);
		InGameUIManager.instance.HidePlaceName();
		/*if (GlobalValue.instance.isShowPersonPanel == false)
		{
				m_Sprite.color = Color.white;
				m_TC.gameObject.SetActive(true);
				//print("OutHoverEvent " + m_PlaceName);
				InGameUIManager.instance.HidePlaceName();
		}*/
	}

}
