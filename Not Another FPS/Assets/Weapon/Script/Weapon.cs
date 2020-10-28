using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    GameObject m_rightHandAttach, m_leftHandAttach;

    public GameObject RightHandAttach
	{
        get { return m_rightHandAttach; }
	}

    public GameObject LeftHandAttach
	{
        get { return m_leftHandAttach; }
	}

}
