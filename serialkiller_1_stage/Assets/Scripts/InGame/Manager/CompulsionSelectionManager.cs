using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class CompulsionSelectionManager : Singleton<CompulsionSelectionManager> {

    private string m_Target;
    private string m_Index;
    public GameObject CompulsionSelectionPopup;


    public CompulsionSelectionItem[] Selections;
    public UISprite[] timerImage;
    private string m_TempText;
    private string m_LocalizationText;

    private string[] m_SelectionList;
    private string[] m_EventList;
    private bool countEnable = false;
    private float countTime = 0;
    private float m_LimitTime = 10f;
   
    public void Show(string target, string index)
    {
        m_Target = target;
        m_Index = index;

        CompulsionSelectionPopup.SetActive(true);

        SelectionHide();

        SettingSelection();

        SettingTime();

    }

    public void Setting(string s1, string s2)
    {
        m_SelectionList = s1.Split('+');
        m_EventList = s2.Split('+');
    }

    private void SettingSelection()
    {
        int count = m_SelectionList.Length;
        for (int i = 0; i < count; i++)
        {
            m_TempText = "CompulsionSelection_" + PlayDataManager.instance.m_StageName + "_" + m_Target + "_" + m_Index + "_" + m_SelectionList[i];
            m_LocalizationText = Localization.Get(m_TempText);
            Selections[i].Setting(m_Target, m_SelectionList[i], m_LocalizationText);
            Selections[i].gameObject.SetActive(true);
        }
    }

    public void SelectionHide()
    {
        for (int i = 0; i < Selections.Length; i++)
        {
            Selections[i].gameObject.SetActive(false);
        }
    }

    public void Select(string index)
    {
        GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.Dialog, m_Target, m_Index + index);
        CompulsionSelectionPopup.SetActive(false);
        countEnable = false;
        countTime = 0f;
    }

    public void TimeOver()
    {
        Select((m_SelectionList.Length).ToString());
    }

    public void Update()
    {
        if (countEnable)
        {
            countTime += Time.deltaTime;
            if (m_LimitTime < countTime)
            {
                TimeOver();
            }
            float displayCount = m_LimitTime - countTime;
            string countString = displayCount.ToString("00.00");
            char[] countSet = countString.ToCharArray();
            timerImage[0].spriteName = "countdown_clock_" + countSet[0];
            timerImage[1].spriteName = "countdown_clock_" + countSet[1];
            timerImage[2].spriteName = "countdown_clock_" + countSet[3];
            timerImage[3].spriteName = "countdown_clock_" + countSet[4];
        }
    }

    public void CloseSelectionPopup()
    {
        CompulsionSelectionPopup.SetActive(false);
    }

    public void SettingTime()
    {
        countEnable = true;
        countTime = 0f;
    }


}
