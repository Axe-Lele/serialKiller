using UnityEngine;
using System.Collections;

public class NewsItem : MonoBehaviour
{

    public UILabel m_YearLabel, m_DateLabel;
    public int m_Date;
    public string m_NewsIndex;
    private int m_Index;

    public UISprite SeletedSprite;
    private string m_BGName;
    private int m_Mode = 0;

    private string m_TempSubstring = string.Empty;
    public void Setting(int date, string code, int index, int mode)
    {
        //m_BGName = "Note_News_CaseNewsListBtn";

        m_Mode = mode;
        m_Date = date;
        m_NewsIndex = code;
        m_Index = index;
        m_TempSubstring = GameManager.instance.ReturnNowDate(date).Replace("/", ".");
        if (m_YearLabel != null)
            m_YearLabel.text = m_TempSubstring.Substring(0, 5);
        if (m_DateLabel != null)
            m_DateLabel.text = m_TempSubstring.Substring(5);
        //date + "";//Localization.Get("News_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode +"_" + code + "_Title");
    }

    public void UnSelect()
    {
        //SeletedSprite.spriteName = m_BGName;
    }

    public void Select()
    {
        switch (m_Mode)
        {
            case 0:
                NewsManager.instance.ShowNews(m_Index);
                break;
            case 1:
                NoteManager.instance.SelectNews(m_Index);
                break;
            case 2:
                // suggest
                break;
        }

        //SeletedSprite.spriteName = m_BGName + "_Selected";
    }

    /*public void ShowContent()
    {
      
        NewsManager.instance.ShowContent(index, nType);

        /*if(StageDataManager.instance.ReturnHaveNewsItemList(index, nType) == false)
        {
            StageDataManager.instance.SetNewsItem(index, nType);
            SuggestManager.instance.InputNewsItem(index, nType);
        }*/
    //  }
    public void ShowContentInSuggest()
    {
        /*SuggestManager.instance.NewsPopup.SetActive(true);

        if (nType == NewsType.Case)
        {
            string name = StageDataManager.instance.ReturnCaseParameter(CaseParameter.VictimName, index);
            string sex = Localization.Get("Sex" + StageDataManager.instance.ReturnCaseParameter(CaseParameter.VictimSex, index));
            string age = StageDataManager.instance.ReturnCaseParameter(CaseParameter.VictimAge, index);


            SuggestManager.instance.NewsTitleLabel.text = Localization.Get(InGameGlobalValue.instance.StageName + "_News_" + InGameGlobalValue.instance.CriminalCode + "_" + InGameGlobalValue.instance.CriminalCharacter + "_" +
            nType.ToString() + "_" + index + "_Title");
            SuggestManager.instance.NewsContentLabel.text = string.Format((InGameGlobalValue.instance.StageName + "_News_" + InGameGlobalValue.instance.CriminalCode + "_" + InGameGlobalValue.instance.CriminalCharacter + "_" +
            nType.ToString() + "_" + index + "_Content"), name, sex, age);
        }
        else if (nType == NewsType.Common || nType == NewsType.Event)
        {
            SuggestManager.instance.NewsTitleLabel.text = Localization.Get(InGameGlobalValue.instance.StageName + "_News_" + nType.ToString() + "_" + index + "_Title");
            SuggestManager.instance.NewsContentLabel.text = Localization.Get(InGameGlobalValue.instance.StageName + "_News_" + nType.ToString() + "_" + index + "_Content");
        }
        */
    }
}
