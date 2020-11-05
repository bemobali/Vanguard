﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ScenePause pauses the game and allows the user to either quit or continue the game
//This script is initially disabled. It needs to be activated and deactivated with the game object because right now, during editing, it does a super annoying
//mouse cursor lock. Somehow OnDisabled gets called during build, and locks the cursor to the middle of the Game Screen.
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
