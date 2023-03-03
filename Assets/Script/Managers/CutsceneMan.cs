using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneMan : MonoBehaviour
{
    private void Start()
    {
        ScreenSwipe.instance.Enter();
    }
}
