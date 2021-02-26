using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CheckPointMission implements the mission logic. Right now the logic is to step close to the checkpoint and kill x number of zombies.
//I can do a CRC now, but the collaboration list will get outdated quickly. Just see the member variables for class collaboration.
//Each zombies is expected to know its checkpoint, and properly notify the checkpoint that it is dead.
public class CheckPointMission : MonoBehaviour
{
    [SerializeField]
    HUD m_hud;
    [SerializeField]
    Transform m_playerTransform;
    //If player is this close to the checkpoint, consider it has reached the checkpoint
    [SerializeField, Range(0.1f,2f)]
    float m_completionDistance = 1.0f;
    //Consider checkpoint secured if there are at most this number of zombies
    [SerializeField, Range(0,10)]
    int m_maxNumZombiesAllowed = 10;
    int m_totalZombiesToKill = 0;
    int m_totalZombiesKilled = 0;
    //To be set by the check point mission
    string m_checkPointName;
    const string m_missionStatement = "Reach and Secure Check Point ";
    const string m_sideMissionStatement = "Kill Zombies : ";

    // Start is called before the first frame update
    void Start()
    {
        int totalZombies = 0;
        //The checkpoint should have a spawn points with a number of scripts
        CircularSpawn[] zombieSpawns = gameObject.GetComponentsInChildren<CircularSpawn>();
        foreach (CircularSpawn spawn in zombieSpawns)
        {
            totalZombies += spawn.TotalSpawn;
        }
        m_totalZombiesToKill = totalZombies - m_maxNumZombiesAllowed;
    }

    public string CheckPointName
    {
        get { return m_checkPointName; }
        set { m_checkPointName = value; }
    }

    //Completion condition
    public bool IsMissionCompleted()
    {
        return ((Vector3.Distance(m_playerTransform.position, transform.position) < m_completionDistance) &&
                (m_totalZombiesKilled >= m_totalZombiesToKill));
    }

    string MissionStatement()
    {
        string statement = m_missionStatement + m_checkPointName + Environment.NewLine;
        string sideMissionStatement = m_sideMissionStatement + m_totalZombiesKilled + "/" + m_totalZombiesToKill;
        statement += sideMissionStatement;
        return statement;
    }
    //ProgressUpdate to be called only when a zombie dies
    public void ProgressUpdate()
    {
        m_hud.UpdateMissionStatement(MissionStatement());
    }

    //This is a message handler to respond to a checkpoint's message whenever a zombie dies.
    public void KillOneZombie()
    {
        m_totalZombiesKilled += 1;
        ProgressUpdate();
    }
}
