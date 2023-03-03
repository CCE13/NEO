using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public string levelToUnlock;

    public void UnlockLevel()
    {
        PlayerPrefs.SetInt(levelToUnlock, 1);
    }
}
