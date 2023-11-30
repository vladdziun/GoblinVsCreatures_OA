using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        animator.SetBool("isOpen", true);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void CloseDoor()
    {
        animator.SetBool("isOpen", false);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
