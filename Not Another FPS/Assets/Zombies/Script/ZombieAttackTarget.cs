using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
		if (currentTarget != null)
		{
			agent.SetDestination(currentTarget.transform.position);
			if (!animator.GetBool("isAttacking"))
			{
				animator.SetBool("isAttacking", true);
			}
		}
	}

	void LateUpdate()
	{
		//I think it is possible to signal the ZombieController that a state change probably needs to happen. At this point collisions should have been resolved
		//if the last collider has left then the currentTarget == null. So time for a state change
		if (currentTarget == null && this.enabled)
		{
			//this.enabled = false;
			//controller.ContextSwitch();
		}
	}

	public void ProcessContact(GameObject target)
	{
		//Engage the first contact. Yeah the zombies are that hungry and dumb
		if (currentTarget == null)
		{
			currentTarget = target;
			Debug.Log("Engaging target " + currentTarget.name);
			return;
		}
	}

	public void RemoveContact(GameObject target)
	{
		//A dead target will be eventually destroyed.
		//Yes I need to be this specific
		if (currentTarget == null || (target.GetInstanceID() == currentTarget.GetInstanceID()))
		{
			Debug.Log("Removing target " + currentTarget.name);
			currentTarget = null;
			if (animator && animator.GetBool("isAttacking"))
			{
				animator.SetBool("isAttacking", false);
			}
		}
	}
}
