using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleManager : MonoBehaviour {

    private bool b = false;

    public TweenPosition m_LeftSideTP;
    public TweenPosition m_RightSideTP;
    public TweenPosition m_TextTP;

    private bool m_LefSideMoveFlag = false;
    private bool m_RightideMoveFlag = false;
    private bool m_TextMoveFlag = false;

    private bool m_IsClick = false;

    public void MoveLeftSide()
    {
        if (m_LefSideMoveFlag)
            return;

        m_LefSideMoveFlag = true;

        m_LeftSideTP.from = new Vector3(-9.17f, 0.27f, 16.3f);
        m_LeftSideTP.to = new Vector3(-8.53f, 0.27f, 16.3f);
        m_LeftSideTP.ResetToBeginning();
        m_LeftSideTP.duration = 3f;
        m_LeftSideTP.enabled = true;
    }

    public void MoveRightSide()
    {
        if (m_RightideMoveFlag)
            return;

        m_RightideMoveFlag = true;

        m_RightSideTP.from = new Vector3(9.16f, 0.27f, 16.3f);
        m_RightSideTP.to = new Vector3(8.59f, 0.27f, 16.3f);
        m_RightSideTP.ResetToBeginning();
        m_RightSideTP.duration = 3f;
        m_RightSideTP.enabled = true;
    }

    public void MoveText()
    {

        if (m_TextMoveFlag)
        {
            return; 
        }
        
        m_TextTP.from = new Vector3(0f, 2.05f, 11.74f);
        m_TextTP.to = new Vector3(0f, 1.37f, 11.74f);
        m_TextTP.ResetToBeginning();
        m_TextTP.delay = 0f;
        m_TextTP.duration = 0.5f;

            //m_TextTP.callWhenFinished = "OnFlag";
        m_TextTP.enabled = true;
        m_TextMoveFlag = true;
        
    }
    
    public void OnClick()
    {
        if (m_IsClick == true)
        {
            return;
        }

        if (m_TextMoveFlag == false)
        {
            return;
        }

        m_IsClick = true;
        SceneManager.LoadScene(1);

    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            OnClick();
        }

    }




}
