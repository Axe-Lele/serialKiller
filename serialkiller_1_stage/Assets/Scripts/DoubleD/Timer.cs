using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
	public enum Style
	{
		Once,
		Loop,
		PingPong
	}

	public Style style = Style.Once;

	public float duration = 1f;

	public bool ignoreTimescale = true;

	[HideInInspector]
	public float factor = 0f;

	[HideInInspector]
	public float amountPerDelta
	{
		get
		{
			if (mDuration != duration)
			{
				mDuration = duration;
				mAmountPerDelta = Mathf.Abs((duration > 0f) ? 1f / duration : 1000f) * Mathf.Sign(mAmountPerDelta);
			}

			return mAmountPerDelta;
		}
	}
	
	float mDelta = 0f;
	float mDuration = 0f;
	float mAmountPerDelta = 1000f;

	private void LateUpdate()
	{
		mDelta = ignoreTimescale ? RealTime.deltaTime : Time.deltaTime;

		factor += amountPerDelta * mDelta;

        switch (style)
        {
            case Style.Once:
                if (factor > 1f || factor < 0f)
                {
                    factor = Mathf.Clamp01(factor);
                    if ((factor == 1f && mAmountPerDelta > 0f || factor == 0f && mAmountPerDelta < 0f))
                        enabled = false;
                }
                break;

            case Style.Loop:
                if (factor > 1f)
                {
                    factor -= Mathf.Floor(factor);
                }
                break;

            case Style.PingPong:
                // Ping-pong style reverses the direction
                if (factor > 1f)
                {
                    factor = 1f - (factor - Mathf.Floor(factor));
                    mAmountPerDelta = -mAmountPerDelta;
                }
                else if (factor < 0f)
                {
                    factor = -factor;
                    factor -= Mathf.Floor(factor);
                    mAmountPerDelta = -mAmountPerDelta;
                }
                break;

            default:
                break;
        }

	}
}
