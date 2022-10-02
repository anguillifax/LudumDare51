using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
	public interface IMowable
	{
		bool CanMow { get; }
		void Mow(GameObject sender);
	}
}