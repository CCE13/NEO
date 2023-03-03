using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LoadTimeline : MonoBehaviour
{
    public PlayableDirector yo;
    private void Awake()
    {
        yo = GetComponent<PlayableDirector>();
        yo.Play();
    }
}
