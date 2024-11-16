using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class SuggestManager : Singleton<SuggestManager>
{

	public TextAsset SuggestTextAsset;
	private JSONNode SuggestNode;
	//public bool m_IsCheckSuggest;

	private string m_Mode;
	public string m_Target;
	public string m_Item;

	private void Awake()
	{
		SuggestNode = JSONNode.Parse(SuggestTextAsset.text);
		m_Target = m_Item = "";
	}

	public void SetTarget(string target)
	{
		m_Target = target;
	}

	public void Setting(string mode, string index)
	{
		GlobalValue.instance.m_SelectSuggestItemName = m_Item = index;
		GlobalValue.instance.m_SelectSuggestModeName = m_Mode = mode;
	}

	public void CaseSetting(CaseMode mode, string index)
	{
		if (GlobalValue.instance.m_SelectSuggestItemName == index && GlobalValue.instance.m_SelectSuggestModeName == "Case")
		{
			Suggest();
		}
		else
		{
			NoteManager.instance.SelectReset();
			GlobalValue.instance.m_SelectSuggestItemName = m_Item = index;
			GlobalValue.instance.m_SelectSuggestModeName = m_Mode = "Case";
			GlobalValue.instance.m_SelectSuggestCaseMode = mode;
			print("mode : " + mode + " / target : " + m_Target + " / item : " + m_Item);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="target">NPC</param>
	/// <param name="index">EventCode</param>
	public void ShowSelectionEvent(string target, string index)
	{
		if (index.Length == 0)
		{
			return;
		}

		int count = SuggestNode[target].Count;
		for (int i = 0; i < count; i++)
		{
			if (SuggestNode[target][i]["m_Index"].Value == index)
				continue;

			string m_DialogIndex = SuggestNode[target][i]["m_DialogIndex"];
			// 강제 선택지
			if (SuggestNode[target][i]["m_Type"] != null)
			{
				if (SuggestNode[target][i]["m_Type"].Value != "CompulsionSelection")
				{
					print("Wrong m_Type Keyword !!");
					return;
				}

				string p = "";
				for (int k = 0; k < SuggestNode[target][i]["m_Selection"].Count; k++)
				{
					if (k == SuggestNode[target][i]["m_Selection"].Count - 1)
					{
						p += (SuggestNode[target][i]["m_Selection"][k]);
					}
					else
					{
						p += (SuggestNode[target][i]["m_Selection"][k] + "+");
					}
				}

				string t = "";
				for (int k = 0; k < SuggestNode[target][i]["m_Event"].Count; k++)
				{
					if (k == SuggestNode[target][i]["m_Event"].Count - 1)
					{
						t += (SuggestNode[target][i]["m_Event"][k]);
					}
					else
					{
						t += (SuggestNode[target][i]["m_Event"][k] + "+");
					}
				}
				CompulsionSelectionManager.instance.Setting(p, t);
				GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.CompulsionSelection, target, m_DialogIndex, PlaceManager.instance.ReturnPlace());
			}
			// 선택지
			else
			{
				GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.Suggest, target, m_DialogIndex, PlaceManager.instance.ReturnPlace());
			}
		}
	}

	public void Suggest()
	{
		string item2 = m_Item;
		string target = m_Target;

		GlobalValue.instance.m_SelectSuggestItemName = "";
		GlobalValue.instance.m_SelectSuggestModeName = "";

		if (item2.Length == 0 || target.Length == 0)
		{
			string s = "너흰 아직 준비가 안 되어 있다. 너는 " + target + "에게 " + item2 + "을 주는게 맞느냐";
			SystemTextManager.instance.InputText(s);
			print(s);
			return;
		}

		string item = "";

		switch (m_Mode)
		{
			case "Case":
				item = "Case_" + PlayDataManager.instance.m_StageName + "_" + StageDataManager.instance.m_CriminalCode + "_" + item2;
				break;
			case "Dialog":
				item = "Dialog_" + PlayDataManager.instance.m_StageName + "_" + item2;
				break;
			case "Evidence":
				item = "Evidence_" + PlayDataManager.instance.m_StageName + "_" + item2;
				break;
			case "News":
				item = "Evidence_" + PlayDataManager.instance.m_StageName + "_" + item2;
				break;
		}

		// Evidence_Stage0_Main_A10


		int count = SuggestNode[target].Count;
		bool b = false;
		print("index : " + item + " / count : " + count);
		for (int i = 0; i < count; i++)
		{
			if (SuggestNode[target][i]["m_Index"].Value == item)
			{
				b = true;
				string m_DialogIndex = SuggestNode[target][i]["m_DialogIndex"];
				// 강제 선택지
				if (SuggestNode[target][i]["m_Type"] != null)
				{
					if (SuggestNode[target][i]["m_Type"].Value == "CompulsionSelection")
					{
						string p = "";
						for (int k = 0; k < SuggestNode[target][i]["m_Selection"].Count; k++)
						{
							if (k == SuggestNode[target][i]["m_Selection"].Count - 1)
							{
								p += (SuggestNode[target][i]["m_Selection"][k]);
							}
							else
							{
								p += (SuggestNode[target][i]["m_Selection"][k] + "+");
							}
						}

						string t = "";
						for (int k = 0; k < SuggestNode[target][i]["m_Event"].Count; k++)
						{
							if (k == SuggestNode[target][i]["m_Event"].Count - 1)
							{
								t += (SuggestNode[target][i]["m_Event"][k]);
							}
							else
							{
								t += (SuggestNode[target][i]["m_Event"][k] + "+");
							}
						}
						CompulsionSelectionManager.instance.Setting(p, t);
						GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.CompulsionSelection, target, m_DialogIndex, PlaceManager.instance.ReturnPlace());
					}
				}
				// 일반 선택지
				else
				{
					GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.Suggest, target, m_DialogIndex, PlaceManager.instance.ReturnPlace());
				}

				InGameUIManager.instance.ControlNotePopup();
				break;
			}
		}

		if (b == false)
		{
			GameManager.instance.m_DialogManager.StartDialogInGame(DialogType.Suggest, target, "Nothing", PlaceManager.instance.ReturnPlace());
		}

	}
}
