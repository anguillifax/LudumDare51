using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
	public class PlayerController : MonoBehaviour
	{
		public enum ControlState
		{
			Parked,
			Drive,
			Reflect,
		}

		// =========================================================
		// Variables
		// =========================================================

		public PlayerConfig config;
		public ControlState state;
		public PlayerInputState inputs;

		[Space]
		public Vector2 outVel;
		public float driveDir;
		public float driveSpeed;
		public float driveSpeedTarget;
		public Vector2 inputVel;
		public Vector2 reflectNormal; // (0,0) if none
		public float reflectDownBuffer;
		public float postReflectTimer;

		[Space]
		public bool debugDrawHeading;
		public float debugMult;

		private Rigidbody body;
		public Animator anim;
		public SpriteRenderer spriteRen;

		// =========================================================
		// Initialization
		// =========================================================

		private void Awake()
		{
			body = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			Reset();
		}

		private void Reset()
		{
			driveDir = body.rotation.eulerAngles.y;
			driveSpeed = 0;
			inputVel = Vector2.zero;
			reflectNormal = Vector2.zero;
			reflectDownBuffer = 0;
			postReflectTimer = 0;
		}

		// =========================================================
		// Public API
		// =========================================================

		public void BeginDriving(Vector3 initialDir)
		{
			driveDir = Vector2.SignedAngle(FromVec3(initialDir), Vector2.right);
			state = ControlState.Drive;
		}

		// =========================================================
		// Update
		// =========================================================

		private void Update()
		{
			FetchInput();

			UpdateAnimator();

			if (Input.GetKeyDown(KeyCode.T))
			{
				Vector3 randomAngle = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0) * Vector3.forward;
				BeginDriving(randomAngle);
			}

			if (debugDrawHeading)
			{
				Debug.DrawRay(body.position, FromVec2(FromAngle(driveDir), 0), Color.yellow);
			}
		}

		private void FetchInput()
		{
			inputs.moveX = Input.GetAxisRaw("Horizontal");
			inputs.moveY = Input.GetAxisRaw("Vertical");
			inputs.reflectDown = Input.GetButtonDown("Reflect");
		}

		private void UpdateAnimator()
		{
			if (body.velocity.magnitude < 0.1f && inputs.Move.sqrMagnitude == 0)
			{
				anim.Play("Idle");
			}
			else
			{
				if (inputs.Move.sqrMagnitude > 0)
				{
					string clip;
					float angle = Vector2.SignedAngle(inputs.Move, Vector2.up);
					//Debug.Log($"Angle {angle}, Delta0 {Mathf.DeltaAngle(angle, 0)}, Delta180 {Mathf.DeltaAngle(angle, 180)}");
					if (Mathf.Abs(Mathf.DeltaAngle(angle, 0)) < 45f)
						clip = "PlayerWalkUp";
					else if (Mathf.Abs(Mathf.DeltaAngle(angle, 180)) < 45f)
						clip = "PlayerWalkDown";
					else
						clip = "PlayerWalkLeft";
					anim.Play(clip);
				}

				if (inputs.moveX != 0)
				{
					spriteRen.flipX = inputs.moveX < 0;
				}
			}
		}

		// =========================================================
		// Fixed Updated
		// =========================================================

		private void FixedUpdate()
		{
			DelegateStateCallback();
		}

		private void DelegateStateCallback()
		{
			switch (state)
			{
				case ControlState.Parked: ParkedUpdate(); break;
				case ControlState.Drive: DriveUpdate(); break;
				case ControlState.Reflect: ReflectUpdate(); break;
				default:
					state = ControlState.Drive;
					break;
			}
		}

		// =========================================================
		// Parked
		// =========================================================

		private void ParkedUpdate()
		{
			inputVel = Vector2.MoveTowards(
				inputVel,
				Vector2.zero,
				config.moveAccel * Time.fixedDeltaTime
			);

			driveSpeedTarget = 0;
			driveSpeed = Mathf.MoveTowards(
				driveSpeed,
				config.driveVel * driveSpeedTarget,
				config.driveAccel * Time.fixedDeltaTime
			);
		}

		// =========================================================
		// Drive
		// =========================================================

		private void DriveUpdate()
		{
			if (inputs.moveX != 0 || inputs.moveY != 0)
			{
				outVel = Vector2.MoveTowards(
					FromVec3(body.velocity),
					inputs.MoveClamped * config.mowerInputVel,
					config.mowerInputAccel * Time.fixedDeltaTime
				);
			}

			debugMult = 1 + config.mowerExponent.Evaluate(outVel.magnitude - config.mowerVel);
			outVel *= debugMult;

			//inputVel = Vector2.MoveTowards(
			//	inputVel,
			//	config.moveVel * inputs.MoveClamped,
			//	config.moveAccel * Time.fixedDeltaTime
			//);

			//if (inputs.moveX != 0 || inputs.moveY != 0)
			//{
			//	driveDir = Mathf.MoveTowardsAngle(
			//		driveDir,
			//		Vector2.SignedAngle(new Vector2(inputs.moveX, -inputs.moveY), Vector2.right),
			//		config.steerAccel * Time.fixedDeltaTime
			//	);
			//}

			//float targetDir;
			{
				//Vector2 mowerPos = FromVec3(body.position);
				//Vector2 inferredCharPos = mowerPos - (FromAngle(driveDir) * config.steerSpatialOffset);
				//Vector2 steerInfluence = Vector2.ClampMagnitude(new Vector2(inputs.moveX, inputs.moveY), 1) * config.steerInfluence;
				//Vector2 projectedCharPos = inferredCharPos + steerInfluence;
				//targetDir = Vector2.SignedAngle(projectedCharPos - mowerPos, Vector2.right);

				//float posY = body.position.y;
				//Debug.DrawRay(body.position, FromVec2(inferredCharPos, posY), Color.gray);
				//Debug.DrawLine(FromVec2(inferredCharPos, posY), FromVec2(projectedCharPos, posY), Color.green);
			}


			// TODO: Dynamic target speed
			//driveSpeedTarget = config.driveVel;

			//driveSpeed = Mathf.MoveTowards(
			//	driveSpeed, /* lastWorldVel */
			//	driveSpeedTarget,
			//	config.driveAccel * Time.fixedDeltaTime
			//);

			//Vector2 outVel = (FromAngle(driveDir) * driveSpeed) + inputVel;

			// TODO: Pre-reflect time dilation

			// TODO: Reflection handler

			body.velocity = FromVec2(outVel, 0);
		}

		// =========================================================
		// Reflect
		// =========================================================

		private void ReflectUpdate()
		{
		}

		// =========================================================
		// Physics Callbacks
		// =========================================================

		private void OnTriggerEnter(Collider other)
		{
			TryMow(other);
		}

		private void TryMow(Collider target)
		{
			var mowable = target.GetComponent<IMowable>();
			if (mowable != null && mowable.CanMow)
			{
				mowable.Mow(gameObject);
			}
		}

		// =========================================================
		// Vector Conversions
		// =========================================================

		private static Vector2 FromVec3(Vector3 v3)
		{
			return new Vector2(v3.x, v3.z);
		}

		private static Vector3 FromVec2(Vector2 v2, float y)
		{
			return new Vector3(v2.x, y, v2.y);
		}

		private static Vector2 FromAngle(float angle)
		{
			return new Vector2(
				Mathf.Cos(angle * Mathf.Deg2Rad),
				Mathf.Sin(angle * Mathf.Deg2Rad));
		}
	}
}