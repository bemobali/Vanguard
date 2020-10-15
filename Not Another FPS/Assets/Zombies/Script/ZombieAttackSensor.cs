using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ZombieAttackSensor : MonoBehaviour
{
	//@todo add a public collider list
	//Sensor target refresh rage in updates per second
	[SerializeField, Range(0.25f, 120f)]
	float refreshRate = 0.25f;
	float refreshTimer;
	bool refreshTarget;
	[SerializeField]
	ZombieAttackTarget stateToModify = null;
    // Start is called before the first frame update
    void Start()
    {
		refreshTimer = 0;
		refreshTarget = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		/*refreshTimer += Time.fixedDeltaTime;
		//Commit the target list to the listener after a specific refresh rate
		if (refreshTimer > 1/refreshRate)
		{
			//turn on target recording. 
			refreshTarget = true;
		}*/
	}

	//This is the workhorse for updating the target list
	void OnTriggerStay(Collider target)
	{
		//if (refreshTarget)
		{
			//Debug.Log(ToString() + " attacking collider in range " + target.gameObject.tag + " position " + target.transform.position.ToString() + " of GameObject " + target.gameObject.name);
			stateToModify.ProcessContact(target.gameObject);
		}
		
	}

	void OnTriggerExit(Collider target)
	{
		stateToModify.RemoveContact(target.gameObject);
	}

	void LateUpdate()
	{
		if (refreshTarget)
		{
			//upload list to the responsible state manager
			refreshTarget = false;
			refreshTimer = 0;
		}
	}
	
}
