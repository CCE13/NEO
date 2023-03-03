using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AstarDebugText : MonoBehaviour
{

    [SerializeField] private RectTransform _arrow;

    [SerializeField] private TextMeshProUGUI g, h, f, p;

    public RectTransform arrow { get => _arrow; set => _arrow = value; }
    public TextMeshProUGUI G { get => g; set => g = value; }
    public TextMeshProUGUI H { get => h; set => h = value; }
    public TextMeshProUGUI F { get => f; set => f = value; }
    public TextMeshProUGUI P { get => p; set => p = value; }
}
