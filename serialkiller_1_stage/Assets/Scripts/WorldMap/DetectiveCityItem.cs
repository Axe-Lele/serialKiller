using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectiveCityItem : MonoBehaviour
{
	public int m_CityIndex;

	public UISprite m_Illust;
	public GameObject m_LockIcon;
	public UILabel m_CityName, m_Complish;

	private string complish;

	private void Awake()
	{
		m_CityIndex = name.Split('(')[1][0] - '0';

		complish = Localization.Get("Text_Complish");
	}

	public void Init()
	{
		gameObject.SetActive(true);

		if (StageInfoManager.instance.GetCityIsOpen(m_CityIndex) == false)
			m_LockIcon.SetActive(true);
		else
		{
			m_LockIcon.SetActive(false);
		}

		m_CityName.text = Localization.Get(string.Format("Text_World_Title_{0:00}", m_CityIndex));
		m_Complish.text = string.Format("{0} {1}%", complish, GetComplish());
	}

	public int GetComplish()
	{
		return 0;
	}

	public void OnClick()
	{
		if (StageInfoManager.instance.GetCityIsOpen(m_CityIndex) == false)
			return;
		else
			DetectiveManager.instance.ShowCaseBoard(m_CityIndex);
	}
}
