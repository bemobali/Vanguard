using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CheckPointEntranceMission defines the mission to accomplish while the player progresses towards the gate entrance checkpoint
public class CheckPointEntranceMission : MonoBehaviour
{
    [SerializeField]
    string m_checkPointName = "Entrance";
    [SerializeField]
    Story m_story;
    [SerializeField]
    GameObject m_artifact;
    AudioSource m_sound;
    CheckPointMission m_mission;
    void Start()
    {
        m_mission = GetComponent<CheckPointMission>();
        m_mission.CheckPointName = m_checkPointName;
        m_mission.ProgressUpdate();
        m_sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_mission.IsMissionCompleted())
        {
            m_story.ProceedToTheNextWaypoint();
            m_artifact.SetActive(false);
            m_sound.Play();
            Destroy(m_mission);
            Destroy(this);
            return;
        }
    }
}
