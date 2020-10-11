using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Zombies.Script
{
	//Enter the dead state. Either play a ragdoll death or animated death.
	//Sink the zombie through the terrain
	//Combining the ragdoll with the main character really complicates the death sequence
	//This death sequence works only on Zombies with an active ragdoll. Currently only useful for Jill, because it is the only zombie that can be dismembered, and I need 
	//the colliders for dismemberment
	public class ZombieDeath: IZombieState
	{
		UnityEngine.GameObject zombieToKill;
		UnityEngine.Animator animator;
		UnityEngine.AI.NavMeshAgent navMeshAgent;
		//Need to disable the colliders so we can sink the ragdoll
		UnityEngine.Collider[] ragdollColliders;
		//Need to turn off isKinematic so gravity will take hold
		UnityEngine.Rigidbody[] ragdollRB;
		//Mini state within a state. Not worth a full state pattern implementation
		bool animatedDeath;
		//Kill zombie once it is under graveDepth
		const float graveDepth = -2f;
		const float startRagdollSink = 3.0f;
		//Sink rate in unit per seconds
		const float sinkRate = -1f;
		float ragdollSinkTimer;
		public ZombieDeath(UnityEngine.GameObject zombie)
		{
			zombieToKill = zombie;
			animator = zombieToKill.GetComponent<UnityEngine.Animator>();
			navMeshAgent = zombieToKill.GetComponent<UnityEngine.AI.NavMeshAgent>();
			//Decide on a death method
			animatedDeath = UnityEngine.Random.Range(0.0f, 1.0f) < 0.6;
			ragdollSinkTimer = 0f;
		}
		public void Start()
		{
		}

		public void Update(float deltaT) 
		{
			if (navMeshAgent.enabled) navMeshAgent.enabled = false;

			ragdollSinkTimer += deltaT;
			//UnityEngine.Debug.Log("Ragdoll sink timer = " + ragdollSinkTimer);
			if (ragdollSinkTimer > startRagdollSink)
			{
				Sink(deltaT);
			}

			if (animatedDeath)
			{
				//Animation event cannot link to any functions here without using a wrapper from a MonoBehaviour script attached to the zombie.
				if (animator.GetBool("isWalking")) animator.SetBool("isWalking", false);
				if (!animator.GetBool("isDead")) animator.SetBool("isDead", true);
				return;
			}

			
			if (ragdollRB == null)
			{
				EnableRigidBodyPhysics();
			}
		}

		public void FixedUpdate(float deltaT) { }

		void EnableRigidBodyPhysics()
		{
			//Let the ragdoll physics takes over
			animator.enabled = false;
			ragdollRB = zombieToKill.GetComponentsInChildren<UnityEngine.Rigidbody>();
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
			ragdollColliders = zombieToKill.GetComponentsInChildren<UnityEngine.Collider>();
			if (ragdollColliders != null)
			{
				foreach (UnityEngine.Collider collider in ragdollColliders)
				{
					//Kill the colliders so we can sink the object
					collider.enabled = false;
				}
			}
		}

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
			zombieToKill.transform.Translate(0f, sinkRate * deltaT, 0f);
			if (zombieToKill.transform.position.y < graveDepth)
			{
				zombieToKill.SetActive(false);
				UnityEngine.Object.Destroy(zombieToKill);
			}
		}
	}
}
