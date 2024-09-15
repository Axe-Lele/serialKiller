using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTextItem : MonoBehaviour {

    private float m_DelayTime = 2f;

    public UILabel m_Label;
    public UISprite m_BG;

    public TweenAlpha m_TALAbel;
    public TweenAlpha m_TABG;
    public TweenPosition m_TP;

    public int m_Index;

    private bool m_Flag = false;

    public bool ReturnFlag()
    {
        return m_Flag;
    }

    public void SystemTextShow(string str)
    {
        m_Flag = true;
        
        m_Label.text = str;
        m_BG.width = (int)(m_Label.width * 1.2f);
        m_BG.height = (int)(m_Label.height * 1.2f);

        StartCoroutine(SystemTextHide());
    }

    private IEnumerator SystemTextHide()
    {
        yield return new WaitForSeconds(1f);

        m_TALAbel.from = m_TABG.from = 1f;
        m_TALAbel.to = m_TABG.to = 0f;
        m_TALAbel.duration = m_TABG.duration = m_DelayTime;
        m_TALAbel.ResetToBeginning();
        m_TABG.ResetToBeginning();
        m_TALAbel.enabled = true;
        m_TABG.enabled = true;

        yield return new WaitForSeconds(m_DelayTime);

        m_Flag = false;
        m_Label.text = "";
        transform.localPosition = Vector2.zero;
        gameObject.SetActive(false);
    }

    public void SystemTextChangePosition()
    {
        m_TP.to = new Vector2(0f, transform.localPosition.y + 100f);
        m_TP.from = transform.localPosition;
        m_TP.ResetToBeginning();
        m_TP.enabled = true;
    }
}
