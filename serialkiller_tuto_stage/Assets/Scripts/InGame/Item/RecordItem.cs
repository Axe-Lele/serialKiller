using UnityEngine;
using System.Collections;

public class RecordItem : MonoBehaviour {

    public int index = -1;
	public UILabel titleLabel;

    private string TargetType;
    private string Target;

    private string type;

   // public RecordDetailView view;

    private string code;
	private string DialogCode;

    public void SettingSearchItem(string tType, string target)
    {
        type = "Search";
        TargetType = tType;
        Target = target;

        if (TargetType == "Suspect")
        {
     //       titleLabel.text = string.Format(Localization.Get("SearchItem_Suspect_Title_Label"), StageDataManager.instance.ReturnParameter(SuspectParameter.Name, int.Parse(Target)));
        }
        
    }

    public void SettingDialogItem(string tType, string target)
    {
        type = "Dialog";
        TargetType = tType;
        Target = target;

        string TargetName = "None";


        if (TargetType == "Suspect")
        {
        //    TargetName =  StageDataManager.instance.ReturnParameter(SuspectParameter.Name, int.Parse(Target));
        }
        else if (TargetType == "Teacher")
        {
            TargetName = Localization.Get(PlayDataManager.instance.m_StageName + "_Teacher_" + Target);
        }
        else if (TargetType == "Colleague")
        {
            TargetName = Localization.Get(PlayDataManager.instance.m_StageName + "_Colleague_" + Target);
            //titleLabel.text = string.Format(Localization.Get("DialogItem_Colleague_Title_Label"),  StageDataManager.instance.ReturnParameter(SuspectParameter.Name, int.Parse(Target)));
        }

        titleLabel.text = string.Format(Localization.Get("DialogItem_Title_Label"), TargetName);

        code = TargetType + "_" + Target;

    }

    public void Select()
    {
        if (type == "Search")
        {
            if (TargetType == "Suspect")
            {
                SystemMessageManager.instance.ShowSearchTextMessage(int.Parse(Target), 0, false);//.text = string.Format(Localization.Get("SearchItem_House_Title_Label"), StageDataManager.instance.ReturnParameter(SuspectParameter.Name, int.Parse(target)));
            }
        }
        else if (type == "Dialog")
        {
        //    view.RecordDetailSetting(TargetType, Target);
        }
    }

    public void Selecting()
    {
        if (type == "Search")
        {
            if (TargetType == "Suspect")
            {
                SystemMessageManager.instance.ShowSearchTextMessage(int.Parse(Target), 0, false);//.text = string.Format(Localization.Get("SearchItem_House_Title_Label"), StageDataManager.instance.ReturnParameter(SuspectParameter.Name, int.Parse(target)));
            }
        }
        else if (type == "Dialog")
        {
        //    view.RecordDetailSetting(TargetType, Target);
//            SuggestManager.instance.SelectItem(1, index);
        }
    }
    
    public void SetRecordItem(bool b)
    {
        if (b)
        {
  //          index = InGameGlobalValue.instance.CurrentRecordItemIndex;
    //        InGameGlobalValue.instance.CurrentRecordItemIndex++;
        }
        else
        {
        }

        gameObject.transform.localPosition = new Vector2(0f, 270f - (index * 90f));
        gameObject.SetActive(true);
    }

    public int ReturnIndex()
    {
        return index;
    }

    public string ReturnTarget()
    {
        return Target;
    }

    public string ReturnType()
    {
        return type;
    }

    public string ReturnTargetType()
    {
        return TargetType;
    }

    public string ReturnCode()
    {
        return type + "_" + TargetType + "_" + Target;
    }
    

   
}
