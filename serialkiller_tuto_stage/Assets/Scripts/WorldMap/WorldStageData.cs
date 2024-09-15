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
    public bool[] IsOpenStage;
}