using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLevelManager : MonoBehaviour
{

    public float timer;
    public Text timerText;
    public string timerStr;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float seconds = timer % 60;
        timerText.text = seconds.ToString();
    }
}
