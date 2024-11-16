using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratoryManager : Singleton<LaboratoryManager>
{
    public enum LaboratoryMenu
    {
        Analyzed,
        Matched
    }

    // 전체
    public GameObject[] m_BG;
    public GameObject[] m_Tabs;
    public UISprite[] m_TabButtons;
    public string[] m_LaboratoryTabButtonOnSprites;
    public string[] m_LaboratoryTabButtonOffSprites;

    public GameObject m_TitleLabel;

    public Camera UICamera;

    private LaboratoryMenu m_TapIndex;

    //public UILabel m_ItemDescriptionTitleLabel;
    public UILabel m_ItemDescriptionContentLabel;

    private Vector3 m_PrevPosition;

    private string m_Mode;

    public Transform m_TapChecker;

    // 분석
    // 분석 창
    [Header("Analyzed")]
    public AnalyzedManager m_AnalyzedManager;

    public LaboratoryItem m_AnalyzedItem; // 현재 분석 중인 아이템(데이터)
    public LaboratoryItem m_NowSelectedAnalyzedItem;  // 현재 선택된 아이템

    public UILabel m_AnalyzedItemNameLabel; // 분석 중인 아이템의 이름.
    public UISprite m_AnalyzedItemIconSprite; // 분석 중인 아이템의 아이콘 이미지.
    public UILabel m_AnalyzedItemDescLabel; // 분석 중인 아이템의 설명.

    public UILabel m_AnalyzedStateLabel; // 현재 분석 창의 상태
    public UILabel m_AnalyzedRemainTimeLabel; // 남은 시간 
    public LaboratoryItem[] m_AnalyzedItemReservationList; // 분석 아이템 예약 리스트

    // 분석 할 수 있는 아이템 리스트
    public List<LaboratoryItem> m_AnalyzedItemList;
    public LaboratoryItem m_OriginalAnalyzedItem;
    public UIGrid m_AnalyzedTable;
    public UIScrollView m_AnalyzedScrollView;
    public UILabel m_AnalyzedReulstLabel;


    // 매치
    // 메치 창
    [Header("Matched")]
    public MatchedManager m_MatchedManager;
    public LaboratoryItem[] m_MatchedList;
    public UIButton m_MatchedBtn;
    public UILabel m_RemainMatchedTimeLabel;
    public UILabel m_MatchedResultLabel;
    // 매치 할 수 있는 아이템 리스트

    public List<LaboratoryItem> m_MatchedItemList;
    public LaboratoryItem m_OriginalMatchedItem;
    public UIGrid m_MatchedTable;
    public UIScrollView m_MatchedScrollView;

    public GameObject m_MapParent;
    public GameObject m_NewsPanel;

    public void SelectedItem(string itemCode)
    {
        m_AnalyzedManager.SelectedItem(itemCode);
    }

    public void SelectedItem(LaboratoryItem selectedItem)
    {
        switch (selectedItem.m_ModeType)
        {
            case LaboratoryType.Analyzing:
                break;
            case LaboratoryType.AnalyzedReverve:
                break;
            case LaboratoryType.CanAnalyzed:
                m_AnalyzedManager.SelectedItem(selectedItem);
                break;
            case LaboratoryType.Matching:
                break;
            case LaboratoryType.CanMatched:
                m_MatchedManager.SelectedItem(selectedItem);
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        m_PrevPosition = Vector2.zero;
        m_AnalyzedItemList = new List<LaboratoryItem>();
        m_TapIndex = 0;
    }

    private void Start()
    {
        for (int i = 0; i < m_AnalyzedItemReservationList.Length; i++)
        {
            m_AnalyzedItemReservationList[i].gameObject.SetActive(false);
        }
    }

    public void ControlLaboratoryUI(bool isOpen)
    {
        m_MapParent.SetActive(!isOpen);
        m_NewsPanel.SetActive(!isOpen);

        if (isOpen)
            ShowAnalyzedUI();
    }

    public void ShowAnalyzedUI()
    {
        m_TapIndex = LaboratoryMenu.Analyzed;
        m_Mode = "Analyzed";

        m_AnalyzedManager.SetActivePanel(true);
        m_MatchedManager.SetActivePanel(false);

        m_BG[(int)LaboratoryMenu.Matched].SetActive(false);
        m_Tabs[(int)LaboratoryMenu.Matched].SetActive(false);

        m_TapChecker.SetParent(m_TabButtons[(int)LaboratoryMenu.Analyzed].transform);
        m_TapChecker.transform.localPosition = Vector3.zero;
        m_TapChecker.gameObject.SetActive(true);
    }

    public void SelectMatchedPopup()
    {
        m_TapIndex = LaboratoryMenu.Matched;
        m_Mode = "Matched";

        m_AnalyzedManager.SetActivePanel(false);
        m_MatchedManager.SetActivePanel(true);

        m_BG[(int)LaboratoryMenu.Matched].SetActive(true);
        m_Tabs[(int)LaboratoryMenu.Matched].SetActive(true);

        m_TapChecker.SetParent(m_TabButtons[(int)LaboratoryMenu.Matched].transform);
        m_TapChecker.transform.localPosition = Vector3.zero;
        m_TapChecker.gameObject.SetActive(true);

        m_MatchedScrollView.ResetPosition();
    }

    public void CheckMatched(int time)
    {
        if (LaboratoryDataManager.instance.m_IsStartMatched)
        {
            LaboratoryDataManager.instance.m_MatchedRemainTime -= time;

			int h = 0;
			int m = 0;

			if (LaboratoryDataManager.instance.m_MatchedRemainTime > 0)
            {
                h = LaboratoryDataManager.instance.m_MatchedRemainTime / 60;
                m = LaboratoryDataManager.instance.m_MatchedRemainTime % 60;
                //string str = "";
                if (m == 0)
                {
                    //str = string.Format(Localization.Get("Analzed_RemainHour"), h);
                }
                else
                {
                    //str = string.Format(Localization.Get("Analzed_RemainHourMinute"), h, m);
                }

                m_RemainMatchedTimeLabel.text = string.Format("{0:00}:{1:00}", h.ToString(), m.ToString());
            }
            else
            {
                EvidenceDataManager.instance.CompleteMatched(LaboratoryDataManager.instance.ReturnMatchedIndexList(0), LaboratoryDataManager.instance.ReturnMatchedIndexList(1));
                //LaboratoryDataManager.instance.InputMatchResult(LaboratoryDataManager.instance.ReturnMatchedIndexList(0), LaboratoryDataManager.instance.ReturnMatchedIndexList(1), true);

                string str = string.Format(Localization.Get("System_Text_CompleteMatched")
									, LaboratoryItem.GetItemName(m_MatchedList[0])
									, LaboratoryItem.GetItemName(m_MatchedList[1]));
                SystemTextManager.instance.InputText(str);
                LaboratoryDataManager.instance.m_IsStartMatched = false;
                m_MatchedList[0].Reset();
                m_MatchedList[1].Reset();

				m_RemainMatchedTimeLabel.text = string.Format("{0:00}:{1:00}", h.ToString(), m.ToString());
			}
        }
        else
        {
            m_RemainMatchedTimeLabel.text = string.Format("-");

            m_MatchedList[0].Reset();
            m_MatchedList[1].Reset();
        }
    }

    public void CheckAnalyzed(int time)
    {
        m_AnalyzedManager.CheckAnalyzed(time);
    }

    /// <summary>
    /// '대기열 0 ~ 5번(전부) 분석' 데이터 및 UI 초기화
    /// </summary>
    public void ResetAnalyzedReservationItemList()
    {
        for (int i = 0; i < m_AnalyzedItemReservationList.Length; i++)
        {
            m_AnalyzedItemReservationList[i].Reset();
        }
    }

    /// <summary>
    /// '대기열 0번 분석' UI 초기화
    /// </summary>
    public void NowAnalzedItemUIReset()
    {
        m_AnalyzedRemainTimeLabel.text = string.Empty;

        m_AnalyzedItemNameLabel.text = string.Empty;
        m_AnalyzedItemIconSprite.spriteName = string.Empty;
        m_AnalyzedItemDescLabel.text = string.Empty;
    }

    public void AddAnalyzedItemList(string itemCode)
    {
        m_AnalyzedManager.AddItem(itemCode);
    }

    public void RemoveAnalyzedItemList(string itemCode)
    {
        m_AnalyzedManager.RemoveItem(itemCode);
    }

    public void SetAnalyzedItem(string itemCode)
    {
        m_AnalyzedManager.ReadyAnalyzed(itemCode);
    }

    public void StartAnalyzed(string itemCode)
    {
        m_AnalyzedManager.StartAnalyzed(itemCode);
    }

    public void AddAnalyzedReservationItemList(string itemCode)
    {
        m_AnalyzedManager.AddReserveItem(itemCode);
    }

    public void RemoveAnalyzedReservationItemList(int k)
    {
        m_AnalyzedManager.RemoveReserveItem(k);
    }

    #region Matched
    #endregion
    public void StartMatch()
    {
        bool b = false;
        for (int i = 0; i < m_MatchedList.Length; i++)
        {
            if (LaboratoryDataManager.instance.ReturnMatchedIndexList(i) == "")
            {
                b = true;
                break;
            }
        }

        if (b == false)
        {
            if (LaboratoryDataManager.instance.ReturnMatchedIndexList(1).Contains("A") == false)
            {
                if (LaboratoryDataManager.instance.ReturnMatchedIndexList(0).Contains("A") == false)
                {
                    b = true;
                    print("0 : " + LaboratoryDataManager.instance.ReturnMatchedIndexList(0) + " / 1 : " + LaboratoryDataManager.instance.ReturnMatchedIndexList(1));
                    SystemTextManager.instance.InputText(Localization.Get("System_Text_Matched_Impossible_Same_Type"));
                }
            }
        }

        if (b == false)
		{
			LaboratoryDataManager.instance.m_MatchedRemainTime = 120;

            int h = LaboratoryDataManager.instance.m_MatchedRemainTime / 60;
            int m = LaboratoryDataManager.instance.m_MatchedRemainTime % 60;
            string str = "";
            if (m == 0)
            {
                str = string.Format(Localization.Get("Analzed_RemainHour"), h);
            }
            else
            {
                str = string.Format(Localization.Get("Analzed_RemainHourMinute"), h, m);
            }
            m_RemainMatchedTimeLabel.text = str;
            LaboratoryDataManager.instance.m_IsStartMatched = true;
        }

        m_MatchedManager.StartMatching();
    }

    public void InputMatchedItem(string str)
    {
        //bool b = false;
        for (int i = 0; i < m_MatchedList.Length; i++)
        {
            if (LaboratoryDataManager.instance.ReturnMatchedIndexList(i) == str)
            {
                print("aleady : " + str);
                //b = true;
                break;
            }
            if (LaboratoryDataManager.instance.ReturnMatchedIndexList(i) == "")
            {
                print("empty : " + str);
                LaboratoryDataManager.instance.SetMatchedIndexList(i, str);
                m_MatchedList[i].Setting(str);

                if (i == m_MatchedList.Length - 1)
                {
                    m_MatchedBtn.gameObject.SetActive(true);
                }
                break;
            }
        }
    }
    public void ClearDescItem(int i)
    {
        m_MatchedManager.ClearDescItem(i);
    }

    public void AddMatchedItemList(string itemCode)
    {
        m_MatchedManager.AddItem(itemCode);

        return;

        LaboratoryItem item = Instantiate(m_OriginalMatchedItem);
        item.Setting(itemCode);
        item.transform.parent = m_MatchedTable.transform;
        item.transform.localScale = Vector3.one;

        m_MatchedItemList.Add(item);
        item.gameObject.SetActive(true);
        m_MatchedTable.enabled = true;
        m_MatchedScrollView.ResetPosition();

        item = null;
    }

    public void InputMatchResult(string a, string b, bool result)
    {
        m_MatchedManager.InputMatchResult(a, b, result);
        return;
    }

    public void OnSelected_Func(string str)
    {
        if (str == "-1" || str == "")
            return;

        if (m_PrevPosition != UICamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)))
        {
            m_PrevPosition = UICamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

            //m_ItemDescriptionTitleLabel.text = Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + str + "_Title");
            m_ItemDescriptionContentLabel.text = Localization.Get("Evidence_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + str + "_Content");
        }
    }

    public int ContainAnalyzedList(string itemCode)
    {
        for (int i = 0; i < m_AnalyzedManager.m_ReserveItemList.Length; i++)
        {
            if (m_AnalyzedManager.m_ReserveItemList[i].GetItemCode() == itemCode)
                return i;
                
        }
        return -1;
    }

    public bool ContainMatchedList(string itemCode)
    {
        for (int i = 0; i < m_MatchedManager.m_DescItem.Length; i++)
        {
            if (m_MatchedManager.m_DescItem[i].GetItemCode() == itemCode)
                return true;

        }
        return false;

    }
}