using System;
using System.Collections.Generic;
[Serializable]
public class EvidenceDataItem
{
    public string m_EvidenceName; // 증거물 이름
    public string m_EvidenceState; // 증거물 상태
    public DateTime m_StartTime; // 분석 및 비교 분석이 시작한 시간.
    public bool m_IsAnalyzed; // 분석 가능 여부. true면 가능
    public bool m_ResultAnalyzed; // 분석을 했는지 안 했는지 저장하는 값. 
    public bool m_IsMathced; // 비교 분석 가능 여부. true면 가능.
    public string m_MatchTarget;// 비교 분석 중인 아이템 이름
    public List<string> m_MatchedEvidenceName; // 비교 분석한 아이템 이름
    public List<bool> m_ResultMatched; // 비교 분석 결과
    public bool m_Alter; // 분석 후 이름이나 내용이 달라지는 경우.
    public string m_AlterName;
}
