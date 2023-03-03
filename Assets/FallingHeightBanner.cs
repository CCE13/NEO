using UnityEngine;
using TMPro;

public class FallingHeightBanner : MonoBehaviour
{
    TextMeshPro _bannerText;
    Dashing _dashing;

    private void Awake()
    {
        _bannerText = GetComponent<TextMeshPro>();
        _dashing = GameObject.FindGameObjectWithTag("Player").GetComponent<Dashing>();
    }


    private void Update()
    {
        _bannerText.text = $"You Are Going To {Result()}";
    }


    private string Result()
    {
        if (_dashing._mustDie)
        {
            _bannerText.color = Color.red;
            return "Die";
        }
        else
        {
            return "Live";
        }
        
    }

}
