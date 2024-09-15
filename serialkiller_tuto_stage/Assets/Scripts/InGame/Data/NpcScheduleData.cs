using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class NpcScheduleData
{
    public string m_Index;
    public List<NpcScheduleDataItem> m_Common;
    public List<NpcScheduleDataItem> m_Event0;
    public List<NpcScheduleDataItem> m_Event1;
    public List<NpcScheduleDataItem> m_Event2;
    public List<NpcScheduleDataItem> m_Event3;
    public List<NpcScheduleDataItem> m_Event4;
}
