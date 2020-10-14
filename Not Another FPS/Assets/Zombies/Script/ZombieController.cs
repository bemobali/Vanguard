using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyZombieStuff = Assets.Zombies.Script;

//ZombieController controls the movement of the zombies based on its current states
//ZombieContrtoller can be used for PeonZombies and BossZombies
//@todo Get rid of ZombieStateContect. The ZombieController will directly transition Zombie states
//@todo Make colliders update each zombie states.
public class ZombieController : MonoBehaviour
{
	//public ZombieLongRangeSensor longRangeSensor;
	//The overall health of the zombie. Maybe not the optimal place to put this.
	//public because this needs to be accessed by multiple BattleDamage instances
	public Health health;
	[SerializeField]
	ZombieDead zombieDied;
	//Zombie is walking to a particular target
	[SerializeField]
	ZombieWalkToTarget walkToTarget;
	[SerializeField]
	ZombieRandomWalk randomWalk;
	[SerializeField]
	ZombieRunToTarget runToTarget;
	[SerializeField]
	ZombieAttackTarget attackTarget;
	
	public enum ZombieState
	{
		RandomWalk,
		WalkToTarget,
		RunToTarget,
		AttackTarget,
	};

	// Start is called before the first frame update
	void Start()
	{
		//Death occurs only once, so no point caching this state
		//Make sure the initial transition state is RandomWalk. Disable all other state behavior scripts
		zombieDied.enabled = false;
	}

	bool IsDead()
	{
		return health.HealthPoint < 1;
	}

	void Update()
	{
	}

	void LateUpdate()
	{
		//no more state transition allowed
		if (IsDead())
		{
			if (!zombieDied.enabled)
			{
				Debug.Log("State transition to " + zombieDied.ToString());
				walkToTarget.enabled = false;
				runToTarget.enabled = false;
				attackTarget.enabled = false;
				randomWalk.enabled = false;
				zombieDied.enabled = true;
			}
			return;
		}
		//This enforces the activation sequence from attack, run, walk, then random walk
		//Relies on the state script deactivating itself once it has no target
		if (attackTarget.HasTarget())
		{
			if (!attackTarget.enabled)
			{
				Debug.Log("State transition to " + attackTarget.ToString());
				walkToTarget.enabled = false;
				runToTarget.enabled = false;
				randomWalk.enabled = false;
				attackTarget.enabled = true;
			}
			return;
		}

		if (runToTarget.HasTarget())
		{ 
			if (!runToTarget.enabled)
			{
				Debug.Log("State transition to " + runToTarget.ToString());
				walkToTarget.enabled = false;
				runToTarget.enabled = true;
				randomWalk.enabled = false;
				attackTarget.enabled = false;
			}
			return;
		}

		if (walkToTarget.HasTarget())
		{
			if (!walkToTarget.enabled)
			{
				Debug.Log("State transition to " + walkToTarget.ToString());
				walkToTarget.enabled = true;
				runToTarget.enabled = false;
				randomWalk.enabled = false;
				attackTarget.enabled = false;
			}
			return;
		}
		
		if (!randomWalk.enabled)
		{
			Debug.Log("State transition to " + randomWalk.ToString());
			//Finally enable random walk
			walkToTarget.enabled = false;
			runToTarget.enabled = false;
			attackTarget.enabled = false;
			randomWalk.enabled = true;
		}
	}

	//@note I may still need the ContextSwitch() function, because each state actually knows where to transition to if it has no target
}
