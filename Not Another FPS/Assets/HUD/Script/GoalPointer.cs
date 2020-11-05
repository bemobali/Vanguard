using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GoalPointer manipulates the goal pointing Goal Pointer sprite to point to the goal. Up is pointing forward, 90 points left, -90 point right.
//So the range is [-180, 180) with -180 pointing at the rear of the player
public class GoalPointer : MonoBehaviour
{
    [SerializeField]
    RectTransform m_goalPointerSprite;
    [SerializeField]
    Transform m_activeCamera;
    //I need to compensate the camera pan angle from the player's forward vector. That's why I need this
    [SerializeField]
    Transform m_player; //Should this have its get-set?
    //In degrees
    float m_cameraToPlayerAngle;
    //This must be modifyable by a god scenario/mission function. That's why it is public
    public Transform m_target;
    //Given the nature of the camera being constrained to a point, I think this is a LateUpdate call
    Vector3 m_currentEulerAngles = new Vector3(0f, 0f, 0f);

    void Start()
	{
        m_cameraToPlayerAngle = Vector3.SignedAngle(m_activeCamera.forward, m_player.forward, m_player.up);
    }

    void LateUpdate()
    {
        Vector3 toTarget = m_target.position - m_activeCamera.position;
        //Positive in the clockwise direction. 
        float angle = Vector3.SignedAngle(toTarget.normalized, m_player.forward, m_player.up) - m_cameraToPlayerAngle;
        //UI rotation angles are positive in the COUNTER-CLOCKWISE direction!
        //Also, when using the camera gameobject directly, there is an annoying jump between a positive and negative angle. 
        //Using the body orientation compensated for the camera pan angle from the body seems like the smoothest way to prevent this.
        //Naturally, WHY?
        m_currentEulerAngles.Set(0f, 0f, angle);
        m_goalPointerSprite.eulerAngles = m_currentEulerAngles;
        //m_goalPointerSprite.Rotate(new Vector3(0, 0, angle)); //This is cumulative, make sense
    }

    public Transform ActiveCamera
	{
		get { return m_activeCamera; }
        set
		{
            m_activeCamera = value;
            m_cameraToPlayerAngle = Vector3.SignedAngle(m_activeCamera.forward, m_player.forward, m_player.up);
        }
	}
}
