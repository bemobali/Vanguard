using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//RadarScreen is responsible for drawing the contacts on the RadarScreen image. The radar blip image comes from the GameObject contacts.
//I know this puts a semantic burden on the radar contact object, but it is by far the cheapest in terms of processing overhead. 
//RadarScreen is not responsible for instantiating the radar blips. Not responsible for matching the blips to an actual GameObject.
//Update becomes cheaper because there is no need to nuke a blip list
//The pivot point of the RadarScreen image must be in the center. All calculations depends on the pivot point in the middle of the RadarScreen image.
public class RadarScreen : MonoBehaviour
{
    //Contact source
    [SerializeField]
    Radar m_radar;
    [SerializeField]
    GameObject m_radarBlip;
    //Instead of deleting all m_radarBlip copies, we cache the blips. At every update we do a hash table lookup to Radar to see if the blips are still valid.
    //I think this is faster than reinstantiating the blip images. Registering this as a RadarTarget observer would be the fastest, but can be tricky to synchronize,
    //Especially if radar object pops dynamically.
    Hashtable m_blipTable;
    //Pixel boundary of the scope
    float m_radarScopeRadius;
    void Start()
    {
        RectTransform radarRect = gameObject.GetComponent<RectTransform>();
        m_radarScopeRadius = Mathf.Min(radarRect.rect.width, radarRect.rect.height) / 2f;
        m_blipTable = new Hashtable();
    }

    //
   void Update()
    {
        IDictionaryEnumerator contacts = m_radar.ActiveContacts();
        contacts.Reset();

        while (contacts.MoveNext())
        {
            DictionaryEntry entry = (DictionaryEntry)contacts.Current;
            GameObject contact = (GameObject)entry.Value;
            //This should not happen. Just in case our radar code intentionally keep null contacts, lets not crash
            if (contact == null)
			{
                RemoveRadarBlip((int)entry.Key);
                continue;
			}
            //YES! This code block works!
            Vector3 bearing = contact.transform.position - m_radar.gameObject.transform.position;
            //Debug.Log("World Distance " + bearing.magnitude);
            float angle = Vector3.SignedAngle(m_radar.gameObject.transform.forward, bearing.normalized, m_radar.gameObject.transform.up);
            float distance = m_radarScopeRadius * (bearing.magnitude / m_radar.Range());
            //Debug.Log("(Angle, Scope Distance) = (" + angle.ToString() + " , " + distance.ToString() + ")");
            //Got the angle w.r.t the scope y-axis, got the pixel length of the unit vector, now just need the unit vector
            Vector3 vector = new Vector3(0f, 1f, 0f);
            vector = Quaternion.Euler(0f, 0f, -angle) * vector * distance;
            //Debug.Log("Vector " + vector.ToString());

            GameObject blip;
            int contactId = contact.GetInstanceID();
            if (m_blipTable.ContainsKey(contactId))
            {
                blip = (GameObject)m_blipTable[contactId];
            }
            else
			{
                blip = Instantiate(m_radarBlip, gameObject.transform, false);
                m_blipTable.Add(contactId, blip);
			}
            blip.transform.localPosition = vector;

            /*Don't do this. If it resides in a prefab, Unity won't generally let you do this.
            Image blip = contact.GetComponent<Image>();
            blip.transform.localPosition = vector;
            blip.transform.SetParent(m_radarScope.transform, false);*/
        }
    }

    public void OnDestroy()
    {
        foreach(DictionaryEntry en in m_blipTable)
		{
            GameObject obj = (GameObject)en.Value;
            Destroy(obj);
		}
        m_blipTable.Clear();
	}

    //To be called by the HUD, since I have decided to make the HUD a facade
    public void RemoveRadarBlip(int id)
	{
        if (m_blipTable.ContainsKey(id))
		{
            GameObject blip = (GameObject)m_blipTable[id];
            if (blip)
			{
                blip.transform.SetParent(null);
                Destroy(blip);
			}
            m_blipTable.Remove(id);
        }
	}
}
