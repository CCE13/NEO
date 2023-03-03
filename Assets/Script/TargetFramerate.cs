using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetFramerate : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }


}
