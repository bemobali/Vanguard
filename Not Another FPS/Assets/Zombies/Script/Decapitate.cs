using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decapitate : MonoBehaviour
{
    public GameObject m_ragDoll;
    GameObject m_head;
    // Start is called before the first frame update
    void Start()
    {
        if (m_ragDoll != null)
        {
            m_head = GameObject.Find("Head");   //can't use the transform Find function out-of-the-box. I would have to drill down the transform hierarcy using BFS to find Head
            if (m_head == null)
			{
                Debug.Log("Cannot find Head GameObject instance");
			}
        }
        else
		{
            Debug.Log("Ragdoll game object has not been assigned."); //Most likely the system is already balking at you in the Console
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire1") > 0f)
		{

            Rigidbody headMass = m_head.GetComponent<Rigidbody>();
            if (headMass == null)
			{
                Debug.Log("Not finding the Head Rigidbody component");
                return;
			}
            headMass.isKinematic = false;   //turn off isKinematic so I can break the Head off
            headMass.AddForce(new Vector3(1, 1, 0));
            m_head.transform.parent = null;
		}
    }
}
