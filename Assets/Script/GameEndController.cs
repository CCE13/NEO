using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEndController : UIController
{
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private float countUpDuration;
    private GameTimer _timer;
    private ScoreDisplay _scoreDisplay;
    private Dashing dashing;
    public static bool S_GameEnded;
    private void Awake()
    {
        _timer = FindObjectOfType<GameTimer>();
        _scoreDisplay = FindObjectOfType<ScoreDisplay>();
        dashing = FindObjectOfType<Dashing>();
    }
    private void Start()
    {
        endScreen.SetActive(false);
    }
    public void ShowEndScreen()
    {
        endScreen.SetActive(true);
        AudioManager.Instance.Stop("BackgroundAmbience");
        Debug.Log(ChapterControl.ChapterSelected);
        _timer.StoreTime(ChapterControl.ChapterSelected);
        _scoreDisplay.SetHighscore(ChapterControl.ChapterSelected);
        Time.timeScale = 0.001f;
    }
    public void ResumeTime()
    {
        dashing.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        dashing.playerState = Dashing.PlayerState.Falling;
        Time.timeScale = 1f;
        FindObjectOfType<PauseMenuController>().gameObject.SetActive(false);
    }
    public IEnumerator ShowScore()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        int targetScore = _scoreDisplay.currentScore;
        int currentScore = 0;
        int stepAmount = Mathf.CeilToInt((targetScore - currentScore) / (60 * countUpDuration));
        if (currentScore < targetScore)
        {
            while (currentScore < targetScore)
            {
                currentScore += stepAmount;
                if (currentScore > targetScore)
                {
                    currentScore = targetScore;
                }
                score.text = currentScore.ToString("000000");
                yield return null;
            }
        }
    }
    public IEnumerator ShowTime()
    {
        
        yield return new WaitForSecondsRealtime(countUpDuration);
        float targetMilliseconds = GameTimer.S_millisecondsEnded;
        float currentMilliseconds = 0;
        float targetSeconds = GameTimer.S_secondsEnded;
        float currentSeconds = 0;
        float targetMinutes = GameTimer.S_minutesEnded;
        float currentMinutes = 0;
        float stepAmount = (targetMilliseconds - currentMilliseconds) / (60 * countUpDuration);


        if (currentMilliseconds < targetMilliseconds || currentSeconds < targetSeconds || currentMinutes<targetMinutes)
        {
            while (currentMilliseconds < targetMilliseconds || currentSeconds < targetSeconds || currentMinutes < targetMinutes)
            {
                currentMilliseconds += stepAmount;
                currentSeconds += stepAmount;
                currentMinutes += stepAmount;
                if (currentMilliseconds > targetMilliseconds)
                {
                    currentMilliseconds = targetMilliseconds;
                }

                if (currentSeconds > targetSeconds)
                {
                    currentSeconds = targetSeconds;
                }

                if (currentMinutes > targetMinutes)
                {
                    currentMinutes = targetMinutes;
                }
                timeText.text = $"{currentMinutes.ToString("00")}:{Mathf.FloorToInt(currentSeconds).ToString("00")}:{currentMilliseconds.ToString("000")}";
                yield return null;
            }
        }
       
    }

    public void ClearAnimationPlayerPref()
    {
        PlayerPrefs.DeleteKey("FIRSTTIMEOPENING");
    }
}
