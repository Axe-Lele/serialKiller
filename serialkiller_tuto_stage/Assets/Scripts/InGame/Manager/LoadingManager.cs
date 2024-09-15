using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : MonoBehaviour {

    public TweenAlpha ta;
    public UILabel m_TitleLabel;

    public void LoadingComplete()
    {
        int ran = Random.Range(0, 6);

        m_TitleLabel.text = Localization.Get("LoadingText_" + ran);

        ta.enabled = true;
    }
}
