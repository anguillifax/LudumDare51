using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJam
{
	public class QuitToMenu : MonoBehaviour
	{
		public float holdTime = 2f;
		public float curTime;
		public GameObject hud;

		void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				hud.SetActive(true);
				curTime -= Time.deltaTime;
			}
			else
			{
				hud.SetActive(false);
				curTime = holdTime;
			}

			if (curTime < 0)
			{
				SceneManager.LoadScene("MainMenu");
			}
		}
	}
}

