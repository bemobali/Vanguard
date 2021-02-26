using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CheckPointWoodsMission defines the subgoal at CheckPoint Woods
//Currently simply kill all zombies.
//Collaborate with HUD for displaying the sub-mission statement
//Each zombies is expected to know its checkpoint, and properly notify the checkpoint that it is dead.
public class CheckPointWoodsMission : MonoBehaviour
{
    [SerializeField]
    Story m_story;
    //Supposed to be artifact to recover
    [SerializeField]
    GameObject m_artifact;
    AudioSource m_sound;
    CheckPointMission m_mission;
    [SerializeField]
    string m_checkPointName = "Forest";
    void Start()
    {
        m_mission = gameObject.GetComponent<CheckPointMission>();
        m_mission.CheckPointName = m_checkPointName;
        m_mission.ProgressUpdate();
        m_sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_mission.IsMissionCompleted())
		{
            m_artifact.SetActive(false);
            m_sound.Play();
            m_story.ProceedToTheNextWaypoint();
            Destroy(m_mission);
            Destroy(this);  //This one should allow remaining gameobjects to stay alive. Just more zombie leftpver to kill
            return;
		}
    }
}
