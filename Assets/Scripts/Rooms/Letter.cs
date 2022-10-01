using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
	[CreateAssetMenu(menuName ="LD51/New Letter")]
	public class Letter : ScriptableObject
	{
		[Multiline]
		public string text;

		public int cost;

		public string associatedScene;
	}
}