using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class EventData
{
    public List<EventDataItem> m_AreaEventList;
    public List<EventDataItem> m_NewsEventList;
    public List<EventDataItem> m_NpcEventList;
    public List<EventDataItem> m_CaseEventList;
    public List<EventDataItem> m_DialogEventList;
    public List<EventDataItem> m_EndDialogEventList;
    public List<EventDataItem> m_CasePlaceEventList;
    public List<EventDataItem> m_SearchEventList;
    public List<EventDataItem> m_StartDialogEventList;


//    public List<EventDataItem> m_EventDataItemList;
/*   public List<int> m_Date;
   public List<string> m_EventCode;
   public List<bool> m_IsStart;
   public List<bool> m_IsProcess;
   public List<int> m_EventType;
   public List<string> m_OnFinished;*/
}
