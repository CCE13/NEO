using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineEvents : MonoBehaviour
{
    //private static bool played = false;
    public GameObject cam;
    private Dashing _player;

    private void Start()
    {
        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        {
            Debug.Log("First Time Opening");
            cam.SetActive(false);
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);

        }
        else
        {
            cam.SetActive(false);
            Debug.Log("NOT First Time Opening");
            gameObject.SetActive(false);
        }
    }
    public void DisablePlayer()
    {
        //if (played)
        //{
        //    return; 
        //}
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Dashing>();
        _player.enabled = false;  
    }
    public void GetPlayerAndSpawn()
    {
        _player.enabled = true;
        //played = true;
    }

    public void NoSlowMo()
    {
        TimeController.sharedInstance.TransitionToRealTime();
    }

    public void PlayMusic(string musicName)
    {
        AudioManager.Instance.PlaySoundSimplified(musicName);
    }
}
