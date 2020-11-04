using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ScenePause pauses the game and allows the user to either quit or continue the game
public class ScenePause : MonoBehaviour
{
   public void QuitScene()
	{
		SceneManager.LoadScene("OpeningPage");
	}

	void Pause()
	{
		Time.timeScale = 0;
		//note And more to add. There is music and menu interaction, at least
	}

	public void Resume()
	{
		Time.timeScale = 1;
		//Practically reverse everything in Pause()
		gameObject.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
	}

	void OnEnable()
	{
		Pause();
	}

	void OnDisable()
	{
		Resume();
	}
}
