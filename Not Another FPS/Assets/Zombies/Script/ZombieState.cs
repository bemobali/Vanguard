using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Zombies.Script
{
	//Interface for all Zombie states
	interface IZombieState
	{
		void Start();
		void Update(float deltaT);
		void FixedUpdate(float deltaT);
	}
}
