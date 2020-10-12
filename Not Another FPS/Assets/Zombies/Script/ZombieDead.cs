using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//ZombieDead defines the zombie's death sequence. Zombie can die using the pre-defined animation, or it can die as a ragdoll
public class ZombieDead : MonoBehaviour
{
	//Some zombies are already ragdolls.
	//However some are not. So set this to indicate that the death sequence will have a ragdoll option
	//Don't set this if your zombie are ragdolls, or if you only want to use the death animation
	//ragdoll game object to clone
	public GameObject ragdoll;
	Animator animator;
	NavMeshAgent navMeshAgent;
	//Select which zombie to kill, this or a ragdoll clone
	GameObject zombieToSink;

	#region Ragdoll Specific Variables
	//Need to disable the colliders so we can sink the ragdoll
	Collider[] ragdollColliders;
	//Need to turn off isKinematic so gravity will take hold
	Rigidbody[] ragdollRB;
	#endregion 
	
	//Call Destroy on zombie once it is under graveDepth
	[SerializeField, Range(-2f, -6f)]
	const float graveDepth = -2f;
	//How many seconds to wait before sinking ragdoll. Leave enough time to visually cue the player that the target is dead
	[SerializeField, Range(1f, 5f)]
	const float startToSink = 3.0f;
	//Sink rate in depth unit per seconds. Negative because the roc is against the terrain's Y  axis
	[SerializeField, Range(-0.5f, -5f)]
	const float sinkRate = -1f;
	//InvokeRepeating period every 40ms
	const float invokeRepeatingPeriod = 0.04f;
	//Update() starts the timer
	float sinkTimer;
	//Mini state within a state. Not worth a full state pattern implementation
	bool animatedDeath;

	void AssignGameComponents()
	{
		animator = gameObject.GetComponent<Animator>();
		navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
		sinkTimer = 0f;
		zombieToSink = this.gameObject;
	}

	void Start()
	{
		AssignGameComponents();
	}

	//I expect this to be done only once. So please have the script disabled initially
	void OnEnable()
	{
		//@todo restore
		/*if (navMeshAgent) Destroy(navMeshAgent);
		animatedDeath = UnityEngine.Random.Range(0.0f, 1.0f) < 0.6;
		if (animatedDeath)
		{
			return;
		}*/
		animatedDeath = false;	//@todo remove
		Joint[] ragdollJoint = gameObject.GetComponents<Joint>();
		if (ragdollJoint.Length == 0)
		{
			//So this is not a ragdoll. Can't do ragdoll animation without the ragdoll.
			if (ragdoll == null)
			{
				animatedDeath = true;
			}
			else
			{
				//ragdollToSink should already have a behavior script that sinks the ragdoll within a pre-determined time.
				GameObject ragdollToSink = Instantiate(ragdoll, transform.position, transform.rotation);
				Destroy(gameObject);
			}
			return;
			//This is the part that bugs me because of the Destroy call. Code gets fragile because this same code also caters for animated ragdoll game objects.
		}
	}

	void Update()
	{
		if (sinkTimer > startToSink)
		{
			Sink(Time.deltaTime);
		}
		else
		{
			sinkTimer += Time.deltaTime;
		}

		if (animatedDeath)
		{
			//Animation event cannot link to any functions here without using a wrapper from a MonoBehaviour script attached to the zombie.
			//@todo abstract the walking and dead concept. This is too much detail for the death because I have 3 more zombie types with their own animations
			if (animator.GetBool("isWalking")) animator.SetBool("isWalking", false);
			if (!animator.GetBool("isDead")) animator.SetBool("isDead", true);
			return;
		}


		if (ragdollRB == null)
		{
			EnableRigidBodyPhysics();
		}
	}

	void FixedUpdate() { }

	void EnableRigidBodyPhysics()
	{
		//Let the ragdoll physics takes over
		animator.enabled = false;
		ragdollRB = zombieToSink.GetComponentsInChildren<UnityEngine.Rigidbody>();
		if (ragdollRB != null)
		{
			foreach (UnityEngine.Rigidbody rb in ragdollRB)
			{
				rb.isKinematic = false;
				//rb.useGravity = false;	//Perfect for DeadSpace.
			}
		}
	}

	void DisableAllColliders()
	{
		ragdollColliders = zombieToSink.GetComponentsInChildren<UnityEngine.Collider>();
		if (ragdollColliders != null)
		{
			foreach (UnityEngine.Collider collider in ragdollColliders)
			{
				//Kill the colliders so we can sink the object
				collider.enabled = false;
			}
		}
	}

	//@todo put this in a separate script and activate to sink the gameObject.
	//Just pull the zombie down using gravity
	void Sink(float deltaT)
	{
		if (animatedDeath)
		{
			EnableRigidBodyPhysics();
		}

		if (ragdollColliders == null)
		{
			DisableAllColliders();
		}
		//This works because sink keeps getting repeated calls from Update() The ragdoll position is not the same as the gameobject position because of gravity
		zombieToSink.transform.Translate(0f, sinkRate * deltaT, 0f);
		if (zombieToSink.transform.position.y < graveDepth)
		{
			zombieToSink.SetActive(false);
			Destroy(zombieToSink);
		}
	}
}