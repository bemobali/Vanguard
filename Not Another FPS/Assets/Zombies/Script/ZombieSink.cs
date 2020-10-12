using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSink : MonoBehaviour
{
	//Call Destroy on zombie once it is under graveDepth
	[SerializeField, Range(-2f, -6f)]
	const float graveDepth = -2f;
	//How many seconds to wait before sinking ragdoll. Leave enough time to visually cue the player that the target is dead
	[SerializeField, Range(1f, 5f)]
	const float startToSink = 3.0f;
	//Sink rate in depth unit per seconds. Negative because the roc is against the terrain's Y  axis
	[SerializeField, Range(-0.5f, -5f)]
	const float sinkRate = -1f;
	//Update() starts the timer
	float sinkTimer;
	Collider[] ragdollColliders;

	// Start is called before the first frame update
	void Start()
    {
		sinkTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
		if (sinkTimer < startToSink) sinkTimer += Time.deltaTime;
		else
		{
			Sink(Time.deltaTime);
		}
	}

	//Just pull the zombie down using gravity
	void Sink(float deltaT)
	{
		DisableAllColliders();
		//This works because sink keeps getting repeated calls from Update() The ragdoll position is not the same as the gameobject position because of gravity
		gameObject.transform.Translate(0f, sinkRate * deltaT, 0f);
		if (gameObject.transform.position.y < graveDepth)
		{
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}

	void DisableAllColliders()
	{
		if (ragdollColliders == null)
		{
			ragdollColliders = gameObject.GetComponentsInChildren<Collider>();
			if (ragdollColliders != null)
			{
				foreach (Collider collider in ragdollColliders)
				{
					//Kill the colliders so we can sink the object
					collider.enabled = false;
				}
			}
		}
		
	}
}
