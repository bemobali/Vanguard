using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BattleDamage take hits from ballistic projectiles and modifies the Health object with a final tally of the damage
//BattleDamage activates BattleDamageFX, when available
//BattleDamage can be used to simulate critical damage, like head shots that kills the character on 1 hit
public class BattleDamage : MonoBehaviour
{
    //Main health point of the character, not localized joint health, or body part health. 
    public Health characterHealth;
    //Deduct more main health points. Use this to simulate critical, or fatal hits
    [SerializeField, Range(0,100)]
    float extraHealthDeduction = 0f;
    //Optional battle damage special effects, like blood gushing, or decapitation, or damage sprites/textures. Right now 1 battle damage collaborates with 1 fx only
    //BattleDamageFX fx;
    //My first battle damage FX: blood spurt
    [SerializeField]
    GameObject m_bloodSpurt;
    // Start is called before the first frame update
    void Start()
    {
        //fx = gameObject.GetComponent<BattleDamageFX>()
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Takes a damage from a projectile
    public void TakeDamage(RaycastHit hit, float damagePoint)
    {
        //if (fx != null) fx.Damage(hit, damagePoint);
        characterHealth.HealthPoint -= (damagePoint + extraHealthDeduction);
        if (m_bloodSpurt)
		{
            GameObject bloodSpurt = Instantiate(m_bloodSpurt, hit.point, Quaternion.identity);
            //More like rotate the Z axis and line it up with the normal at the hit point. Or not
            //The headshots are not dramatic enough, but should be OK for now
            //bloodSpurt.transform.forward = hit.normal;
            Destroy(bloodSpurt, 0.5f);
		}
    }

    //Takes a damage from a bitch slap
    public void TakeDamage(Collider hit, float damagePoint)
    {
        //if (fx != null) fx.Damage(hit, damagePoint);
        characterHealth.HealthPoint -= (damagePoint + extraHealthDeduction);
    }
}
