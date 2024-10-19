using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameObject StartMenu;
    public GameObject TheImpossible;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (SceneController.hasRestartedScene)
        {
            StartMenu.SetActive(false);
        }
        else
        {
            StartMenu.SetActive(true);
        }
        TheImpossible.SetActive(false);
    }

    void Update()
    {
        StartMenuFunc();
    }

    void StartMenuFunc()
    {
        bool isActive = StartMenu.activeSelf;
        if (StartMenu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void StartBtn()
    {
        AudioManager.Instance.backGroundAudioSource.Play();
        StartMenu.SetActive(false);
    }

    public void WaitWhat()
    {
        Time.timeScale = 0;
        TheImpossible.SetActive(true);
    }

}