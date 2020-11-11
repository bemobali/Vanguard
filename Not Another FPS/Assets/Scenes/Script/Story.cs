using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Story defines the story behavior. Player's goals are dictated by this script. Each subplots will be contained in the checkpoint objects
//Collaborates with the Vanguard unit and CheckPoint XXX GameObjects.
//Each checkpoint has a subplot. Each subplot has this story as an observer. Each subplot will tell this Story to activate and move to the next waypoint.
public class Story : MonoBehaviour
{
    [SerializeField]
    Vanguard m_player;
    [SerializeField]
    HUD m_hud;
    [SerializeField]
    //There are 3 goals. Expand array as you add more goals. I don't know if you serialize a list like this
    const int NUM_WAYPOINTS = 3;
    public GameObject[] m_waypoints = new GameObject[NUM_WAYPOINTS];
    // Start is called before the first frame update
    int m_currentWaypoint = 0;
    void Start()
    {
        if (!m_waypoints[m_currentWaypoint].activeSelf)
        {
            m_waypoints[m_currentWaypoint].SetActive(true);
        }
        SetHUDWaypoint(m_waypoints[m_currentWaypoint].transform);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    void SetHUDWaypoint(Transform newWaypoint)
	{
        GoalPointer goalPointer = m_hud.GetComponentInChildren<GoalPointer>();
        if (goalPointer)
        {
            goalPointer.m_target = newWaypoint;
        }
    }
    //Observer function to be triggered by each waypoint subplots
    public void ProceedToTheNextWaypoint()
	{
        //If I stick with destroying the mission script, I don't need to deactivate the waypoint. Just have to disappear the artifact.
        //m_waypoints[m_currentWaypoint].SetActive(false);
        //We are done. Don't crash the program
        if (++m_currentWaypoint >= NUM_WAYPOINTS) return;
        m_waypoints[m_currentWaypoint].SetActive(true);
        SetHUDWaypoint(m_waypoints[m_currentWaypoint].transform);
    }
}
