using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDialogManager : DialogManager
{
    public string m_CityIndex = string.Empty;

    public void StartDialogInWorld(DialogType type, string target, string index)
    {
        m_CityIndex = index;
        base.StartDialog(type, target, index);
    }

    protected override void Close()
	{
		base.Close();

		m_DialogPopup.SetActive(false);
    }

    protected override void TypingEnd()
    {
        if (m_IsTypingFlag)
        {
            StopAllCoroutines();

			m_NowTextLabel.text = m_CompleteText;

            m_IsTypingFlag = false;
        }
        else
        {
            if (m_TotalBlockCount - 1 > m_BlockIndex)
			{
				SoundManager.instance.changeSFXVolume(1.0f);
				SoundManager.instance.PlaySFX("dialog_jump");

				NextTyping();
			}
            else
            {
                switch (m_Type)
                {
                    // 입장 질문
                    case DialogType.Dialog:
                        SelectionManager.instance.Show(m_CityIndex);
                        break;

                    // 선택 후 비행기 이동
                    case DialogType.End:
                        m_DialogPopup.SetActive(false);
                        WorldManager.instance.GoFly();
                        break;

                    // 미사용 목록
                    case DialogType.Start:
                        break;
                    case DialogType.Selection:
                        break;
                    case DialogType.Suggest:
                        break;
                    case DialogType.EventDialog:
                        break;
                    case DialogType.CompulsionSelection:
                        break;
                    case DialogType.InNote:
                        break;
                    case DialogType.ForcedEnd:
                        break;
                    default:
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
