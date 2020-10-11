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
    public Camera fpsCamera;
    [SerializeField, Range(0.5f, 1000f)]
    float weaponRange;
    [SerializeField, Range(1, 95)]
    float damagePoint;
    #endregion

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
            Debug.Log("Target hit at " + hitTarget.distance + "m at collider " + hitTarget.collider.ToString() + " belonging to " + hitTarget.collider.gameObject.name);
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
            Vector3 lineOrigin = fpsCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            Debug.DrawRay(lineOrigin, fpsCamera.transform.forward * hitTarget.distance, Color.green, Time.deltaTime, true);
        }
        
    }

    //This is why I call this bullshit laser ballistics. I basically play camera tag with the zombies 
    void FixedUpdate()
	{
        Vector3 lineOrigin = fpsCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        // Check if our raycast has hit anything
        targetHit = Physics.Raycast(lineOrigin, fpsCamera.transform.forward, out hitTarget, weaponRange);
	}
}
