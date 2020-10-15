using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

//ZombieAttackTarget attacks the target.
//@todo need a better way to detect a dead target. Maybe deactivate the GameObject, since the currentTarget is actually a GameObject in the SensorTarget layer?
public class ZombieAttackTarget : MonoBehaviour
{
	GameObject currentTarget;
	NavMeshAgent agent;
	Animator animator;
	// Start is called before the first frame update
	void Start()
	{
		agent = gameObject.GetComponent<NavMeshAgent>();
		animator = gameObject.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (HasTarget())
		{
			//incorrect. agent has to stop
			//agent.SetDestination(currentTarget.transform.position);	
			if (!animator.GetBool("isAttacking"))
			{
				animator.SetBool("isAttacking", true);
			}
		}
	}

	void FixedUpdate()
	{
		if (HasTarget())
		{
			//if (agent.enabled) agent.isStopped = true; //does not work
			if (!animator.applyRootMotion)
			{
				//This is giving a weird behaviour on rooted animation. Almost like a numeric instability. 
				//Took me 2 hours to figure this out
				gameObject.transform.LookAt(currentTarget.transform);   
			}
			
		}
	}
	void LateUpdate()
	{
		//I think it is possible to signal the ZombieController that a state change probably needs to happen. At this point collisions should have been resolved
		//if the last collider has left then the currentTarget == null. So time for a state change
		if (currentTarget == null && this.enabled)
		{
			this.enabled = false;
			animator.SetBool("isAttacking", false);
		}
	}

	public void ProcessContact(GameObject target)
	{
		//Engage the first contact. Yeah the zombies are that hungry and dumb
		if (currentTarget == null)
		{
			currentTarget = target;
			Debug.Log(ToString() + "Engaging target " + currentTarget.name);
			return;
		}
	}

	public bool HasTarget()
	{
		return currentTarget != null;
	}

	public void RemoveContact(GameObject target)
	{
		if (!HasTarget()) return;
		//A dead target will be eventually destroyed.
		//Yes I need to be this specific
		if (target.GetInstanceID() == currentTarget.GetInstanceID())
		{
			Debug.Log(ToString() + "Removing target " + currentTarget.name);
			currentTarget = null;
			if (animator) animator.SetBool("isAttacking", false);
			//if (agent.enabled) agent.isStopped = false;	//does not work
		}
	}
}
