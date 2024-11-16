using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldUIManager : Singleton<WorldUIManager>
{
    [SerializeField]
    public InfoPanel m_CityInfoPanel;
    public bool isDebug = false;

    public GameObject m_NowOpenPanel;

    private void print(string s)
    {
        if (isDebug)
            MonoBehaviour.print(s);
    }

    public void Init()
    {
        m_CityInfoPanel.Init();
        m_CityInfoPanel.HideImmediatePanel();
    }

    public void OnClickedBackGround()
    {
        m_NowOpenPanel.SendMessage("HidePanel");
        print("Hide Panel : " + m_NowOpenPanel.ToString());
        m_NowOpenPanel = null;
    }

    public void ControlCityInfoUI(bool isOpen, bool isUnlock, StageItem item)
    {
        print("CityInfo UI Activate : " + isOpen.ToString());
        m_CityInfoPanel.ControlChildrenUI(isOpen, isUnlock, item);
        if (isOpen)
        {
            m_NowOpenPanel = m_CityInfoPanel.gameObject;
            WorldManager.instance.SetSelectedCity(item.m_Index.ToString());
        }
        else
        {
            m_NowOpenPanel = null;
        }
    }

    public void CallbackHidePanel(GameObject panel)
    {
        print("Hide Panel : " + panel.name.ToString());
        m_NowOpenPanel = null;
    }

    #region prev
    public GameObject m_WarningStageStartPopup;


    public void ControlWarningStageStartPopup()
    {
        if (m_WarningStageStartPopup.activeInHierarchy)
        {
            m_WarningStageStartPopup.SetActive(false);
        }
        else
        {
            m_WarningStageStartPopup.SetActive(true);
        }
    }
    #endregion
}
