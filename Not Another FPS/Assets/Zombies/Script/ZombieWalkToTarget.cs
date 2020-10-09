using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Zombies.Script
{
	//This state stees the zombie to walk to a particular target. The zombie is not committed to the current target yet.
	//If a new, higher priority target pops up, the zombie will switch to that target
	//Targets are ranked by tags. See targetRank function to rank the target
	class ZombieWalkToTarget : IZombieState
	{
		#region From constructor parameter
		//The zombie game object to control
		UnityEngine.GameObject zombie;
		#endregion

		UnityEngine.AI.NavMeshAgent navMeshAgent;
		UnityEngine.GameObject currentTarget;

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

		public void Start() 
		{ }
		
		public void Update(float deltaT)
		{
			//navMeshAgent.SetDestination(currentTarget.transform.position);
		}

		public void FixedUpdate(float deltaT)
		{ }

		//Call this to deal with contacts from the colliders, or any other form of target detection
		public void AddLongRangeContact(UnityEngine.GameObject target)
		{
			//Don't reverse the evaluation order
			if ((currentTarget == null) || higherPriority(target, currentTarget))
			{
				currentTarget = target;
			}
		}

		public void UpdateLongRangeContact(UnityEngine.GameObject target)
		{
			//Find closest target.
			if (higherPriority(currentTarget, target)) return;

			UnityEngine.Vector3 toTarget = target.transform.position - zombie.transform.position;
			UnityEngine.Vector3 toCurrentTarget = currentTarget.transform.position - zombie.transform.position;
			if (toTarget.magnitude < toCurrentTarget.magnitude)
			{
				currentTarget = target;
			}
		}

		public void RemoveLongRangeContact(UnityEngine.GameObject target)
		{ 
			//Yes I need to be this specific
			if (target.GetInstanceID() == currentTarget.GetInstanceID())
			{
				currentTarget = null;
			}
		}

		public void AddMidRangeContact(UnityEngine.GameObject contact) 
		{
			//Transition to run
		}

		public void UpdateMidRangeContact(UnityEngine.GameObject contact)
		{ 
			//don't care
		}

		public void RemoveMidRangeContact(UnityEngine.GameObject contact)
		{ 
			//don't care
		}

		public void AddShortRangeContact(UnityEngine.GameObject contact)
		{ 
			//transition to attack
		}

		public void UpdateShortRangeContact(UnityEngine.GameObject contact)
		{ 
			//don't care
		}

		public void RemoveShortRangeContact(UnityEngine.GameObject contact)
		{ 
			//don't care
		}
	};
}
