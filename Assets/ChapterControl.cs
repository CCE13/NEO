using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChapterControl : MonoBehaviour
{
    public string chapterNumName;
    public TMP_Text bestScoreText;
    public TMP_Text bestTimeText;
    public string loadSceneID;
    public bool unlocked;
    public static string ChapterSelected;
    private Button button;
    public Color locked;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();

        //checks if the current chapter is the first chapter and unlocks it
        if(chapterNumName =="Chapter 1")
        {
            PlayerPrefs.SetInt(chapterNumName, 1);
        }


        if(PlayerPrefs.GetInt(chapterNumName) == 1)
        {
            button.interactable = true;
            unlocked = true;
            GetComponentInChildren<Image>().color = Color.white;
        }
        else
        {
            button.interactable = false;
            unlocked = false;
            GetComponentInChildren<Image>().color = locked;
        }
        SetTime();
        SetHighScore();
    }
    private void OnValidate()
    {
        chapterNumName = gameObject.name;
    }
    private void SetTime()
    {
        float FastestMinutes = PlayerPrefs.GetFloat($"{chapterNumName}Minutes");
        float FastestSeconds = PlayerPrefs.GetFloat($"{chapterNumName}Seconds");
        float FastestMilleSeconds = PlayerPrefs.GetFloat($"{chapterNumName}MilliSeconds");
        bestTimeText.text = $"{FastestMinutes.ToString("00")}:{Mathf.FloorToInt(FastestSeconds).ToString("00")}:{FastestMilleSeconds.ToString("000")}";
    }
    private void SetHighScore()
    {
        bestScoreText.text = PlayerPrefs.GetInt($"{chapterNumName}Highscore").ToString("00000");
    }

    public void CurrentChapter()
    {
        ChapterSelected = chapterNumName;
    }

    public void ResetPlayerPrefs(string playerPrefToReset)
    {
        PlayerPrefs.DeleteKey(playerPrefToReset);
    }
}
