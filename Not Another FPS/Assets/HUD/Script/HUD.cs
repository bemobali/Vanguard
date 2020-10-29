using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //AmmoCounter encapsulates the UI widget used to display ammo count to the user
    [SerializeField]
    AmmoCounter m_ammoCounter;
    [SerializeField]
    RawImage m_currentWeaponImage;
    [SerializeField]
    Image m_healthSlider;
    Color m_healthy = new Color(0f, 1f, 0.2013595f, 0.4980392f);
    Color m_healthCaution = new Color(1f, 0.9398658f, 0.240566f, 0.4980392f);
    Color m_healthCritical = new Color(0.7735849f, 0.174f, 0.08392668f, 0.4980392f);
    // Start is called before the first frame update
    void Start()
    {
    }

    public AmmoCounter BulletCounter
    {
        get { return m_ammoCounter; }
    }

    public void ChangeWeaponImage(Texture newTexture)
	{
        m_currentWeaponImage.texture = newTexture;
	}
    //@todo Figure out how to do weapon swaps with different max round capacity

    public void SetHealth(float healthPoint)
	{
        float healthPercent = healthPoint / 100f;
        m_healthSlider.fillAmount = healthPercent;
        if (healthPercent < 0.25f)
		{
            m_healthSlider.color = m_healthCritical;
            return;
		}

        if (healthPercent < 0.5f)
		{

            m_healthSlider.color = m_healthCaution;
            return;
		}

        m_healthSlider.color = m_healthy;
	}

}
