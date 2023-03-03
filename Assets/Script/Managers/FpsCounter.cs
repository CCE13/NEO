
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI FpsText;

    
    // Update is called once per frame
    void Update()
    {
        
        FpsText.text = (1 / Time.unscaledDeltaTime).ToString();
        
    }
}
