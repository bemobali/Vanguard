﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//MainPage handles the behavior of the Main Title Page. Contains the callbacks for the Main Title page menu and buttons.
public class MainPage : MonoBehaviour
{
    //Ensures that there is only 1 MainPage instance created by DontDestroyOnLoad
    static GameObject m_singleton;
    [SerializeField]
    AudioSource m_backgroundMusic;
    [SerializeField]
    GameObject m_controlInstruction;
    void Awake()
	{
        m_backgroundMusic.Play();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (m_singleton == null)
		{
            DontDestroyOnLoad(gameObject);
            //This is where a managed memory shines. I don't have to worry about resource leaks here because the virtual machine handles the cleanup on application exit
            //And this class object is at the top of the application layer
            m_singleton = gameObject;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
	{
        m_backgroundMusic.Stop();
        SceneManager.LoadScene("Viking Valley");
	}

    public void QuitGame()
	{
        Application.Quit(0);
	}

    //Toggle the control instruction on and off
    public void ControlInstruction()
    {
        m_controlInstruction.SetActive(!m_controlInstruction.activeSelf);
    }

}
