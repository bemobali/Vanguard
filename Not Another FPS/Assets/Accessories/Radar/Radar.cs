using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

//Radar manages collider information and expose it to the user
//Collaborates with HUD objects. HUD will query the information from Radar object at every Update() for contact world position
//Implemented as an empty Gameobject with a sphere collider.
//The Gameobject is in the Radar layer, and the layer interacts with Weapon Target, Resupply, and Pickup layer.
//Make sure that the detectable objects have colliders, which they should.
//This is a more expensive approach than making the detectable types register its objects to the radar.
//However, this approach is definitely easier to expand to multiple types on multiple radar objects.
public class Radar : MonoBehaviour
{
    [SerializeField]
    RadarScreen m_radarScreen;
    //This register a RadarContact prefab instance
    Hashtable m_activeContacts;
    SphereCollider m_radarBoundary;
    // Start is called before the first frame update
    void Start()
    {
        m_radarBoundary = gameObject.GetComponent<SphereCollider>();
        m_activeContacts = new Hashtable();
    }

    //This is the more expensive call if the number of target gets big. Can I actually add a listener to GameObjects for Destroy?
    //I need to create a class whose function is to remove a game object from the hash table. This class will have a 
    //public void RemoveContact() that registers itself to the gameobject's event system component.
    void Update()
	{
        //NO NEED TO DO THIS ONCE TARGET KEEPS TRACK OF THE RADAR
        /*List<int> staleKeys = new List<int>();
        //Collect stale object ids
        foreach (DictionaryEntry contact in m_activeContacts)
		{
            GameObject go = (GameObject)contact.Value;
            if (go == null)
			{
                staleKeys.Add((int)contact.Key);    //so this is a dynamic type cast?
			}
		}

        foreach(int key in staleKeys)
		{
            m_activeContacts.Remove(key);
            m_radarScreen.RemoveRadarBlip(key);
        }*/
    }

    //@note Should Radar tells its contacts that it is dead?

    //This is the expensive call, to take care of objects with large colliders.
    //Best practice will be to keep the collider as tightly as possible to the Gameobject
    void OnTriggerStay(Collider col)
	{
        /*int id = col.gameObject.GetInstanceID();
        //Contact originator may not have been added to the contact list
        if (!m_activeContacts.ContainsKey(id)) return;

        Transform contact = col.gameObject.transform;
        if (Vector3.Distance(gameObject.transform.position, contact.position) > m_radarBoundary.radius)
		{
            Debug.Log("OnTriggerStay Radar removing contact " + col.gameObject.ToString());
            m_activeContacts.Remove(id);
		}*/
	}

    //This is the only way to get the contacts managed by a Radar object. Intended user is the RadarScreen script
    public IDictionaryEnumerator ActiveContacts()
	{
        return m_activeContacts.GetEnumerator();
	}

    //Circular range of the radar. Err more like spherical
    public float Range()
	{
        return m_radarBoundary.radius;
	}

    public void RemoveContact(GameObject contact)
    {
        int id = contact.GetInstanceID();
        //Remove id from the contact list.
        if (m_activeContacts.ContainsKey(id))
        {
            //Debug.Log("Radar removing contact " + contact.ToString());
            m_activeContacts.Remove(id);
            m_radarScreen.RemoveRadarBlip(id);
        }
    }

    //Contact calls AddContact to add itself to the radar. If this is a new contact the Radar script will add the object
    //Otherwise Radar will silently reject the contact
    public void AddContact(GameObject target)
	{
        //Identifies the contact
        int id = target.GetInstanceID();
        //Compound colliders must not add the same id at every collision
        if (m_activeContacts.ContainsKey(id)) return;
        //Contact information. At this point I just need the position
        Transform contact = target.transform;
        float distance = Vector3.Distance(contact.position, gameObject.transform.position);
        Debug.Log("Radar contact with object id " + id + " distance " + distance + " radar boundary " + m_radarBoundary.radius);
        //This is causing more trouble than good. Keep your collider tight!
        //if (distance > m_radarBoundary.radius) return;
        m_activeContacts.Add(id, target);
        //Debug.Log("Radar adding contact " + col.gameObject.ToString());
    }

}
