using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager> {

    public GameObject SelectionPopup;
    public SelectionItem[] Selections;

    private string m_Target;

    private string m_TempText;
    private string m_LocalizationText;

    private int tempNum;
    private string localizationText;

    //public int CaseItemNumber;


    //private List<string> KeyWordTemp;
    // 선택지에서 {0}의 역할은 이름만 지정되어있음. type은 대화 대상이 용의자인지 경찰인지 그 외 NPC인지 구분함

    private void Awake()
    {
    }

    public void Show(string target)
    {
       
        //KeyWordTemp = new List<string>();

        m_Target = target;

        SelectionPopup.SetActive(true);

        SelectionHide();

        SettingSelection();

    }


    private void SettingSelection()
    {
        int index = SelectionDataManager.instance.ReturnCharacterIndex(m_Target);

        int count = SelectionDataManager.instance.m_SelectionDataItemList[index].m_Selection.Count;
        for (int i = 0; i < count; i++)
        {
            m_TempText = "Selection_" + PlayDataManager.instance.m_StageName + "_" + m_Target + "_" + SelectionDataManager.instance.m_SelectionDataItemList[index].m_Selection[i];
            m_LocalizationText = Localization.Get(m_TempText);
            Selections[i].SetText(m_LocalizationText);
            SelectionCheck(i, SelectionDataManager.instance.m_SelectionDataItemList[index].m_Selection[i]);
            Selections[i].Setting(m_Target, SelectionDataManager.instance.m_SelectionDataItemList[index].m_Selection[i]);
            Selections[i].gameObject.SetActive(true);
        }
    }

    private void SelectionCheck(int index, string str)
    {
        if(SelectionDataManager.instance.ReturnCheckRead(m_Target, str))
            Selections[index].ReadCheckPoint();
    }


    public void SelectionHide()
    {
        for (int i = 0; i < Selections.Length; i++)
        {
            Selections[i].gameObject.SetActive(false);
            Selections[i].UnreadCheckPoint();
        }
    }

    public void Select(string target, string index)
    {
        
        if (GlobalMethod.instance.ReturnSceneName().Contains("Stage"))
        {
            GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.Selection, target, index, "Selection");
            if (SelectionDataManager.instance.ReturnCheckRead(target, index) == false)
            {
                int m_TargetIndex = SelectionDataManager.instance.ReturnCharacterIndex(target);
                int m_ReadIndex = SelectionDataManager.instance.ReturnReadSelectionIndex(target, index);
                SelectionDataManager.instance.m_SelectionDataItemList[m_TargetIndex].m_IsReadSelection[m_ReadIndex] = true;
            }
        }
        else
        {
            //WorldManager - 헤어짐 대사 처리
            print("1");
            WorldManager.instance.SetSelectedCase(index);
            WorldManager.instance.m_DialogManager.StartDialogInWorld(DialogType.End, target, index);
        }
        SelectionPopup.SetActive(false);
    }

    public void CloseSelectionPopup()
    {
        SelectionPopup.SetActive(false);
    }
}
