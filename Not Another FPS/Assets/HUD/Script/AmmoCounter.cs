using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{
    //Shows the amount of ammunition left in the HUD
    [SerializeField]
    Slider m_counter;
    
    //Reload sets the counter value to the maxLoad. Observer function
    //Call this to reload to max count, or when refreshing the m_counter slider after a weapon swap
    public void Reload(int maxLoad)
	{
        m_counter.value = maxLoad;
	}

    //Reduce ammo 1 at a time. this can be multiple per second
    public void Shoot()
	{
        m_counter.value -= 1f;
	}
}
