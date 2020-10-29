using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//AmmoInventoryDisplay shows the type of ammunition and the ammo count
public class AmmoInventoryDisplay : MonoBehaviour
{
	//Allows user to change the ammo image
	[SerializeField]
	Image m_ammoImage;
	//Shows the ammo count. Maybe later I will start using TextMeshPro to sylize the counter. Or not
	[SerializeField]
	Text m_ammoCount;
	
	//I don't want to rely on Start for the display component because of potential sequencing problem. The player object need this script started when it calls
	//its own start function. Besides, these are children of the AmmoInventory GameObject, so the field assigment can be done at the Prefab level.
	//The prefab is guaranteed to exist in the HUD.

	//There will be time that I need to instantiate an ammo inventor on the fly. So I will need to swap images
	public void SetAmmoImage(Image ammoPic)
	{
		//@todo is this an image swap or a texture swap?
	}

	public void SetAmmoCount(uint count)
	{
		m_ammoCount.text = count.ToString();
	}
}
