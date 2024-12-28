using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    private bool mIsOpen = false;
    private Animator mAnimator;

    private void Start()
    {
        mAnimator = GetComponent<Animator>();
        if (mAnimator == null)
        {
            Debug.LogError("Animator not found on the Crate object.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TryInteraction(other);
    }

    private void TryInteraction(Collider other)
    {
        if (other.CompareTag("Player")) // Use 'E' for interaction
        {
            OnInteract();
        }
    }

    public void OnInteract()
    {
        Debug.LogError("Interacted with the crate!");
        mIsOpen = !mIsOpen;
        mAnimator.SetBool("open", mIsOpen);

        string interactText = mIsOpen ? "Press E to close" : "Press E to open";
        Debug.Log(interactText); // This would update your UI or HUD
    }
}
