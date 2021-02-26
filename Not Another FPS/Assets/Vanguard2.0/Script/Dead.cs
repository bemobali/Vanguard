using Assets.Vanguard.Script;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations;
using MyStuff = Assets.Vanguard.Script;

public class Dead : MonoBehaviour
{
    [SerializeField]
    GameObject fpsCamera;
    [SerializeField]
    GameObject deathCamera;
    [SerializeField]
    GameObject spine;
    Animator animator;
    [SerializeField]
    GameObject HUD;
    [SerializeField]
    MainGUI m_mainGUI;

    const float killAfterTime = 5f;
    float deathTimer;
    bool startDeathTimer;

    void OnEnable()
	{
        Die();
        deathTimer = 0f;
        startDeathTimer = false;
	}

    void Die()
    {
        LookAtConstraint constraint = fpsCamera.GetComponent<LookAtConstraint>();
        if (constraint != null)
        {
            constraint.enabled = false;
        }

        LookAtConstraint chestLookAt = spine.GetComponent<LookAtConstraint>();
        if (chestLookAt != null)
        {
            chestLookAt.enabled = false;
        }
        animator = GetComponent<Animator>();
        animator.SetBool("Dead",true);
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
		{
            Destroy(rb);
		}
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    void Update()
	{
        if (startDeathTimer)
		{
            deathTimer += Time.deltaTime;
            if (deathTimer > killAfterTime)
			{
                Destroy(gameObject);
                return;
			}
		}
	}
    void EnableRagdollPhysics()
    {
        Rigidbody[] rb = gameObject.GetComponentsInChildren<Rigidbody>();
        Collider[] col = gameObject.GetComponentsInChildren<Collider>();
        foreach (Rigidbody rigid in rb)
        {
            rigid.isKinematic = false;
        }

        foreach (Collider collide in col)
        {
            collide.isTrigger = false;
        }
    }

    //Call this from inside the Dead animation state
    public void SwitchToDeathCamera()
    {
        //Debug.Log("Firing SwitchToDeahtCamera");
        HUD.SetActive(false);
        fpsCamera.SetActive(false);
        deathCamera.SetActive(true);
        //Enable physics
        EnableRagdollPhysics();
        m_mainGUI.GameOver(killAfterTime);
    }

    public void DestroyPlayer()
	{
        //Debug.Log("Firing DestroyPlayer");
        startDeathTimer = true;
	}
}
