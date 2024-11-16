using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionItem : MonoBehaviour
{
	[SerializeField]
	private string m_Target;
	private string m_SelectionIndex;
	public int num;
	public UISprite sprite;
	public UISprite ReadPointSprite;
	public UISprite ColorCoverSprite;
	public UILabel SelectionLabel;

	public GameObject icon;

	private Color ReadColor;

	//private bool b = false;

	void Start()
	{
		ReadColor = new Color(0.77f, 0.77f, 0.77f);
	}

	public void SetText(string str)
	{
		SelectionLabel.text = str;
	}

	public void ReadCheckPoint()
	{
		if (ColorCoverSprite.gameObject.activeSelf == false)
			ColorCoverSprite.gameObject.SetActive(true);

		ReadPointSprite.spriteName = "Selection_Read";
		ColorCoverSprite.color = ReadColor;
		//SelectionLabel.trueTypeFont = GameManager.instance.SelectionReadFont;
		//    CheckSprite.SetActive(true);
	}

	public void UnreadCheckPoint()
	{
		ReadPointSprite.spriteName = "Selection_Unread";
		ColorCoverSprite.color = Color.white;
		//SelectionLabel.trueTypeFont = GameManager.instance.SelectionUnreadFont;
	}

	public void Select()
	{
		SoundManager.instance.changeSFXVolume(1.0f);
		SoundManager.instance.PlaySFX("dialog_jump");
		if (ColorCoverSprite != null)
		{
			if (ColorCoverSprite.gameObject.activeSelf == true)
				ColorCoverSprite.gameObject.SetActive(false);
		}
		SelectionManager.instance.Select(m_Target, m_SelectionIndex);
		icon.SetActive(!SelectionDataManager.instance.ReturnCheckRead(m_Target, m_SelectionIndex));
	}

	public void CaseSelect()
	{
		//b = true;
		//    StartCoroutine(BtnImageChange());
	}

	public void Setting(string target, string index)
	{
		m_Target = target;
		m_SelectionIndex = index;

		print("NPC : " + m_Target + " / Index : " + m_SelectionIndex);
		icon.SetActive(!SelectionDataManager.instance.ReturnCheckRead(m_Target, m_SelectionIndex));
	}

	/*IEnumerator BtnImageChange()
	{
			yield return new WaitForSeconds(preDelayTime);
			if (ColorSprite != null)
			{
					if (ColorSprite.gameObject.activeSelf == true)
							ColorSprite.gameObject.SetActive(false);
			}
			sprite.spriteName = pressBtnName;
			yield return new WaitForSeconds(nextDelayTime);
			sprite.spriteName = btnName;
			if (b)
			{
					DistractorManager.instance.SelectCase(num);
			}
			else
			{
					DistractorManager.instance.Select(direction, num);
			}
	}*/
}
