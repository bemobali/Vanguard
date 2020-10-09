using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JillDecapitate : MonoBehaviour
{
    public GameObject m_ragDoll;
    public GameObject m_headDetachedMesh;
    public GameObject m_head;
    public GameObject m_headMesh;
    public GameObject m_hairMesh;
    [SerializeField]
    public Vector3 shotDirection;
    Rigidbody headMass;
    // Start is called before the first frame update
    void Start()
    {
        if (m_ragDoll != null)
        {
            headMass = m_head.GetComponent<Rigidbody>();
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
        
       
    }
    void FixedUpdate()
    {
        if (Input.GetAxis("Fire1") > 0f)
        {
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