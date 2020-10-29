using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//M4Shotgun implements the shooting behavior of the shotgun.
public class M4Shotgun : MonoBehaviour
{
    [SerializeField, Range(4,100)]
    uint m_maxCapacity = 8;
    uint m_roundsRemaining;
    //Rate of fire in rounds per second
    [SerializeField, Range(1, 20)]
    float m_rateOfFire = 1f;
    float shotPeriod;
    //Use this to limit the rate of fire
    float shotTimer;
    [SerializeField]
    EjectShell m_shellEjector;
    //HUD to interact with. Particularly the ammo counter and the weapon static image
    [SerializeField]
    HUD m_hud;
    //Weapon image to display to HUD
    [SerializeField]
    Texture2D m_weaponImage;
    public AudioSource boom;
    public AudioSource click;

    #region Laser Pointer
    LineRenderer laserRenderer;
    public GameObject laserPointerBase;//@todo get this from the weapon
    const float laserPointerRange = 100;
    #endregion

    BullshitLaser shotgunBallistics;
    // Start is called before the first frame update
    void Start()
    {
        laserRenderer = GetComponent<LineRenderer>();
        shotgunBallistics = GetComponent<BullshitLaser>();
        m_roundsRemaining = m_maxCapacity;
        shotPeriod = 1 / m_rateOfFire;
    }

    bool CanShoot()
	{
        return (m_roundsRemaining > 0) && (shotTimer > shotPeriod) && shotgunBallistics.enabled;
    }
    public bool Fire()
	{
        if (!CanShoot()) return false;

        //@todo Respect rate of fire
        if (boom.isPlaying) boom.Stop();
        boom.Play();
        m_roundsRemaining -= 1;
        m_hud.BulletCounter.Shoot();
        shotgunBallistics.Shoot();
        m_shellEjector.Eject();
        if (m_roundsRemaining == 0)    //click
		{
            click.Play();
		}
        shotTimer = 0;  //restart the timer
        return true;
	}

    //Add numBullets to remaining rounds
    //numBullets is not always m_maxCapacity because of inventory supply limit
    public void Reload(uint numBullets)
	{
        m_roundsRemaining += numBullets;
        m_hud.BulletCounter.Reload(m_roundsRemaining);
	}
    //Update HUD with the current weapon's bullet count. Use this during weapon swaps
    public void RefreshHUD()
	{
        m_hud.BulletCounter.Reload(m_roundsRemaining);
        m_hud.ChangeWeaponImage(m_weaponImage);
	}

    public uint MaxCapacity
	{
        get { return m_maxCapacity; }
	}

    public uint RoundsRemaining
	{
        get { return m_roundsRemaining; }
	}

    public float RateOfFire
	{
        get { return m_rateOfFire; }
	}
    // Update is called once per frame
    void Update()
    {
        //guarantees that shot timer exceeds shot period before stopping
        if (shotTimer <= shotPeriod)
		{
            shotTimer += Time.deltaTime;
		}

        
        // Draw a line in the Scene View  from the point lineOrigin in the direction of fpsCam.transform.forward * weaponRange, using the color green
        //Debug.DrawRay(lineOrigin, fpsCamera.transform.forward * weaponRange, Color.green,300, false);
        //Vector3 laserOrigin = laserPointerBase.transform.position;
        //laserRenderer.SetPosition(0, laserOrigin);
        //laserRenderer.SetPosition(1, laserOrigin + (laserPointerBase.transform.forward * laserPointerRange));
    }

    //Set the active camera of the shotgun for calculating laserline ballistic. Perhaps other ballistics need it too. Set this to null if the viewer's active camera is not
    //needed
    public void SetActiveCamera(Camera camera)
	{
        shotgunBallistics.ActiveCamera = camera;
    }

    //enable/disable shotgun ballistics. A shotgun with a disabled ballistics cannot shoot
    //This should be a time saving measure so a shotgun is not continuously calculating ballistics.
    public void SetEnableBallistics(bool enable)
	{
        shotgunBallistics.enabled = enable;
	}
}
