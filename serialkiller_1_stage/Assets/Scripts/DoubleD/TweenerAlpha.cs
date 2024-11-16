using UnityEngine;

public class TweenerAlpha: Tweener
{
	[Range(0, 1f)]
	public float from = 1f;

	[Range(0, 1f)]
	public float to = 1f;

	protected override void OnUpdate(float factor)
	{
		float value = from * (1f - factor) + to * factor;

		GetComponent<UISprite>().alpha = value;
	}
}
