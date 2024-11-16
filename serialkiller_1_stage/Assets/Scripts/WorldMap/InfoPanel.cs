using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
	public UILabel[] m_ButtonLabels;
	public UILabel m_InfoTitle;
	public UILabel m_InfoLabel;
	public UILabel m_InfoLock;

	public GameObject[] m_LockObjs;
	public GameObject[] m_UnlockObjs;

	public StageItem m_SeletedItem;

	private string key;

	public void Init()
	{
		m_ButtonLabels[0].text = Localization.Get("Text_StartGame");
		m_ButtonLabels[1].text = Localization.Get("Text_DetectiveBoard");
		m_ButtonLabels[2].text = Localization.Get("Text_Back");
	}

	public void ShowCityDialog()
	{
		if (m_SeletedItem == null)
		{
			print("Doesnt have SelectedItem : " + m_SeletedItem);
			return;
		}

		WorldManager.instance.m_DialogManager.StartDialogInWorld(
				DialogType.Dialog, "N0", m_SeletedItem.m_Index.ToString("00"));
	}

	public void ShowBoard()
	{
		DetectiveManager.instance.ShowCaseBoard(m_SeletedItem.m_Index);
	}

	public void ShowCityBoard()
	{
		DetectiveManager.instance.ShowCityBoard();
	}

	public void ControlChildrenUI(bool isShow, bool isUnlock, StageItem item)
	{
		m_SeletedItem = item;
		int idx = item.m_Index;

		if (!isShow)
		{
			HidePanel();
			return;
		}

		item.m_Collider.enabled = !isShow;
		m_InfoTitle.text = string.Empty;
		m_InfoLabel.text = string.Empty;
		m_InfoLock.text = string.Empty;

		key = "Text_World_{0}";
		print(key);
		m_InfoTitle.text = Localization.Get(string.Format(key + m_SeletedItem.m_Index.ToString("00"), "Title_"));
		m_InfoLabel.text = Localization.Get(string.Format(key + m_SeletedItem.m_Index.ToString("00"), "Info_"));

		if (!isUnlock)
		{
			for (int i = 0; i < m_UnlockObjs.Length; i++)
				m_UnlockObjs[i].SetActive(false);
			for (int i = 0; i < m_LockObjs.Length; i++)
				m_LockObjs[i].SetActive(true);

			m_InfoLock.text
				= StageInfoManager.instance.GetUnlockIndex(m_SeletedItem.m_Index);
		}
		else
		{
			for (int i = 0; i < m_LockObjs.Length; i++)
				m_LockObjs[i].SetActive(false);
			for (int i = 0; i < m_UnlockObjs.Length; i++)
				m_UnlockObjs[i].SetActive(true);

		}
		StageInfoManager.instance.m_CityChecks[idx].SetActive(true);

		ShowPanel();
	}

	#region Control Panel
	private void ShowPanel()
	{
		gameObject.SetActive(true);
	}

	public void HidePanel()
	{
		if (m_SeletedItem == null)
			return;

		int idx = m_SeletedItem.m_Index;
		bool isUnlock = m_SeletedItem.m_IsUnlock;
		StageInfoManager.instance.m_CityChecks[idx].SetActive(false);
		if (isUnlock)
		{
			StageInfoManager.instance.m_CitySprites[idx].spriteName = string.Format("stage{0}_unlock", idx + 1);
		}
		else
		{
			StageInfoManager.instance.m_CitySprites[idx].spriteName = string.Format("stage{0}_lock", idx + 1);
		}

		m_SeletedItem.Unselected();
		m_SeletedItem.m_Collider.enabled = true;
		gameObject.SetActive(false);
		CameraController.instance.ControlCameraZoomRatio(null, false);
		WorldUIManager.instance.CallbackHidePanel(gameObject);

	}

	public void HideImmediatePanel()
	{
		gameObject.SetActive(false);
	}
	#endregion
}
