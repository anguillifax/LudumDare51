using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJam
{
	public class GameSession : MonoBehaviour
	{
		/// <summary>
		/// Returns the current Game Session. Returns null if it doesn't exist.
		/// </summary>
		public static GameSessionState Current => Instance.current;

		public static GameSession Instance { get; private set; } 

		public Letter[] commonEventPool;
		public Letter[] bossEventPool;

		public int startingCoins = 12;

		[Header("-- PREVIEW --")]
		[SerializeField]
		private GameSessionState current;

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}

		private void Start()
		{
			SceneManager.LoadScene("MainMenu");
		}

		public void CreateNewSession()
		{
			current = new GameSessionState
			{
				coins = startingCoins,
				currentLevelNumber = 0,
				commonEvents = new EventPool(commonEventPool),
				bossEvents = new EventPool(bossEventPool),
			};
		}
	}
}

