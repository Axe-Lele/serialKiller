using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageItem : MonoBehaviour
{
	public enum ItemType
	{
		City,
		Stage
	}
	public ItemType Type = ItemType.City;
	public BoxCollider m_Collider;

	public int m_Index = 0;
	public bool m_IsUnlock = false;

	public UISprite m_SpriteRenderer;
	public string m_UnlockSpt;
	public string m_LockSpt;


	public void Init()
	{
		gameObject.SetActive(true);

		if (!m_IsUnlock && Type == ItemType.City)
			m_SpriteRenderer.spriteName = m_LockSpt;
		else
			m_SpriteRenderer.spriteName = m_UnlockSpt;
	}

	public void OnClicked()
	{
		switch (Type)
		{
			case ItemType.City:
				m_SpriteRenderer.spriteName = "worldmap_select";

				WorldUIManager.instance.ControlCityInfoUI(true, m_IsUnlock, this);
				CameraController.instance.ControlCameraZoomRatio(transform, true);
				m_Collider.enabled = false;
				break;

			case ItemType.Stage: // Obsolate Type
													 // call event
				break;
		}
	}

	public void Unselected()
	{
		if (!m_IsUnlock && Type == ItemType.City)
			m_SpriteRenderer.spriteName = m_LockSpt;
		else
			m_SpriteRenderer.spriteName = m_UnlockSpt;
	}

	public void ShowSelection()
	{

	}

	//public void ShowCriminals()
	//{
	//    WorldManager.instance.ControlAllStageButton(false);
	//    for (int i = 0; i < m_Criminals.Length; i++)
	//    {
	//        m_Criminals[i].SetActive(true);
	//    }

	//    WorldUIManager.instance.m_CityInfoPanel.ControlChildrenUI(true, true);
	//}
}
