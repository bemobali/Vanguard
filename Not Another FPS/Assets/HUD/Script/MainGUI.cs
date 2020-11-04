using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MainGUI handles input for the GUI components of the current Scene.
public class MainGUI : MonoBehaviour
{
    [SerializeField]
    GameObject m_pauseMenu; //ScenePause menu allows the user to quit or continue gaming
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
		{
            m_pauseMenu.SetActive(!m_pauseMenu.activeSelf);
		}
    }
}
