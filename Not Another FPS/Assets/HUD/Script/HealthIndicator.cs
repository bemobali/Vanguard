using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{
    //Enable flasher when health reaches critical 
    Animator m_flasher;
    //Need the fill amount here
    [SerializeField]
    Image m_healthArc;
    // Start is called before the first frame update
    Color m_healthy = new Color(0f, 1f, 0.2013595f, 0.5f);
    Color m_healthCaution = new Color(1f, 0.9398658f, 0.240566f, 0.70f);
    Color m_healthCritical = new Color(0.7735849f, 0.174f, 0.08392668f, 0.9f);
    //Color change to caution below this
    const float m_caution = 0.5f;
    //Color change to critical below this
    const float m_critical = 0.25f;
    void Start()
    {
        m_flasher = GetComponent<Animator>();
        m_flasher.enabled = false;
    }

    // Update is called once per frame
    public void SetHealth(float healthPercent)
    {
        m_healthArc.fillAmount = healthPercent;
        if (m_healthArc.fillAmount < m_caution)
		{
            m_flasher.enabled = true;
		}

        if (m_healthArc.fillAmount < m_critical)
		{
            m_healthArc.color = m_healthCritical;
            m_flasher.SetBool("Critical", true);
            return;
        }
        
        if (m_healthArc.fillAmount < m_caution)
		{
            m_healthArc.color = m_healthCaution;
            m_flasher.SetBool("Critical", false);
            return;
		}

        if (m_flasher.enabled) m_flasher.enabled = false;
        m_healthArc.color = m_healthy;
    }
}
