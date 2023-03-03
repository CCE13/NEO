using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenuController : UIController
{

    [Header("REQUIRED!!")]
    public GameObject pauseMenu;
    public Animator swordAnimator;
    public bool inSettings;


    public static bool S_isPaused;
    public static Action gamePaused;
    private SavingManager _saveSystem;
    public static event Action Restarting;
    public static bool restarting;
   
    // Start is called before the first frame update
    void Start()
    {
        
        pauseMenu.SetActive(false);
        S_isPaused = false;
        _saveSystem = FindObjectOfType<SavingManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseSetup();
        }
    }
    public void OpenSettings()
    {
        StartCoroutine(OpenSettingsPanel());
        inSettings = true;
    }

    public void CloseSettings()
    {
        StartCoroutine(CloseSettingsPanel());
        inSettings = false;
    }
    public void PauseSetup()
    {
        if (S_isPaused)
            ResumeGame();
        else
            PauseGame();
    }
    public override void Restart()
    {
        
        restarting = true;
        base.Restart();    
        PauseSetup();
        Restarting?.Invoke();

    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void ResumeGame()
    {
        if (inSettings)
        {
            StartCoroutine(CloseSettingsAndPauseMenu());
            inSettings = false;
        }
        else
        {
            StartCoroutine(ClosePauseMenu());
        }
        
        TimeController.sharedInstance.TransitionToRealTime();
        InputActionManager.S_Touched = false;
        S_isPaused = false;
        gamePaused?.Invoke();
    }

    private void PauseGame()
    {
        InputActionManager.S_Touched = false;
        TimeController.StopTime();
        pauseMenu.SetActive(true);
        StartCoroutine(OpenPauseMenu());
        S_isPaused = true;
        gamePaused?.Invoke();

    }

    #region AnimationPlaying
    IEnumerator ClosePauseMenu()
    {
        swordAnimator.Play("Sword Close");
        yield return new WaitForSecondsRealtime(0.3f);
        pauseMenu.SetActive(false);
    }
    IEnumerator OpenPauseMenu()
    {
        swordAnimator.Play("Sword Open");
        yield return null;
    }

    IEnumerator OpenSettingsPanel()
    {
        swordAnimator.Play("Sword Setting Reveal");
        yield return null;
    }

    IEnumerator CloseSettingsPanel()
    {
        swordAnimator.Play("Sword Setting Keep");
        yield return null;
    }

    IEnumerator CloseSettingsAndPauseMenu()
    {
        swordAnimator.Play("Sword Keep and Close");
        yield return new WaitForSecondsRealtime(0.2f);
        pauseMenu.SetActive(false);

    }
    #endregion

}


