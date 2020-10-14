using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRunToSensor : MonoBehaviour
{
	[SerializeField]
	ZombieRunToTarget stateToUpdate = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	//Pick the first target detected that remains. Remember, zombies are stupid!
	void OnTriggerStay(Collider target)
	{
		//if (refreshTarget)
		{
			//Debug.Log(ToString() + " engaging collider in range " + target.gameObject.tag + " position " + target.transform.position.ToString() + " of GameObject " + target.gameObject.name);
			stateToUpdate.ProcessContact(target.gameObject);
			//@todo add target into the target list
			//@note you can use the distance as the key to a SortedList of collider. Just saying.
		}

	}

	void OnTriggerExit(Collider target)
	{
		stateToUpdate.RemoveContact(target.gameObject);
	}
}
