using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIScrollView))]
public class ScrollButtonController : MonoBehaviour
{

	public Collider m_Collider;
	public bool m_CanClicked = false;

	public UIScrollView m_ScrollView;
	public UIGrid m_Grid;
	public float m_ItemHeight, m_ItemWidth;
	public GameObject m_UpButton, m_DownButton;

	private Vector2 m_StartOffset;
	private int m_DefaultShowItemCount;
	private Vector2 m_PanelSize;
	// Use this for initialization
	private void Awake()
	{
		if (m_ScrollView == null)
		{
			m_ScrollView = GetComponent<UIScrollView>();
			if (m_ScrollView == null)
				gameObject.SetActive(false);
		}
		m_ScrollView.ResetPosition();

		if (m_ItemHeight == 0)
			m_ItemHeight = m_Grid.cellHeight;
		if (m_ItemWidth == 0)
			m_ItemWidth = m_Grid.cellWidth;

		m_ScrollView.onDragFinished = new UIScrollView.OnDragNotification(CheckSCV);

		if (m_CanClicked == false && m_Collider != null)
		{
			m_Collider.enabled = false;
		}

		if (m_UpButton != null)
			m_UpButton.SetActive(false);
		if (m_DownButton != null)
			m_DownButton.SetActive(false);

		m_StartOffset = m_ScrollView.panel.clipOffset;
		m_PanelSize = m_ScrollView.panel.GetViewSize();
		m_DefaultShowItemCount = Mathf.FloorToInt(m_PanelSize.y / m_ItemHeight);
	}

	private void Update()
	{
		if (m_ScrollView.isDragging)
			CheckSCV();
	}

	public void CheckSCV()
	{
		Vector2 _constraint = m_ScrollView.panel.clipOffset;
		int _itemCount = m_Grid.transform.childCount;
		print(_itemCount);

		if (_constraint.y > m_StartOffset.y + -m_ItemHeight / 2f)
		{
			print(_constraint.y);
			print("Cannot Up Scroll");
			m_UpButton.SetActive(false);
		}
		else if (_constraint.y < m_StartOffset.y + (_itemCount - m_DefaultShowItemCount - 1) * -m_ItemHeight - m_ItemHeight * 0.5f)
		{
			print(_constraint.y);
			print("Cannot Down Scroll");
			m_DownButton.SetActive(false);
		}
		else
		{
			m_UpButton.SetActive(true);
			m_DownButton.SetActive(true);
		}
	}
}
