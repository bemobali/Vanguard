using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BullshitLaser shoots a laser beam ray from the middle of the viewport in the direction of the camera. Whatever target that hits it is the target of the wewpon,
//Practically the weapon becomes just a prop. 
public class BullshitLaser : MonoBehaviour
{
    #region Raycast Bullshit Ballistics
    RaycastHit hitTarget;
    public Camera fpsCamera;
    [SerializeField, Range(0.5f, 1000f)]
    float weaponRange;
    [SerializeField, Range(1, 95)]
    float damagePoint;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
    }

    //Shoot calculates the initial ballistics direction
    public void Shoot()
	{
        Vector3 lineOrigin = fpsCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        // Check if our raycast has hit anything
        if (Physics.Raycast(lineOrigin, fpsCamera.transform.forward, out hitTarget, weaponRange))
        {
            BattleDamage battleDamage = hitTarget.collider.gameObject.GetComponent<BattleDamage>();
            if (battleDamage)
            {
                //damage the target
                battleDamage.TakeDamage(hitTarget, damagePoint);
            }
        }
        else
		{
            Debug.Log("You miss");
		}
    }
    //@note Update is not the correct place to put this function. Line origins is calculated only once per shot.
    //FixedUpdate would be the correct place to update the bullet trajectory
    void Update()
    {
        //Vector3 lineOrigin = fpsCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        //Debug.DrawRay(lineOrigin, fpsCamera.transform.forward * weaponRange, Color.green, 300, false);
    }
}
