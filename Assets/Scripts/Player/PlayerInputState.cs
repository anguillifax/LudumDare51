using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
	[Serializable]
	public struct PlayerInputState
	{
		public float moveX;
		public float moveY;
		public bool reflectDown;

		public Vector2 Move => new Vector2(moveX, moveY);
		public Vector2 MoveClamped => Vector2.ClampMagnitude(Move, 1);
	}
}