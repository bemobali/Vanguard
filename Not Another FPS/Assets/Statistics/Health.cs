using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Health manages the health point of a game character
//Collaborates with anything that modifies healthpoint of a character, such as bullet, hazmat, health pack, and zombie bitch slap
//@note Some day i will consider having a hierarchy of Health, where the character either track of one main health point, and several bone health points.
//Inspired by Mech Commander, where a battle mech can be destroyed with several body parts intact.
public class Health : MonoBehaviour
{
	const float MAX_HEALTH_POINT = 100f;
	[SerializeField, Range(0, 100)]
	float m_healthPoint;
	[SerializeField]
	HUD m_hud = null;   //Optional HUD to update
	public float HealthPoint
	{
		get { return m_healthPoint; }
		set { m_healthPoint = System.Math.Min(value, MAX_HEALTH_POINT); }
	}

	public float MaxHealthPoint
	{
		get { return MAX_HEALTH_POINT;}
	}
	public void Update()
	{
		if (m_hud) m_hud.SetHealth(m_healthPoint);
	}
}
