using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SceneLoader : MonoBehaviour
{

    public enum CheckMethod { DistanceFromPlayer, TriggerPlayer }

    public float LoadRange;
    public bool LoadThisSceneOnStart;
    public bool isLoaded;

    public CheckMethod _checkMethod;
    private GameObject _player;
    // Start is called before the first frame update

    private void Awake()
    {
        if (LoadThisSceneOnStart)
        {
            LoadScene();
        }
    }

    //


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");


    }

    // Update is called once per frame
    void Update()
    {
        if( _checkMethod == CheckMethod.DistanceFromPlayer )
        {
            CheckDistance();
        }   
    }

    private void CheckDistance()
    {
        if(Vector3.Distance(_player.transform.position, transform.position) < LoadRange)
        {
            LoadScene();
        }
        else
        {
            UnLoadScene();
        }
    }

    private void LoadScene()
    {
        if (!isLoaded)
        {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            isLoaded = true;
        }
    }

    private void UnLoadScene()
    {
        if (isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            isLoaded = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( _checkMethod == CheckMethod.TriggerPlayer && collision.CompareTag("Player"))
        {
            LoadScene();
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_checkMethod == CheckMethod.TriggerPlayer && collision.CompareTag("Player"))
        {
            UnLoadScene();
        }
    }
}
