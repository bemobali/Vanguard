using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Spawn a number of game objects in a circular fashion. The object will be placed on a walkable nav mesh point
public class CircularSpawn : MonoBehaviour
{
    //In game units
    [SerializeField, Range(1f, 50f)]
    float m_radius = 1f;
    //How many game objects are we spawning
    [SerializeField, Range(1, 20)]
    int m_totalToSpawn = 1;
    //What game object are we spawning
    [SerializeField]
    GameObject m_spawnObject;
    // Start is called before the first frame update
    void Start()
    {
        //Number of position sample tries
        const int numTries = 6;
        if (m_spawnObject == null) return;  //Unity should have issued a warning about the null object
        for (int numSpawn=0; numSpawn < m_totalToSpawn; ++numSpawn)
	    {
            Vector3 pos = gameObject.transform.position;
            for (int attempt = 0; (attempt < numTries && !SamplePosition(out pos)); ++attempt)
			{
                float newRadius = m_radius / 2.0f;
                m_radius = Mathf.Clamp(newRadius, 1.0f, newRadius);
			}
            //@todo Research into bunching up prevention
            Instantiate(m_spawnObject, pos, Quaternion.identity); //Worst case scenario: spawn at spawn point position + 1.0f.
	    }
    }

    //Find a space in the NavMesh to spawn a game object. If unable, return false and the position of the spawn point
    bool SamplePosition(out Vector3 result)
	{
        Vector2 point2D = UnityEngine.Random.insideUnitCircle * m_radius;
        Vector3 randomPoint = gameObject.transform.position + new Vector3(point2D.x, 0, point2D.y);
        randomPoint.y = Terrain.activeTerrain.SampleHeight(randomPoint);
        NavMeshHit hit;
        //@todo Use navmesh distance to obstacles as max distance
        if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        Debug.Log(ToString() + " failed to sample navmesh at position " + randomPoint.ToString());
        result = gameObject.transform.position;
        return false;
    }
}
