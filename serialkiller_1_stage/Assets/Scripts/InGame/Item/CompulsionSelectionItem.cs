using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompulsionSelectionItem : MonoBehaviour {

    private string m_Target;
    private string m_SelectionIndex;
    public int num;
    public UILabel SelectionLabel;

    private bool b = false;

    public void Select()
    {
        CompulsionSelectionManager.instance.Select(m_SelectionIndex);
    }

    public void Setting(string target, string index, string str)
    {
        m_Target = target;
        m_SelectionIndex = index;
        SelectionLabel.text = str;
    }
}
