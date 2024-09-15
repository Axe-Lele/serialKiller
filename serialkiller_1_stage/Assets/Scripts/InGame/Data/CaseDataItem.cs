using System;
using System.Collections.Generic;
[Serializable]
public class CaseDataItem
{
    public string CaseIndex;
    public int CaseLocation;
    public string VictimName;
    public int Date;
    public int NewsDate;
    public int Time;
    public List<string> Event;
}
