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
	public class Controller
	{
		global::Vanguard vanguard;
		public Controller(global::Vanguard obs) => vanguard = obs;
		
		void UpdateLateralInput(float x, float z, bool leftShiftPressed)
		{
			if (x == 0f && z == 0f)
			{
				vanguard.Stay();
				return;
			}
			
			if (leftShiftPressed)
			{
				vanguard.Run(x, z);
			}
			else
			{
				vanguard.Walk(x, z);
			}
		}

		void UpdatePan()
		{
			vanguard.Pan(Input.GetAxis("Mouse X") * vanguard.panSensitivity);
		}

		public void UpdateTilt()
		{
			vanguard.Tilt(Input.GetAxis("Mouse Y") * vanguard.tiltSensitivity);
		}

		public bool Jump()
		{
			return Input.GetAxis("Jump") > 0f;
		}

		public void Update(float deltaT)
		{
			float x = Input.GetAxis("Horizontal") * deltaT;
			float z = Input.GetAxis("Vertical") * deltaT;
			bool leftShiftPressed = Input.GetKey(KeyCode.LeftShift);
			
			UpdateLateralInput(x, z, leftShiftPressed);
			UpdatePan();
		}
	}
}
