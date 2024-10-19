using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    
    public static bool hasRestartedScene = false;
    public static bool noFunPressed = false;
    public static bool sweatPressed = false;
    public static bool hardPressed = false;
    public static bool SkillIssuePressed = false;
    
    
    public void RestartScene()
    {
        hasRestartedScene = true;
        TetrisBlock.ResetStaticVariables();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void EnterShop()
    {
        SceneManager.LoadScene(3);
    }

    public static void SkillIssue()
    {
        SkillIssuePressed = true;
        SceneManager.LoadScene(2);
    }

    public static void Normal()
    {
        SceneManager.LoadScene(2);
    }

    public static void Hard()
    {
        hardPressed = true;
        SceneManager.LoadScene(2);
    }

    public static void Sweat()
    {
        sweatPressed = true;
        SceneManager.LoadScene(2);
    }

    public static void NoFun()
    {
        noFunPressed = true;
        SceneManager.LoadScene(2);
    }

    public void BackToPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    
    public static void ResetRestartFlag()
    {
        hasRestartedScene = false;
    }
}