using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputActionManager : MonoBehaviour
{
    [SerializeField] private SwipeAction swipeAction;
    [SerializeField] private SwipeController swipeController;

    public static bool S_Touched;

    private Camera cam;


    private void Awake()
    {
        swipeAction = new SwipeAction();
        cam = Camera.main;
    }

    private void OnEnable()
    {
        swipeAction.Enable();
    }

    private void OnDisable()
    {
        swipeAction.Disable();
    }

    private void Start()
    {
        swipeAction.Touch.PrimaryContact.started += ctx => StartPrimaryTouch(ctx);
        swipeAction.Touch.PrimaryContact.canceled += ctx => EndPrimaryTouch(ctx);
    }


    private void StartPrimaryTouch(InputAction.CallbackContext ctx)
    {
        swipeController.StartTouch(Utils.ScreenToWorld(cam, swipeAction.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.startTime);

        
    }


    private void EndPrimaryTouch(InputAction.CallbackContext ctx)
    {
        swipeController.EndTouch(Utils.ScreenToWorld(cam, swipeAction.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.time);
        
    }

    public Vector2 touchPosition()
    {
        return Utils.ScreenToWorld(cam, swipeAction.Touch.PrimaryPosition.ReadValue<Vector2>());
    }
}
