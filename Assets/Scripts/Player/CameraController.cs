using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
	public class CameraController : MonoBehaviour
	{
		public Transform target;

		private void LateUpdate()
		{
			transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
		}
	}
}