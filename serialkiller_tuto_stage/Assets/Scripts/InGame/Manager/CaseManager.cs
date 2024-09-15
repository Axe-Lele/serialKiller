using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using SimpleJSON;

public class CaseManager : Singleton<CaseManager> {

    public TextAsset CaseTextAsset;
    private JSONNode CaseNode;
    private void Awake()
    {
        CaseNode = JSONNode.Parse(CaseTextAsset.text);
    }

    public string ReturnVictimName(string caseindex)
    {
        return CaseNode[StageDataManager.instance.m_CriminalCode.ToString()]["Case"][caseindex]["VictimName"];
    }

    public int ReturnCaseLocation(string caseindex)
    {
        return CaseNode[StageDataManager.instance.m_CriminalCode.ToString()]["Case"][caseindex]["CaseLocation"].AsInt;
    }

    public void SetItem(int i, string caseindex)
    {
        //print("setitem : " + i + " / caseindex : " + caseindex);
        if (i == 0)
        {
            EventDataManager.instance.InputCaseEventData(CaseNode[StageDataManager.instance.m_CriminalCode.ToString()]["Case"][caseindex]["Date"].AsInt, caseindex, CaseNode[StageDataManager.instance.m_CriminalCode.ToString()]["Case"][caseindex]["CaseLocation"].AsInt, false);
        }
        else
        {
            int date = CaseNode[StageDataManager.instance.m_CriminalCode.ToString()]["Case"][caseindex]["Date"].AsInt + EventDataManager.instance.ReturnCaseDate(i - 1);
            int newsdate = date + CaseNode[StageDataManager.instance.m_CriminalCode.ToString()]["Case"][caseindex]["NewsDate"].AsInt;
            EventDataManager.instance.InputCaseEventData(date, caseindex, CaseNode[StageDataManager.instance.m_CriminalCode.ToString()]["Case"][caseindex]["CaseLocation"].AsInt, false);
            EventDataManager.instance.InputNewsEventData(newsdate, caseindex);
        }

        /*    CaseDataItem item = new CaseDataItem();
            item.CaseIndex = caseindex;
            item.CaseLocation = CaseNode[suspectindex]["Case"][caseindex]["CaseLocation"].AsInt;
            item.Date = CaseNode[suspectindex]["Case"][caseindex]["Date"].AsInt;
            item.Time = CaseNode[suspectindex]["Case"][caseindex]["Time"].AsInt;
            item.NewsDate = CaseNode[suspectindex]["Case"][caseindex]["NewsDate"].AsInt;
            item.Event = new List<string>();
            for (int i = 0; i < CaseNode[suspectindex]["Case"][caseindex]["Event"].Count; i++)
            {
                item.Event.Add(CaseNode[suspectindex]["Case"][caseindex]["Event"][i]);
                /*string[] str = item.Event[i].Split('-');

                switch ((GameEventType)Enum.Parse(typeof(GameEventType), str[0]))
                {
                    case GameEventType.AddArea:
                        string[] AreaTemp = str[1].Split('_');
                        print("aaa : " + (PlaceType)Enum.Parse(typeof(PlaceType), AreaTemp[0]) + " / temp : " + AreaTemp[1]);
                        PlaceDataManager.instance.ControlPlace((PlaceType)Enum.Parse(typeof(PlaceType), AreaTemp[0]), int.Parse(AreaTemp[1]), true);
                        break;
                    case GameEventType.AddNews:
                        EventDataManager.instance.InputEventData(item.NewsDate, str[1], (int)GameEventType.AddNews, "");
                        break;
                    case GameEventType.AddSelection:
                        string[] SelectionTemp = str[1].Split('_');
                        SelectionDataManager.instance.InputSelection(SelectionTemp[0], SelectionTemp[1]);
                        break;
                }*/
        // }


        //StageDataManager.instance.m_CaseDataItemList.Add(item);
    }
    /*
    public void OpenCase(int count)
    {
        
       for (int i = 0; i < StageDataManager.instance.m_CaseDataItemList[count].Event.Count; i++)
        {
            string[] str = StageDataManager.instance.m_CaseDataItemList[count].Event[i].Split('-');

            switch ((GameEventType)Enum.Parse(typeof(GameEventType), str[0]))
            {
                case GameEventType.AddArea:
                    string[] AreaTemp = str[1].Split('_');
                    PlaceDataManager.instance.ControlPlace((PlaceType)Enum.Parse(typeof(PlaceType), AreaTemp[0]), int.Parse(AreaTemp[1]), true);
                    break;
                case GameEventType.AddNews:
                    //EventDataManager.instance.InputEventData(StageDataManager.instance.m_CaseDataItemList[count].NewsDate, str[1], (int)GameEventType.AddNews, "");
                    break;
                case GameEventType.AddSelection:
                    string[] SelectionTemp = str[1].Split('_');
                    SelectionDataManager.instance.InputSelection(SelectionTemp[0], SelectionTemp[1]);
                    break;
                case GameEventType.Dialog:
                    string[] DialogTemp = str[1].Split('_');
                    GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.Dialog, DialogTemp[0], DialogTemp[1]);
                    break;
                case GameEventType.SystemMessage:
                    SystemMessageManager.instance.ShowSystemMessage("System_Message_" + PlayDataManager.instance.m_StageName + "_" + str[1]);
                    break;
            }
        }
    }
    
    public void OpenCase(string caseindex)
    {
        int count = 0;

        /*for (int i = 0; i < StageDataManager.instance.m_CaseDataItemList.Count; i++)
        {
            if (StageDataManager.instance.m_CaseDataItemList[i].CaseIndex == caseindex)
            {
                count = i;
                break;
            }
        }

        for (int i = 0; i < StageDataManager.instance.m_CaseDataItemList[count].Event.Count; i++)
        {
            string[] str = StageDataManager.instance.m_CaseDataItemList[count].Event[i].Split('-');

            switch ((GameEventType)Enum.Parse(typeof(GameEventType), str[0]))
            {
                case GameEventType.AddArea:
                    string[] AreaTemp = str[1].Split('_');
                    PlaceDataManager.instance.ControlPlace((PlaceType)Enum.Parse(typeof(PlaceType), AreaTemp[0]), int.Parse(AreaTemp[1]), true);
                    break;
                case GameEventType.AddNews:
                    //EventDataManager.instance.InputEventData(StageDataManager.instance.m_CaseDataItemList[count].NewsDate, str[1], (int)GameEventType.AddNews, "");
                    break;
                case GameEventType.AddSelection:
                    string[] SelectionTemp = str[1].Split('_');
                    SelectionDataManager.instance.InputSelection(SelectionTemp[0], SelectionTemp[1]);
                    break;
            }
        }
    }
    */
    public string ReturnOrder(string suspectindex, string caseindex)
    {
        return CaseNode[suspectindex]["Order"][caseindex];
    }

}
