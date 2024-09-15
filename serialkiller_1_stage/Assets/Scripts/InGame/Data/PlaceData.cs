using System;
using System.Collections.Generic;

[Serializable]
public class PlaceData2
{
	public string	Type;
	public int    Index;
	public bool		IsSearched;
	public bool		IsOpened;
	public List<string> Evidences;
}

[Serializable]
public class PlaceSaveData
{
	public List<PlaceData2> Places;
}