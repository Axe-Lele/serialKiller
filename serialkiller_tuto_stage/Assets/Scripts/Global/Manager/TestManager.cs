using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour {

    public void SetTest1()
    {
        print("SetTest1");
        Screen.SetResolution(1280, 720, false);
    }

    public void SetTest2()
    {
        print("SetTest2");
        Screen.SetResolution(1366, 768, false);
    }

    public void SetTest3()
    {
        print("SetTest3");
        Screen.SetResolution(1600, 900, false);
    }

    public void SetTest4()
    {
        print("SetTest4");
        Screen.SetResolution(1920, 1080, false);
    }

    public void SetTest5()
    {
        print("SetTest5");
        Screen.SetResolution(2560, 1440, false);
    }

    public void SetTest6()
    {
        print("SetTest6");
        Screen.SetResolution(Screen.width, Screen.height, true);
    }
}
