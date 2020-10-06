using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Vanguard.Script
{
	//A Controller object takes user input and transforms the Vanguard object according to the user input type
	//Vanguard needs to supply interfaces for walking, running, jumping, strafing, and grenade toss
	//awsd walks the character
	//Shift + awsd runs the character
	//@todo Need to define the responsibility and collaboration of Controller with Vanguard. Right now the Jump definition and the Tilt and Pan definition are not similar
	public class Controller
	{
		global::Vanguard vanguard;
		public Controller(global::Vanguard obs) => vanguard = obs;
		
		void UpdateLateralInput(float x, float z, bool leftShiftPressed, float deltaT)
		{
			if (x == 0f && z == 0f)
			{
				vanguard.Stay();
				return;
			}
			
			if (leftShiftPressed)
			{
				vanguard.Run(x, z, deltaT);
			}
			else
			{
				vanguard.Walk(x, z, deltaT);
			}
		}

		//Pan the player left and right
		void Pan()
		{
			vanguard.Pan(Input.GetAxis("Mouse X") * vanguard.panSensitivity);
		}

		//Tilt the player up and down
		public void Tilt()
		{
			vanguard.Tilt(Input.GetAxis("Mouse Y") * vanguard.tiltSensitivity);
		}

		//Player jump
		public bool Jump()
		{
			return Input.GetAxis("Jump") > 0f;
		}

		//F1 for first person camera. Seriously this kb mapping needs to be finalized during design
		void CheckFirstPersonCamera()
		{ 
			if (Input.GetKeyDown(KeyCode.F1))
			{
				vanguard.SwitchToFirstPersonCamera();
			}
		}

		//F2 for second person camera. Whatever that is
		//F3 for third person camera
		void CheckThirdPersonCamera()
		{
			if (Input.GetKeyDown(KeyCode.F3))
			{
				vanguard.SwitchToThirdPersonCamera();
			}
		}

		public void Update(float deltaT)
		{
			CheckFirstPersonCamera();
			CheckThirdPersonCamera();
		}

		public void FixedUpdate(float deltaT)
		{
			//@note this works as long as I don't need to use the axis for animation blending. Now that I need it, the deltaT must come in only when we are calculating
			//world space movement. Now that I am using the Horizontal and Vertical axis user input for animation blending, I need these values unmolested.
			//float x = Input.GetAxis("Horizontal") * deltaT;
			//float z = Input.GetAxis("Vertical") * deltaT;
			float sideways = Input.GetAxis("Horizontal");
			float forward = Input.GetAxis("Vertical");
			bool leftShiftPressed = Input.GetKey(KeyCode.LeftShift);
			
			UpdateLateralInput(sideways, forward, leftShiftPressed, deltaT);
			Pan();
		}
	}
}
