using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class ScreenSwipe : MonoBehaviour
{
    public static ScreenSwipe instance;
    public Animator loadingSwipe;
    public PlayableDirector director;

    public static bool returningToMainmenu;
    public static bool firstTime = true;
    private string nextScene;

    private void Awake()
    {
        instance = this;
        loadingSwipe = GetComponent<Animator>();
        if (firstTime)
        {
            loadingSwipe.Play("Stop");
            firstTime = false;
            return;
        }
        loadingSwipe.Play("Exit");
    }

    private void OnEnable()
    {
        loadingSwipe = GetComponent<Animator>();
        if (loadingSwipe && returningToMainmenu)
        {
            PauseMenuController.S_isPaused = false;
            loadingSwipe.Play("Exit");
            returningToMainmenu = false;
        }
    }

    public void ReturnMainMenu()
    {
        returningToMainmenu = true;
    }

    public void Enter()
    {
        loadingSwipe.Play("Enter");
    }

    public void Respawn()
    {
        loadingSwipe.Play("Respawn");
    }

    public void goNextScene(string scene)
    {
        nextScene = scene;
    }

    public void LoadScene()
    {
        Time.timeScale = 1f;
        if (nextScene.Length < 1) return;
        SceneManager.LoadScene(nextScene);
    }

    public void StartTimeline()
    {
        if (director) director.Play();
    }

    public void StraightToMainMenu()
    {
        ReturnMainMenu();
        goNextScene("MainMenu v2");
        LoadScene();
    }
}
