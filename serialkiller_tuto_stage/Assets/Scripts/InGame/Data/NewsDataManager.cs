using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleJSON;
using System.Security.Cryptography;
public class NewsDataManager : Singleton<NewsDataManager>
{

	NewsData newsdata;
	private string filePath;

	public List<NewsDataItem> m_NewsItemList;

	/*public List<NewsDataItem> m_ReadNewsIndexList;
	public List<NewsDataItem> m_UnreadNewsIndexList;
	*/
	private void Awake()
	{
		filePath = Application.persistentDataPath + "/data02.bin";
		m_NewsItemList = new List<NewsDataItem>();
		//string aasb = "http://";

	}


	public void LoadNewsData()
	{
		if (GlobalMethod.instance.ReturnFileExist(filePath))
		{
			BinaryDeserialize();
			ReadData();
		}
	}

	private void WriteData()
	{
		newsdata = new NewsData();

		if (m_NewsItemList.Count > 0)
		{
			newsdata.m_NewsItemList = new NewsDataItem[m_NewsItemList.Count];
			for (int i = 0; i < m_NewsItemList.Count; i++)
			{
				NewsDataItem item = new NewsDataItem();
				item.m_PublishDate = m_NewsItemList[i].m_PublishDate;
				item.m_IsRead = m_NewsItemList[i].m_IsRead;
				item.m_NewsIndex = m_NewsItemList[i].m_NewsIndex;
				newsdata.m_NewsItemList[i] = item;
				item = null;
			}
		}

		BinarySerialize(newsdata);
	}

	private void ReadData()
	{
		m_NewsItemList = new List<NewsDataItem>();

		for (int i = 0; i < newsdata.m_NewsItemList.Length; i++)
		{
			NewsDataItem item = new NewsDataItem();
			item.m_PublishDate = newsdata.m_NewsItemList[i].m_PublishDate;
			item.m_IsRead = newsdata.m_NewsItemList[i].m_IsRead;
			item.m_NewsIndex = newsdata.m_NewsItemList[i].m_NewsIndex;

			m_NewsItemList.Add(item);
			if (item.m_IsRead == false)
			{
				NewsManager.instance.AddUnreadNewsList(item.m_PublishDate, item.m_NewsIndex);
				NewsManager.instance.MovingNewsPanel(true);
			}
			else
			{
				NoteManager.instance.AddReadNewsList(item.m_PublishDate, item.m_NewsIndex);
			}

			item = null;
		}

	}

	public void InputNewsItem(int date, string code)
	{
		if (m_NewsItemList.Count > 0)
		{
			bool b = false;
			for (int i = 0; i < m_NewsItemList.Count; i++)
			{
				if (m_NewsItemList[i].m_PublishDate == date && m_NewsItemList[i].m_NewsIndex == code)
				{
					b = true;
					break;
				}
			}
			if (b == false)
			{
				NewsDataItem data = new NewsDataItem();
				data.m_NewsIndex = code;
				data.m_PublishDate = date;
				data.m_IsRead = false;

				m_NewsItemList.Add(data);
				//NewsManager.instance.AddUnreadNewsList(date, code);
				data = null;
			}
		}
		else
		{
			NewsDataItem data = new NewsDataItem();
			data.m_NewsIndex = code;
			data.m_PublishDate = date;
			data.m_IsRead = false;
			m_NewsItemList.Add(data);

			//NewsManager.instance.AddUnreadNewsList(date, code);
			data = null;
		}
		NewsManager.instance.MovingNewsPanel(true);
	}

	public void MoveNewsItem(int date, string code)
	{
		//int index = 0;
		for (int i = 0; i < m_NewsItemList.Count; i++)
		{
			if (m_NewsItemList[i].m_PublishDate == date && m_NewsItemList[i].m_NewsIndex == code)
			{
				m_NewsItemList[i].m_IsRead = true;
				//index = i;
				NoteManager.instance.AddReadNewsList(date, code);
				break;
			}
		}
	}

	public void BinarySerialize(NewsData data)
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
		NewsData d = (NewsData)formatter.Deserialize(stream);
		newsdata = d;
		stream.Close();
	}

	public void Save()
	{
		print("NewsDataManager Save");
		WriteData();
	}
}
