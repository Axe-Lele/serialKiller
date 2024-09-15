using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class SuggestManager : Singleton<SuggestManager> {

    public TextAsset SuggestTextAsset;
    private JSONNode SuggestNode;
    //public bool m_IsCheckSuggest;

    private string m_Mode;
    private string m_Target;
    private string m_Item;

    private void Awake()
    {
        SuggestNode = JSONNode.Parse(SuggestTextAsset.text);
        m_Target = m_Item = "";
    }

    /*public void SetTarget(string target)
    {
        m_Target = target;
    }
    public void SetItem(string item)
    {
        m_Item = item;
    }
    */
    public void SetTarget(string target)
    {
        m_Target = target;
    }

    public void Setting(string mode, string index)
    {
        if (GlobalValue.instance.m_SelectSuggestItemName == index && GlobalValue.instance.m_SelectSuggestModeName == mode)
        {
            Suggest();
        }
        else
        {
            NoteManager.instance.SelectReset();
            GlobalValue.instance.m_SelectSuggestItemName = m_Item = index;
            GlobalValue.instance.m_SelectSuggestModeName = m_Mode = mode;
            print("mode : " + mode + " / target : " + m_Target + " / item : " + m_Item);
        }
    }

    public void CaseSetting(CaseMode mode, string index)
    {
        if (GlobalValue.instance.m_SelectSuggestItemName == index && GlobalValue.instance.m_SelectSuggestModeName == "Case")
        {
            Suggest();
        }
        else
        {
            NoteManager.instance.SelectReset();
            GlobalValue.instance.m_SelectSuggestItemName = m_Item = index;
            GlobalValue.instance.m_SelectSuggestModeName = m_Mode = "Case";
            GlobalValue.instance.m_SelectSuggestCaseMode = mode;
            print("mode : " + mode + " / target : " + m_Target + " / item : " + m_Item);
        }
    }

    public void Suggest()
    {
        GlobalValue.instance.m_SelectSuggestItemName = "";
        GlobalValue.instance.m_SelectSuggestModeName = "";

        if (m_Item == "" || m_Target == "")
        {
            string s = "너흰 아직 준비가 안 되어 있다. 너는 " + m_Target + "에게 " + m_Item + "을 주는게 맞느냐";
            SystemTextManager.instance.InputText(s);
            print(s);
            return;
        }
        string item = "";

        switch (m_Mode)
        {
            case "Case":
                item = "Case_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + m_Item;
                break;
            case "Dialog":
                item = "Dialog_" + PlayDataManager.instance.m_StageName + "_" + m_Item;
                break;
            case "Evidence":
                item = "Evidence_" + PlayDataManager.instance.m_StageName + "_" + m_Item;
                break;
            case "News":
                item = "Evidence_" + PlayDataManager.instance.m_StageName + "_" + m_Item;
                break;
        }

        int count = SuggestNode[m_Target].Count;
        bool b = false;
        print("index : " + item + " / count : " + count);
        for (int i = 0; i < count; i++)
        {
            print("index : " + item + " / m_index : " + SuggestNode[m_Target][i]["m_Index"]);
            if (SuggestNode[m_Target][i]["m_Index"].ToString().Replace("\"","")  == item)
            {
                b = true;
                string m_DialogIndex = SuggestNode[m_Target][i]["m_DialogIndex"];
                // 강제 선택지
                if (SuggestNode[m_Target][i]["m_Type"] != null)
                {
                    if (SuggestNode[m_Target][i]["m_Type"].ToString().Replace("\"", "") == "CompulsionSelection")
                    {
                        string p = "";
                        for (int k = 0; k < SuggestNode[m_Target][i]["m_Selection"].Count; k++)
                        {
                            if (k == SuggestNode[m_Target][i]["m_Selection"].Count - 1)
                            {
                                p += (SuggestNode[m_Target][i]["m_Selection"][k]);
                            }
                            else
                            {
                                p += (SuggestNode[m_Target][i]["m_Selection"][k] + "+");
                            }
                        }

                        string t = "";
                        for (int k = 0; k < SuggestNode[m_Target][i]["m_Event"].Count; k++)
                        {
                            if (k == SuggestNode[m_Target][i]["m_Event"].Count - 1)
                            {
                                t += (SuggestNode[m_Target][i]["m_Event"][k]);
                            }
                            else
                            {
                                t += (SuggestNode[m_Target][i]["m_Event"][k] + "+");
                            }
                        }
                        CompulsionSelectionManager.instance.Setting(p, t);
                        GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.CompulsionSelection, m_Target, m_DialogIndex, PlaceManager.instance.ReturnPlace());

                    }


                }
                // 일반 선택지
                else
                {
                    GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.Suggest, m_Target, m_DialogIndex, PlaceManager.instance.ReturnPlace());
                }

                InGameUIManager.instance.ControlNotePopup();
                break;
            }
        }

        if (b == false)
        {
            GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.Suggest, m_Target, "Nothing", PlaceManager.instance.ReturnPlace());
        }
        
    }
}
