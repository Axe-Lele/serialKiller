using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class SystemMessageManager : TextManager
{
	public GameObject obj;
    private string temp;
    private UIPanel panel;
	private bool isTurnEnd;
    private string m_Temp;

 
    private void SystemMessageInit()
    {
        obj.SetActive(true);

        isProgress = true;
        isReady = true;

        ta.to = 1f;
        ta.from = 0f;
        ta.ResetToBeginning();

        count = 1;
        len = s.Length;
        textLabel.text = "";

        ta.enabled = true;
    }

    /*
    public override void ShowSearchTextMessage(int suspectIndex, int caseIndex, bool turnEnd)
    {
        if (isProgress)
        {
            if (isFinished)
                TouchBG();

            HidePanel();
        }
        
        isTurnEnd = turnEnd;
        tempStr = s = "";
//           string.Format(Localization.Get(InGameGlobalValue.instance.StageName + "_Search_" + InGameGlobalValue.instance.CriminalCode + "_" + InGameGlobalValue.instance.CriminalCharacter + "_" + suspectIndex), (StageDataManager.instance.ReturnParameter(SuspectParameter.Name, suspectIndex)));

        SystemMessageInit();

        StartCoroutine(ShowPanel());
    }
    */
 
    public override void ShowSystemMessage(string str)
    {
        if (isProgress)
        {
            if (isFinished)
                TouchBG();

            HidePanel();
        }
        m_Temp = str;
        tempStr = s = Localization.Get("System_Message_" + PlayDataManager.instance.m_StageName + "_" + str);
        isTurnEnd = false;
        print("str : " + str + " / s : " + s);
        SystemMessageInit();
        StartCoroutine(ShowPanel());
    }

    public void InputSystemMessage(string str)
    {
        if (isProgress)
        {
            if (isFinished)
                TouchBG();

            HidePanel();
        }
        m_Temp = str;
        tempStr = s = Localization.Get(str);
        isTurnEnd = false;
        print("str : " + str + " / s : " + s);
        SystemMessageInit();
        StartCoroutine(ShowPanel());
    }

    public override void ShowSystemMessage(string str, string arg1)
    {
        if (isProgress)
        {
            if (isFinished)
                TouchBG();

            HidePanel();
        }
        m_Temp = str;
        tempStr = s = string.Format(Localization.Get("System_Message_" + PlayDataManager.instance.m_StageName + "_" + str), arg1);
        isTurnEnd = false;
        SystemMessageInit();

        StartCoroutine(ShowPanel());
    }

    protected override IEnumerator Typing()
    {
        print("typing");
		int[] findStartNum;
		int[] findEndNum;
		int specialLen = 0;
		int index = 0;

		for(int i = 0; i < s.Length-3; i++)
		{
			if(s.Substring(i,3) == "[-]") 
			{
				specialLen++;
			}
		}

		if(specialLen == 0)
		{
			findStartNum = new int[1];
			findEndNum  = new int[1];
			findStartNum[0] = 0;
			findEndNum[0] = 0;
		}
		else
		{

			findStartNum = new int[specialLen];
			findEndNum  = new int[specialLen];

			for(int i = 0; i < s.Length-3; i++)
			{
				if(s.Substring(i,3) == "[-]") 
				{
					findEndNum[index] = i+1;
					index++;
				}
			}
		}

		int startIndex = 0;
		int endIndex = 0;
		count = 0;
        tempStr += "[-]";
        isFinished = true;


        while (isFinished)
		{
		//	print("start : " + startIndex + " [] : " + findStartNum[startIndex] + " count : " + count);
            if (findStartNum[startIndex] != 0 && 
                count == findStartNum[startIndex])
            {
				startIndex++;
				if(specialLen == startIndex)
				{
					startIndex = specialLen - 1;
				}
                count += 8;
            }

			if (findEndNum[endIndex] != 0 && count == findEndNum[endIndex])
			{
				endIndex++;
				if(specialLen == endIndex)
				{
					endIndex = specialLen - 1;
				}
				count += 3;
            }

            temp = tempStr.Insert(count, "[55]");
            textLabel.text = temp;// s.Substring(0, count);

            count++;
            if (count <= len)
            {
                yield return new WaitForSeconds(typingSpeed);
            }
            else
            {
                isFinished = false;
                textLabel.text = s;
            }
        }
     
    }

    public void TouchBG()
    {
        if (isReady)
            return;

        if (isFinished == false)
        {
            HidePanel();
        }
        else
        {
            StopAllCoroutines();
            textLabel.text = tempStr;
            isFinished = false;
        }
    }

    protected override void HidePanel()
    {
        StopAllCoroutines();

        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        ta.to = 0f;
        ta.from = 1f;
        ta.ResetToBeginning();
        ta.enabled = true;
        isReady = true;

        yield return new WaitForSeconds(1f);
		obj.SetActive(false);
		if(isTurnEnd)
		{
			//GameManager.instance.TurnEnd();
		}
		isProgress = false;
        EventManager.instance.SetEvent("SystemMessage", m_Temp);
    }

    public void SystemMessagePanelHide()
    {
        HidePanel();
    }

    public override bool ReturnIsProgress()
    {
        return isProgress;
    }


}
