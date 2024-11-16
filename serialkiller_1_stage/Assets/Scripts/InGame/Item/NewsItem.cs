using UnityEngine;
using System.Collections;

public class NewsItem : MonoBehaviour
{
	public bool m_IsSelected = false;

	[Header("Data")]
	[SerializeField]
	private int m_Index;
	public string m_ItemCode;
	public int m_Date;

	public UILabel m_NewsTitle, m_DateLabel;

	public int Index
	{
		get
		{
			return m_Index;
		}
		set
		{
			m_Index = value;
		}
	}

	public UISprite SeletedSprite;
	private int m_Mode = 0;

	private string m_TempSubstring = string.Empty;
	public void Setting(int date, string code, int index, int mode)
	{
		m_TempSubstring = GameManager.instance.ReturnNowDate(date).Replace("/", ".");
		m_Mode = mode;
		m_Date = date;
		m_ItemCode = code;
		m_Index = index;

		if (m_NewsTitle != null)
		{
			m_NewsTitle.text = NewsManager.GetNewsTitle(m_ItemCode);
		}

		if (m_DateLabel != null)
		{
			m_DateLabel.text = m_TempSubstring;
		}
	}

	public void UnSelect()
	{
		m_IsSelected = false;
		//SeletedSprite.spriteName = m_BGName;
	}

	public void Select()
	{
		switch (GameManager.instance.m_NoteMode)
		{
			case NoteMode.None:
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
				break;

			case NoteMode.Suggest:
				if (m_IsSelected)
				{
					SuggestManager.instance.Suggest();
					m_IsSelected = false;

				}
				else
				{
					SuggestManager.instance.Setting("News", m_ItemCode);
					NoteManager.instance.SelectNews(m_Index);
					m_IsSelected = true;
				}
				break;

			case NoteMode.Warrant:
				if (m_IsSelected)
				{
					WarrantManager.instance.SubmitEvidence("News", m_ItemCode);
					m_IsSelected = false;
				}
				else
				{
					NoteManager.instance.SelectNews(m_Index);
					m_IsSelected = true;
				}
				break;

			case NoteMode.SelectCase:
				// non-used
				break;

			default:
				break;
		}
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
