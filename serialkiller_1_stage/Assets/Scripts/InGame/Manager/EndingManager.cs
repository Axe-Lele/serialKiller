using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using SimpleJSON;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

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

	public JSONNode EndingNode;
	public TextAsset EndingTextAsset;

	private bool isFinished = false;
	//public void SetEnding(string Illust, string Ending)
	//{
	//    m_EndingSprite.spriteName = "Ending_" + Illust;

	//    m_EndingTitle.text = Localization.Get("Ending_" + Ending + "_Title");
	//    str = Localization.Get("Ending_" + Ending + "_Content");
	//    len = str.Length;
	//    count = 0;
	//    m_EndingPopup.SetActive(true);
	//    ta.enabled = true;
	//}

	public List<UIAtlas> m_Atlses;
	public List<Ending> m_SuccesEnding;

	public List<Ending> m_EndingDB;

	[System.Serializable]
	public class Ending
	{
		public string Index;
		public int Atlas;
		public string Illust;
	}

	private void Awake()
	{
		filePath = Application.persistentDataPath + "/data50.bin";
		LoadDB();
	}

	#region DB & Data

	private string filePath;

	private void LoadDB()
	{
		System.Type itemType = typeof(Ending);

		FieldInfo[] fId = itemType.GetFields(BindingFlags.Instance |
											 BindingFlags.Static |
											 BindingFlags.Public |
											 BindingFlags.NonPublic);

		m_EndingDB = new List<Ending>();
		EndingNode = JSONNode.Parse(EndingTextAsset.text);
		Ending item;

		for (int i = 0; i < EndingNode.Count; i++)
		{
			item = new Ending();
			item.Index = EndingNode[i]["Index"].Value;
			item.Illust = EndingNode[i]["Illust"].Value;
			item.Atlas = EndingNode[i]["Atlas"].AsInt;

			//for (int flag = 0; flag < fId.Length; flag++)
			//{
			//	object point = fId[flag].GetValue(item);
			//	if (point is string)
			//	{
			//		print(EndingNode[i][flag].Value);
			//		fId[flag].SetValue(item, EndingNode[i][flag].Value);
			//	}
			//}

			m_EndingDB.Add(item);
		}
	}

	private void WriteData()
	{

	}

	private void ReadData()
	{

	}

	public void BinarySerialize(StageData data)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream;
		if (GlobalMethod.instance.ReturnFileExist(filePath))
		{
			stream = new FileStream(filePath, FileMode.Open);
		}
		else
		{
			stream = new FileStream(filePath, FileMode.Create);
		}
		formatter.Serialize(stream, data);
		stream.Close();
	}

	public void BinaryDeserialize()
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(filePath, FileMode.Open);
		List<Ending> d = (List<Ending>)formatter.Deserialize(stream);
		m_EndingDB = d;
		stream.Close();
	}

	#endregion

	public void SetEnding(string endingIndex)
	{
		Ending item = null;
		for (int i = 0; i < m_EndingDB.Count; i++)
		{
			if (m_EndingDB[i].Index == endingIndex)
			{
				item = m_EndingDB[i];
				break;
			}
		}

		if (item.Index == null)
		{
			Debug.LogErrorFormat("Has no Ending Index : {0}", endingIndex);
			return;
		}

		if (m_Atlses.Count > item.Atlas)
			m_EndingSprite.atlas = m_Atlses[item.Atlas];

		m_EndingSprite.spriteName = item.Illust;

		m_EndingTitle.text = Localization.Get("Ending_" + endingIndex + "_Title");
		str = Localization.Get("Ending_" + endingIndex + "_Content");

		len = str.Length;
		count = 0;
		m_EndingPopup.SetActive(true);
		ta.enabled = true;

		m_SuccesEnding.Add(item);
	}

	public void TypingText()
	{
		isFinished = true;
		StartCoroutine("Typing");
	}

	private IEnumerator Typing()
	{
		while (isFinished)
		{
			m_EndingContent.text = str.Substring(0, count);

			count++;
			if (count <= len)
			{
				yield return new WaitForSeconds(0.02f);
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

		ShowDetectiveBoard();
	}

	public void ShowDetectiveBoard()
	{
		DetectiveManager.instance.ShowDetectiveBoard(
			DetectiveManager.instance.m_StageIndex,
			DetectiveManager.instance.m_CaseIndex);
	}
}

