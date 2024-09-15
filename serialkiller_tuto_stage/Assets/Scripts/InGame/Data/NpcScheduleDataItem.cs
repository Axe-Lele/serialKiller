using UnityEngine;
using System;
using System.Collections.Generic;
[Serializable]
public class NpcScheduleDataItem : MonoBehaviour
{
    public List<ScheduleItem> m_Monday;
    public List<ScheduleItem> m_Tuesday;
    public List<ScheduleItem> m_Wednesday;
    public List<ScheduleItem> m_Thursday;
    public List<ScheduleItem> m_Friday;
    public List<ScheduleItem> m_Saturday;
    public List<ScheduleItem> m_Sunday;

}
