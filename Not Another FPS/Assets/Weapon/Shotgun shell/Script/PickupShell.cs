using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PickupShell allows a  player's trigger collider to pickup the shell
//I have a sweeper collider that allows the player to pickup object at a distance. This collider can only be a trigger because otherwise it will interfere with
//player's rigidbody
public class PickupShell : MonoBehaviour
{
	[SerializeField]
	ShotgunShell m_shell;	//The actual code that handles the pickup
    void OnTriggerEnter(Collider col)
	{
		//Magic numbers : 0 is default layer, 11 is resupply layer
		if (m_shell.UserTimerSelfDestruct || (col.gameObject.layer == 0) || (col.gameObject.layer == 11)) return;
		m_shell.GrabAmmo(col.gameObject.GetComponent<Vanguard>());
	}
}
