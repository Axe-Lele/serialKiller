using UnityEngine;
using DetectiveBoard;
using System;

[Serializable]
public class DetectiveNodeItem : MonoBehaviour
{
	public enum DetectiveNodeType
	{
		undefined = -1,
		Portrait,
		Evidence
	}

	public string m_Index;
	public string m_Title, m_Content;

	public UISprite m_Portrait, m_Evidence, m_Lock;
	public UISprite m_Line, m_Shadow;
	public bool m_IsOpen
	{
		get
		{
			if (data == null)
				return false;

			return data.m_IsOpen;
		}
	}

	[SerializeField, Header("DB")]
	private DetectiveDataItem data;

	public void ReadyToShow(ref DetectiveDataItem item)
	{
		data = item;
		data.SetSpriteInfo();

		gameObject.SetActive(true);
	}

	private void Awake()
	{
	}

	public void Init()
	{
		if (m_Portrait != null)
			m_Portrait.spriteName = string.Empty;

		if (m_Evidence != null)
			m_Evidence.spriteName = string.Empty;

		data = null;
		m_Index = string.Empty;
		m_Title = string.Empty;
		m_Content = string.Empty;

		gameObject.SetActive(false);
	}

	public void SetNode(DetectiveNodeType type, string key)
	{
		return;

		this.Init();
		if (m_IsOpen == false)
		{
			// I need LockImage
			m_Lock.gameObject.SetActive(true);
			m_Portrait.spriteName = string.Empty;
			m_Evidence.spriteName = string.Empty;
			return;
		}

		switch (type)
		{
			case DetectiveNodeType.undefined:
				m_Lock.gameObject.SetActive(true);
				m_Portrait.spriteName = string.Empty;
				m_Evidence.spriteName = string.Empty;
				return;

			case DetectiveNodeType.Portrait:
				m_Portrait.atlas = NpcDataManager.instance.ReturnAtlas(key);
				m_Portrait.spriteName = key;
				break;

			case DetectiveNodeType.Evidence:
				m_Evidence.atlas = NpcDataManager.instance.ReturnAtlas(key);
				m_Evidence.spriteName = key;
				break;

			default:
				break;
		}
		m_Lock.gameObject.SetActive(false);

		print("Atlas : " + m_Portrait.atlas.name + " / Sprite : " + m_Portrait.spriteName);
	}

	public void ChangeAlpha(float alpha)
	{
		m_Portrait.alpha = 1f - alpha;
		m_Evidence.alpha = 1f - alpha;
		m_Lock.alpha = alpha;
	}

	public void ChangeSprite()
	{
		if (data == null)
			return;

		if (data.m_SpriteInfo == null || data.m_SpriteInfo.Length == 0)
			return;

		string stageIndex = DetectiveManager.instance.m_StageIndex.ToString();

		UIAtlas atlas = AtlasManager.instance.GetAtlas(stageIndex, data.m_SpriteType, data.m_SpriteName);

		m_Evidence.gameObject.SetActive(false);
		m_Portrait.gameObject.SetActive(false);
		if (data.m_SpriteType.Equals("Evidence"))
		{
			m_Evidence.atlas = atlas;
			m_Evidence.spriteName = data.m_SpriteName;
			m_Evidence.gameObject.SetActive(true);
		}
		else if (data.m_SpriteType.Equals("Character"))
		{
			m_Portrait.atlas = atlas;
			m_Portrait.spriteName = data.m_SpriteName;
			m_Portrait.gameObject.SetActive(true);
        }
        print(data.m_SpriteName);
    }

	public void ChangeSprite(string atlasname, string spritename)
	{


		return;
		//m_Sprite.atlas = 
		//	Resources.Load(string.Format("{0}/{1}", m_FilePath, atlasname), typeof(UIAtlas)) as UIAtlas;
		m_Portrait.spriteName = spritename;
	}
}
