using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyZombieStuff = Assets.Zombies.Script;

public class ZombieController : MonoBehaviour
{
	MyZombieStuff.ZombieStateContext zombieStates;
	public GameObject zombieToControl;
	// Start is called before the first frame update
	void Start()
	{
		zombieStates = new MyZombieStuff.ZombieStateContext(zombieToControl);
		zombieStates.Start();
	}

	// Update is called once per frame
	void Update()
	{
		zombieStates.Update(Time.deltaTime);
	}
}
