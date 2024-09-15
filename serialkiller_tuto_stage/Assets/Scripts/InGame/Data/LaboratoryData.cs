using System;
using System.Collections.Generic;

[Serializable]
public class LaboratoryData
{
    // 분석
    public string[] m_AnalyzedIndexList;
    public int m_AnalyzedRemainTime;

    // 매치
    public string[] m_MatchedList;
    public int m_MatchedRemainTime;
    public bool m_IsStartMatched;

    public List<string> m_MatchItemA;
    public List<string> m_MatchItemB;
    public List<bool> m_MatchResult;
}
