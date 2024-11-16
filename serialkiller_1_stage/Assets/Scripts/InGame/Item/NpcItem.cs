
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class NpcItem
{
	// 기본 입력값
	[NonSerialized]
	public bool m_IsInit = false;
	public string m_Index; // Npc의 인덱스 값
												 //public NpcRace m_Race; // 인종
												 //public NpcSex m_Sex; // 성별
	public NpcReligion m_Religion; // 종교
	public int m_Age; // 나이
	public int m_Height; // 키
	public int m_AtlasIndex; // 이 캐릭터가 속해 있는 아틀라스 번호
	public int m_Home;
	public int m_Company;
	public bool m_IsMeet = false;
	public NpcState m_NpcInitState;
	public string m_NpcEventCode = string.Empty; // npc has eventcode;
	
	// 추출 값    
	private string m_Name; // 이름
	private string m_HomeName; // 
	private string m_CompanyName; // 회사
	private string m_Job; // 직업
	private NpcState m_NpcState; // 상태
	[SerializeField]
	private string m_NpcPosition; // 위치



	public string Name
	{
		get { return m_Name; }
		set { m_Name = value; }
	}

	public string HomeName
	{
		get { return m_HomeName; }
		set { m_HomeName = value; }
	}

	public string CompanyName
	{
		get { return m_CompanyName; }
		set { m_CompanyName = value; }
	}

	public string Job
	{
		get { return m_Job; }
		set { m_Job = value; }
	}

	public NpcState NpcState
	{
		get { return m_NpcState; }
		set { m_NpcState = value; }
	}

	public string NpcPosition
	{
		get { return m_NpcPosition; }
		set { m_NpcPosition = value; }
	}

}
