using UnityEngine;
using System.Collections;

public class AirPlaneManager : Singleton<AirPlaneManager> {
	public Transform[] tranforms;

	public TweenPosition tp;
	public UISprite sprite;


	public void Move(int from,int to )
	{
		tp.from = new Vector2(tranforms[from].localPosition.x, tranforms[from].localPosition.y);
		tp.to = new Vector2(tranforms[to].localPosition.x, tranforms[to].localPosition.y);

		sprite.transform.localPosition = tp.from;

		sprite.gameObject.SetActive(true);

        tp.enabled = true;
	}

}
