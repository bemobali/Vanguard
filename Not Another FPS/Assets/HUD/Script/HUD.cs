using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //AmmoCounter encapsulates the UI widget used to display ammo count to the user
    [SerializeField]
    AmmoCounter m_ammoCounter;
    [SerializeField]
    AmmoInventoryDisplay m_ammoInventoryDisplay;
    [SerializeField]
    RawImage m_currentWeaponImage;
    [SerializeField]
    HealthIndicator m_healthIndicator;
    [SerializeField]
    Text m_missionStatement;

    RadarScreen m_radarScreen;
    // Start is called before the first frame update
    void Start()
    {
        //Currently there is only 1 screen. Multiple screen requires call to GetComponentsInChildren. OK in this case English wins. The plural form is more concise
        m_radarScreen = GetComponentInChildren<RadarScreen>();
    }

    //@note I am beginning to think that this leads to confusions. The HUD should act as the facade for the game components. HUD then manages the information display
    public AmmoCounter BulletCounter
    {
        get { return m_ammoCounter; }
    }

    public void ChangeWeaponImage(Texture newTexture)
	{
        m_currentWeaponImage.texture = newTexture;
	}
    //@todo Figure out how to do weapon swaps with different max round capacity
    public void SetHealth(float healthPoint)
	{
        m_healthIndicator.SetHealth(healthPoint / 100f);
	}

    //Right now I have a generalized shotgun shell as the only ammo. Later I will have to add 9mm for the side arm
    //Definitely not responsible for limiting the amount of ammo to add. 
    public void SetAmmoInventory(uint howMany)
	{
        m_ammoInventoryDisplay.SetAmmoCount(howMany);
	}

    public void SetActiveCamera(Camera activeCamera)
	{
        GoalPointer goalPointer = GetComponentInChildren<GoalPointer>();
        if (goalPointer == null)
		{
            Debug.Log("Goal pointer is not supposed to be null");
            return;
		}
        goalPointer.ActiveCamera = activeCamera.transform;
	}

    public void UpdateMissionStatement(string statement)
	{
        m_missionStatement.text = statement;
	}
}
