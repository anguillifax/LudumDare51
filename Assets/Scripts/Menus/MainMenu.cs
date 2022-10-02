using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
	{
		Debug.Log("AH");
		SceneManager.LoadScene("MainGameplay");
	}

	public void QuitGame()
	{
		Debug.Log("PERISH");
		Application.Quit();
	}
}
