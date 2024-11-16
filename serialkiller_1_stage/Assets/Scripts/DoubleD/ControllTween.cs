using UnityEngine;
using System.Collections;

public class ControllTween : MonoBehaviour
{
	[GetComponent]
	public UITweener m_TweenPos;
	public bool m_IsReset = false;

	public void ResetTweenPos()
	{
		if (m_IsReset)
		{
			Vector3 pos = transform.localPosition;
			m_TweenPos.ResetToBeginning();
			transform.localPosition = pos;
		}
	}
}
