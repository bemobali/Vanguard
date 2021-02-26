using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

//MainGUI handles input for the GUI components of the current Scene.
public class MainGUI : MonoBehaviour
{
    [SerializeField]
    GameObject m_pauseMenu; //ScenePause menu allows the user to quit or continue gaming
    ScenePause m_scenePauseScript; //See the script comment on why I need this. The script is initially disabled.
    [SerializeField]
    GameObject m_gameOver;
    [SerializeField]
    GameObject m_victory;
    //Show gameover for x seconds
    float m_maxGameOverTime = 5f;
    float m_currentGameOverTimer = 0f;
    //This is the clunkyness of not using a coroutine, or invoke
    bool m_gameOverTimerOn = false;
    void Start()
	{
        m_scenePauseScript = m_pauseMenu.GetComponent<ScenePause>();
	}

    // Update is called once per frame
    void Update()
    {
        if (m_gameOverTimerOn)
		{
            m_currentGameOverTimer += Time.deltaTime;
		}

        if (m_currentGameOverTimer > m_maxGameOverTime)
		{
            SceneManager.LoadScene("OpeningPage");
		}

        if (Input.GetKeyDown(KeyCode.Escape))
		{
            m_pauseMenu.SetActive(!m_pauseMenu.activeSelf);
            m_scenePauseScript.enabled = m_pauseMenu.activeSelf;
        }
    }

    public void GameOver(float delay)
	{
        //no need to use coroutines yet, although clunky
        m_gameOver.SetActive(true);
        m_gameOverTimerOn = true;
        m_maxGameOverTime = delay;
	}

    public void Victory(float delay)
	{
        if (m_victory.activeSelf) return;
        m_victory.SetActive(true);
        m_gameOverTimerOn = true;
        m_maxGameOverTime = delay;
    }
}
