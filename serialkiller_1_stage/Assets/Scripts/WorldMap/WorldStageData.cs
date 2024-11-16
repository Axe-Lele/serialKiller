using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WorldStageData
{
    public List<CityData> Cities;
}

[Serializable]
public class CityData
{
    public int Index;
    public bool IsOpen;
	public bool[] IsFirstStage;
    public bool[] IsOpenStage;
	public string[] UnlockIndexes;
}

[Serializable]
public class UnlockDataItem
{
	public string Index;
	public string Type;
	public bool IsOpen;

	public string ComplishStage;
	public int Complish;
	public int Pay;
	public string UnlockStage;
	public string ClearStage;
}

[Serializable]
public class UnlockData
{
	public List<UnlockDataItem> Items;
}