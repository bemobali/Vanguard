using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MainGUI handles input for the GUI components of the current Scene.
public class MainGUI : MonoBehaviour
{
    [SerializeField]
    GameObject m_pauseMenu; //ScenePause menu allows the user to quit or continue gaming
    ScenePause m_scenePauseScript; //See the script comment on why I need this. The script is initially disabled.

    void Start()
	{
        m_scenePauseScript = m_pauseMenu.GetComponent<ScenePause>();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
		{
            m_scenePauseScript.enabled = !m_pauseMenu.activeSelf;
            m_pauseMenu.SetActive(!m_pauseMenu.activeSelf);
		}
    }
}
