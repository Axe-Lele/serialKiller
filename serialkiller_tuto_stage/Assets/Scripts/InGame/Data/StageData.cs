using System;

[Serializable]
public class StageData 
{
    public int m_PastDay; // 게임 시작 후 지난간 일 수
    public int m_Stamina; // 유저의 행동력
    public int m_CriminalCode; // 현재 범인 코드
    //public string[] m_CaseList; // 범인이 일으킬 사건 리스트
    public int m_RemainStamina; // 휴식 전에 남은 행동력의 총합
    public int m_RemainCheckCount; // 휴식 전에 남은 행동력이 일정 값 이상을 남겼을 경우 체크하는 카운터
    //public CaseDataItem[] m_CaseDataItemList; // 일어날 사건 리스트
    public string[] m_CheckCaseList; // 현재까지 유저가 확인한 사건 리스트
}
