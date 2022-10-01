using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameJam
{
	[Serializable]
	public class GameSessionState
	{

		/// <summary>
		/// Current number of coins. Can be negative.
		/// </summary>
		public int coins;

		/// <summary>
		/// Invoked when <see cref="coins"/> changes. Used for UI repaint.
		/// </summary>
		public UnityEvent coinsChanged;

		/// <summary>
		/// The count of completed levels. Tutorial is 0. Common levels start at 1.
		/// </summary>
		public int currentLevelNumber;

		/// <summary>
		/// Available letters to be pulled as the next level.
		/// </summary>
		public EventPool commonEvents;

		/// <summary>
		/// Available letters to be pulled as the boss level.
		/// </summary>
		public EventPool bossEvents;

	}
}