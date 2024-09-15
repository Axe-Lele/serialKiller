using UnityEngine;
using System.Collections;

public class DetectiveNodeItem : MonoBehaviour
{
	public int m_Index;
	public UISprite m_Sprite;
	public bool m_IsOpen;

	private void Awake()
	{
		m_Sprite.spriteName = string.Empty;
	}

	public void Init()
	{
		m_Sprite.spriteName = string.Empty;
	}

	public void Set(string npcKey)
	{
		this.Init();
		if(m_IsOpen == false)
		{
			// I need LockImage
			m_Sprite.spriteName = string.Empty;
			return;
		}

		m_Sprite.atlas = NpcDataManager.instance.ReturnAtlas(npcKey);
		m_Sprite.spriteName = npcKey;

		print("Atlas : " + m_Sprite.atlas.name + " / Sprite : " + m_Sprite.spriteName);
	}

	public void ChangeSprite(string atlasname, string spritename)
	{
		//m_Sprite.atlas = 
		//	Resources.Load(string.Format("{0}/{1}", m_FilePath, atlasname), typeof(UIAtlas)) as UIAtlas;
		m_Sprite.spriteName = spritename;
	}
}
