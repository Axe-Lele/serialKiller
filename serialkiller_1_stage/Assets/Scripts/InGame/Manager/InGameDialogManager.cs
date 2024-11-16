using UnityEngine;
using System.Collections;

public class InGameDialogManager : DialogManager
{
	public void StartDialogInGame(DialogType type, string target, string index, string place)
	{
		int atlasIndex = -1;
		string spriteIndex = string.Empty;
		
		GlobalValue.instance.m_IsNpcForcedMoveFlag = false;

		if (place == "Selection" || place == "Prev")
		{
			base.StartDialog(type, target, index, atlasIndex, "Prev");
		}
		else
		{
			Debug.Log(place);
			m_PlaceBackground = place;
			atlasIndex = PlaceDataManager.instance.GetAtlasIndex(place);
			spriteIndex = PlaceDataManager.instance.GetSpriteIndex(place);

			base.StartDialog(type, target, index, atlasIndex, spriteIndex);
		}

	}

	public void StartDialogInGame(DialogType type, string target, string index)
	{
		base.StartDialog(type, target, index);
	}

	protected override void Close()
	{
		base.Close();

		if (ReturnDialogType() == DialogType.Selection || ReturnDialogType() == DialogType.Start)
		{
			SelectionManager.instance.CloseSelectionPopup();
			StartDialogInGame(DialogType.End, m_Target, "0", "Prev");
		}
		else if (ReturnDialogType() == DialogType.Dialog || ReturnDialogType() == DialogType.EventDialog)
		{
			m_DialogPopup.SetActive(false);
			EventManager.instance.StartDelayEvent();
		}
		else if (ReturnDialogType() == DialogType.InNote)
		{
			m_DialogPopup.SetActive(false);
		}
		else
		{
			print("현재 대화 타입은 " + GameManager.instance.m_DialogManager.ReturnDialogType().ToString());
		}
	}


	public void CheckDialog(string str)
	{
		//print("b : " + Localization.LoadDictionary(str));
	}

	/// <summary>
	/// 타이핑 중이라면 타이핑을 즉시 완료.
	/// 
	/// </summary>
	protected override void TypingEnd()
	{
		base.TypingEnd();

		if (m_IsTypingFlag)
		{
			StopAllCoroutines();

			m_NowTextLabel.text = m_CompleteText;

			m_IsTypingFlag = false;
		}
		else
		{
			// 출력해야할 대화가 남아있음
			if (m_TotalBlockCount - 1 > m_BlockIndex)
			{
				SoundManager.instance.changeSFXVolume(1.0f);
				SoundManager.instance.PlaySFX("dialog_jump");

				//Invoke("NextTyping", 1);
				NextTyping();
			}

			// 출력해
			else
			{
				if(m_PlaceBackground.Length != 0)
				{
					int atlasIndex = PlaceDataManager.instance.GetAtlasIndex(m_PlaceBackground);
					m_BG.atlas = BackgroundManager.instance.GetBackgroundAtlas(atlasIndex);
					string spriteIndex = PlaceDataManager.instance.GetSpriteIndex(m_PlaceBackground);
					m_BG.spriteName = string.Format("BG_{0}", spriteIndex);
				}
				switch (m_Type)
				{
					case DialogType.Start:
						m_CloseBtn.gameObject.SetActive(true);
						SelectionManager.instance.Show(m_Target);
						print("Get Selection");
						break;

					case DialogType.Selection:
						// 여기서 NPC의 스케쥴에 따라서 시간이 지나면 대화를 못 하게 한다.
						//m_CloseBtn.gameObject.SetActive(true);
						EventManager.instance.SetEvent("Dialog", m_Target, StageDataManager.instance.m_CriminalCode.ToString(), m_DialogIndex);
						GameManager.instance.UserAction(UserActionType.Dialog);
						DialogDataManager.instance.InputDialog(m_Target, m_DialogIndex, m_PlaceBackground);

						if (GlobalValue.instance.m_IsNpcForcedMoveFlag == true)
						{
							StartDialog(DialogType.ForcedEnd, m_Target, "ForcedEnd");
							GlobalValue.instance.m_IsNpcForcedMoveFlag = false;
							//print("종료 대화");
						}
						else
						{
							SelectionManager.instance.Show(m_Target);
							//print("GET SELECTIOn");
						}
						break;

					case DialogType.End:
						print("Dialog End");
						m_BG.spriteName = string.Empty;
						EventManager.instance.DialogEndEvent(m_Target);

						m_DialogPopup.SetActive(false);
						// ("EndDialog", m_Target, StageDataManager.instance.m_CriminalCode.ToString(), m_DialogIndex);
						break;

					case DialogType.ForcedEnd:
						m_BG.spriteName = string.Empty;
						EventManager.instance.DialogEndEvent(m_Target);
						m_DialogPopup.SetActive(false);
						break;

					case DialogType.Suggest:
						GameManager.instance.UserAction(UserActionType.Suggest);
						EventManager.instance.SetEvent("Dialog", m_Target, StageDataManager.instance.m_CriminalCode.ToString(), m_DialogIndex);
						m_DialogPopup.SetActive(false);
						break;

					case DialogType.Dialog:
						Close();

						EventManager.instance.SetEvent("Dialog", m_Target, StageDataManager.instance.m_CriminalCode.ToString(), m_DialogIndex);
						break;

					case DialogType.EventDialog:
						Close();

						EventManager.instance.SetEvent("Dialog", m_Target, StageDataManager.instance.m_CriminalCode.ToString(), m_DialogIndex);
						break;

					case DialogType.CompulsionSelection:
						CompulsionSelectionManager.instance.Show(m_Target, m_DialogIndex);
						break;

					case DialogType.InNote:
						//EventManager.instance.SetEvent("Dialog", m_Target, StageDataManager.instance.m_CriminalCode.ToString(), m_DialogIndex);
						Close();
						break;
				}
			}
		}
		/*if (SuggestManager.instance.m_IsCheckSuggest)
		{
				if (m_IsTypingFlag)
				{
						StopAllCoroutines();

						m_TextLabel.text = m_CompleteText;

						m_IsTypingFlag = false;
				}
				else
				{
						if (m_TotalBlockCount - 1 > m_BlockIndex)
						{
								m_BlockIndex++;
								Set();
								//StartCoroutine(SetDelay());
						}
						else
						{
								GameManager.instance.UserAction(UserActionType.Suggest);
								if (SuggestManager.instance.m_IsCheckSuggest == true)
										SuggestManager.instance.m_IsCheckSuggest = false;

								m_DialogPopup.SetActive(false);
								print("대화 종료");
								//SetSection(m_DialogIndex);
								 //StartCoroutine(DelayClose());
						}
				}
		}
		else
		{
				if (m_IsTypingFlag)
				{
						StopAllCoroutines();

						m_TextLabel.text = m_CompleteText;

						m_IsTypingFlag = false;
				}
				else
				{
						if (m_TotalBlockCount - 1 > m_BlockIndex)
						{
								m_BlockIndex++;
								Set();
						}
						else
						{
								switch (m_Type)
								{
										case DialogType.Start:
												m_CloseBtn.gameObject.SetActive(true);
												SelectionManager.instance.Show(m_Target);
												print("Get Selection");
												break;
										case DialogType.Selection:
												// 여기서 NPC의 스케쥴에 따라서 시간이 지나면 대화를 못 하게 한다.
												//m_CloseBtn.gameObject.SetActive(true);
												GameManager.instance.UserAction(UserActionType.Dialog);
												SelectionManager.instance.Show(m_Target);
												print("GET SELECTIOn");
												break;
										case DialogType.End:
												print("Dialog End");
												m_DialogPopup.SetActive(false);
												break;
										case DialogType.Suggest:
												GameManager.instance.UserAction(UserActionType.Suggest);
												if (SuggestManager.instance.m_IsCheckSuggest == true)
														SuggestManager.instance.m_IsCheckSuggest = false;

												m_DialogPopup.SetActive(false);
												break;
								}
						}
				}
		}*/
	}
}
