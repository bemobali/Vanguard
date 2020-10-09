using Assets.Vanguard.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.PlayerLoop;

namespace Assets.Zombies.Script
{
	/// <summary>
	//Zombie units state employs state patterns to dictate their current state
	//The states are: RandomWalk, WalkToFood, RunToFood, AttackFood, Dead
	//RandomWalk: the default state. As a NavMesh agent, the Zombie will randomly pick a destination within 50 units from its current position. The position will be a space in the walkable NavMesh
	//At this state, activate the walk animation.
	//
	//WalkToFood: enter this state if the Zombie detects food. This can be the player, or other non-zombie organic GameObjects. Detection distance determined from the inspector
	//Zombie prioritize the player as the target. Other organic non-zombie GameObjects takes lesser priority, but ranks the same among themselves
	//If a zombie detects multiple targets with the same rank, the closest target will be picked. Once a higher rank target enters the zombie's detection zone, it must change target to the higher
	//rank target.
	//At this state, activate the walk animation
	//
	//RunToFood: enter this state if the zombie gets close enough to the target. At this state, the zombie is committed to attack the target, until the target moves away from the zombie's run zone.
	//At this state, activate the run animation
	//
	//AttackFood: enter this state once the zombie gets withn attack range. At this state, the zombie engages the target until it moves away from the attack zone.
	//At this state, activate the attack animation
	//
	//Dead: enter this state once zombie's health drops to below 1. At this state, randomly select either the dead animation or deactivate the ragdoll's Rigidbody IsKinematic to drop the zombie.
	//Sink the zombie slowly by deactivating the ragdoll, and pull the zombie to the terrain at a pre-determined sink rate. Destroy the zombie object after a pre-determined sink amount.
	/// </summary>
	//Interesting C# rule, without the public keyword, this class is a private class of the namespace.

	//This implementation requires that ZombieStateContext transitions the states. Someone needs to keep track of the number of targets in the short-range, medium, and long-range sensors
	//and the order of checking has to be from short, medium, long, then random walk. Dead depends only on remaining health points. I wish there is a torus-like collider, with hole in the 
	//middle.
	//State update and transitions does not have to be done every frame
	[ObsoleteAttribute("ZombieStateContext has been replaced by ZombieController. DO NOT USE", true)]
	public class ZombieStateContext
	{
		//Number of frames to elapse before refreshing the zombie state. Allows the designer to tweak the AI performance
		int frameRefreshRate;
		IZombieState currentState;
		public enum ZombieState
		{
			RandomWalk,
			WalkToTarget,
			RunToTarget,
			AttackTarget,
			Dead
		};
		Dictionary<ZombieState, IZombieState> movementStates;
		UnityEngine.GameObject zombieToControl;

		//Allow designer to tweak the number of frames to elapse before refreshing the state context
		public void SetRefreshRate(int refreshRate)
		{
			frameRefreshRate = refreshRate;
		}

		#region Builtin Functions
		//Call this at MonoBehaviour's Start()
		public void Start()
		{
			currentState.Start();
		}

		//Call this at MonoBehaviour's Update()
		public void Update(float deltaT)
		{
			//Evaluate state switch based on Zombie state
			//Do nothing if current state is DEAD
			currentState.Update(deltaT);
		}

		//Call this at MonoBehaviour's FixedUpdate()
		public void FixedUpdate(float deltaT)
		{
			//Do nothing if current state is DEAD. We could be waiting for the zombie death animation to complete
			currentState.FixedUpdate(deltaT);
		}

		public void LateUpdate()
		{
			//do nothing if current state is DEAD. Or deactivate the ZombieController
			//currentState.LateUpdate();
		}
		#endregion

		public void ContextSwitch(ZombieState nextState)
		{
			currentState = movementStates[nextState];
		}
	};
}
