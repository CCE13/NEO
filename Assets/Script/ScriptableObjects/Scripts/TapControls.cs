using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TouchInput", menuName = "InputController/TouchDetector")]
public class TapControls : InputController
{

    public Vector2 direction;
    public Vector2 DetectTap(Vector2 touchPosition)
    {
        return direction = touchPosition;
    }

    public override Vector2 MoveDirection()
    {
        return direction;
    }
}
