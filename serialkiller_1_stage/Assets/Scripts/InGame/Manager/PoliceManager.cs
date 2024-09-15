using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoliceManager : Singleton<PoliceManager> {

    public GameObject PatrolConfirmPopup;
    public UILabel label;

    //private int SelectedSuspect = -1;

    public WarrantNpcItem WarrantOrigin;
    public UIGrid WrrantGrid;

    public List<WarrantNpcItem> items;
    public UILabel WarrantTargetLabel;

    public GameObject WarrantConfirmPanel;

    public int TempNum;

    /*public void Setting()
    {
        items = new List<WarrantItem>();
        TempNum = 0;
        for (int i = 0; i < InGameGlobalValue.instance.SuspectCount; i++)
        {
            WarrantItem go = Instantiate(WarrantOrigin) as WarrantItem;

            go.transform.parent = WrrantGrid.transform;
            go.transform.localPosition = new Vector2(-480f + (i * 240f), 0f);
            go.transform.localScale = Vector2.one;
            go.num = i;
            go.Setting();
            items.Add(go);
            if (StageDataManager.instance.ReturnParameter(SuspectParameter.IsAppear, i) == "true")
            {
                go.gameObject.SetActive(true);
            }
        }
        WarrantUnselected();
        WrrantGrid.enabled = true;
    }

    //public void ShowSusepct

    void WarrantUnselected()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].Unselect();
        }
    }

    public void SelectSuspect(int i)
    {
        WarrantUnselected();

        SelectedSuspect = i;

        items[SelectedSuspect].Select();
    }

    public void ControlWarrantConfirmPanel()
    {
        if (WarrantConfirmPanel.activeSelf)
        {
            WarrantConfirmPanel.SetActive(false);
        }
        else
        {
            WarrantTargetLabel.text = string.Format(Localization.Get("WarrantConfirm"), StageDataManager.instance.ReturnParameter(SuspectParameter.Name, SelectedSuspect));
            WarrantConfirmPanel.SetActive(true);
        }
    }

    public void StartWarrant()
    {
        if (SelectedSuspect == -1)
        {
            SystemMessageManager.instance.ShowSystemMessage("UnselectedSuspect");
            return;
        }
        ControlWarrantConfirmPanel();
    }

    public void Warrant()
    {

        StartCoroutine(GameOver("Temp_" + TempNum));
        /*if (SelectedSuspect == InGameGlobalValue.instance.CriminalCode)
        {
            print("suscess");
            StartCoroutine(GameOver("Suscess"));
        }
        else
        {
            StartCoroutine(GameOver("Fail"));
        }*/
    //}

    /*IEnumerator GameOver(string str)
    {
        LoadingManager.instance.ShowLoading();

        yield return new WaitForSeconds(2f);

        EndingManager.instance.Show(str);

        LoadingManager.instance.HideLoading();
    }

    public void CheckPatrolResult()
    {
        if (InGameGlobalValue.instance.IsPatrol == false)
        {
            return;
        }
        print("EndPatrolDay : " + InGameGlobalValue.instance.EndPatrolDay + " / day : " + InGameGlobalValue.instance.Date);
        if (InGameGlobalValue.instance.EndPatrolDay == InGameGlobalValue.instance.Date)
        {
            InGameGlobalValue.instance.IsPatrol = false;
            BuildingManager.instance.areaBuildingManager[InGameGlobalValue.instance.PatrolArea].StopPatrol();
        }

        print("currentCasePointer : " + InGameGlobalValue.instance.CurrentCasePointer + " / caseDate  : " + StageDataManager.instance.ReturnCaseParameter(CaseParameter.CaseDate, InGameGlobalValue.instance.CurrentCasePointer + 1));
        if ((StageDataManager.instance.ReturnCaseParameter(CaseParameter.CaseDate, InGameGlobalValue.instance.CurrentCasePointer + 1)) == InGameGlobalValue.instance.Date.ToString())
        {
            DialogManager.instance.StartDialog(DialogTargetType.Police, "0", "-1");
        }
        else
        {
            DialogManager.instance.StartDialog(DialogTargetType.Police, "1", "-1");
        }

        /*if (StageDataManager.instance.ReturnCaseParameter(CaseParameter.Location, InGameGlobalValue.instance.CurrentCasePointer + i) == areaNum)
        {
            if (StageDataManager.instance.ReturnCaseParameter(CaseParameter.Date, InGameGlobalValue.instance.CurrentCasePointer + i) == InGameGlobalValue.instance.Day + 1)
            {
                InGameGlobalValue.instance.IsPatrolSuccess = true;
                InGameGlobalValue.instance.EndPatrolDay = InGameGlobalValue.instance.Day + 1;
            }
            else if (StageDataManager.instance.ReturnCaseParameter(CaseParameter.Date, InGameGlobalValue.instance.CurrentCasePointer + i) == InGameGlobalValue.instance.Day + 2)
            {
                InGameGlobalValue.instance.EndPatrolDay = InGameGlobalValue.instance.Day + 2;
                InGameGlobalValue.instance.IsPatrolSuccess = true;
            }
            else
            {
                InGameGlobalValue.instance.EndPatrolDay = InGameGlobalValue.instance.Day + 2;
                InGameGlobalValue.instance.IsPatrolSuccess = false;
            }
        }


        if (InGameGlobalValue.instance.IsPatrolSuccess)
        {
            DialogManager.instance.StartDialog(DialogTargetType.Police, "0", InGameGlobalValue.instance.CriminalCode.ToString());
        }
        else
        {
            DialogManager.instance.StartDialog(DialogTargetType.Police, "1", InGameGlobalValue.instance.CriminalCode.ToString());
        }*/
    //}

    /*public void ShowPatrolConfirmPopup(int i)
    {
        InGameGlobalValue.instance.PatrolArea = i;
        label.text = string.Format(Localization.Get("SystemText_PatrolReinforce"), Localization.Get(InGameGlobalValue.instance.StageName + "_Area_" + i));
        PatrolConfirmPopup.SetActive(true);
    }

    public void HidePatrolConfirmPopup()
    {
        PatrolConfirmPopup.SetActive(false);
    }

    public void PatrolReinforce()
    {
        if (InGameGlobalValue.instance.IsPatrol)
        {
            SystemMessageManager.instance.ShowSystemMessage("AlreadyPolicePatrol");
            return;
        }
        InGameGlobalValue.instance.IsPatrol = true;
        InGameGlobalValue.instance.IsPatrolSuccess = false;
        HidePatrolConfirmPopup();
        GameUIManager.instance.ControlPolicePatrolPanel();
        InGameGlobalValue.instance.EndPatrolDay = InGameGlobalValue.instance.Date + 3;
        BuildingManager.instance.areaBuildingManager[InGameGlobalValue.instance.PatrolArea].StartPatrol();

    }
    /*
    public void PatrolStart(int areaNum)
    {
        BuildingManager.instance.areaBuildingManager[areaNum].StartPatrol();

        InGameGlobalValue.instance.EndPatrolDay = InGameGlobalValue.instance.Day + 3;
        InGameGlobalValue.instance.IsPatrol = true;


        for (int i = 0; i < (InGameGlobalValue.instance.CaseCount); i++)
        {
            print("i : " + i + " / result day : " + (InGameGlobalValue.instance.Day + StageDataManager.instance.ReturnCaseParameter(CaseParameter.Date, InGameGlobalValue.instance.CurrentCasePointer + i)) + " / " + InGameGlobalValue.instance.Day);

            if (StageDataManager.instance.ReturnCaseParameter(CaseParameter.Location, InGameGlobalValue.instance.CurrentCasePointer + i) == areaNum)
            {
                if (StageDataManager.instance.ReturnCaseParameter(CaseParameter.Date, InGameGlobalValue.instance.CurrentCasePointer + i) == InGameGlobalValue.instance.Day + 1)
                {
                    InGameGlobalValue.instance.IsPatrolSuccess = true;
                    InGameGlobalValue.instance.EndPatrolDay = InGameGlobalValue.instance.Day + 1;
                }
                else if (StageDataManager.instance.ReturnCaseParameter(CaseParameter.Date, InGameGlobalValue.instance.CurrentCasePointer + i) == InGameGlobalValue.instance.Day + 2)
                {
                    InGameGlobalValue.instance.EndPatrolDay = InGameGlobalValue.instance.Day + 2;
                    InGameGlobalValue.instance.IsPatrolSuccess = true;
                }
                else
                {
                    InGameGlobalValue.instance.EndPatrolDay = InGameGlobalValue.instance.Day + 2;
                    InGameGlobalValue.instance.IsPatrolSuccess = false;
                }
            }
        }
        print("result : " + InGameGlobalValue.instance.IsPatrolSuccess + " / End : " + InGameGlobalValue.instance.EndPatrolDay);
       
    }*/

   
}
