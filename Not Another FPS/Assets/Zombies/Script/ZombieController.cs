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
	[Range(0, 20)]
	public int healthPoint;

	//How often should we refresh the current state. Fastest will be 1 for refresh every 1 frame.
	int frameRefreshRate;
	//All possible movement states
	Dictionary<ZombieState, MyZombieStuff.IZombieState> zombieStates;
	MyZombieStuff.IZombieState zombieDied;

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
		zombieDied = null;
		healthPoint = 3;
		zombieStates = new Dictionary<ZombieState, MyZombieStuff.IZombieState>();
		zombieStates.Add(ZombieState.RandomWalk, new MyZombieStuff.ZombieRandomWalk(gameObject));
		//Death occurs only once, so no point caching this state
		//zombieStates.Add(ZombieState.Dead, new MyZombieStuff.ZombieDeath(gameObject));
		currentState = zombieStates[ZombieState.RandomWalk];
		currentState.Start();
	}

	// Update is called once per frame
	void Update()
	{
		currentState.Update(Time.deltaTime);
	}

	void LateUpdate()
	{
		if ((healthPoint == 0) && (zombieDied == null))
		{
			zombieDied = new MyZombieStuff.ZombieDeath(gameObject);
			//no more state transition allowed
			currentState = zombieDied;
		}
		//currentState.LateUpdate()
		//Do state transition checks here
	}
}
