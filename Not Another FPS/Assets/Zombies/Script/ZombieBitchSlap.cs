using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ZombieBitchSlap : MonoBehaviour
{
    //Damage incurred per bitch slap
    [SerializeField, Range(1,100)]
    float hitDamage;
    [SerializeField]
    AudioSource m_splatSound;

    //Just in case the script gets enabled/disabled repeatedly
    void OnEnable()
	{
        Collider col = gameObject.GetComponent<Collider>();
        if (col)
		{
            col.enabled = true;
		}
	}

    //When zombie dies, this should execute
    void OnDisable()
	{
        Collider col = gameObject.GetComponent<Collider>();
        if (col)
        {
            col.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider target)
	{
        BattleDamage damage = target.gameObject.GetComponent<BattleDamage>();
        if (damage)
        {
            damage.TakeDamage(target, hitDamage);
            m_splatSound.Play();
        }

    }

    void OnCollisionEnter(Collision target)
	{
        //Debug.Log(ToString() + " bitch slapping collision target " + target.gameObject.ToString() + " with impulse " + target.impulse.ToString() + " and relative velocity " + target.relativeVelocity.ToString());
        BattleDamage damage = target.gameObject.GetComponent<BattleDamage>();
        if (damage)
		{
            damage.TakeDamage(target.collider, hitDamage);
            m_splatSound.Play();
		}
	}
}
