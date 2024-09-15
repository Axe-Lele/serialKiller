using UnityEngine;
using System.IO;
using System;

public class GlobalMethod : Singleton<GlobalMethod>
{
	public string ReturnSceneName()
	{
		return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
	}

	public void GoToWorld()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("WorldMap");
	}

	public bool ReturnFileExist(string filepath)
	{
		return File.Exists(filepath);
	}

	public void ResetGame()
	{
		File.Delete(Application.persistentDataPath + "/" + "data01.bin");
		File.Delete(Application.persistentDataPath + "/" + "data02.bin");
		File.Delete(Application.persistentDataPath + "/" + "data03.bin");
		File.Delete(Application.persistentDataPath + "/" + "data04.bin");
		File.Delete(Application.persistentDataPath + "/" + "data05.bin");
		File.Delete(Application.persistentDataPath + "/" + "data06.bin");
		File.Delete(Application.persistentDataPath + "/" + "data07.bin");
		File.Delete(Application.persistentDataPath + "/" + "data08.bin");
		File.Delete(Application.persistentDataPath + "/" + "data09.bin");
		//File.Delete(Application.persistentDataPath + "/" + "data01.bin");
		//File.Delete(Application.persistentDataPath + "/" + "data01.bin");
		//File.Delete(Application.persistentDataPath + "/" + "data01.bin");
	}

	internal bool ReturnFileExist(object filepath)
	{
		throw new NotImplementedException();
	}
}
