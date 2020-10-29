using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ZombieBitchSlap : MonoBehaviour
{
    //Damage incurred per bitch slap
    [SerializeField, Range(1,100)]
    float hitDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision target)
	{
        Debug.Log(ToString() + " bitch slapping collision target " + target.gameObject.ToString() + " with impulse " + target.impulse.ToString() + " and relative velocity " + target.relativeVelocity.ToString());
        BattleDamage damage = target.gameObject.GetComponent<BattleDamage>();
        if (damage)
		{
            damage.TakeDamage(target.collider, hitDamage);
		}
	}
}
