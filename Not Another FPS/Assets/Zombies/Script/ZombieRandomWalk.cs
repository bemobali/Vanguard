using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.AI;

//ZombieRandomWalk picks a random walkable spot to walk to
//ZombieController is responsible for state transitions.
//Not responsible for prioritizing the target.
//Collaborate with the animator to set the correct zombie animation state
class ZombieRandomWalk: MonoBehaviour
{
	[SerializeField]
	readonly float walkRange = 50f;
	//How many times should the zombie try to recalculate its next destination
	const int numAttempts = 30;
	NavMeshAgent navMeshAgent;
	Vector3 walkDestination;

	bool RandomPoint(Vector3 center, float range, out Vector3 result)
	{
		const float maxSampleDistance = 1f;
		//Is 30 attempts too many?
		for (int i = 0; i < numAttempts; i++)
		{
			Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
			NavMeshHit hit;
			//@todo Use navmesh distance to obstacles as max distance
			if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, maxSampleDistance, UnityEngine.AI.NavMesh.AllAreas))
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
		if (RandomPoint(gameObject.transform.position, walkRange, out walkDestination))
		{
			navMeshAgent.SetDestination(walkDestination);
		}
		//or not
	}
	public void Start() 
	{
		navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
		walkDestination = new Vector3();
		RandomWalk();
	}
	public void Update() 
	{
		//Set a new random destination
		if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
		{
			RandomWalk();
		}
	}
}
