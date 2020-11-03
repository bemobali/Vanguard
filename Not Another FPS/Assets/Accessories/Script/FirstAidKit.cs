using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//FirstAidkit add health points to the colliding player. Adds partial or full health points depending on the player's need
//@todo Add a visual indicator for used med kits, so player gets the hint of how much it can recharge.
public class FirstAidKit : MonoBehaviour
{
    const float MAX_HEALTH_BOOST = 50;
    //How much health to add for the player from this kit
    [SerializeField, Range(1, MAX_HEALTH_BOOST)]
    float m_healthBoost = 1;
    // Update is called once per frame
    void Update()
    {
        if (m_healthBoost == 0f)
		{
            Destroy(gameObject);
            return;
		}
    }

    void OnTriggerEnter(Collider col)
	{
        GameObject collider = col.gameObject;
        //Layer 12 is player. Got to be a way to dispatch this using a string!
        if (collider.layer != 12) return;
        Health playerHealth = collider.GetComponent<Health>();
        if (playerHealth == null)
        {
            Debug.Log("Player missing its Health script");
            return;
        }
        float recharge = Mathf.Min(playerHealth.MaxHealthPoint - playerHealth.HealthPoint, m_healthBoost);
        playerHealth.HealthPoint += recharge;
        m_healthBoost -= recharge;
	}
}
