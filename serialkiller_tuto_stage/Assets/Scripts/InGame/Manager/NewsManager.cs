using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class NewsManager : Singleton<NewsManager>
{
    // 기본 UI의 뉴스 패널에서 다루는 변수와 컴포넌트
    private int m_ProgressIndex;
    private int m_TargetIndex;
    private bool m_IsProgressView;
    public UILabel m_ViewDateLabel;
    public UILabel m_ViewTitleLabel;

    // 뉴스 팝업에서 다루는 변수와 컴포넌트
    public List<NewsItem> m_NewsItemList;
    public List<GameObject> m_DateItemList;
    //public List<int> m_Date;
    public GameObject m_NewsDateLabel;
    public NewsItem m_OriginalNewsItem;
    public UIGrid m_NewsTable;
    public UIScrollView m_NewsScrollView;

    // 뉴스 상세
    public UISprite m_NewsIllust;
    public UILabel m_NewsTitleLabel;
    public UILabel m_NewsContentLabel;

    public GameObject m_NewsPanel;
    public TweenPosition m_TP;


    public UILabel m_NoticeLabel;
    public Vector3 m_ShowNewsPanelPosition;
    public Vector3 m_HideNewsPanelPosition;

    public BoxCollider m_Collider;

    // 기타
    public TextAsset m_NewsTextAsset;
    private JSONNode NewsNode;

    private void Awake()
    {
        NewsNode = JSONNode.Parse(m_NewsTextAsset.text);
        m_NewsItemList = new List<NewsItem>();
        m_ProgressIndex = 0;
        m_IsProgressView = false;
    }

    public int ReturnNewsDate(string str)
    {
        return NewsNode[str]["m_Date"].AsInt;
    }

    public void AddUnreadNewsList(int date, string index)
    {
        bool b = false;

        m_NoticeLabel.gameObject.SetActive(false);
        

        for (int i = 0; i < m_NewsItemList.Count; i++)
        {
            if (m_NewsItemList[i].m_NewsIndex == index)
                return;

            if (m_NewsItemList[i].m_Date == date)
            {
                b = true;
            }
        }
        if (b == false)
        {
            GameObject go1 = Instantiate(m_NewsDateLabel) as GameObject;
            go1.gameObject.SetActive(true);
            go1.transform.parent = m_NewsTable.transform;
            go1.transform.localScale = Vector2.one;
            go1.transform.localPosition = Vector2.zero;

            go1.GetComponent<UILabel>().text = GameManager.instance.ReturnNowDate(date);
            m_DateItemList.Add(go1);

          //  m_NewsTable.enabled = true;
          //  m_NewsScrollView.ResetPosition();
        }

        NewsItem go = Instantiate(m_OriginalNewsItem) as NewsItem;
        go.gameObject.SetActive(true);
        go.transform.parent = m_NewsTable.transform;
        go.transform.localScale = Vector2.one;
        go.transform.localPosition = Vector2.zero;
        go.Setting(date, index, m_NewsItemList.Count,0);
        //print("news add : " + index);
        //Debug.LogWarning("Add Unread News List : " + index);
        m_NewsItemList.Add(go);
        
        m_NewsTable.enabled = true;
        m_NewsScrollView.ResetPosition();
        if (m_IsProgressView == false)
        {
            m_IsProgressView = true;
            StartCoroutine(View());
        }
    }



    private IEnumerator View()
    {
        //m_ViewDateLabel.text = GameManager.instance.ReturnNowDate(m_NewsItemList[m_ProgressIndex].m_Date);
        m_ViewTitleLabel.text = Localization.Get("News_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + m_NewsItemList[m_ProgressIndex].m_NewsIndex + "_Title");
        yield return new WaitForSeconds(5f);
        if (m_NewsItemList.Count == 1)
        {
            StartCoroutine(View());
        }
        else if (m_NewsItemList.Count == 0)
        {
            m_ProgressIndex = 0;
            m_IsProgressView = false;
        }
        else
        {
            m_ProgressIndex++;
            if (m_ProgressIndex >= m_NewsItemList.Count)
            {
                m_ProgressIndex = 0;
            }
            StartCoroutine(View());
        }
    }

    public void ShowNews()
    {
        m_TargetIndex = m_ProgressIndex;
        ShowNews(m_TargetIndex);
    }

    public void ShowNews(int index)
    {
        for (int i = 0; i < m_NewsItemList.Count; i++)
        {
            m_NewsItemList[i].UnSelect();
        }

        //m_NewsIllust.spriteName = "News_Illust_" + m_NewsItemList[index].m_NewsIndex;
        m_NewsTitleLabel.text = Localization.Get("News_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + m_NewsItemList[index].m_NewsIndex + "_Title");
        m_NewsContentLabel.text = Localization.Get( "News_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + m_NewsItemList[index].m_NewsIndex + "_Content");
        SystemTextManager.instance.InputText(Localization.Get("System_Text_MoveNewsToReadNewsList"));

        if (m_NewsIllust.gameObject.activeInHierarchy == false)
        {
            m_NewsIllust.gameObject.SetActive(true);
        }

        //NewsDataManaer에 있는 데이터 중 Unread 데이터를 Read 데이터 옮긴다. + NewsDataList에 있는 '안 읽은 뉴스 리스트'에서 해당 뉴스를 NewsManager에 있는 '읽은 뉴스 리스트'로 옮긴다.
        NewsDataManager.instance. MoveNewsItem(m_NewsItemList[index].m_Date, m_NewsItemList[index].m_NewsIndex);

        //임시적으로 지울 날짜를 알아둔다.
        int m_TempDate = m_NewsItemList[index].m_Date;
        Destroy(m_NewsItemList[index].gameObject);

        //리스트에 있는 뉴스 삭제
        m_NewsItemList.RemoveAt(index);

        //리스트에 있는 날짜 삭제
        bool b = false;
        for (int i = 0; i < m_NewsItemList.Count; i++)
        {
            if (m_NewsItemList[i].m_Date == m_TempDate)
            {
                b = true;
                print("같은 날짜가 있다");
                break;
            }

            if (m_NewsItemList[i].m_Date > m_TempDate)
            {
                print("날짜 지났다");
                break;
            }
        }

        if (b == false)
        {
            for (int i = 0; i < m_DateItemList.Count; i++)
            {
                if (m_DateItemList[i].GetComponent<UILabel>().text == GameManager.instance.ReturnNowDate(m_TempDate))
                {
                    Destroy(m_DateItemList[index].gameObject);
                    m_DateItemList.RemoveAt(i);
                    print("해당 날짜가 없다.");
                    break;
                }
            }
        }

            //m_DateItemList
        
        InGameUIManager.instance.ControlNewsPopup();
        print(m_NewsItemList.Count);
        if (m_NewsItemList.Count <= 0)
        {
            MovingNewsPanel(false);
            m_NoticeLabel.gameObject.SetActive(true);

        }
        
    }

    public void MovingNewsPanel(bool isShow)
    {
        if (isShow == true)
        {
            m_Collider.enabled = true;

            if (m_NewsPanel.transform.localPosition == m_ShowNewsPanelPosition)
                return;

            m_TP.from = m_HideNewsPanelPosition;
            m_TP.to = m_ShowNewsPanelPosition;
            m_TP.ResetToBeginning();
            m_TP.enabled = true;
        }
        else
        {
            m_Collider.enabled = false;

            if (m_NewsPanel.transform.localPosition == m_HideNewsPanelPosition)
                return;

            m_TP.from = m_ShowNewsPanelPosition;
            m_TP.to = m_HideNewsPanelPosition;
            m_TP.ResetToBeginning();
            m_TP.enabled = true;
        }

    }
}
