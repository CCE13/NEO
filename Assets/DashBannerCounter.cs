using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DashBannerCounter : MonoBehaviour
{
    private TextMeshPro textMeshPro;
    private Dashing _dashing;
    // Start is called before the first frame update
    void Start()
    {
        _dashing = GameObject.FindGameObjectWithTag("Player").GetComponent<Dashing>();
        textMeshPro = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text = $"Only {_dashing._dashCount} left";
    }
}
