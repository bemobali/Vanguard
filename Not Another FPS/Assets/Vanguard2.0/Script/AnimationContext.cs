using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Vanguard.Script
{
	//AnimationContext is an animation state manager instance for Vanguard. 
	//Vanguard updates an AnimationContext instance with the current user action as observed from a Controller instance
	//AnimationContext supplies movement speed to the Vanguard instance based on the current animation state
	//Still very easy to get out of sync with the Unit Animator Controller state
	public class AnimationContext
	{
		//IAnimationState interface reflects the Vanguard-specific property or behavior matching the current Unity Animator Controller state
		//State transitions must match the transition in the animator member. The idea is great as long as I can query the next state from animator given a set of parameter settings
		//otherwise the state pattern can be out of sync with the Animator very very quickly
		private interface IAnimationState
		{
		}

		Animator animator;
		public AnimationContext(Animator vanguardAnim) => animator = vanguardAnim;
		public void running(float sideways, float forward)
		{
			animator.SetFloat("Forward", forward);
			animator.SetFloat("Sideway", sideways);
			//animator.SetFloat("RunningSpeedMultiplier", 2.5f);

			//@todo Do I transition to a different animation state, or do I just speed up the animation speed?
			//if (!animator.GetBool("isRunning")) animator.SetBool("isRunning", true);
			//if (animator.GetBool("isWalking")) animator.SetBool("isWalking", false);
		}

		public void walking(float sideways, float forward)
		{
			animator.SetFloat("Forward", forward);
			animator.SetFloat("Sideway", sideways);
			//animator.SetFloat("RunningSpeedMultiplier", 1f);

			//if (animator.GetBool("Running")) animator.SetBool("Running", false);
			//if (!animator.GetBool("Walking")) animator.SetBool("Walking", true);

		}

		public void Jumping()
		{
			animator.SetTrigger("Jump");
		}

		public void Stay()
		{
			animator.SetFloat("Forward", 0f);
			animator.SetFloat("Sideway", 0f);
			//if (animator.GetBool("Running")) animator.SetBool("Running", false);
			//if (animator.GetBool("Walking")) animator.SetBool("Walking", false);
		}

		public void Dead()
		{
			if (!animator.GetBool("Dead")) animator.SetBool("Dead", true);
		}

		public void Firing()
		{
			animator.SetTrigger("Firing");
		}
	}
}
