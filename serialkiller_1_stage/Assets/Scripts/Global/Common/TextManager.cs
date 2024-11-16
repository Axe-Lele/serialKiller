using UnityEngine;
using System.Collections;

public class TextManager : MonoBehaviour {
	private static TextManager _instance;
	public static TextManager instance
	{
		get{return _instance;}
		set{_instance = value;}
	}

    protected float typingSpeed = 0.05f;

  
    //public UIPanel textPanel;
	public UILabel textLabel;
    public TweenAlpha ta;
	protected string s;
    protected string m_TempText;

	protected int count, len, findStartNum, findEndNum;

    protected int index;
    protected bool isProgress = false;
    protected bool m_IsPlayingFlag = false;
    protected bool isReady = false;
	protected bool isNextFlag;

    void Awake()
	{
		_instance = this;
	}

	void Start()
	{

	}

	public virtual void GetItem(int index, int connect, int mIndex)
	{
		print ("1");
	}

    /*public virtual void StartSearch(PlaceInfo info, bool turnEnd)
    {
    }*/

    public virtual void Init(int index, bool turnEnd)
    {

    }

    public virtual void ShowSearchTextMessage(int suspectNum, int caseIndex, bool turnEnd)
    {

    }

    public virtual void ShowSystemMessage(string str)
    { }

    public virtual void ShowSystemMessage(string str, string arg1)
    {  }

    public virtual bool ReturnIsProgress()
    {
        return m_IsPlayingFlag;
    }

    protected IEnumerator ShowPanel()
    {
        print("showpanel");
		SoundManager.instance.changeSFXVolume(1.0f);
		SoundManager.instance.PlaySFX("systemmagase");
		SoundManager.instance.PlaySFX("dialog_ing", true);
		yield return new WaitForSeconds(0.5f);
        isReady = false;
        StartCoroutine( Typing());
    }

    protected virtual IEnumerator Typing()
    {
        print("typing");
        m_IsPlayingFlag = true;

		count = 0;

		while (m_IsPlayingFlag)
        {
            textLabel.text = s.Substring(0, count);

            count++;
            if (count <= len)
            {
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                textLabel.text = s;
				m_IsPlayingFlag = false;
			}
		}
		SoundManager.instance.StopSFXByName("dialog_ing");
	}

   
    protected virtual void HidePanel()
	{
		SoundManager.instance.StopSFXByName("dialog_ing");
		StopAllCoroutines();

        ta.from = 1f;
        ta.to = 0f;
        ta.ResetToBeginning();
        ta.enabled = true;

        if (isNextFlag)
            gameObject.SendMessage("StartProcess", SendMessageOptions.DontRequireReceiver);
    }

	public void Finished()
	{

		m_IsPlayingFlag = false;

	}

	/*public void TouchBG()
	{
		if(isFinished == false)
		{
			HidePanel();
		}
		else 
		{
			StopAllCoroutines();
			textLabel.text = s;
			isFinished = false;
		}
	}*/

	/*public void Set()
	{
		count = 1;
		len = s.Length;
		textLabel.text = "";
		StartCoroutine(Typing());
	}*/

	
}
