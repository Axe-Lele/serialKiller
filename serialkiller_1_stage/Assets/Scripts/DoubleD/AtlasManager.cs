using UnityEngine;
using System.Collections.Generic;

public class AtlasManager : Singleton<AtlasManager>
{
	[System.Serializable]
	public class AtlasSet
	{
		/// <summary>
		/// 그룹 인덱스
		/// </summary>
		[HideInInspector]
		public string m_Index;

		[SerializeField]
		public UIAtlas[] m_Atlases;
	}

	public enum SpriteType
	{
		Character,
		Evidence,
		Background
	}

	public AtlasSet m_NowAtlas = null;
	public List<AtlasSet> m_AtlasList;

	private void Awake()
	{
		DontDestroyOnLoad(this);

		m_AtlasList = new List<AtlasSet>();

		AtlasSet item = null;
		UIAtlas[] _atlases;
		for (int i = 0; i < 1; i++)
		{
			_atlases = Resources.LoadAll<UIAtlas>(string.Format("Atlas/Stage{0}", i));
			if (_atlases.Length == 0 || _atlases == null)
				break;

			item = new AtlasSet()
			{
				m_Atlases = _atlases,
				m_Index = string.Format("S{0:00}", i)
			};
			m_AtlasList.Add(item);
			item = null;
		}
	}

	public UIAtlas GetAtlas(string spriteType, string spriteName)
	{
		string stageIndex = DetectiveManager.instance.m_StageIndex.ToString();
		return this.GetAtlas(stageIndex, spriteType, spriteName);
	}

	/// <summary>
	/// z
	/// </summary>
	/// <param name="stageIndex">StageIndex</param>
	/// <param name="spriteType"></param>
	/// <returns></returns>
	public UIAtlas GetAtlas(string stageIndex, string spriteType, string spriteName)
	{
		m_NowAtlas = null;

		Debug.Log("GetAtlas - stageIndex : " + stageIndex + " / spriteType : " + spriteType + " / spriteName : " + spriteName);

		string stage = string.Format("S{0:00}", stageIndex.ToInt());

		for (int i = 0; i < m_AtlasList.Count; i++)
		{
			if (!m_AtlasList[i].m_Index.Equals(stage))
				continue;

			m_NowAtlas = m_AtlasList[i];
			break;
		}

		if (m_NowAtlas == null)
			return null;

		UIAtlas rAtlas = null;
		for (int i = 0; i < m_NowAtlas.m_Atlases.Length; i++)
		{
			if (m_NowAtlas.m_Atlases[i].name.Contains(spriteType) == false)
				continue;

			rAtlas = m_NowAtlas.m_Atlases[i];
			UISpriteData data = rAtlas.GetSprite(spriteName);

			if (data == null)
				continue;

			break;
		}

		if (rAtlas == null)
			return null;

		return rAtlas;
	}
}
