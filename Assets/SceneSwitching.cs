using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitching : MonoBehaviour
{
    public void SwitchScene(string sceneName)
    {
        ScreenSwipe.instance.goNextScene(sceneName);
        ScreenSwipe.instance.Enter();
    }
}
