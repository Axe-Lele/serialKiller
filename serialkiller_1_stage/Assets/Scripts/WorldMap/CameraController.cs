using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public Camera m_ThisCamera = null;
    public Transform m_UIRoot;
    public float m_ZoomInSize = 0.2f;
    public float m_ZoomOutSize = 1f;
    public float m_ZoomChangeTime = 0.5f;

    private float from, to;
    private Vector2 baseVec, fromVec, toVec;



    private void Awake()
    {
        m_ThisCamera = GetComponent<Camera>();
        baseVec = transform.localPosition;
    }

    public void ControlCameraZoomRatio(Transform target, bool isZoomIn)
    {
        if (isZoomIn)
        {
            transform.parent = target;
            fromVec = transform.localPosition;
            toVec = Vector2.zero;
            toVec.x += 85f;

            from = m_ZoomOutSize;
            to = m_ZoomInSize;
        }
        else
        {
            transform.parent = m_UIRoot;
            fromVec = transform.localPosition;
            toVec = Vector2.zero;

            from = m_ZoomInSize;
            to = m_ZoomOutSize;
        }

        StartCoroutine(ChangeCameraZoom());
    }

    private IEnumerator ChangeCameraZoom()
    {
        float nowTime = 0f;
        while (nowTime <= m_ZoomChangeTime)
        {
            nowTime += Time.deltaTime;

            m_ThisCamera.transform.localPosition = Vector2.Lerp(transform.localPosition, toVec, nowTime/m_ZoomChangeTime);
            m_ThisCamera.orthographicSize = Mathf.Lerp(m_ThisCamera.orthographicSize, to, nowTime/m_ZoomChangeTime);

            yield return new WaitForEndOfFrame();
        }

        m_ThisCamera.transform.localPosition = toVec;
        m_ThisCamera.orthographicSize = to;
    }
}
