using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScroll : MonoBehaviour
{
    private ScrollRect scrollRect;

    // Start is called before the first frame update

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    public void Snap(Vector2 value)
    {
        var num = Mathf.Round(value.x / (1f / 3f)) * (1f / 3f);

        if(Mathf.Abs(scrollRect.horizontalNormalizedPosition - num) < 0.2f)
        {
            scrollRect.horizontalNormalizedPosition = num;
        }
        else
        {
            scrollRect.horizontalNormalizedPosition = scrollRect.horizontalNormalizedPosition;
        }
        
    }
}
