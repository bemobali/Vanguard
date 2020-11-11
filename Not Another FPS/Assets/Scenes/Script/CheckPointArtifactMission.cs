using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CheckPointArtifactMission is the second to final mission. Objective is to collect all 3 artifacts then escort villagers back to the original lz
//Cut n paste from CheckPointWoodsMission for starter. Expect the code to diverge
public class CheckPointArtifactMission : MonoBehaviour
{
    int m_totalZombiesToKill = 0;
    int m_totalZombiesAlive = 0;
    [SerializeField]
    HUD m_hud;
    [SerializeField]
    Transform m_playerTransform;
    [SerializeField]
    Story m_story;
    //If player is this close to the checkpoint, consider it has reached the checkpoint
    [SerializeField]
    float m_completionDistance = 1.0f;
    //Consider checkpoint secured if there are at most this number of zombies
    [SerializeField]
    int m_maxNumZombiesAllowed = 3;

    const string m_missionStatement = "Reach and Secure Check Point Woods";
    const string m_sideMissionStatement = "Kill At Least Zombies : ";
    void Start()
    {
        //The checkpoint should have a spawn points with a number of scripts
        CircularSpawn[] zombieSpawns = gameObject.GetComponentsInChildren<CircularSpawn>();
        foreach (CircularSpawn spawn in zombieSpawns)
        {
            m_totalZombiesToKill += spawn.TotalSpawn;
        }
        m_totalZombiesAlive = m_totalZombiesToKill;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMissionCompleted())
        {
            m_story.ProceedToTheNextWaypoint();
            Destroy(this);
            return;
        }
    }

    //Completion condition
    bool IsMissionCompleted()
    {
        return ((Vector3.Distance(m_playerTransform.position, transform.position) < m_completionDistance) &&
                (m_totalZombiesAlive < m_maxNumZombiesAllowed));
    }

    //ProgressUpdate to be called only when a zombie dies
    void ProgressUpdate()
    {
        string sideMissionStatement = m_sideMissionStatement;
        sideMissionStatement += m_totalZombiesAlive + "/" + m_totalZombiesToKill;
        //@todo update HUD
        Debug.Log(m_missionStatement);
        Debug.Log(sideMissionStatement);
    }

    //This is a message handler to respond to a checkpoint's message whenever a zombie dies.
    public void KillOneZombie()
    {
        m_totalZombiesAlive -= 1;
        ProgressUpdate();
    }
}
