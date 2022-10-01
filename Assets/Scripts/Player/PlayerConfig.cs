using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
	[CreateAssetMenu(menuName = "LD51/PlayerConfig", order = 100)]
	public class PlayerConfig : ScriptableObject
	{
		public float driveVel;
		public float driveAccel;

		[Space]
		public float steerSpatialOffset;
		public float steerInfluence;
		public float steerAccel;

		[Space]
		public float moveVel;
		public float moveAccel;

		[Space]
		public LayerMask reflectMask;
		public float reflectWindowBefore;
		public float reflectWindowAfter;
		public float reflectVelBoost;

		[Space]
		public float preReflectDist;
	}
}