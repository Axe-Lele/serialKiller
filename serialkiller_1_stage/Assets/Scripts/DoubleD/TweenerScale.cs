using UnityEngine;

public class TweenerScale : Tweener
{
	public Vector3 from = Vector3.one;
	public Vector3 to = Vector3.one;

	protected override void OnUpdate(float factor)
	{
		Vector3 value = from * (1f - factor) + to * factor;

		GetComponent<Transform>().localScale = value;
	}
}
