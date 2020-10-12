using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ActivateRagdollKinematics turns of IsKinematics on all ragdoll Rigidbody
//This allows an Animation to take over the bone and joints movements.
public class ActivateRagdollKinematics : MonoBehaviour
{
    //get_gameObject is not allowed to be called from the constructor. Weird. I wonder if this is a Mono restriction
    /*public ActivateRagdollKinematics()
	{
        Rigidbody[] rigidBody = gameObject.GetComponentsInChildren<Rigidbody>();
        if (rigidBody != null)
        {
            foreach (Rigidbody rb in rigidBody)
            {
                rb.isKinematic = true;
            }
        }
    }*/

    // Start is called before the first frame update
    void Awake()
    {
        Rigidbody[] rigidBody = gameObject.GetComponentsInChildren<Rigidbody>();
        if (rigidBody != null)
		{
            foreach(Rigidbody rb in rigidBody)
            {
                rb.isKinematic = true;
            }
		}
    }
}
