using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameKeyboardManager : Singleton<InGameKeyboardManager>
{


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlaceManager.instance.ShowLabel();
        }

        /*if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            GameManager.instance.ControlCameraZoom(-0.1f);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            GameManager.instance.ControlCameraZoom(0.1f);
        }*/

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (InGameUIManager.instance.MainUIBar_NotePopup.activeInHierarchy)
            {
                InGameUIManager.instance.ControlNotePopup();
            }
            else if (InGameUIManager.instance.MainUIBar_AnalysisPopup.activeInHierarchy)
            {
                InGameUIManager.instance.ControlLaboratoryPanel();
            }
            else if (InGameUIManager.instance.NewsPopup.activeInHierarchy)
            {
                InGameUIManager.instance.ControlNewsPopup();
            }
            else if (InGameUIManager.instance.WarrantPopup.activeInHierarchy)
            {
                InGameUIManager.instance.ControlWarrantPopup(UIPopupFlag.Close);
            }
            else if (InGameUIManager.instance.ExitPopup.activeInHierarchy)
            {
                InGameUIManager.instance.ControlExitPopup();
            }
            else
            {
                InGameUIManager.instance.ControlExitPopup();
            }
        }
    }

}
