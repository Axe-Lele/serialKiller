using System;
[Serializable]
public class EventDataItem
{
	public string m_EventCode; // 이벤트 코드
	public bool m_IsStart; // 이벤트가 발생했는지에 대한 여부

	public int m_Date; //이벤트 발생 날짜

	public int m_PlaceType; // AreaEvent에서 사용. PlaceType을 저장
	public int m_Place; // AreaEvent에서 사용. PlaceIndex를 저장
	public bool m_IsOpen; // AreaEvent에서 사용. Open 여부를 저장.

	public string m_Who; // DialogEvent에서 사용. 누구랑 대화하는지 저장.
	public string m_StartDialogIndex; // DialogEvent에서 사용. 시작 대화 저장.

	public GameEventType m_GameEventType;
	public string m_DialogIndex;
}
