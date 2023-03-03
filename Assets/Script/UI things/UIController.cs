using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public abstract class UIController : MonoBehaviour
{
    /// <summary>
    /// Methods can be made virtaul in the later stage of production/>
    /// </summary>

    public virtual void Restart()
    {
        InputActionManager.S_Touched = false;
        Time.timeScale = TimeController.sharedInstance.slowMoTimeScale;
    }
    public virtual void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();  
    }

    public virtual void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
