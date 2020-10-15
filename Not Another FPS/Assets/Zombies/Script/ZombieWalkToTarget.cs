using System;
using UnityEngine;
using UnityEngine.AI;

//This state stees the zombie to walk to a particular target. The zombie is not committed to the current target yet.
//If a new, higher priority target pops up, the zombie will switch to that target
//Targets are ranked by tags. See targetRank function to rank the target
class ZombieWalkToTarget : MonoBehaviour
{
	NavMeshAgent agent;
	//The current game object in SensorTarget layer
	GameObject currentTarget;
	Animator animator;

	#region Built In function
	void Start()
	{
		agent = gameObject.GetComponent<NavMeshAgent>();
		animator = gameObject.GetComponent<Animator>();
	}

	void Update()
	{
		if (HasTarget())
		{
			if (!animator.GetBool("isWalking"))
			{
				animator.SetBool("isWalking", true);
			}
			agent.SetDestination(currentTarget.transform.position);
		}
	}

	void LateUpdate()
	{ 
		//I think it is possible to signal the ZombieController that a state change probably needs to happen. At this point collisions should have been resolved
		//if the last collider has left then the currentTarget == null. So time for a state change
		//The question is: am I correct to deduce that Update won't be called in the next cycle after handling OnDeactivate
		if (currentTarget == null && this.enabled)
		{
			this.enabled = false;
			//The transition can only go upward in the sensor state hierarchy
			//controller.ContextSwitch(ZombieController.ZombieState.RandomWalk);
		}
	}
	#endregion

	int rankTarget(string tag)
	{
		if (tag == "Player") return 0;
		if (tag == "OrganicFood") return 1;
		return Int32.MaxValue;
	}

	bool higherPriority(UnityEngine.GameObject lhs, UnityEngine.GameObject rhs)
	{
		return rankTarget(lhs.tag) < rankTarget(rhs.tag);
	}

	void SetTarget(GameObject target)
	{
		currentTarget = target;
		Debug.Log(ToString() + "Switching target to " + currentTarget.name);
	}

	//Call this to deal with contacts from the colliders, or any other form of target detection
	public void ProcessContact(GameObject target)
	{
		//Don't reverse the evaluation order
		if ((currentTarget == null) || higherPriority(target, currentTarget))
		{
			SetTarget(target);
			return;
		}

		if (target.gameObject.GetInstanceID() == currentTarget.GetInstanceID()) return;

		if (rankTarget(currentTarget.tag) == rankTarget(target.tag))
		{
			//Targets can be moving around
			float distanceToNewTarget = (target.transform.position - gameObject.transform.position).magnitude;
			float distanceToCurrentTarget = (currentTarget.transform.position - gameObject.transform.position).magnitude;
			if (distanceToNewTarget < distanceToCurrentTarget)
			{
				SetTarget(target);
			}
		}
	}

	public bool HasTarget()
	{
		return currentTarget != null;
	}
	public void RemoveContact(GameObject target)
	{
		if (!HasTarget()) return;
		//Yes I need to be this specific
		if (target.GetInstanceID() == currentTarget.GetInstanceID())
		{
			Debug.Log(ToString() + "Removing target " + currentTarget.name);
			currentTarget = null;
		}
	}
};
