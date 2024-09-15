using UnityEngine;
using System.Collections;

public class TextManager : MonoBehaviour {
	private static TextManager _instance;
	public static TextManager instance
	{
		get{return _instance;}
		set{_instance = value;}
	}

    protected float typingSpeed = 0.025f;

  
    //public UIPanel textPanel;
	public UILabel textLabel;
    public TweenAlpha ta;
	protected string s;
    protected string tempStr;

	protected int count, len, findStartNum, findEndNum;

    protected int index;
    protected bool isProgress = false;
    protected bool isFinished = false;
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
        return isFinished;
    }

    protected IEnumerator ShowPanel()
    {
        print("showpanel");
        yield return new WaitForSeconds(0.5f);
        isReady = false;
        StartCoroutine( Typing());
    }

    protected virtual IEnumerator Typing()
    {
        print("typing");
        isFinished = true;

		while (isFinished)
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
             //   TypingFinish();
            }
        }
    }

   
    protected virtual void HidePanel()
	{
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

		isFinished = false;

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
