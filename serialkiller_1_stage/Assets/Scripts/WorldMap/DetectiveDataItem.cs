using UnityEngine;
using DetectiveBoard;
using System.Collections;

public class DetectiveDataItem : MonoBehaviour
{
	private DetectiveData m_Data;

	public void Initialize(DetectiveData data)
	{
		m_Data = data;
	}
}
