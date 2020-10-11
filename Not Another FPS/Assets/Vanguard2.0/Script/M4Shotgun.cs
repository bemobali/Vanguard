using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//M4Shotgun implements the shooting behavior of the shotgun.
public class M4Shotgun : MonoBehaviour
{
    [SerializeField, Range(4,100)]
    int maxCapacity;
    int roundsRemaining;
    //Once I have a generalized firearm class, this range will be applicable
    //[SerializeField, Range(1, 20)]
    const float rateOfFire = 1f;
    const float shotPeriod = 1 / rateOfFire;
    //Use this to limit the rate of fire
    float shotTimer;
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
        roundsRemaining = maxCapacity;
    }

    bool CanShoot()
	{
        return (roundsRemaining > 0) && (shotTimer > shotPeriod);
    }
    public void Fire()
	{
        //@todo Respect rate of fire
        if (CanShoot())
		{
            if (boom.isPlaying) boom.Stop();
            boom.Play();
            roundsRemaining -= 1;
            shotgunBallistics.Shoot();
            if (roundsRemaining == 0)    //click
			{
                click.Play();
			}
            shotTimer = 0;  //restart the timer
        }
	}

    public int RoundsRemaining()
	{
        return roundsRemaining;
	}

    public float RateOfFire()
	{
        return rateOfFire;
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
        Vector3 laserOrigin = laserPointerBase.transform.position;
        laserRenderer.SetPosition(0, laserOrigin);
        laserRenderer.SetPosition(1, laserOrigin + (laserPointerBase.transform.forward * laserPointerRange));
    }
}
