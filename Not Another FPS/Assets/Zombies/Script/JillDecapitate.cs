using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JillDecapitate : MonoBehaviour
{
    public GameObject m_ragDoll;
    public GameObject m_headDetachedMesh;
    public Vector3 shotDirection;
    GameObject m_head;
    GameObject m_headMesh;
    GameObject m_hairMesh;
    
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

            m_headMesh = GameObject.Find("Jill_HiRes_Head_Geo");
            if (m_headMesh == null)
			{
                Debug.Log("Cannot find Jill_HiRes_Head_Geo GameObject instanace");
			}

            m_hairMesh = GameObject.Find("Jill_HiRes_Hair_Geo");
            m_headDetachedMesh.transform.SetParent(m_head.transform);
            m_headDetachedMesh.transform.localPosition = new Vector3(0f, 0f, 0f);
            m_headDetachedMesh.SetActive(false);
        }
        else
        {
            Debug.Log("Ragdoll game object has not been assigned."); //Most likely the system is already balking at you in the Console
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Just move Jill
        float z = Time.deltaTime * 0.2f;
        this.transform.position += this.transform.forward * z;
        if (Input.GetAxis("Fire1") > 0f)
        {

            Rigidbody headMass = m_head.GetComponent<Rigidbody>();
            if (headMass == null)
            {
                Debug.Log("Not finding the Head Rigidbody component");
                return;
            }
            if (headMass.isKinematic) headMass.isKinematic = false;   //turn off isKinematic so I can break the Head off
            if (!m_headDetachedMesh.activeSelf)
			{
                headMass.AddForce(shotDirection, ForceMode.Impulse);
                m_head.transform.parent = null;
                m_headMesh.SetActive(false);
                m_hairMesh.SetActive(false);
                m_headDetachedMesh.SetActive(true);
            }
        }
    }
}
/*Key concepts:
 * 1. Use an impulse. See ForeceMode.Impulse
 * 2. Turn off the skinned mesh component. At least they won't be visible when we move the head
 * 3. Need an independent mesh for the detached head. Use meshes that are part of the skinned mesh only. The eyeballs and the jaws are not part of the skinned mesh
 * 4. Set the Head parent to null to completely detach the transformation interaction
 * 5. FIND A WAY TO KILL THE HEAD ANIMATION. The Animation tool shows that the Animation object has a control for the Head. Find this and kill the animation
 * */