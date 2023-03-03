using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    DashHoldingStamina dashHoldingStamina;
    Slider Slider;
    // Start is called before the first frame update
    void Start()
    {
        dashHoldingStamina = GameObject.FindGameObjectWithTag("Player").GetComponent<DashHoldingStamina>();
        Slider = GetComponent<Slider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Slider.value = dashHoldingStamina._currentHoldingTime;
        
    }
}
