using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Include if using TextMeshPro

public class AnswerManager : MonoBehaviour
{
    private int score;
    public TextMeshProUGUI correctCount;

    // Singleton instance
    public static AnswerManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    // Method to add points to the score
    public void AddScore(int points)
    {
        score += points;
        correctCount.text = $"{score.ToString()} / 10"; // Update the UI text
        Debug.Log("Score: " + score); // Optionally log the updated score
    }

    // Method to reset the score
    public void ResetScore()
    {
        score = 0;
        correctCount.text = "0/10"; // Update the UI text
        Debug.Log("Score reset to 0");
    }

    // Method to get the current score
    public int GetScore()
    {
        return score;
    }
}
