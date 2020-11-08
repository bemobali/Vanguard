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
	[SerializeField]
	AudioSource m_angryZombie;

	//During the attack phase, use manual movement control, because I don't want the character controller to swipe away the player's rigidbody
	CharacterController m_controller;
	Rigidbody m_rb;
	CapsuleCollider m_collider;

	void OnEnable()
	{
		if (m_controller) m_controller.enabled = false;
		if (m_collider) m_collider.enabled = true;
		m_angryZombie.enabled = true;
		m_angryZombie.Play();
	}

	void OnDisable()
	{
		m_controller.enabled = true;
		m_collider.enabled = false;
		m_angryZombie.Stop();
		m_angryZombie.enabled = false;
	}

	// Start is called before the first frame update
	void Start()
	{
		agent = gameObject.GetComponent<NavMeshAgent>();
		animator = gameObject.GetComponent<Animator>();
		m_controller = gameObject.GetComponent<CharacterController>();
		m_rb = gameObject.GetComponent<Rigidbody>();
		m_collider = gameObject.GetComponent<CapsuleCollider>();
		m_controller.enabled = false;
		m_collider.enabled = true;
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
			gameObject.transform.LookAt(currentTarget.transform);
			if (agent.enabled) agent.isStopped = true; //does not work
			/*
			if (!animator.applyRootMotion)
			{
				//This is giving a weird behaviour on rooted animation. Almost like a numeric instability. 
				//Took me 2 hours to figure this out
				gameObject.transform.LookAt(currentTarget.transform);   
			}*/
			
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
		//There is a suggestion in Unity forum to dissable collision detection when the target is dead, so it can trigger this object's OnTriggerExit
		//Engage the first live contact. Yeah the zombies are that hungry and dumb
		if (currentTarget == null)
		{
			currentTarget = target;
			//Debug.Log(ToString() + "Engaging target " + currentTarget.name);
			return;
		}
		//Remove dead target contact
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
