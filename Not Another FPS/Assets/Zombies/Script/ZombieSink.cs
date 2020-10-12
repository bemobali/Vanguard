using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sink a GameObject using gravity. Play with the RigidBody drag to vary the sink speed of the avatar.
public class ZombieSink : MonoBehaviour
{
	//Call Destroy on zombie once it is under graveDepth
	[SerializeField, Range(-2f, -6f)]
	const float graveDepth = -2f;
	//How many seconds to wait before sinking ragdoll. Leave enough time to visually cue the player that the target is dead
	[SerializeField, Range(1f, 5f)]
	const float startToSink = 5.0f;
	//Sink rate in depth unit per seconds. Negative because the roc is against the terrain's Y  axis
	//I need this because the sinking avatar does not drag its GameObject. So I still need to manually sink the Gameobject behind the avatar
	[SerializeField, Range(-0.5f, -5f)]
	const float sinkRate = -1f;
	//Update() starts the timer
	float sinkTimer;
	Collider[] childrenColliders;
	Collider [] parentCollider;

	// Start is called before the first frame update
	void Start()
    {
		sinkTimer = 0;
		parentCollider = gameObject.GetComponents<Collider>();
		childrenColliders = gameObject.GetComponentsInChildren<Collider>();
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
			Debug.Log("Destroying game object " + gameObject.name);
			Destroy(gameObject);
		}
	}

	void DisableAllColliders()
	{
		if (parentCollider != null)
		{
			foreach(Collider col in parentCollider)
			{
				col.enabled = false;
			}
		}
		
		if (childrenColliders != null)
		{
			foreach (Collider collider in childrenColliders)
			{
				//Kill the colliders so we can sink the object
				collider.enabled = false;
			}
		}
	}
}
