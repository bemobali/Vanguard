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
		public void running()
		{
			if (!animator.GetBool("isRunning")) animator.SetBool("isRunning", true);
			if (animator.GetBool("isWalking")) animator.SetBool("isWalking", false);
		}

		public void walking()
		{
			if (animator.GetBool("isRunning")) animator.SetBool("isRunning", false);
			if (!animator.GetBool("isWalking")) animator.SetBool("isWalking", true);
		}

		public void Jumping()
		{
			animator.SetTrigger("jump");
		}

		public void stay()
		{
			if (animator.GetBool("isRunning")) animator.SetBool("isRunning", false);
			if (animator.GetBool("isWalking")) animator.SetBool("isWalking", false);
		}
	}
}
