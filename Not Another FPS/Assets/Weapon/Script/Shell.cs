using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    float m_selfDestructTimer = 0;
    [SerializeField, Range(1, 10)]
    //Seconds before object self-destruct
    float m_timeToDie = 1f;

    // Update is called once per frame
    void Update()
    {
        m_selfDestructTimer += Time.deltaTime;
        if (m_selfDestructTimer > m_timeToDie)
		{
            Destroy(gameObject);
            return;
		}
    }
}
