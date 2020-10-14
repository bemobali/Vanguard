using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieWalkToSensor : MonoBehaviour
{
	//State for this sensor
	[SerializeField]
	ZombieWalkToTarget stateToUpdate = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	//This is the workhorse for updating the target list
	void OnTriggerStay(Collider target)
	{
		//if (refreshTarget)
		{
			//Debug.Log(ToString() + " detects collider in range " + target.gameObject.tag + " position " + target.transform.position.ToString() + " of GameObject " + target.gameObject.name);
			stateToUpdate.ProcessContact(target.gameObject);
		}

	}

	void OnTriggerExit(Collider target)
	{
		stateToUpdate.RemoveContact(target.gameObject);
	}

	void LateUpdate()
	{
		/*if (refreshTarget)
		{
			//upload list to the responsible state manager
			refreshTarget = false;
			refreshTimer = 0;
		}*/
	}
}
