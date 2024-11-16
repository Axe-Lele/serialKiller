using System;
using System.Collections.Generic;

[Serializable]
public class WorldDataItem
{
	/// <summary>
	/// 스테이지 번호(0 ~ n)
	/// </summary>
	public int m_StageIndex;
	/// <summary>
	/// 유료 결제 여부
	/// </summary>
	public bool m_IsPayable;
	/// <summary>
	/// 해금 여부
	/// </summary>
	public bool m_IsOpened;
	/// <summary>
	/// 해금 트리거 오픈 여부
	/// </summary>
	public List<bool> m_IsUnlockTriggers;
	public List<string> m_UnlockTriggers;
}

[Serializable]
public class WorldData
{
    public bool[] m_IsOpend; //스테이지가 열려 있는지


}
