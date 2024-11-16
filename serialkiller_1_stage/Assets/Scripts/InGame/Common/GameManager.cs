using UnityEngine;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using DetectiveBoard;

public class GameManager : Singleton<GameManager>
{
	/// <summary>
	/// //////////////////////// 특성에 의해 변하긴 하지만 모든 스테이지가 같은 값들
	public int m_FullStamina = 780; // 다음 날로 넘어가면 가득 채워질 행동력
	public int m_RemainderTimeCheckLimit = 360; // 잉여 행동력을 체크하는 리미트
	public int m_RemainderTimeCheckSum = 60; // 잉여 행동력을 더할건데 그에 대한 최소 리미트
	private int m_StartTime = 540; // 다음 날로 넘어갈 시 시작하는 시간. 분 단위.

	/// </summary>
	public InGameDialogManager m_DialogManager;
	public SystemMessageManager m_SystemMessageManager;
	public LoadingManager m_LoadingManager;

	//// 스테이지마다 다른 변수들
	// 인 게임 내 날짜
	private int m_InitYear;
	private int m_InitMonth;
	private int m_InitDay;

	private int m_Year;
	private int m_Month;
	private int m_Day;

	public Camera m_GameCamera;
	private float m_CameraWheelFactor = 2f;
	private const float m_CameraMinSize = 1f;
	private const float m_CameraMaxSize = 2f;

	public int m_CaseCount;// 용의자 별 일으킬 사건 수
	public int m_NumberOfEventsAssinged; // 용의자 별 배정된 사건 수

	public int m_SuspectCount;// 용의자 수
	public int[] m_PowerfulSuspect;// 유력 용의자

	public string m_DialogTargetCode; // 현재 대화하는 대상의 이름. 만약에 한 번 대화할때 대상이 2명 이상일 경우 수정해야함.
	public string m_DialogState; //대화 모드 Start, Select, End로 구분 

	public int[] m_NeedStamina;



	public Font SelectionReadFont; // 이전에 선택한 적이 있던 선택지의 경우 폰트를 변경하여 읽었다는 것을 알려줬었음
	public Font SelectionUnreadFont;



	private string m_NowDate; //현재 날짜
	private DateTime m_NowDateTime;
	private int m_NowTime; // 현재 시간

	public TextAsset LocalizationText;
	public TextAsset StageDataText;
	private JSONNode node;

	private bool IsMouseDragMode = false;

	public NoteMode m_NoteMode;

	private Vector3 m_NowMousePosition;
	private Vector3 m_PrevMousePosition;
	private float m_DragSpeed = 10f;
	//    public bool m_IsSuggest;

	public bool m_IsFirst = true;

	private void Awake()
	{
		if (PlayerPrefs.GetString("Language") == "한국어")
		{
			print("Localization Korean");
		}
		else
		{
			PlayerPrefs.SetString("Language", "한국어");
			print("not");
		}
		node = JSONNode.Parse(StageDataText.text);
		Localization.LoadCSV(LocalizationText, false);
	}

	// 플레이스 > 이벤트
	private IEnumerator Start()
	{
		DataLoad();
		m_LoadingManager.LoadingComplete();
		yield return new WaitForSeconds(0.1f);

		// 범인 세팅
		// 유력 용의자가 있고 유력 용의자가 범인인 스테이지를 클리어하지 않았을 경우
		if (m_PowerfulSuspect.Length > 0)
		{
			Debug.Log("0");
            StageDataManager.instance.m_CriminalCode = DetectiveManager.instance.m_CaseIndex;
            //StageDataManager.instance.m_CriminalCode = 1;
            //StageDataManager.instance.m_CriminalCode = node["m_PowerfulSuspect"][UnityEngine.Random.Range(0, m_PowerfulSuspect.Length)].AsInt;
        }
		else
		{
			Debug.Log("1");
			StageDataManager.instance.m_CriminalCode = DetectiveManager.instance.m_CaseIndex;
            //StageDataManager.instance.m_CriminalCode = 1;
            //StageDataManager.instance.m_CriminalCode = /*UnityEngine.Random.Range(0, m_SuspectCount)*/0;
        }

		InGameUIManager.instance.ControlMainUIBar();
		StageDataManager.instance.LoadGameData();
		PlaceDataManager.instance.LoadPlaceData();
		EventDataManager.instance.LoadEventData();
		NpcDataManager.instance.LoadNpcData();
		NewsDataManager.instance.LoadNewsData();
		SelectionDataManager.instance.LoadSelectionData();
		EvidenceDataManager.instance.LoadEvidenceData();
		DialogDataManager.instance.LoadDialogData();
		LaboratoryDataManager.instance.LoadLaboratoryData();

		DateSetting();
		PropertySetting();

		//bool _isFirst = (StageDataManager.instance.m_CriminalCode == -1);
		if (m_IsFirst)
			DataInitialize();
		PlaceManager.instance.DataInitialize();
		WarrantManager.instance.DataInitialize();

		yield return new WaitForSeconds(0.1f);

		print("Start Day : " + StageDataManager.instance.m_PastDay);

		NpcDataManager.instance.Init();
		NpcDataManager.instance.ActSetting();
		NpcDataManager.instance.ActNpc();

		EventDataManager.instance.StartEvent(StageDataManager.instance.m_PastDay);

		if (m_IsFirst)
			EventManager.instance.SetStartEvent();


		string bgm = string.Format("stage{0}", DetectiveManager.instance.m_StageIndex.ToString());
		Debug.Log(bgm);
		SoundManager.instance.ChangeBGM(bgm, false);
		SoundManager.instance.changeBGMVolume(0.5f);
		//m_DialogManager.StartDialogInGame(DialogType.Start, "Suspect0", "0");
	}

	private void Update()
	{
		/* if (IsMouseDragMode)
		 {
				 m_NowMousePosition = Input.mousePosition;
			//   print("prev : " + m_PrevMousePosition + " / now : " + m_NowMousePosition);
				 Vector3 pos = (m_PrevMousePosition - m_NowMousePosition).normalized;
				// print("pos : " + pos);
					Vector3 move = new Vector3(m_GameCamera.transform.localPosition.x  + (pos.x * m_DragSpeed), m_GameCamera.transform.localPosition.y + (pos.y * m_DragSpeed), 0f);

				 m_GameCamera.transform.localPosition = move;
		 }*/
	}

	private void DataLoad()
	{
		m_InitYear = m_Year = node["m_Year"].AsInt;
		m_InitMonth = m_Month = node["m_Month"].AsInt;
		m_InitDay = m_Day = node["m_Day"].AsInt;
		m_SuspectCount = node["m_SuspectCount"].AsInt;

		m_PowerfulSuspect = new int[node["m_PowerfulSuspect"].Count];
		for (int i = 0; i < m_PowerfulSuspect.Length; i++)
		{
			m_PowerfulSuspect[i] = node["m_PowerfulSuspect"][i].AsInt;
		}


	}

	private void DataInitialize()
	{
		m_NumberOfEventsAssinged = node["m_Suspect"][StageDataManager.instance.m_CriminalCode.ToString()]["m_NumberOfEventsAssinged"].AsInt;
		m_CaseCount = node["m_Suspect"][StageDataManager.instance.m_CriminalCode.ToString()]["m_CaseCount"].AsInt;
		print("criminal : " + StageDataManager.instance.m_CriminalCode);
		// 사건 세팅
		List<int> m_CaseList = new List<int>();

		for (int i = 0; i < m_NumberOfEventsAssinged; i++)
		{
			m_CaseList.Add(i);
		}
		for (int i = 0; i < m_CaseCount; i++)
		{
			int value = -1;
			// 랜덤 이벤트
			if (CaseManager.instance.ReturnOrder(StageDataManager.instance.m_CriminalCode.ToString(), i.ToString()) == "random")
			{
				value = UnityEngine.Random.Range(0, m_CaseList.Count);
				CaseManager.instance.SetItem(i, m_CaseList[value].ToString());
				//StageDataManager.instance.m_CaseList.Add(m_CaseList[value].ToString());
				m_CaseList.RemoveAt(value);
			}
			// 메인 이벤트
			else
			{
				value = int.Parse(CaseManager.instance.ReturnOrder(StageDataManager.instance.m_CriminalCode.ToString(), i.ToString()));
				//  print("value : " + value + " / m_CaseList : " + m_CaseList.Count);
				/*for (int k = 0; k < m_CaseList.Count; k++)
				{
						if (m_CaseList[k] == value)
						{
								CaseManager.instance.SetItem(i, m_CaseList[value].ToString());
								//StageDataManager.instance.m_CaseList.Add(m_CaseList[k].ToString());
						}
				}*/
				//print("value : " + value + " / i : " + i);
				CaseManager.instance.SetItem(i, value.ToString());

				for (int k = 0; k < m_CaseList.Count; k++)
				{
					if (m_CaseList[k] == value)
					{
						m_CaseList.RemoveAt(k);
						break;
					}
				}

			}
			//    CaseManager.instance.SetItem(StageDataManager.instance.m_CriminalCode.ToString(), StageDataManager.instance.m_CaseList[i].ToString());

		}

		//CaseManager.instance.OpenCase(0);
		m_CaseList.Clear();
		m_CaseList = null;
		//PlaceDataManager.instance.ControlPlace(PlaceType.Case, StageDataManager.instance.m_CaseDataItemList[0].CaseLocation, true);
		//PlaceDataManager.instance.ControlPlace(PlaceType.Case, 0, true);


	}

	private void DateSetting()
	{
		// 로드 후 게임 내에 사용할 값들을 세팅
		//m_NowDate = m_Year + " / " + ((m_Month < 10) ? "0" + m_Month : m_Month.ToString()) + " / " + ((m_Day < 10) ? "0" + m_Day : m_Day.ToString());
		m_NowDate = m_Year + "/" + m_Month + "/" + m_Day;
		m_NowDateTime = Convert.ToDateTime(m_NowDate);
		//print("m_NowDate : " + m_NowDate);

		//m_NowDateTime = DateTime.ParseExact(m_NowDate, "yyyy / MM / dd", null);
		m_NowDateTime = m_NowDateTime.AddDays(StageDataManager.instance.m_PastDay);
		// 시작 시간 세팅
		m_NowTime = m_StartTime;
		print("m_StartTime : " + m_StartTime);

		// 행동에 필요한 행동력 세팅
		m_NeedStamina = new int[Enum.GetValues(typeof(UserActionType)).Length];
		for (int i = 0; i < Enum.GetValues(typeof(UserActionType)).Length; i++)
		{
			m_NeedStamina[i] = 60;
		}
		InGameUIManager.instance.MainUIInitialize(m_NowDateTime.Month, m_NowDateTime.Day, m_StartTime);
	}

	private void PropertySetting()
	{

	}

	public void Rest()
	{
		print("rest");
		// 남은 시간이 너무 많이 남아있으면 체크
		if (m_RemainderTimeCheckLimit <= StageDataManager.instance.m_Stamina)
		{
			StageDataManager.instance.m_RemainCheckCount++;
		}
		else if (m_RemainderTimeCheckSum <= StageDataManager.instance.m_Stamina)
		{
			StageDataManager.instance.m_RemainStamina += StageDataManager.instance.m_Stamina;
		}

		LaboratoryManager.instance.CheckAnalyzed(StageDataManager.instance.m_Stamina);
		LaboratoryManager.instance.CheckMatched(StageDataManager.instance.m_Stamina);

		// 행동력을 가득 채워줌
		StageDataManager.instance.m_Stamina = m_FullStamina;
		// 다음 날로 넘기고 시간과 날짜를 갱신
		StageDataManager.instance.m_PastDay++;
		m_NowDateTime = m_NowDateTime.AddDays(1);
		InGameUIManager.instance.RefreshDate(m_NowDateTime.Month, m_NowDateTime.Day);
		m_NowTime = m_StartTime;
		InGameUIManager.instance.RefreshTime(m_NowTime);
		EventDataManager.instance.StartEvent(StageDataManager.instance.m_PastDay);


		print("setting B : " + NpcDataManager.instance.m_NpcItemList[17].m_NpcEventCode);
		NpcDataManager.instance.ActSetting();
		NpcDataManager.instance.ActNpc();
		print("setting A : " + NpcDataManager.instance.m_NpcItemList[17].m_NpcEventCode);

		//PlayDataManager.instance.Save();

		//*나중에 세이브 필요할 때 이 부분만 주석 처리 해제
		StageDataManager.instance.Save();
		NewsDataManager.instance.Save();
		SelectionDataManager.instance.Save();
		EvidenceDataManager.instance.Save();
		PlaceDataManager.instance.Save();
		EventDataManager.instance.Save();
		print("save B : " + NpcDataManager.instance.m_NpcItemList[17].m_NpcEventCode);
		NpcDataManager.instance.Save();
		print("save A : " + NpcDataManager.instance.m_NpcItemList[17].m_NpcEventCode);
		DialogDataManager.instance.Save();
		LaboratoryDataManager.instance.Save();

		//EventDataManager.instance.CloseEvent(StageDataManager.instance.m_PastDay);d

		DetectiveDataManager.instance.Save();
	}

	public void UserAction(UserActionType t)
	{
		StageDataManager.instance.m_Stamina -= m_NeedStamina[(int)t];
		m_NowTime += m_NeedStamina[(int)t];
		InGameUIManager.instance.RefreshTime(m_NowTime);

		// npc 행동
		NpcDataManager.instance.ActNpc();
		// 연구소 분석, 매칭에 대한 시간경과
		LaboratoryManager.instance.CheckAnalyzed(m_NeedStamina[(int)t]);
		LaboratoryManager.instance.CheckMatched(m_NeedStamina[(int)t]);

		if (StageDataManager.instance.m_Stamina < 0)
		{
			Rest();
		}
	}

	// 시작 일로부터 며칠이 지나면 날짜가 어떻게 되는지를 리턴
	public string ReturnNowDate(int pTime)
	{
		string temp = m_InitYear + "/" + m_InitMonth + "/" + m_InitDay;
		DateTime dt = Convert.ToDateTime(temp);
		dt = dt.AddDays(pTime);
		return dt.Year + "/" + dt.Month + "/" + dt.Day;
	}

	public DateTime ReturnNowDate()
	{
		return m_NowDateTime;
	}

	public string ReturnDayOfWeek()
	{
		return m_NowDateTime.DayOfWeek.ToString();
	}

	public int ReturnNowTime()
	{
		return m_NowTime;
	}

	public void OnMouseDragMode()
	{
		m_PrevMousePosition = Input.mousePosition;
		IsMouseDragMode = true;
	}

	public void OffMouseDragMode()
	{
		IsMouseDragMode = false;
	}

	public void ControlCameraZoom(float f)
	{
		m_CameraWheelFactor += f;

		if (m_CameraWheelFactor <= m_CameraMinSize)
			m_CameraWheelFactor = m_CameraMinSize;
		else if (m_CameraWheelFactor >= m_CameraMaxSize)
			m_CameraWheelFactor = m_CameraMaxSize;

		m_GameCamera.orthographicSize = Mathf.Clamp(m_CameraWheelFactor, m_CameraMinSize, m_CameraMaxSize);
	}

	public void ExitGame()
	{
		GlobalMethod.instance.GoToWorld();
	}

	public float ReturnCameraFactor()
	{ return m_CameraWheelFactor; }
}
