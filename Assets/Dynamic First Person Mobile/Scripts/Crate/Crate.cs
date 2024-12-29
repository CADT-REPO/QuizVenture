using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Crate : MonoBehaviour
{
    private bool mIsOpen = false;
    private Animator mAnimator;
    public Button openBtn;

    private void Start()
    {
        mAnimator = GetComponent<Animator>();
        if (mAnimator == null)
        {
            Debug.LogError("Animator not found on the Crate object.");
        }
        openBtn.gameObject.SetActive(false); // Ensure the button is active at the start
        openBtn.onClick.AddListener(OnButtonClick); // Add listener for button click
    }

    private void OnTriggerEnter(Collider other)
    {
        TryInteraction(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openBtn.gameObject.SetActive(false); // Hide the button when player walks away
        }
    }

    private void TryInteraction(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openBtn.gameObject.SetActive(true); // Show the button when player is near
        }
    }

    private void OnButtonClick()
    {
        OnInteract();
    }

    public void OnInteract()
    {
        // Debug.Log("Interacted with the crate!");
        mIsOpen = !mIsOpen;
        mAnimator.SetBool("open", mIsOpen);

        string interactText = mIsOpen ? "Press E to close" : "Press E to open";
        // Debug.Log(interactText); // This would update your UI or HUD

        if (mIsOpen)
        {
            openBtn.gameObject.SetActive(false); // Hide the button when the crate is open
            gameObject.SetActive(false); // Hide the crate after it opens
        }
        else
        {
            openBtn.gameObject.SetActive(true); // Show the button when the crate is closed
        }
    }
}
