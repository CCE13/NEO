using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// controls the main menu elements EXCEPT for the settings panel
/// </summary>
/// 
public class MainMenuController : UIController
{
    public GameObject settingsPanel;
    public string nextLevel;
    public GameObject[] thingsToDeactive;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayGame()
    {
        
        SavingManager.ClearFile();
        SceneManager.LoadScene(nextLevel);

    } 
    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
        animator.Play("MainMenuSettings Open");
    }

    public void CloseSetting()
    {
        StartCoroutine(CloseSettings());
    }
    public IEnumerator CloseSettings()
    {
        animator.Play("MainMenuSettings Close");
        yield return new WaitUntil( ()=> animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);
        settingsPanel.SetActive(false);
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("Time Taken");
        PlayerPrefs.DeleteKey("Chapter 1");
        PlayerPrefs.DeleteKey("Chapter 2");
        PlayerPrefs.DeleteKey("Chapter 3");
    }
}
