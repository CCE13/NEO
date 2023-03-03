using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeController : MonoBehaviour
{
    public static TimeController sharedInstance;

    [Range(0,1)]public float slowMoTimeScale;
    private float _previousTimeScale;

    public void Awake()
    {
        
        if (sharedInstance == null)
        {
            sharedInstance = this;
            DontDestroyOnLoad(this);
        }
        else if (sharedInstance != null)
        {
            Destroy(this);
        }
    }
    public void Start()
    {
        Time.timeScale = slowMoTimeScale;
        _previousTimeScale = Time.timeScale;
    }

    public virtual void BackToSlowMotion()
    {
        Time.timeScale = slowMoTimeScale;
        _previousTimeScale = Time.timeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        
    }
    public virtual void TransitionToRealTime()
    {
        Time.timeScale = 1f;
        _previousTimeScale = Time.timeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    public static void StopTime()
    {
        Time.timeScale = 0f;
    }
    public float GetPreviousTimeScale()
    {
        return _previousTimeScale;
    }
}


