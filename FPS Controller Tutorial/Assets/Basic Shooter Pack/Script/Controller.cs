﻿using System;
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
			//@todo Restore
			//return Input.GetAxis("Jump") > 0f;
			return false;
		}

		public void Update(float deltaT)
		{
			//@note this works as long as I don't need to use the axis for animation blending. Now that I need it, the deltaT must come in only when we are calculating
			//world space movement. Now that I am using the Horizontal and Vertical axis user input for animation blending, I need these values unmolested.
			//float x = Input.GetAxis("Horizontal") * deltaT;
			//float z = Input.GetAxis("Vertical") * deltaT;
			float sideways = Input.GetAxis("Horizontal");
			float forward = Input.GetAxis("Vertical");
			bool leftShiftPressed = Input.GetKey(KeyCode.LeftShift);
			
			UpdateLateralInput(sideways, forward, leftShiftPressed, deltaT);
			UpdatePan();
		}
	}
}