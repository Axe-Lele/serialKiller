using UnityEngine;
using System.Collections;

public class CaseItem : MonoBehaviour
{
	public int m_Index;
	public UILabel m_Label;

	public GameObject m_LockIcon;

	/// <summary>
	/// non-used
	/// </summary>
	public void Init(int caseIdx)
	{
		m_LockIcon.SetActive(false);

		m_Index = 0;
	}

	public void SetLabel(int stage)
	{
		m_Label.text = Localization.Get(string.Format("Criminal_S00_C{0}", m_Index));
	}

	public void OnClicked()
	{
		DetectiveManager.instance.SetCaseIndex(m_Index);
	}
}
