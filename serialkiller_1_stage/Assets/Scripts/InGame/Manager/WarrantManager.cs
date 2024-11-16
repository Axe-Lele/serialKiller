using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class WarrantManager : Singleton<WarrantManager>
{
    [Header("TextAsset")]
    public TextAsset m_WarrantTextAsset;

    [Header("Root")]
    public GameObject m_MapRoot;
    public GameObject m_NewsBannerRoot;
    public GameObject m_UiLeftRoot;
    public GameObject m_UiRightRoot;
    public GameObject m_UiSelectedMsgbox;

    [Header("Item")]
    public WarrantNpcItem m_OriginalNpcItem;
    public List<WarrantNpcItem> m_NpcItemList;
    private List<NpcItem> m_NpcItemListSort;

    public UILabel m_SearchLabel;
    public UIGrid m_NpcTable;
    public UIScrollView m_NpcScrollView;
    public UIScrollBar m_NpcScrollBar;

    public UISprite m_NpcFace;
    public UILabel[] m_InfoLabel;

    public WarrantEvidenceItem[] m_EvidenceItemList;
    public WarrantEvidenceItem m_CaseItem;

    private string m_Mode;
    private string m_ItemIndex;

    public string[] m_EvidenceList;
    [SerializeField] private int m_SelectEvidenceIndex;

    private string m_NpcCode;
    private string m_CaseIndex;

    public JSONNode m_WarrantNode;

    private string m_EndingIllust;
    private string m_EndingName;

    public Transform m_DisableParent;
    public GameObject m_Checker;


    private void Awake()
    {
        m_SelectEvidenceIndex = 0;
        for (int i = 0; i < m_EvidenceItemList.Length; i++)
        {
            m_EvidenceItemList[i].m_Index = i;
        }
    }

    public void SetActivePanel(bool isOpen)
    {
        WarrantManager.instance.UpdateNpcList();

        m_MapRoot.SetActive(!isOpen);
        m_NewsBannerRoot.SetActive(!isOpen);

        m_UiRightRoot.SetActive(!isOpen);
        m_UiSelectedMsgbox.SetActive(isOpen);

        m_Checker.SetActive(false);
    }

    public void ForcedOpenPanel()
    {
        WarrantManager.instance.UpdateNpcList();

        m_MapRoot.SetActive(false);
        m_NewsBannerRoot.SetActive(false);

        m_UiRightRoot.SetActive(true);
        m_UiSelectedMsgbox.SetActive(false);
    }

    public void ForcedClosePanel()
    {
        WarrantManager.instance.UpdateNpcList();

        m_MapRoot.SetActive(false);
        m_NewsBannerRoot.SetActive(false);

        m_UiRightRoot.SetActive(false);
        m_UiSelectedMsgbox.SetActive(false);
    }

    public void Warrant()
    {
        m_WarrantNode = JSONNode.Parse(m_WarrantTextAsset.text);
        m_EndingIllust = "2";
        m_EndingName = "Bad";

        if (m_CaseIndex == "")
        {
            GameManager.instance.m_SystemMessageManager.InputSystemMessage("System_Text_Not_Select_Case");
            return;
        }

        if (m_NpcCode == "")
        {
            GameManager.instance.m_SystemMessageManager.InputSystemMessage("System_Text_Not_Select_NPC");
            return;
        }

        int count = 0;
        for (int i = 0; i < m_EvidenceItemList.Length; i++)
        {
            if (m_EvidenceList[i] != null || m_EvidenceList[i] != "")
            {
                count++;
                break;
            }
        }

        if (count == 0)
        {
            GameManager.instance.m_SystemMessageManager.InputSystemMessage("System_Text_Not_Select_Evidence");
            return;
        }

        int _correctCount = 0;
        int c = 0;
        print("npc : " + m_NpcCode);
        string warrantIndex = string.Empty;

        for (int i = 0; i < m_WarrantNode[m_NpcCode].Count; i++)
        {
            int _caseindex = int.Parse(m_CaseIndex);

            // 엔피씨 코드가 다른 경우
            if (m_WarrantNode[m_NpcCode][i]["Case"].AsInt != _caseindex)
                continue;

            JSONNode nowNode = m_WarrantNode[m_NpcCode][i];
            c = nowNode["Evidence"].Count;

            _correctCount = 0;

            for (int k = 0; k < c; k++)
            {
                bool _isFind = false;
                for (int p = 0; p < m_EvidenceList.Length; p++)
                {
                    if (m_EvidenceList[p].Length == 0 || m_EvidenceList[p] == null)
                        continue;

                    string evidence = nowNode["Evidence"][k].ToString().Replace("\"", "");

                    print("Case[ : " + _caseindex + "] / Answer  : (" + p + ")" + evidence + " / Evidence : " + m_EvidenceList[p]);
                    if (evidence == m_EvidenceList[p])
                    {
                        _isFind = true;
                        _correctCount++;
                        break;
                    }
                }

                if (_isFind == false)
                    break;
            }

            print("Correct : " + _correctCount);
            // 모든 증거가 맞아 떨어질 때!
            if (_correctCount == c)
            {
                print("All Correct");
                warrantIndex = m_WarrantNode[m_NpcCode][i].Key;
                m_EndingName = m_WarrantNode[m_NpcCode][i]["Index"];
                break;
            }
        }

        //for (int i = 0; i < m_WarrantNode[m_NpcCode].Count; i++)
        //{
        //    c = m_WarrantNode[m_NpcCode][i]["Evidence"].Count;
        //    for (int k = 0; k < m_WarrantNode[m_NpcCode][i]["Evidence"].Count; k++)
        //    {
        //        for (int p = 0; p < m_EvidenceList.Length; p++)
        //        {
        //            if (m_WarrantNode[m_NpcCode][i]["Case"].AsInt == int.Parse(m_CaseIndex))
        //            {
        //                print("p : " + p + " / case  : " + m_WarrantNode[m_NpcCode][i]["Evidence"][k].ToString().Replace("\"", "") + " / parse : " + m_EvidenceList[p].ToString());
        //                if (m_WarrantNode[m_NpcCode][i]["Evidence"][k].ToString().Replace("\"", "") == m_EvidenceList[p].ToString())
        //                {
        //                    _correctCount++;
        //                }
        //            }
        //        }

        //        print(_correctCount);
        //        if (_correctCount == c)
        //        {
        //            print("Wow");
        //            warrantIndex = m_WarrantNode[m_NpcCode][i].Key;
        //            // 모든 증거가 맞아 떨어질 때!
        //            m_EndingName = m_WarrantNode[m_NpcCode][i]["Index"];
        //            break;
        //        }
        //    }
        //}

        EventManager.instance.SetEvent("Warrant", m_EndingName);

        return;
        //EndingManager.instance.SetEnding(m_EndingIllust, m_EndingName);
    }

    public void DataInitialize()
    {
        m_NpcItemListSort = new List<NpcItem>();
        List<string> m_Sort = new List<string>();
        m_NpcCode = "";
        m_CaseIndex = "";

        m_EvidenceList = new string[m_EvidenceItemList.Length];
        for (int i = 0; i < m_EvidenceItemList.Length; i++)
        {
            m_EvidenceList[i] = "";
        }
        //데이터 삽입
        for (int i = 0; i < NpcDataManager.instance.m_NpcItemList.Count; i++)
        {
            if (NpcDataManager.instance.m_NpcItemList[i].Name.Contains("Player") || NpcDataManager.instance.m_NpcItemList[i].Name.Contains("N"))
            {

            }
            else
            {
                m_Sort.Add(NpcDataManager.instance.m_NpcItemList[i].Name);
            }

        }
        // 정렬
        for (int i = 0; i < m_Sort.Count; i++)
        {
            m_Sort.Sort();
        }

        for (int i = 0; i < m_Sort.Count; i++)
        {
            for (int k = 0; i < m_Sort.Count; k++)
            {
                if (NpcDataManager.instance.m_NpcItemList[k].Name == m_Sort[i])
                {
                    m_NpcItemListSort.Add(NpcDataManager.instance.m_NpcItemList[k]);
                    break;
                }
            }
        }

        // 정렬 후
        for (int i = 0; i < m_Sort.Count; i++)
        {
            WarrantNpcItem item = Instantiate(m_OriginalNpcItem);
            for (int k = 0; i < NpcDataManager.instance.m_NpcItemList.Count; k++)
            {
                if (NpcDataManager.instance.m_NpcItemList[k].Name == m_Sort[i])
                {
                    item.Setting(NpcDataManager.instance.m_NpcItemList[k]);
                    break;
                }
            }

            item.transform.parent = m_NpcTable.transform;
            item.transform.localScale = Vector3.one;

            m_NpcItemList.Add(item);

            item.gameObject.SetActive(true);
            m_NpcTable.enabled = true;
        }
    }

    public void UpdateNpcList()
    {
        for (int i = 0; i < m_NpcItemList.Count; i++)
        {
            if (NpcDataManager.instance.IsMeet(m_NpcItemListSort[i].m_Index) == false)
            {
                //print(m_NpcItemList[i].ReturnNpcName() + " : Don't meet That Npc");
                m_NpcItemList[i].gameObject.SetActive(false);
                continue;
            }

            if (m_NpcItemList[i].GetNpcItem().NpcState == NpcState.Dead)
            {
                m_NpcItemList[i].gameObject.SetActive(false);
                continue;
            }

            m_NpcItemList[i].gameObject.SetActive(true);
        }

        m_NpcTable.Reposition();
        m_NpcScrollView.ResetPosition();
    }

    private void MoveChecker(WarrantNpcItem target)
    {
        if (target == null)
        {
            m_Checker.SetActive(false);
        }
        else
        {
            m_Checker.SetActive(true);
            m_Checker.transform.SetParent(target.transform);
            m_Checker.transform.localPosition = Vector3.zero;
        }
    }

    public void SelectNpc(WarrantNpcItem warrantItem)
    {
        NpcItem item = warrantItem.GetNpcItem();

        m_UiRightRoot.SetActive(true);
        m_UiSelectedMsgbox.SetActive(false);

        m_NpcCode = item.m_Index;

        m_NpcFace.atlas = AtlasManager.instance.GetAtlas(AtlasManager.SpriteType.Character.ToString(),
                                                        item.m_Index);
        m_NpcFace.spriteName = item.m_Index;

        MoveChecker(warrantItem);

        m_InfoLabel[0].text = item.Name;
    }

    public void ClickSelectEvidence(int i)
    {
        m_SelectEvidenceIndex = i;
    }

    /*public void SettingEvidence(string mode, string code)
	{
			m_Mode = mode;
			m_ItemIndex = code;
	}*/

    public void SettingMode(string mode, string code)
    {
        if (GlobalValue.instance.m_SelectWarrantItemName == code && GlobalValue.instance.m_SelectWarrantModeName == mode)
        {
            SubmitEvidence();
        }
        else
        {
            NoteManager.instance.SelectReset();

            GlobalValue.instance.m_SelectWarrantItemName = m_ItemIndex = code;
            GlobalValue.instance.m_SelectWarrantModeName = m_Mode = mode;
        }
    }

    public void CaseSetting(CaseMode mode, string code)
    {
        if (GlobalValue.instance.m_SelectWarrantItemName == code && GlobalValue.instance.m_SelectWarrantCaseMode == mode)
        {
            SubmitCase();
        }
        else
        {
            NoteManager.instance.SelectReset();

            GlobalValue.instance.m_SelectWarrantItemName = m_ItemIndex = code;
            GlobalValue.instance.m_SelectWarrantModeName = m_Mode = "Case";
            GlobalValue.instance.m_SelectWarrantCaseMode = mode;
        }
    }

    public void SettingEvidence(int i, string code)
    {
        m_EvidenceList[i] = code;
    }

    public void ResetEvidence(int i)
    {
        m_EvidenceList[i] = "";
    }

    public void SubmitCase(string itemType, string itemCode)
    {
        m_CaseItem.Setting(itemType, itemCode);

        GameManager.instance.m_NoteMode = NoteMode.None;
        InGameUIManager.instance.ControlNotePopup();
        InGameUIManager.instance.ControlWarrantPopup(UIPopupFlag.ForceOpen);
    }

    public void SubmitCase()
    {
        m_CaseItem.Setting("Case", m_ItemIndex);
        GameManager.instance.m_NoteMode = NoteMode.None;
        InGameUIManager.instance.ControlNotePopup();
        InGameUIManager.instance.ControlWarrantPopup(UIPopupFlag.ForceOpen);
    }

    public void SubmitEvidence(string itemType, string itemCode)
    {
        for (int i = 0; i < m_EvidenceItemList.Length; i++)
        {
            if (m_EvidenceItemList[i].ReturnMode() == itemType
                && m_EvidenceItemList[i].ReturnCode() == itemCode)
            {
                print(Localization.Get("System_Text_Already_Input_Evidence"));
                SystemTextManager.instance.InputText(Localization.Get("System_Text_Already_Input_Evidence"));
                return;
            }
        }

        string itemFullCode = string.Empty;
        switch (itemType)
        {
            case "Dialog":
                itemFullCode = "Selection_" + PlayDataManager.instance.m_StageName + "_" + itemCode;
                break;

            case "Evidence":
                itemFullCode = "Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + itemCode;
                break;

            case "News":
                itemFullCode = "News_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + itemCode;
                break;

            default:
                break;
        }

        m_EvidenceItemList[m_SelectEvidenceIndex].Setting(itemType, itemCode);
        GameManager.instance.m_NoteMode = NoteMode.None;

        InGameUIManager.instance.ControlNotePopup();
        InGameUIManager.instance.ControlWarrantPopup(UIPopupFlag.ForceOpen);
    }

    public void SubmitEvidence()
    {
        bool b = false;

        for (int i = 0; i < 10; i++)
        {
            if (m_EvidenceItemList[i].ReturnMode() == m_Mode && m_EvidenceItemList[i].ReturnIndex() == m_ItemIndex)
            {
                b = true;
                print("이미 있음.");
                SystemTextManager.instance.InputText(Localization.Get("System_Text_Already_Input_Evidence"));
                break;
            }
        }

        if (b == false)
        {
            string s = "";
            m_EvidenceItemList[m_SelectEvidenceIndex].Setting(m_Mode, m_ItemIndex);
            switch (m_Mode)
            {

                case "Dialog":
                    s = "Selection_" + PlayDataManager.instance.m_StageName + "_" + m_ItemIndex;
                    break;
                case "Evidence":
                    s = "Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + m_ItemIndex;
                    break;
                case "News":
                    s = "News_" + PlayDataManager.instance.m_StageName + "_" + m_ItemIndex;
                    break;
                default:
                    break;
            }

            m_EvidenceList[m_SelectEvidenceIndex] = s;
            GameManager.instance.m_NoteMode = NoteMode.None;
            InGameUIManager.instance.ControlNotePopup();
            InGameUIManager.instance.ControlWarrantPopup(UIPopupFlag.ForceOpen);
        }
    }


    public void Search()
    {
        string str = m_SearchLabel.text;
        int count = 0;
        for (int i = 0; i < m_NpcItemList.Count; i++)
        {
            m_NpcItemList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < m_NpcItemList.Count; i++)
        {
            if (m_NpcItemListSort[i].Name.Contains(str))
            {
                m_NpcItemList[count].Setting(m_NpcItemListSort[i]);
                m_NpcItemList[count].gameObject.SetActive(true);
                count++;
            }
        }
        m_SearchLabel.text = "";
        m_NpcScrollBar.value = 0f;
    }

    public void SearchInitialize()
    {
        for (int i = 0; i < m_NpcItemList.Count; i++)
        {
            m_NpcItemList[i].Setting(m_NpcItemListSort[i]);
            m_NpcItemList[i].gameObject.SetActive(true);
        }
        m_NpcScrollBar.value = 0f;
        m_SearchLabel.text = "";
    }

    public void SetCaseIndex(string s)
    {
        m_CaseIndex = s;
    }
}
