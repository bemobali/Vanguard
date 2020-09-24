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
			controller.Update(deltaT);
		}

		public void FixedUpdate(float deltaT)
		{
			//context switch.
			if (controller.Jump())
			{
				movementContext.ContextSwitch(MovementContext.MovementStates.VerticalMovement);
				movementContext.FixedUpdate(deltaT);
			}
		}

		public void LateUpdate()
		{
			controller.UpdateTilt();
		}
	}

	class VerticalMovement : IMovementState
	{
		MovementContext movementContext;
		global::Vanguard vanguard;
		bool jumping;
		public VerticalMovement(MovementContext context, global::Vanguard ops)
		{
			movementContext = context;
			vanguard = ops;
			jumping = false;
		}

		public void Update(float deltaT)
		{
			//Last lateral movement vector updated by the previous state
			vanguard.transform.position += vanguard.LateralMovementVector();
		}

		public void FixedUpdate(float deltaT)
		{
			//jump only once
			if (vanguard.IsGrounded() && !jumping)
			{
				jumping = true;
				vanguard.Jump();
				return;
			}

			//Not jumping anymore. Playable is now in the air

			//This weird logic is necessary because fixedupdate runs faster than IsCollisionEnter and IsCollisionExit. So FixedUpdate can be running n times before one IsCollisionExit
			//Since we are dependengin on these collision callbacks to tell us if we are levitating or not, I need a second bool flag to tell me that jump force has been applied,
			//and I am now waiting for the player to actually levitate. Once the player is airborne, then context switch back to lateral movement can be initiated
			if (jumping && !vanguard.IsGrounded()) jumping = false;
		}

		public void LateUpdate()
		{
			//Jus landed
			if (vanguard.IsGrounded() && !jumping)
			{
				//Debug.Log("Landed");
				vanguard.LandingFromJumping();
				//basically a context switch. If this gets too expensive then cache the state instances and index it by name
				movementContext.ContextSwitch(MovementContext.MovementStates.LateralMovement);
			}
		}
	}
}