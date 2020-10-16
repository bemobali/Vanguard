﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//ZombieDead defines the zombie's death sequence. Zombie can die using the pre-defined animation, or it can die as a ragdoll
//Cannot mix ragdoll animation with a physics-based animation. As soon as this script activates, either the Zombie dies as a ragdoll, or using animation
public class ZombieDead : MonoBehaviour
{
	//Some zombies are already ragdolls.
	//However some are not. So set this to indicate that the death sequence will have a ragdoll option
	//Don't set this if your zombie are ragdolls, or if you only want to use the death animation
	//ragdoll game object to clone
	public GameObject ragdoll;
	Animator animator;
	NavMeshAgent navMeshAgent;
	//Need to turn this off to prevent built-in ragdoll from resolving, and fling itself uncontrollably
	CharacterController charController;

	#region Ragdoll Specific Variables
	//Need to disable the colliders so we can sink the ragdoll
	Collider[] ragdollColliders;
	//Need to turn off isKinematic so gravity will take hold
	Rigidbody[] ragdollRB;
	#endregion 
	
	//Mini state within a state. Not worth a full state pattern implementation
	bool animatedDeath;

	void AssignGameComponents()
	{
		if (animator == null) animator = gameObject.GetComponent<Animator>();
		if (navMeshAgent == null) navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
	}

	void Start()
	{
		AssignGameComponents();
	}

	//I expect this to be done only once. So please have the script disabled initially
	void OnEnable()
	{
		AssignGameComponents();
		if (navMeshAgent) navMeshAgent.enabled = false;
		Joint[] ragdollJoint = gameObject.GetComponents<Joint>();
		if (ragdollJoint.Length == 0)
		{
			//So this is not a ragdoll. Can't do ragdoll animation without the ragdoll. So animation is a must
			if (ragdoll == null)
			{
				animatedDeath = true;
			}
		}
		else
		{
			animatedDeath = UnityEngine.Random.Range(0.0f, 1.0f) < 0.6;
		}

		ActivateSink();

		if (animatedDeath)
		{
			//Animation event cannot link to any functions here without using a wrapper from a MonoBehaviour script attached to the zombie.
			//@todo abstract the walking and dead concept. This is too much detail for the death because I have 3 more zombie types with their own animations
			if (animator.GetBool("isWalking")) animator.SetBool("isWalking", false);
			if (!animator.GetBool("isDead")) animator.SetBool("isDead", true);
			return;
		}
		
		if (ragdoll != null)
		{
			//ragdoll to sink should already have a behavior script that sinks the ragdoll within a pre-determined time.
			Instantiate(ragdoll, transform.position, transform.rotation);
			Destroy(gameObject);
			return;
		}
		EnableRigidBodyPhysics();
	}

	void Update()
	{
		//When should we activate sink?
	}

	void FixedUpdate() 
	{
	}

	//If the gameObject is a kinematic ragdoll, this can be called as an event from the animation to enable the ragdoll rigidbodies
	//Another call 
	void EnableRigidBodyPhysics()
	{
		if (ragdollRB != null) return;

		//The collider here conflicts with the built-in ragdoll colliders.
		if (charController == null)
		{
			charController = gameObject.GetComponent<CharacterController>();
			if (charController)
			{
				charController.enabled = false;
			}
		}

		//Let the ragdoll physics takes over
		animator.enabled = false;
		ragdollRB = gameObject.GetComponentsInChildren<UnityEngine.Rigidbody>();
		if (ragdollRB != null)
		{
			foreach (UnityEngine.Rigidbody rb in ragdollRB)
			{
				rb.isKinematic = false;
				//rb.useGravity = false;	//Perfect for DeadSpace.
			}
		}
	}

	//Should I activate sink in the animator, or
	void ActivateSink()
	{
		ZombieSink sink = GetComponent<ZombieSink>();
		if (sink != null)
		{
			sink.enabled = true;
		}
	}
}