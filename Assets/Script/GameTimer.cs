using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    public bool startTimer;
    private float _secondsPast;
    private int _minutesPast;
    private float _millisecondsPast;

    public static GameTimer instance;
    public static float S_minutesEnded;
    public static float S_secondsEnded;
    public static float S_millisecondsEnded;
    private float _time;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }
    private void Start()
    {
        PlayerPrefs.SetFloat("TimeSaved", 0);
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        startTimer = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            UpdateTime();
        }
        
    }
    private void UpdateTime()
    {
        //secondsPast += Time.unscaledDeltaTime;
        _time = PlayerPrefs.GetFloat("TimeSaved");
        _time += Time.unscaledDeltaTime;
        PlayerPrefs.SetFloat("TimeSaved", _time);
        _minutesPast = (int)(_time / 60f) % 60;
        _secondsPast = (int)(_time % 60f);
        _millisecondsPast = (int)(_time * 1000f) % 1000;
        timeText.text = $"{_minutesPast.ToString("00")}:{Mathf.FloorToInt(_secondsPast).ToString("00")}:{_millisecondsPast.ToString("00")}";
    }


    public void StoreTime(string chapter)
    {
        S_secondsEnded = _secondsPast;
        S_minutesEnded = _minutesPast;
        S_millisecondsEnded = _millisecondsPast;
        SetTime(chapter);
        startTimer = false;
    }
    private void SetTime(string chapter)
    {
        float FastestMinutes = PlayerPrefs.GetFloat($"{chapter}Minutes");
        float FastestSeconds = PlayerPrefs.GetFloat($"{chapter}Seconds");
        float FastestMilleSeconds = PlayerPrefs.GetFloat($"{chapter}MilliSeconds");
        //if no best time
        if (FastestMilleSeconds == 0 && FastestMinutes == 0 && FastestSeconds == 0)
        {
            PlayerPrefs.SetFloat($"{chapter}Minutes", S_minutesEnded);
            PlayerPrefs.SetFloat($"{chapter}Seconds",S_secondsEnded);
            PlayerPrefs.SetFloat($"{chapter}MilliSeconds", S_millisecondsEnded);
            return;
        }
        if (S_minutesEnded < FastestMinutes)
        {
            //is faster
            PlayerPrefs.SetFloat($"{chapter}Minutes", S_minutesEnded);
            PlayerPrefs.SetFloat($"{chapter}Seconds", S_secondsEnded);
            PlayerPrefs.SetFloat($"{chapter}MilliSeconds", S_millisecondsEnded);
            return;
        }
        if (S_minutesEnded == FastestMinutes)
        {
            if (S_secondsEnded < FastestSeconds)
            {
                PlayerPrefs.SetFloat($"{chapter}Seconds", S_secondsEnded);
                PlayerPrefs.SetFloat($"{chapter}MilliSeconds", S_millisecondsEnded);
            }
            return;
        }
        if (S_minutesEnded == FastestMinutes && S_secondsEnded == FastestSeconds)
        {
            if (S_millisecondsEnded < FastestMilleSeconds)
            {
                PlayerPrefs.SetFloat($"{chapter}MilliSeconds", S_millisecondsEnded);
            }
            return;

        }
    }
}
