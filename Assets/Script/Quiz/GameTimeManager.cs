using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include if using TextMeshPro 
public class GameTimeManager : MonoBehaviour
{
    public float timer = 10f; // Starting timer in seconds
    public bool isGameRunning = false; // Flag to track if the game is running
    public TextMeshProUGUI gameTimerText; // UI text to display the timer

    private int minutes; // To store the calculated minutes
    private int seconds; // To store the calculated seconds

    // Method to add time to the timer
    public void AddTime(float secondsToAdd)
    {
        timer += secondsToAdd;
    }

    // Method to deduct time from the timer
    public void DeductTime(float secondsToDeduct)
    {
        timer -= secondsToDeduct;
    }

    // Start method called when the script is enabled
    void Start()
    {
        isGameRunning = true;
        timer = Mathf.Ceil(timer); // Ensure the timer starts as an integer
        UpdateTimerText(); // Update the timer UI
        InvokeRepeating("UpdateGameTimer", 0f, 1f); // Start timer updates every second
    }

    // Method to update the game timer
    void UpdateGameTimer()
    {
        if (isGameRunning)
        {
            timer -= 1f; // Decrease the timer by 1 second
            minutes = Mathf.FloorToInt(timer / 60); // Calculate minutes
            seconds = Mathf.FloorToInt(timer % 60); // Calculate seconds

            UpdateTimerText(); // Update the timer UI

            if (timer <= 0)
            {
                EndGame(); // End the game when timer hits zero
            }
        }
    }

    // Method to update the timer text on the UI
    private void UpdateTimerText()
    {
        gameTimerText.text = $"{minutes}mn {seconds:D2}s"; // Format and display the timer
    }

    // Method to handle end game logic
    private void EndGame()
    {
        isGameRunning = false; // Stop the game
        timer = 0; // Set timer to zero
        CancelInvoke("UpdateGameTimer"); // Stop invoking the timer update
    }
}
