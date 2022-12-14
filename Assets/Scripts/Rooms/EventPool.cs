using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
	[Serializable]
	public class EventPool
	{
		[SerializeField]
		private List<Letter> pool;

		public EventPool(IEnumerable<Letter> pool)
		{
			this.pool = new List<Letter>(pool);
		}

		/// <summary>
		/// Selects <paramref name="count"/> events from the pool and returns them in shuffled order.
		/// 
		/// <para>
		/// If there are not enough items in the pool to fulfill the request, this method will throw an exception.
		/// </para>
		/// 
		/// </summary>
		/// <param name="count">Number of events to select.</param>
		/// <param name="fallbackMaxCost">Maximum allowed cost of fallback event to prevent softlocking.</param>
		/// <returns>Selected letters in shuffled order.</returns>
		public Letter[] SelectPreview(int count, int fallbackMaxCost = int.MaxValue)
		{
			if (count < 1)
			{
				throw new ArgumentOutOfRangeException("Count must be at least 1");
			}

			if (pool.Count < count)
			{
				throw new Exception("Event pool does not have enough entries to fullfill request.");
			}

			List<Letter> selection = new List<Letter>();

			List<Letter> shuffled = new List<Letter>(pool);
			shuffled.Shuffle();

			void Add(Predicate<Letter> constraint, string warning = "Failed to find event with given constraint")
			{
				int idx = shuffled.FindIndex(constraint);
				if (idx < 0)
				{
					Debug.LogError(warning + " (Adding random element instead)");
					idx = 0;
				}
				selection.Add(shuffled[idx]);
				shuffled.RemoveAt(idx);
			}


			Add(x => x.cost <= fallbackMaxCost, $"Could not find event with cost <= {fallbackMaxCost}");

			for (int i = 0; i < count - 1; i++)
			{
				Add(x => true);
			}

			selection.Shuffle();
			return selection.ToArray();
		}

		/// <summary>
		/// Removes a letter from the pool in O(n) time.
		/// </summary>
		/// <param name="letter">Reference to letter to remove.</param>
		public void Remove(Letter letter)
		{
			pool.Remove(letter);
		}
	}
}