using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Include if using TextMeshPro
public class AnswerManager : MonoBehaviour
{
    private int score; 
    public TextMeshProUGUI correctCount;

    // Method to add points to the score
    public void AddScore(int points)
    {
        score += points;
        correctCount.text = score.ToString(); // Update the UI text
        Debug.Log("Score: " + score); // Optionally log the updated score
    }

    // Method to reset the score
    public void ResetScore()
    {
        score = 0;
        correctCount.text = "0"; // Update the UI text
        Debug.Log("Score reset to 0");
    }

    // Method to get the current score
    public int GetScore()
    {
        return score;
    }
}
