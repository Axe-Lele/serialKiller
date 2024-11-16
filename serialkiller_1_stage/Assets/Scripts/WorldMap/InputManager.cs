using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(this);
	}
	// Update is called once per frame
	void Update()
	{
#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0))
		{
			SoundManager.instance.changeSFXVolume(0.5f);
			SoundManager.instance.PlaySFX("button_click");
		}
#endif
		if (Input.touchCount > 0)
		{
			SoundManager.instance.changeSFXVolume(0.5f);
			SoundManager.instance.PlaySFX("button_click");
		}
	}
}
