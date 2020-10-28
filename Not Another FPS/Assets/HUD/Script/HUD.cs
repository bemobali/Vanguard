using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //AmmoCounter encapsulates the UI widget used to display ammo count to the user
    AmmoCounter m_ammoCounter;
    [SerializeField]
    RawImage m_currentWeaponImage;
    // Start is called before the first frame update
    void Start()
    {
        m_ammoCounter = gameObject.GetComponentInChildren<AmmoCounter>();
    }

    public AmmoCounter BulletCounter
    {
        get { return m_ammoCounter; }
    }

    public void ChangeWeaponImage(Texture newTexture)
	{
        m_currentWeaponImage.texture = newTexture;
	}
    //@todo Figure out how to do weapon swaps with different max round capacity
}
