using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : Singleton<EndingManager>
{
    public GameObject m_EndingPopup;

    public UISprite m_EndingSprite;
    public UILabel m_EndingTitle;
    public UILabel m_EndingContent;

    public GameObject m_Notice;

    public TweenAlpha ta;

    private string str;
    private int count;
    private int len;

    private bool isFinished = false;
    public void SetEnding(string Illust, string Ending)
    {
        m_EndingSprite.spriteName = "Ending_" + Illust;

        m_EndingTitle.text = Localization.Get("Ending_" + Ending + "_Title");
        str = Localization.Get("Ending_" + Ending + "_Content");
        len = str.Length;
        count = 0;
        m_EndingPopup.SetActive(true);
        ta.enabled = true;
    }

    public void TypingText()
    {
        StartCoroutine("Typing");
    }

    private IEnumerator Typing()
    {
        isFinished = true;

        while (isFinished)
        {
            m_EndingContent.text = str.Substring(0, count);

            count++;
            if (count <= len)
            {
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                m_EndingContent.text = str;
                isFinished = false;
                //   TypingFinish();
                m_Notice.gameObject.SetActive(true);
            }
        }
    }

    public void Finish()
    {
        print("IsFinished :" + isFinished);
        if (isFinished == true)
        {
            return;
        }

        ResetGame();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }

}

	
