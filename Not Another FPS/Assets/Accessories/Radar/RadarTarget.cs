using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RadarTarget adds or remove itself against the colliding radar object
//Make sure the Radar Target layer interacts only with a Radar layer. The whole thing hinges on these layer interaction
//Keeps track of its radar source because, ahem, OnTriggerExit is not triggered when the object is destroyed
public class RadarTarget : MonoBehaviour
{
    Hashtable m_radarTrackers = new Hashtable();
    // Start is called before the first frame update
    void Start()
    {
    }

    void OnDestroy()
    {
        foreach(DictionaryEntry trackers in m_radarTrackers)
		{
            Radar radar = (Radar)trackers.Value;
            if (radar)  //This is necessary. A destroyed collider does not generate an exit event.
			{
                radar.RemoveContact(gameObject);
			}
		}
    }

    //Process collision with a Gameobject containing a Radar script. Otherwise ignore
    void OnTriggerEnter(Collider col)
	{
        Debug.Log(gameObject.ToString() + " is getting radar painted by " + col.gameObject.ToString());
        Radar radar = col.gameObject.GetComponent<Radar>();
        if (radar == null) return;
        radar.AddContact(gameObject);
        if (!m_radarTrackers.ContainsKey(col.gameObject.GetInstanceID()))
		{
            m_radarTrackers.Add(col.gameObject.GetInstanceID(), radar);
		}
	}

    //The collision is transitive until the radartarget object gets destroyed.
    void OnTriggerExit(Collider col)
	{
        Debug.Log(gameObject.ToString() + " is leaving radar " + col.gameObject.ToString());
        Radar radar = col.gameObject.GetComponent<Radar>();
        if (radar == null) return;
        radar.RemoveContact(gameObject);
        if (!m_radarTrackers.ContainsKey(col.gameObject.GetInstanceID()))
        {
            m_radarTrackers.Remove(col.gameObject.GetInstanceID());
        }
    }
}
