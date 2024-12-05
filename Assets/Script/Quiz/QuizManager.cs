/* using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // Include if using TextMeshPro

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText; // Text component to display the question
    public Button[] answerButtons; // Array of answer buttons
    public TextMeshProUGUI timerText; // Text component for the timer
    public float timeLimit = 10f; // Time limit per question in seconds

    private float timeRemaining;
    private bool isTimerRunning = true;

    void Start()
    {
        StartNewQuestion();
        // timeRemaining = timeLtimeLimitimit;
        isTimerRunning = true;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timeRemaining).ToString();

            if (timeRemaining <= 0)
            {
                isTimerRunning = false;
                timerText.text = "0";
                // Trigger the logic for what happens when the timer runs out
                OnTimeOut();
            }
        }

    //      if (isTimerRunning)
    // {
    //     if (timeRemaining > 0)
    //     {
    //         timeRemaining -= Time.deltaTime; // Decrease timer
    //         Debug.Log("Time remaining: " + timeRemaining); // For debugging
    //     }
    //     else
    //     {
    //         isTimerRunning = false; // Stop the timer
    //         Debug.Log("Time is up!");
    //         HandleTimeOut(); // Handle when time runs out
    //     }
    // }
    }


    public void StartNewQuestion()
    {
        // Reset timer and start it
        timeRemaining = timeLimit;
        isTimerRunning = true;

        // Update the question and button texts here
        questionText.text = "What's the point of living?";

        // Example of setting button text
        answerButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = "to eat and die";
        answerButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = "to love and die";
        answerButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = "to earn money and die";
        answerButtons[3].GetComponentInChildren<TextMeshProUGUI>().text = "to grow old and die";

        //  // Attach listeners dynamically (optional if not using Unity's Inspector)
        // for (int i = 0; i < answerButtons.Length; i++)
        // {
        //     int index = i; // Capture loop variable
        //     answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        // }
    }

    public void OnAnswerSelected(int buttonIndex)
    {
        Debug.Log("Button clicked: " + buttonIndex);
        isTimerRunning = false;

        // Check if the answer is correct and handle scoring here
        if (buttonIndex == 3) // Replace with actual answer checking logic
        {
            Debug.Log("Correct Answer!");
        }
        else
        {
            Debug.Log("Wrong Answer.");
        }

        // Logic for the next question or game over can be added here
    }

    void OnTimeOut()
    {
        Debug.Log("Time's up!");
        // Handle time-out logic such as displaying a message or going to the next question
    }

    //  private void HandleTimeOut()
    // {
    //     Debug.Log("Time ran out. Moving to the next question.");
    //     // Implement logic to handle timeouts (e.g., show "time's up" feedback, score penalties)
    //     StartNewQuestion(); // Load the next question
    // }



}
 */


 using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include if using TextMeshPro

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText; // Text component to display the question
    public Button[] answerButtons; // Array of answer buttons
    public TextMeshProUGUI timerText; // Text component for the timer
    public float timeLimit = 10f; // Time limit per question in seconds

    private float timeRemaining;
    private bool isTimerRunning = true;
    private bool correctAnswerSelected = false;

    void Start()
    {
        StartNewQuestion();
    }

    void Update()
    {
        if (isTimerRunning && !correctAnswerSelected)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timeRemaining).ToString();

            if (timeRemaining <= 0)
            {
                isTimerRunning = false;
                timerText.text = "0";
                OnTimeOut();
            }
        }
    }

    public void StartNewQuestion()
    {
        // Reset timer and start it
        timeRemaining = timeLimit;
        isTimerRunning = true;
        correctAnswerSelected = false;

        // Update the question and button texts
        questionText.text = "What's the point of living?";
        string[] answers = {
            "to eat and die",
            "to love and die",
            "to earn money and die",
            "to grow old and die"
        };

        // Assign button texts and listeners dynamically
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // Capture loop variable
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answers[i];
            answerButtons[i].onClick.RemoveAllListeners(); // Clear previous listeners
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }
    }

    public void OnAnswerSelected(int buttonIndex)
    {
        Debug.Log("Button clicked: " + buttonIndex);

        // Check if the selected answer is correct
        if (buttonIndex == 3) // Replace with your correct answer logic
        {
            Debug.Log("Correct Answer!");
            correctAnswerSelected = true; // Stop the timer once the correct answer is chosen
            isTimerRunning = false; // Optionally stop the timer logic if not needed after this
        }
        else
        {
            Debug.Log("Wrong Answer. Try again!");
        }
    }

    void OnTimeOut()
    {
        Debug.Log("Time's up!");
        // Handle time-out logic, e.g., proceed to the next question
    }
}
