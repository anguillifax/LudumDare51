using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
	public class Grass : MonoBehaviour, IMowable
	{
		public bool canMow = true;
		public float destroyDelay = 5f;
		public GameObject destroyPrefab;

		private Animator anim;

		public bool CanMow => canMow;

		private void Awake()
		{
			anim = GetComponentInChildren<Animator>();
		}

		private void Start()
		{
			anim.SetFloat("CycleOffset", UnityEngine.Random.value);
		}

		public void Mow(GameObject sender)
		{
			if (!CanMow)
			{
				return;
			}

			bool fromBack = sender.GetComponent<Rigidbody>().velocity.z < 0;
			anim.SetBool("FromBack", fromBack);
			anim.SetTrigger("Mow");

			if (destroyPrefab)
			{
				Instantiate(destroyPrefab, transform.position, transform.rotation);
			}

			canMow = false;
			Destroy(gameObject, destroyDelay);
		}
	}
}