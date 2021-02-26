using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Story defines the story behavior. Player's goals are dictated by this script. Each subplots will be contained in the checkpoint objects
//Collaborates with the Vanguard unit and CheckPoint XXX GameObjects.
//Each checkpoint has a subplot. Each subplot has this story as an observer. Each subplot will tell this Story to activate and move to the next waypoint.
//Current storyline is a linear, go through the zombie gauntlet while checking in at each checkpoints. Victory if player made it alive to the last checkpoint
public class Story : MonoBehaviour
{
    [SerializeField]
    Vanguard m_player;
    [SerializeField]
    MainGUI m_mainGUI;
    HUD m_hud;
    [SerializeField]
    //There are 3 goals. Expand array as you add more goals. I don't know if you serialize a list like this
    const int NUM_WAYPOINTS = 3;
    //Victory waypoint.
    const int VICTORY_WAYPOINT = 3;
    public GameObject[] m_waypoints = new GameObject[NUM_WAYPOINTS];
    // Start is called before the first frame update
    int m_currentWaypoint = 0;
    void Start()
    {
        m_hud = m_mainGUI.GetComponentInChildren<HUD>();
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
        if (Victory())
        {
            m_mainGUI.Victory(5);
            return;
        }
 
        m_waypoints[m_currentWaypoint].SetActive(true);
        SetHUDWaypoint(m_waypoints[m_currentWaypoint].transform);
    }

    bool Victory()
	{
        //Once the last waypoint is completed, this condition will be true
        return (++m_currentWaypoint >= VICTORY_WAYPOINT);
	}
}
