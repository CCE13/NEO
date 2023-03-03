using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent OnPressed; //Events that will happen when the button is pressed
    [SerializeField] private UnityEvent OnExit; //Events that will happen when the button is not pressed

    List<Collider2D> ObjsOnButton = new List<Collider2D>(); //List on objects which is on the collider
    public bool mainMenu;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        //OnPressed.AddListener(GetComponentInParent<AirLockDoorController>().doorOpen);
        //OnExit.AddListener(GetComponentInParent<AirLockDoorController>().doorClose);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Forcable" || collision.gameObject.tag == "NotForcable")
        {

            ObjsOnButton.Add(collision);

            //Checks if there are objs on it and plays the OnPressed event
            if (ObjsOnButton.Count <= 1)
            {
                OnPressed.Invoke();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Forcable" || collision.gameObject.tag == "NotForcable")
        {

            ObjsOnButton.Remove(collision);

            //Checks if there are no objs on it and plays the OnExit event
            if (ObjsOnButton.Count <= 0)
            {
                OnExit.Invoke();
            }

        }



    }
}

