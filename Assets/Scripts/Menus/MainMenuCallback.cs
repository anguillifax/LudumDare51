using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
	public class MainMenuCallback : MonoBehaviour
	{
		public void StartGame()
		{
			GameSession.Instance.CreateNewSession();
		}
	}
}