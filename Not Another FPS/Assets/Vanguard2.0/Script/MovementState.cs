using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//How do I prevent the generation of a default constructor?
namespace Assets.Vanguard.Script
{
	interface IMovementState
	{
		void Update(float deltaT);
		void FixedUpdate(float deltaT);
		void LateUpdate();
		void OnGroundCollisionEnter();
		void OnGroundCollisionExit();
	}

	//Manage movement state transitions
	class LateralMovement : IMovementState
	{
		MovementContext movementContext;
		Controller controller;

		public LateralMovement(MovementContext context, global::Vanguard ops)
		{
			movementContext = context;
			controller = ops.Controller();
		}

		public void Update(float deltaT)
		{
			//The tutorial https://matthew-isidore.ovh/full-body-fps-controller-part-1-base-character-controller/ uses FixedUpdate for handling input
			controller.Update(deltaT);
		}

		public void FixedUpdate(float deltaT)
		{
			//context switch.
			if (controller.Jump())
			{
				//Debug.Log("User pressed Jump");
				movementContext.ContextSwitch(MovementContext.MovementStates.VerticalMovement);
				movementContext.FixedUpdate(deltaT);
				return;
			}
			controller.FixedUpdate(deltaT);
		}

		public void LateUpdate()
		{
			//Maybe this should be in fixed update
			controller.Tilt();
		}

		public void OnGroundCollisionEnter()
		{
			//We should be on the ground anyway
		}

		public void OnGroundCollisionExit()
		{
			//Falling down maybe?
		}
	}

	class VerticalMovement : IMovementState
	{
		MovementContext movementContext;
		global::Vanguard vanguard;
		Controller controller;
		//jumping flag ensures that our impuse applies only once
		bool jumping;
		public VerticalMovement(MovementContext context, global::Vanguard ops)
		{
			movementContext = context;
			vanguard = ops;
			jumping = false;
			controller = ops.Controller();
		}

		public void Update(float deltaT)
		{
			controller.Update(deltaT);
		}

		public void FixedUpdate(float deltaT)
		{
			//Debug.Log("VerticalMovement.FixedUpdate");
			//Apply impulse only once
			if (!jumping)
			{
				//Debug.Log("Jumping");
				jumping = true;
				vanguard.Jump();
				return;
			}

			//Last lateral movement vector updated by the previous state. So player cannot change direction while in the air
			vanguard.transform.position += vanguard.LateralMovementVector();
		}

		public void OnGroundCollisionEnter()
		{
			//We landed
			if (jumping)
			{
				//Debug.Log("Landed");
				vanguard.LandingFromJumping();
				jumping = false;
				//Back to walking/running state
				movementContext.ContextSwitch(MovementContext.MovementStates.LateralMovement);
			}
		}

		public void OnGroundCollisionExit()
		{
			//Debug.Log("Levitating");
		}

		public void LateUpdate()
		{
		}
	}
}