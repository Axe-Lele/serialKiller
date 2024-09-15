using UnityEngine;
using System.Collections;

public class GlobalValue : Singleton<GlobalValue>
{
    public TextAsset CSV; // Localization.CSV
    public TextAsset DialogDataFile; // 대화 Json

    public bool isShowPersonPanel = false;
    public bool m_IsNpcForcedMoveFlag = false;
    public string m_DialogTarget;

    public string m_SelectSuggestModeName;
    public CaseMode m_SelectSuggestCaseMode;
    public string m_SelectSuggestItemName;

    public string m_SelectWarrantModeName;
    public CaseMode m_SelectWarrantCaseMode;
    public string m_SelectWarrantItemName;

}
