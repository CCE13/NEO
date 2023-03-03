using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUI : MonoBehaviour
{

    private void Start()
    {
        PauseMenuController.gamePaused += DisbaleUIElements;
    }

    private void OnDestroy()
    {
        PauseMenuController.gamePaused -= DisbaleUIElements;
    }

    private void DisbaleUIElements()
    {
        if (PauseMenuController.S_isPaused)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                if (child != null)
                    child.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                if (child != null)
                    child.SetActive(true);
            }
        }
    }
}
