namespace Extension.NGUI
{

	public static class NGUIExtension
	{
		public static bool SetSprite(this UISprite ui, string spriteType, string spriteName)
		{
			if (ui == null)
			{
				UnityEngine.Debug.LogWarning("Where is your UISprite Component.");
				return false;
			}

			if (string.IsNullOrEmpty(spriteName))
				return false;

			string npcName = spriteName;
			// 감정 표현
			if (npcName.Contains("_"))
			{
				string[] temp = npcName.Split('_');
				npcName = temp[0];
			}
			UIAtlas atlas = NpcDataManager.instance.ReturnAtlas(npcName);

			UnityEngine.MonoBehaviour.print("Set Sprite [" + atlas + "] / [" + spriteName + "]");

			if (atlas == null)
				return false;

			ui.atlas = atlas;

			if (atlas.GetSprite(spriteName) == null)
				return false;

			ui.spriteName = spriteName;

			return true;
		}

		public static void ResetSprite(this UISprite ui)
		{
			if (ui == null)
				return;

			ui.spriteName = string.Empty;
		}
	}
}
