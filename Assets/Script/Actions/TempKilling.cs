using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class TempKilling : MonoBehaviour
{

    public string whoToKillTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == whoToKillTag)
        {
            collision.gameObject.SetActive(false);
        }
    }
}
