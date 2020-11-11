using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//BullshitLaser shoots a laser beam ray from the middle of the viewport in the direction of the camera. Whatever target that hits it is the target of the wewpon,
//Practically the weapon becomes just a prop. 
public class BullshitLaser : MonoBehaviour
{
    #region Raycast Bullshit Ballistics
    RaycastHit hitTarget;
    bool targetHit;
    Camera activeCamera;
    [SerializeField, Range(0.5f, 1000f)]
    float weaponRange = 50f;
    [SerializeField, Range(1, 95)]
    float damagePoint = 1f;
    #endregion
    //Target layer is layer 9.
    const int ballisticLayer = 1 << 9;//LayerMask.GetMask("Weapon Targets"); //I don't want to lose this compile-time const.
    // Start is called before the first frame update
    void Start()
    {
        targetHit = false;
    }

    //Shoot calculates the initial ballistics direction
    public void Shoot()
	{
        // Check if our raycast has hit anything
        if (targetHit)
		{
            //Debug.Log("Target hit at " + hitTarget.distance + "m at collider " + hitTarget.collider.ToString() + " belonging to " + hitTarget.collider.gameObject.name);
            BattleDamage battleDamage = hitTarget.collider.gameObject.GetComponent<BattleDamage>();
            if (battleDamage)
            {
                //damage the target
                battleDamage.TakeDamage(hitTarget, damagePoint);
            }
        }
    }
    //@note Update is not the correct place to put this function. Line origins is calculated only once per shot.
    //FixedUpdate would be the correct place to update the bullet trajectory
    void Update()
    {
        if (targetHit)
		{
            Vector3 lineOrigin = activeCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            Debug.DrawRay(lineOrigin, activeCamera.transform.forward * hitTarget.distance, Color.green, Time.deltaTime, true);
        }
        
    }

    //This is why I call this bullshit laser ballistics. I basically play camera tag with the zombies 
    void FixedUpdate()
	{
        Vector3 lineOrigin = activeCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        // Check if our raycast has hit anything
        targetHit = Physics.Raycast(lineOrigin, activeCamera.transform.forward, out hitTarget, weaponRange, ballisticLayer);
	}

    public Camera ActiveCamera
	{
        get { return activeCamera; }
        set { activeCamera = value; }
	}
}
