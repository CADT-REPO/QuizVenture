using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crate : MonoBehaviour 
{
    // public QuizManager quizManager;
    private bool mIsOpen = false;
    private Animator mAnimator;
    public Button openBtn;
    public Canvas btnCanvas;
    public Transform player; 
    // public TextMeshProUGUI timerText;
    public float activationDistance = 2.0f; 

    // public TimerManager boxTimer;
    private void Start()
    {
        mAnimator = GetComponent<Animator>();
        if (mAnimator == null)
        {
            Debug.LogError("Animator not found on the Crate object.");
        }

        // btnCanvas.gameObject.SetActive(false); // Hide the button when the game starts
        openBtn.gameObject.SetActive(false); // Hide the button when the game starts
        openBtn.onClick.AddListener(OnButtonClick); // Add listener for button click
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not assigned.");
            return;
        }

        // Calculate the distance between the player and the crate
        float distance = Vector3.Distance(transform.position, player.position);

        // Show or hide the button canvas based on distance
        if (distance <= activationDistance)
        {
            openBtn.gameObject.SetActive(true); // Show the button
            // btnCanvas.gameObject.SetActive(true); // Show the button
        }
        else
        {
            openBtn.gameObject.SetActive(false); // Hide the button
            // btnCanvas.gameObject.SetActive(false); // Hide the button
        }

    }

    private void OnButtonClick()
    {
        OnInteract();
    }

    public void OnInteract()
    {
        // Toggle the crate's open state
        mIsOpen = !mIsOpen;
        mAnimator.SetBool("open", mIsOpen);
        GetComponent<Animator>().SetBool("open", mIsOpen);
        
        
        // Handle the button visibility
        if (mIsOpen)
        {
            // quizManager.OnStartButtonClick();
            // boxTimer.StartTimer(10f); 
            openBtn.gameObject.SetActive(false); // Hide the button when the crate is open
            // btnCanvas.gameObject.SetActive(false); // Hide the button when the crate is open
            this.gameObject.SetActive(false); // Hide the crate itself
        }
    }
}

// using UnityEngine;
// using UnityEngine.UI;

// public class Crate : MonoBehaviour
// {
//     private bool mIsOpen = false;
//     private Animator mAnimator;
//     public Button openBtn;
//     // public QuizManager quizManager; // Reference to QuizManager

//     private void Start()
//     {
//         mAnimator = GetComponent<Animator>();
//         if (mAnimator == null)
//         {
//             Debug.LogError("Animator not found on the Crate object.");
//         }
//         openBtn.gameObject.SetActive(false); // Hide the button at the start
//         openBtn.onClick.AddListener(OnButtonClick); // Add a listener for the button
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         TryInteraction(other); // Check if the player enters the trigger
//     }

//     private void OnTriggerExit(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             openBtn.gameObject.SetActive(false); // Hide the button when the player leaves
//         }
//     }

//     private void TryInteraction(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             openBtn.gameObject.SetActive(true); // Show the button when the player is nearby
//         }
//     }

//     private void OnButtonClick()
//     {
//         OnInteract(); // Handle the interaction when the button is clicked
//     }

//     public void OnInteract()
//     {
//         // Open the quiz when the button is clicked
//         if (!mIsOpen)
//         {
//             // quizManager.StartQuiz(); // Start the quiz via QuizManager
//             openBtn.gameObject.SetActive(false); // Hide the button
//         }
//     }
// }
