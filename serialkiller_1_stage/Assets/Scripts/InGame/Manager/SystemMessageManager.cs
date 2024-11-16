using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class SystemMessageManager : TextManager
{
	public GameObject obj;
	private string m_TypingText;
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

	public override void ShowSystemMessage(string str)
	{
		Debug.Log("Show SystemMessage[" + str + "]");
		if (EventDataManager.instance.IsFirstShowSystemMessage(str) == false)
		{
			Debug.Log("You have seen this SystemMessage[" + str + "].");
			return;
		}

		if (isProgress)
		{
			if (m_IsPlayingFlag)
				TouchBG();

			HidePanel();
		}

		m_Temp = str;
		m_TempText = s = Localization.Get("System_Message_" + PlayDataManager.instance.m_StageName + "_" + str);
		Debug.Log(str + " : " + s);
		isTurnEnd = false;
		print("str : " + str + " / s : " + s);
		SystemMessageInit();
		StartCoroutine(ShowPanel());
	}

	public void InputSystemMessage(string str)
	{
		if (isProgress)
		{
			if (m_IsPlayingFlag)
				TouchBG();

			HidePanel();
		}
		m_Temp = str;
		m_TempText = s = Localization.Get(str);
		isTurnEnd = false;
		print("str : " + str + " / s : " + s);
		SystemMessageInit();
		StartCoroutine(ShowPanel());
	}

	public override void ShowSystemMessage(string str, string arg1)
	{
		if (isProgress)
		{
			if (m_IsPlayingFlag)
				TouchBG();

			HidePanel();
		}
		m_Temp = str;
		m_TempText = s = string.Format(Localization.Get("System_Message_" + PlayDataManager.instance.m_StageName + "_" + str), arg1);
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

		for (int i = 0; i < s.Length - 3; i++)
		{
			if (s.Substring(i, 3) == "[-]")
			{
				specialLen++;
			}
		}

		if (specialLen == 0)
		{
			findStartNum = new int[1];
			findEndNum = new int[1];
			findStartNum[0] = 0;
			findEndNum[0] = 0;
		}
		else
		{

			findStartNum = new int[specialLen];
			findEndNum = new int[specialLen];

			for (int i = 0; i < s.Length - 3; i++)
			{
				if (s.Substring(i, 3) == "[-]")
				{
					findEndNum[index] = i + 1;
					index++;
				}
			}
		}

		int startIndex = 0;
		int endIndex = 0;
		count = 0;
		m_TempText += "[-]";
		m_IsPlayingFlag = true;


		bool isColor = false;
		while (m_IsPlayingFlag)
		{
			//	print("start : " + startIndex + " [] : " + findStartNum[startIndex] + " count : " + count);
			if (findStartNum[startIndex] != 0 &&
					count == findStartNum[startIndex])
			{
				startIndex++;
				if (specialLen == startIndex)
				{
					startIndex = specialLen - 1;
				}
				count += 8;
			}

			if (findEndNum[endIndex] != 0 && count == findEndNum[endIndex])
			{
				endIndex++;
				if (specialLen == endIndex)
				{
					endIndex = specialLen - 1;
				}
				count += 3;
			}


			if (count < len)
			{
				Debug.Log("[" + count + "]" + m_TempText + " / " + len);
				if (isColor == true)
				{
					for (; m_TempText[count].Equals(']') == false;)
					{
						Debug.Log(m_TempText[count]);
						count++;
					}
					count++;
					isColor = false;
				}
				else if (m_TempText[count + 1].Equals('-') == false)
				{
					isColor = true;
					for (; m_TempText[count].Equals(']') == false;)
					{
						//Debug.Log(m_TempText[count]);
						count++;
					}
					count++;
				}
				m_TypingText = m_TempText.Insert(count, "[55]");
				textLabel.text = m_TypingText;// s.Substring(0, count);
				count++;
				yield return new WaitForSeconds(typingSpeed);
			}
			else
			{
				//m_TypingText = s;
				//m_TypingText = m_TempText.Insert(count, "[55]");
				textLabel.text = s;
				m_IsPlayingFlag = false;
			}


		}

		SoundManager.instance.StopSFXByName("dialog_ing");
	}

	public void TouchBG()
	{
		if (isReady)
			return;

		if (m_IsPlayingFlag == false)
		{
			HidePanel();
		}
		else
		{
			StopAllCoroutines();
			textLabel.text = m_TempText;
			m_IsPlayingFlag = false;
		}
	}

	protected override void HidePanel()
	{
		StopAllCoroutines();
		SoundManager.instance.StopSFXByName("dialog_ing");

		StartCoroutine(Hide());
	}

	IEnumerator Hide()
	{
		SoundManager.instance.StopSFXByName("dialog_ing");

		ta.to = 0f;
		ta.from = 1f;
		ta.ResetToBeginning();
		ta.enabled = true;
		isReady = true;

		yield return new WaitForSeconds(1f);
		obj.SetActive(false);
		if (isTurnEnd)
		{
			//GameManager.instance.TurnEnd();
		}
		isProgress = false;
		if (m_Temp != null)
			EventManager.instance.SetEvent("SystemMessage", m_Temp);
	}

	public void SystemMessagePanelHide()
	{
		SoundManager.instance.StopSFXByName("dialog_ing");
		HidePanel();
	}

	public override bool ReturnIsProgress()
	{
		return isProgress;
	}


}
