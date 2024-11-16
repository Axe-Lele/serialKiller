using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tweener : MonoBehaviour
{
	public Timer timer;
	
	float mFactor
	{
		get
		{
			if (timer == null) { enabled = false; return 0f; }
			return timer.factor;
		}
	}

	private void Update()
	{
		if (timer == null || timer.enabled == false)
		{
			enabled = false;
			return;
		}
		
		OnUpdate(mFactor);
	}

	abstract protected void OnUpdate(float factor);
}
