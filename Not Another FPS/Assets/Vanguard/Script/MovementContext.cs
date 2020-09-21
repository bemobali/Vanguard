using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Vanguard.Script
{
	//MovementContext is a hybrid state manager and observer.
	//As a state manager, MovementContext manages the movement state. Necessary because the character's jumping state requires some collider and rigidbody state management
	//Collaborate with the player Vanguard instance for access to Controller and AnimationContext.
	class MovementContext
	{
		public enum MovementStates
		{
			LateralMovement, VerticalMovement
		}

		global::Vanguard vanguard;
		//This is what the context is managing
		IMovementState currentState;
		private Dictionary<MovementStates, IMovementState> movementStates;

		public MovementContext(global::Vanguard obs)
		{
			vanguard = obs;
			movementStates = new Dictionary<MovementStates, IMovementState>();
			movementStates.Add(MovementStates.LateralMovement, new LateralMovement(this, vanguard));
			movementStates.Add(MovementStates.VerticalMovement, new VerticalMovement(this, vanguard));
			currentState = movementStates[MovementStates.LateralMovement];
		}
		//Handles state changes during a regular update
		public void Update(float deltaT)
		{
			currentState.Update(deltaT);
		}

		//Handles state changes during a fixedUpdate physics update
		public void FixedUpdate(float deltaT)
		{
			currentState.FixedUpdate(deltaT);

		}

		//Handles state changes during a lateupdate call
		public void LateUpdate()
		{
			currentState.LateUpdate();
		}

		public void ContextSwitch(MovementStates nextState)
		{
			currentState = movementStates[nextState];
		}
	}
}
