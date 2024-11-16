using UnityEngine;
using DetectiveBoard;
using System;
using System.Collections.Generic;

namespace DetectiveBoard
{
	public enum DetectiveItemType
	{
		undefined = -1,
		Murder = 0,
		Motive = 1,
		Method = 2,
		Victim = 3,
		Etc = 4
	}

	[Serializable]
	public class DetectiveDataItem
	{
		public string Index = string.Empty;

		public string m_Stage;
		public string m_Case;
		public string m_GroupIndex;
		public int m_NodeIndex;
		public string m_SpriteInfo;

		public bool m_IsFirst = false;
		public bool m_IsOpen = false;
		public DetectiveItemType m_Type = DetectiveItemType.undefined;

		[SerializeField]
		private string spritename;
		[SerializeField]
		private string spriteType;

		public string m_SpriteName
		{
			get { return spritename; }
		}
		public string m_SpriteType
		{
			get { return spriteType; }
		}

		public void SetSpriteInfo()
		{
			string[] temp = m_SpriteInfo.Split('-');
			if(spritename == null || spritename.Length == 0)
			{
				spritename = temp[1];
			}

			if (spriteType == null || spriteType.Length == 0)
			{
				spriteType = temp[0];
			}
		}
	}

	[Serializable]
	public class DetectiveData
	{
		public List<DetectiveDataItem> Items;

		public DetectiveDataItem this[string key]
		{
			get
			{
				for(int i = 0;i < Items.Count; i++)
				{
					if(Items[i].Index.Equals(key))
					{
						return Items[i];
					}
				}

				return null;
			}
		}
	}
}