using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Zombies.Script
{
	//ZombieRandomWalk picks a 
	class ZombieRandomWalk: IZombieState
	{
		//The zombie game object to control
		UnityEngine.GameObject zombie;
		//@todo get range from zombie
		const float walkRange = 30f;
		UnityEngine.AI.NavMeshAgent navMeshAgent;
		UnityEngine.Vector3 walkDestination;
		public ZombieRandomWalk(UnityEngine.GameObject zombieToControl) 
		{
			zombie = zombieToControl;
			navMeshAgent = zombie.GetComponent<UnityEngine.AI.NavMeshAgent>();
			walkDestination = new Vector3();
		}

		bool RandomPoint(Vector3 center, float range, out Vector3 result)
		{
			//Is 30 attempts too many?
			for (int i = 0; i < 30; i++)
			{
				Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
				UnityEngine.AI.NavMeshHit hit;
				if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
				{
					result = hit.position;
					return true;
				}
			}
			result = Vector3.zero;
			return false;
		}

		void RandomWalk()
		{
			//Select a target
			if (RandomPoint(zombie.transform.position, walkRange, out walkDestination))
			{
				navMeshAgent.SetDestination(walkDestination);
			}
			//or not
		}

		public void Start() 
		{
			RandomWalk();
		}
		public void Update(float deltaT) 
		{
			//Either a state change or set a new random destination
			if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
			{
				RandomWalk();
			}
		}

		public void FixedUpdate(float deltaT) 
		{
		}
	}
}
