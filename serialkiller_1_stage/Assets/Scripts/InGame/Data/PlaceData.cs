using System;
using System.Collections.Generic;

[Serializable]
public class PlaceData2
{
	#region Start Setting
	public string Type;
	public int Index;
	public bool IsSearched;
	public bool IsOpened;
	#endregion

	#region After Data
	public List<string> Evidences;
	#endregion
}

[Serializable]
public class PlaceSaveData
{
	public List<PlaceData2> Places;
}