using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.AI;

//ZombieRandomWalk picks a random walkable spot to walk to
//ZombieController is responsible for state transitions.
//Not responsible for prioritizing the target.
//Collaborate with the animator to set the correct zombie animation state
//@todo What should the zombie do when it cannot decide where to go?
class ZombieRandomWalk: MonoBehaviour
{
	[SerializeField, Range(10,50)]
	float walkRange = 10f;
	//How many times should the zombie try to recalculate its next destination
	[SerializeField, Range(1,100)]
	int numAttempts = 30;
	NavMeshAgent agent;
	Animator animator;
	Vector3 walkDestination;

	bool RandomPoint(Vector3 center, float range, out Vector3 result)
	{
		const float maxSampleDistance = 1f;
		//Is 30 attempts too many?
		for (int i = 0; i < numAttempts; ++i)
		{
			Vector2 point2D = UnityEngine.Random.insideUnitCircle * range;
			Vector3 randomPoint = center + new Vector3(point2D.x, 0 , point2D.y);
			randomPoint.y = Terrain.activeTerrain.SampleHeight(randomPoint);
			NavMeshHit hit;
			//@todo Use navmesh distance to obstacles as max distance
			if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, maxSampleDistance, UnityEngine.AI.NavMesh.AllAreas))
			{
				result = hit.position;
				return true;
			}
			//Debug.Log(ToString() + " failed to sample navmesh at position " + randomPoint.ToString());
		}
		result = Vector3.zero;
		//or not
		Debug.Log(ToString() + " failed to get a random walk destination");
		return false;
	}

	void RandomWalk()
	{
		//Select a target
		if (RandomPoint(gameObject.transform.position, walkRange, out walkDestination))
		{
			agent.SetDestination(walkDestination);
		}
	}
	public void Start() 
	{
		animator = gameObject.GetComponent<Animator>();
		agent = gameObject.GetComponent<NavMeshAgent>();
		walkDestination = new Vector3();
		RandomWalk();
	}

	public void Update() 
	{
		//Set a new random destination
		if (agent.remainingDistance < agent.stoppingDistance)
		{
			RandomWalk();
		}
	}
}
