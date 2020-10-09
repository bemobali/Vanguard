using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//@todo look into using layers to reduce the collider contacts. If collider can be limited to a certain layers, then we can get rid of the if checks
namespace Assets.Zombies.Script
{
	//ZombieRandomWalk picks a random walkable spot to walk to
	//ZombieStateContext is responsible for state transitions.
	//Not responsible for prioritizing the target.
	//Collaborate with the animator to set the correct zombie animation state
	public class ZombieRandomWalk: IZombieState
	{
		#region From constructor parameter
		//The zombie game object to control
		UnityEngine.GameObject zombie;
		#endregion

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
				//@todo Use navmesh distance to obstacles as max distance
				if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1f, UnityEngine.AI.NavMesh.AllAreas))
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
			//Set a new random destination
			if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
			{
				RandomWalk();
			}
		}

		public void FixedUpdate(float deltaT) 
		{
		}

		//@todo Try to remove this function by limiting the collider to only a layer with all of the acceptable contacts
		bool isContactATarget(string contactTag)
		{
			return (contactTag == "Player") || (contactTag == "OrganicFood");
		}
	}
}
