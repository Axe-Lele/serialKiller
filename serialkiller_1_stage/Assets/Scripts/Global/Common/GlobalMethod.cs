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
		ResetGame();
		UnityEngine.SceneManagement.SceneManager.LoadScene("WorldMap");
	}

	public bool ReturnFileExist(string filepath)
	{
		return File.Exists(filepath);
	}

	public void ResetGame()
	{
		for(int i = 0; i < 20; i++)
		{
			print(string.Format(Application.persistentDataPath + "/" + "data{0:00}.bin", i));
			File.Delete(string.Format(Application.persistentDataPath + "/" + "data{0:00}.bin", i));
		}
		//File.Delete(Application.persistentDataPath + "/" + "data02.bin");
		//File.Delete(Application.persistentDataPath + "/" + "data03.bin");
		//File.Delete(Application.persistentDataPath + "/" + "data04.bin");
		//File.Delete(Application.persistentDataPath + "/" + "data05.bin");
		//File.Delete(Application.persistentDataPath + "/" + "data06.bin");
		//File.Delete(Application.persistentDataPath + "/" + "data07.bin");
		//File.Delete(Application.persistentDataPath + "/" + "data08.bin");
		//File.Delete(Application.persistentDataPath + "/" + "data09.bin");
	}

	internal bool ReturnFileExist(object filepath)
	{
		throw new NotImplementedException();
	}
}
