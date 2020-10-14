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
	ZombieWalkToTarget zombieWalkToTarget;
	//How often should we refresh the current state. Fastest will be 1 for refresh every 1 frame.
	int frameRefreshRate;
	//All possible movement states
	Dictionary<ZombieState, MyZombieStuff.IZombieState> zombieStates;

	//Zombie's current state
	MyZombieStuff.IZombieState currentState;
	
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
		zombieStates = new Dictionary<ZombieState, MyZombieStuff.IZombieState>();
		zombieStates.Add(ZombieState.RandomWalk, new MyZombieStuff.ZombieRandomWalk(gameObject));
		//Death occurs only once, so no point caching this state
		//zombieStates.Add(ZombieState.Dead, new MyZombieStuff.ZombieDeath(gameObject));
		currentState = zombieStates[ZombieState.RandomWalk];
		currentState.Start();
		//Make sure the initial transition state is RandomWalk. Disable all other state behavior scripts
		zombieDied.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{
		//@todo remove the if statement once we can consolidate all states using a MonoBehavior-derived class/interface, whichever works
		if (!isDead())
		{

			currentState.Update(Time.deltaTime);
		}
	}

	bool isDead()
	{
		return health.HealthPoint < 1;
	}

	void LateUpdate()
	{
		//no more state transition allowed
		if (isDead() && !zombieDied.enabled)
		{
			zombieDied.enabled = true;
			return;
		}
		//If we use MonoBehaviour-derived script objects, our state transitions will be activating and deactivating Gameobject
		/*if (isDead() && (zombieDied == null))
		{
			//zombieDied = new ZombieDead();
			//no more state transition allowed
			//currentState = zombieDied;
		}*/
		//currentState.LateUpdate()
		//Do state transition checks here
	}
}
