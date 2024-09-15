using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : Singleton<DebugTools>
{
	public void GetAllEvidence()
	{
		EvidenceDataManager.instance.GetAllEvidence();
		print("!! [Debug] Get all evidence !!");
	}
}
