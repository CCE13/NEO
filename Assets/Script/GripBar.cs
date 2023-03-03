using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GripBar : MonoBehaviour
{

    private Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    void Start()
    {
        DashHoldingStamina.setHoldingTime += SetVaue;
    }
    private void OnDestroy()
    {
        DashHoldingStamina.setHoldingTime -= SetVaue;
    }
    // Update is called once per frame

    private void SetVaue(float holdingValue)
    {
        _image.fillAmount = holdingValue;

        bool notDecreasing = _image.fillAmount != 1;

        if (notDecreasing)
        {
            var tempColor = _image.color;
            tempColor.a = 1f;
            _image.color = Color.Lerp(_image.color, tempColor, 0.2f);
            
        }
        else
        {
            var tempColor = _image.color;
            tempColor.a = 0f;
            _image.color = Color.Lerp(_image.color, tempColor, 0.2f);
        }
    }
}
