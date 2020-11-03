using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//ZombieRunToTarget engages the zombie to a specific target. Disengage when contact must be removed
//@note Look at Unity's manual again on contolling animation using nav mesh agent.
public class ZombieRunToTarget : MonoBehaviour
{
	GameObject currentTarget;
	NavMeshAgent agent;
	Animator animator;
	[SerializeField]
	AudioSource m_zombieMoan;
	
	void OnEnable()
	{
		m_zombieMoan.enabled = true;
		m_zombieMoan.Play();
	}

	void OnDisable()
	{
		m_zombieMoan.Stop();
		m_zombieMoan.enabled = false;
	}

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
			agent.SetDestination(currentTarget.transform.position);
			if (!animator.GetBool("isRunning"))
			{
				animator.SetBool("isRunning", true);
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
			animator.SetBool("isRunning", false);
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
			if (animator) animator.SetBool("isRunning", false);
		}
	}
}
