using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CheckPointArtifactMission is the second to final mission. Objective is to collect all 3 artifacts then escort villagers back to the original lz
//Cut n paste from CheckPointWoodsMission for starter. Expect the code to diverge
public class CheckPointArtifactMission : MonoBehaviour
{
    [SerializeField]
    string m_checkPointName = "Village";
    [SerializeField]
    Story m_story;
    //If player is this close to the checkpoint, consider it has reached the checkpoint
    [SerializeField]
    float m_completionDistance = 1.0f;
    [SerializeField]
    GameObject m_artifact;
    AudioSource m_sound;
    CheckPointMission m_mission;

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
            Destroy(this);
            return;
        }
    }
}
