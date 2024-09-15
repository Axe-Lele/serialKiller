using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratoryItem : MonoBehaviour
{
	[Header("Param")]
	public LaboratoryType m_ModeType;
	public int m_Index;
	public string m_Mode;

	[Header("UI")]
	private LaboratoryManager _labotoryManager;
	public LaboratoryManager LabotoryManager
	{
		get
		{
			if (_labotoryManager == null)
				_labotoryManager = LaboratoryManager.instance;

			return _labotoryManager;
		}
		private set { }
	}

	public UILabel m_ButtonLabel;
	public UISprite m_ButtonSprite;

	public UISprite m_Icon;
	public UILabel m_IconLabel;
	public UILabel m_Desc;

	//public UILabel m_ItemRemainTimeLabel; // Cargold : UI 레이아웃 변경에 따른 미사용
	//public UISprite m_RemoveSprite; // Cargold : UI 레이아웃 변경에 따른 미사용

	private string m_ItemCode = "-1";
	private int m_CoolTime;

	[Header("Const String")]
	public string SpriteName_ButtonSelected;
	public string SpriteName_ButtonUnselected;
	public string SpriteName_ButtonAnalyzing;
	private readonly static string Mode_Anlyzed = "AnalyzedTime";
	private readonly static string Mode_Matched = "MatchedTime";

	private static string StageName
	{
		get
		{
			if (PlayDataManager.instance == null)
				return "!Error!";
			return PlayDataManager.instance.m_StageName;
		}
	}

	private static string CriminalCode
	{
		get
		{
			if (StageDataManager.instance == null)
				return "!Error!";
			return StageDataManager.instance.m_CriminalCode.ToString();
		}
	}

	private readonly static string IconSpriteNameTemplet = "Evidence_{0}_{1}";
	private static string GetIconSpriteName(LaboratoryItem item)
	{
		return string.Format(IconSpriteNameTemplet, StageName, item.m_ItemCode);
	}

	private readonly static string ItemName = "Evidence_{0}_{1}_{2}_Title";
	private static string GetItemName(LaboratoryItem item)
	{
		return Localization.Get(string.Format(ItemName, StageName, CriminalCode, item.m_ItemCode));
	}

	private readonly static string ItemDesc = "Evidence_{0}_{1}_{2}_Content";
	private static string GetItemDesc(LaboratoryItem item)
	{
		return Localization.Get(string.Format(ItemDesc, StageName, CriminalCode, item.m_ItemCode));
	}

	private void Awake()
	{
		if (m_ModeType == LaboratoryType.AnalyzedReverve || m_ModeType == LaboratoryType.Analyzing)
		{
			//m_ItemSprite.spriteName = "-1";
		}
		else if (m_ModeType == LaboratoryType.Matching)
		{
			//m_ItemSprite.spriteName = "-1";
		}

		if (m_ModeType == LaboratoryType.Analyzing || m_ModeType == LaboratoryType.CanAnalyzed || m_ModeType == LaboratoryType.AnalyzedReverve)
		{
			m_Mode = Mode_Anlyzed;
		}
		else
		{
			m_Mode = Mode_Matched;
		}
	}

	public void Reset()
	{
		m_ItemCode = "-1";
		if (m_ModeType == LaboratoryType.Analyzing
				|| m_ModeType == LaboratoryType.CanAnalyzed
				|| m_ModeType == LaboratoryType.AnalyzedReverve)
		{
			LaboratoryDataManager.instance.SetAnalyzedIndexList(m_Index, string.Empty);
		}
		else
		{
			LaboratoryDataManager.instance.SetMatchedIndexList(m_Index, string.Empty);
		}
		m_CoolTime = 0;

		if (m_ButtonLabel != null)
			m_ButtonLabel.text = string.Empty;
		if (m_ButtonSprite != null)
			m_ButtonSprite.spriteName = string.Empty;
		if (m_Icon != null)
			m_Icon.spriteName = string.Empty;
		if (m_IconLabel != null)
			m_IconLabel.text = string.Empty;
		if (m_Desc != null)
			m_Desc.text = string.Empty;

		//if (m_RemoveSprite != null)
		//{
		//    m_RemoveSprite.gameObject.SetActive(false);
		//}
	}

	public void ShowRemoveSprite()
	{
		//m_RemoveSprite.gameObject.SetActive(true);
	}

	public void Setting(string index)
	{
		if (m_ModeType == LaboratoryType.AnalyzedReverve)
			print("2 : " + index);
		else if (m_ModeType == LaboratoryType.Analyzing)
			print("1 : " + index);
		m_ItemCode = index;
		m_CoolTime = EvidenceDataManager.instance.ReturnCoolTime(m_ItemCode, m_Mode);
		
		switch (m_ModeType)
		{
			case LaboratoryType.Analyzing:
				break;

			case LaboratoryType.AnalyzedReverve:
				int h = m_CoolTime / 60;
				int m = m_CoolTime % 60;

				m_Icon.spriteName = GetIconSpriteName(this);
				m_IconLabel.text = string.Format("{0:00}:{1:00}", h, m);
				break;

			case LaboratoryType.CanAnalyzed:
				m_ButtonLabel.text = GetItemName(this);
				break;

			case LaboratoryType.Matching:
				break;

			case LaboratoryType.CanMatched:
				m_ButtonLabel.text = GetItemName(this);
				break;

			default:
				break;
		}

		//if (m_ButtonSprite != null)
		//	m_ButtonSprite.spriteName = "Evidence_" + PlayDataManager.instance.m_StageName + "_" + index;

		//if (m_ItemRemainTimeLabel != null)
		//{
		//    m_ItemRemainTimeLabel.text = m_CoolTime + "M";
		//}

		if (!IsLaboratoryItem()) return;

		
	}

	public void Selected(bool isSelected)
	{
		if (!IsLaboratoryItem()) return;

		if (m_ButtonSprite.spriteName == SpriteName_ButtonAnalyzing)
		{
			return;
		}

		m_ButtonLabel.text = GetItemName(this);
	}

	/// <summary>
	/// 아이템 버튼을 '분석중'으로 변경
	/// </summary>
	public void OnAnalyzed()
	{
		if (!IsLaboratoryItem()) return;

		m_ButtonSprite.spriteName = SpriteName_ButtonAnalyzing;
	}

	public void ControlIcon(bool isAnalyzing)
	{
		if (!isAnalyzing)
		{
			m_Icon.spriteName = string.Empty;
			m_Icon.enabled = false;

			m_IconLabel.text = string.Empty;
		}
	}

	public void ShowInfomation(LaboratoryItem targetItem)
	{
		if (m_Icon == null)
		{
			print("!! This is NOT Desc Laboratory Item. !!");
			return;
		}

		m_ItemCode = targetItem.GetItemCode();

		m_Icon.spriteName = GetIconSpriteName(targetItem);
		m_IconLabel.text = GetItemName(targetItem);
		m_Desc.text = GetItemDesc(targetItem);
	}

	private bool IsLaboratoryItem()
	{
		if (m_ButtonSprite == null)
		{
			print("!! This is NOT Laboratory Item. !!");
			return false;
		}
		return true;
	}

	public void OnClicked()
	{
		LabotoryManager.SelectedItem(this);
	}

	public void OnClicked_Match()
	{

	}

	public void OnClickEvent()
	{
		if (m_ModeType == LaboratoryType.Analyzing
				|| m_ModeType == LaboratoryType.CanAnalyzed
				|| m_ModeType == LaboratoryType.AnalyzedReverve)
			LaboratoryManager.instance.OnSelected_Func(m_ItemCode);

		switch (m_ModeType)
		{
			case LaboratoryType.Analyzing:
				// 분석 중인 아이라서 클릭해도 이벤트가 없다.
				print("분석 중이니라");
				break;
			case LaboratoryType.CanAnalyzed:
				// 분석이 가능한 아이템을 예약 리스트에 올린다.
				print("m_ItemCode ; " + m_ItemCode);
				LaboratoryManager.instance.SetAnalyzedItem(m_ItemCode);
				break;
			case LaboratoryType.AnalyzedReverve:
				// 분석 예약 리스트에 있는 애니까 애를 취소시킨다.
				LaboratoryManager.instance.RemoveAnalyzedReservationItemList(m_Index);
				break;
			case LaboratoryType.Matching:
				// 매칭 중일 때는 빼서는 안 되고 매칭 시작 전에는 뺄 수 있어야 함 
				LaboratoryManager.instance.RemoveMatchedItem(m_Index);
				print("매칭 중이니라");
				break;
			case LaboratoryType.CanMatched:
				// 매칭이 가능한 아이템을 매칭 리스트에 올린다.
				LaboratoryManager.instance.InputMatchedItem(m_ItemCode);
				break;
		}
	}

	public string GetItemCode()
	{
		return m_ItemCode;
	}

}
