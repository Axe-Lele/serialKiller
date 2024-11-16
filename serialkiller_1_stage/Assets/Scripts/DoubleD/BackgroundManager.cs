using UnityEngine;
using SimpleJSON;
using System.Collections;

public class BackgroundManager : Singleton<BackgroundManager>
{
	public UIAtlas[] m_Atlases;
	
	public UIAtlas GetBackgroundAtlas(int atlasIndex)
	{
		return m_Atlases[atlasIndex];
	}
}
