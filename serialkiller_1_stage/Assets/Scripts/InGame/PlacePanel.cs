using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Extension.NGUI;

public class PlacePanel : MonoBehaviour
{
	public PlaceItem m_NowItem;

	[Space]
	public GameObject m_NpcContentRoot;
	public GameObject m_PlaceContentRoot;

	[Space]
	public UILabel m_PlaceName;
	public UISprite m_Illust;
	public UILabel m_PlaceContent;

	[Space]
	public UILabel m_NpcName;
	public UILabel m_NpcContent;
	public UISprite m_Portrait;
	public TweenPosition m_Shadow;
	public GameObject m_Checker;

	[Space]
	public GameObject table;
	public GameObject tabItem;

	[Header("Root")]
	public Transform m_PlaceItemRoot;
	public Transform m_NPCItemRoot;

	public PlaceTabItem m_SelectedItem;

	[Space]
	public GameObject m_MapRoot;
	public GameObject m_NewsRoot;

	private enum KeyType
	{
		PLACE_TITLE,
		PLACE_INFO,
		PLACE_BUTTON,
		PLACE_NPC_INFO,
	}
	private string[] keyTitle =
	{
		"Place_{0}_{1}",
		"Place_{0}_{1}_Info"
	};
	private string GetKey(KeyType type)
	{
		string placeName = m_NowItem.m_Type.ToString() + "_" + m_NowItem.ReturnIndex();
		switch (type)
		{
			case KeyType.PLACE_TITLE:
				return string.Format(keyTitle[(int)KeyType.PLACE_TITLE]
						, PlayDataManager.instance.m_StageName
						, placeName);

			case KeyType.PLACE_INFO:
				print("PlaceContent : " + string.Format(keyTitle[(int)KeyType.PLACE_INFO]
						, PlayDataManager.instance.m_StageName
						, placeName));
				return string.Format(keyTitle[(int)KeyType.PLACE_INFO]
						, PlayDataManager.instance.m_StageName
						, placeName);

			case KeyType.PLACE_BUTTON:
				return string.Format(keyTitle[(int)KeyType.PLACE_TITLE]
						, PlayDataManager.instance.m_StageName
						, placeName);

			case KeyType.PLACE_NPC_INFO:
				return string.Format(keyTitle[(int)KeyType.PLACE_INFO]
						, PlayDataManager.instance.m_StageName
						, placeName);

			default:
				return string.Empty;
		}
	}

	public void Init(PlaceItem data)
	{
		m_Checker.transform.SetParent(transform);
		m_NowItem = data;

		m_PlaceName.text = Localization.Get(GetKey(KeyType.PLACE_TITLE));
		m_PlaceContent.text = Localization.Get(GetKey(KeyType.PLACE_INFO));
		m_NpcContent.text = string.Empty;

		m_NpcContentRoot.SetActive(false);
		m_PlaceContentRoot.SetActive(true);

		for (int i = 0; i < m_PlaceItemRoot.childCount; i++)
			DestroyImmediate(m_PlaceItemRoot.GetChild(0).gameObject);

		int childCount = m_NPCItemRoot.childCount;
		for (int i = 0; i < childCount; i++)
		{
			DestroyImmediate(m_NPCItemRoot.GetChild(0).gameObject);
		}

		int index = 0;
		string _npcname = string.Empty;
		BuildTab(index++, 0, m_PlaceName.text, m_PlaceItemRoot).Select();
		foreach (var character in m_NowItem.m_CharacterList)
		{
			BuildTab(index++, 1, character, m_NPCItemRoot);
		}

		string place = m_NowItem.m_Type.ToString() + "_" + m_NowItem.ReturnIndex();
		int atlasIndex = PlaceDataManager.instance.GetAtlasIndex(place);
		m_Illust.atlas = BackgroundManager.instance.GetBackgroundAtlas(atlasIndex);
		m_Illust.spriteName = string.Format("BG_{0}", PlaceDataManager.instance.GetSpriteIndex(place));

		m_MapRoot.SetActive(false);
		m_NewsRoot.SetActive(false);
		m_NPCItemRoot.GetComponent<UIGrid>().Reposition();
	}

	private void MoveChecker(PlaceTabItem target)
	{
		if (target == null)
		{
			m_Checker.SetActive(false);
			return;
		}

		m_Checker.transform.SetParent(target.transform);
		m_Checker.transform.localPosition = Vector3.zero;
		m_Checker.SetActive(true);
	}

	public void Dialog()
	{
		Action("Dialog");
	}
	public void Suggest()
	{
		Action("Suggest");
	}
	public void Action(string command)
	{
		PlaceManager.instance.m_Mode = command;
		PlaceManager.instance.ClickFace(m_SelectedItem.content);
	}

	PlaceTabItem BuildTab(int index, int type, string data, Transform parent)
	{
		GameObject tab = Instantiate(tabItem, parent);
		tab.transform.localPosition = new Vector2(0, -1 * 100 * index);
		tab.SetActive(true);
		tab.GetComponent<PlaceTabItem>().Init(type, data);
		return tab.GetComponent<PlaceTabItem>();
	}

	public void ShowDesc(int type, string itemCode)
	{
		MoveChecker(m_SelectedItem);
		if (type == 1)
		{
			m_NpcContentRoot.SetActive(true);
			m_PlaceContentRoot.SetActive(false);

			m_Portrait.atlas = AtlasManager.instance.GetAtlas(AtlasManager.SpriteType.Character.ToString(),
															  itemCode);
			m_Portrait.spriteName = itemCode;

			m_Shadow.GetComponent<UISprite>().atlas = m_Portrait.atlas;
			m_Shadow.GetComponent<UISprite>().spriteName = m_Portrait.spriteName;
			m_Shadow.ResetToBeginning();
			m_Shadow.enabled = true;

			m_NpcName.text = Localization.Get("Name_" + PlayDataManager.instance.m_StageName + "_" + itemCode);
			m_NpcContent.text = string.Empty;
		}
		else
		{
			m_NpcContentRoot.SetActive(false);
			m_PlaceContentRoot.SetActive(true);

			//m_PlaceContent.text = content;
		}
	}
}