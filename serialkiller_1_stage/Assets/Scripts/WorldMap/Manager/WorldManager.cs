using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DetectiveBoard;

public class WorldManager : Singleton<WorldManager>
{
	private bool m_LockFlag = false;

	public GameObject[] m_Stage;
	public WorldDialogManager m_DialogManager;

	private string filePath;
	[Header("Localization")]
	public TextAsset LocalizationText;

	[Header("Stage Loader")]
	public List<StageItem> m_Stages = new List<StageItem>();
	public GameObject m_StageRoot;

	public string m_NowSelectCityIndex;
	public string m_NowSelectCaseIndex;

	private void Awake()
	{
		filePath = Application.persistentDataPath + "/" + "data01.bin";

		m_Stages.Clear();
		m_Stages.AddRange(m_StageRoot.GetComponentsInChildren<StageItem>());

		if (PlayerPrefs.GetString("Language") == "한국어")
		{
			print("Localization Korean");
		}
		else
		{
			PlayerPrefs.SetString("Language", "한국어");
			print("not");
		}
		Localization.LoadCSV(LocalizationText, false);
	}


	private void Start()
	{
		UnlockDataManager.instance.Init();

		StageInfoManager.instance.LoadData();
		StageInfoManager.instance.Init();

		WorldUIManager.instance.Init();

		SelectionDataManager.instance.LoadSelectionData();

		//UserDataManager.instance.LoadUserData();// 유저 정보 로드
		//WorldDataManager.instance.LoadWorldData();// 월드맵 정보 로드
		//WorldEventDataManager.instance.LoadEventData();

		//PlayDataManager.instance.LoadPlayData();// 스테이지 플레이 정보 로드 

		NpcDataManager.instance.LoadNpcData(); // 월드맵의 NPC 데이터 로드
		NpcDataManager.instance.Init();
		//NpcDataManager.instance.ActSetting();
		//NpcDataManager.instance.ActNpc();
		//GameStart();

		DetectiveManager.instance.Init();
	}

	public void SetSelectedCity(string index)
	{
		m_NowSelectCityIndex = index;
		DetectiveManager.instance.m_StageIndex = m_NowSelectCityIndex.ToInt();
	}

	public void SetSelectedCase(string index)
	{
		m_NowSelectCaseIndex = index;
		DetectiveManager.instance.m_CaseIndex = m_NowSelectCaseIndex.ToInt();

	}

	public void GoFly()
	{
		SceneManager.LoadScene(string.Format("Stage" + m_NowSelectCityIndex + "_Main"));
	}

	public void MoveScene()
	{
		SceneManager.LoadScene("Stage0_Main");
	}

	public void ClickStage()
	{
		if (m_LockFlag)
			return;

		m_LockFlag = true;

		if (GlobalMethod.instance.ReturnFileExist(filePath) == true)
		{
			WorldUIManager.instance.ControlWarningStageStartPopup();
		}
		else
		{
			ContinueGame();
		}
	}

	public void ControlStage(int index, bool b)
	{
		m_Stage[index].gameObject.SetActive(b);
	}

	public void GameStart()
	{
		if (WorldEventDataManager.instance.ReturnEventData() != null)
		{
			//WorldEventManager.instance.startev
		}
		else
		{
			WorldEventManager.instance.StartEvent();

		}
	}

	public void Save()
	{
		UserDataManager.instance.Save();

		WorldDataManager.instance.Save();
		WorldEventDataManager.instance.Save();
	}

	public void ContinueGame()
	{
		if (WorldUIManager.instance.m_WarningStageStartPopup.activeInHierarchy)
			WorldUIManager.instance.ControlWarningStageStartPopup();

		AirPlaneManager.instance.Move(0, 1);
	}

	public void InitGame()
	{
		GlobalMethod.instance.ResetGame();
		ContinueGame();
	}






	public void ControlAllStageButton(bool isShow)
	{
		for (int i = 0; i < m_Stages.Count; i++)
		{
			m_Stages[i].gameObject.SetActive(isShow);
		}
	}
}
