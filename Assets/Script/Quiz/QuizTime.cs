using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuizTime : MonoBehaviour
{
    public float timeLimit = 10f;
    public float timer = 10f;
    public bool isTimerRunning = false;
    public TextMeshProUGUI timerText;
    public GameObject questionScreen;
    public GameTimeManager gameTime;
    void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        if (!gameObject.activeInHierarchy) return; // Ensure GameObject is active
        timer = timeLimit;
        isTimerRunning = true;
        timer = Mathf.Ceil(timer);
        UpdateTimerText();
        StartCoroutine(UpdateQuizTimerCoroutine());
    }

    IEnumerator UpdateQuizTimerCoroutine()
    {
        while (isTimerRunning)
        {
            float elapsedTime = Time.unscaledDeltaTime; // Using unscaledDeltaTime
            timer -= elapsedTime;
            UpdateTimerText();

            if (timer <= 0)
            {
                EndQuizTime();
            }

            yield return null; // Wait until next frame
        }
    }

    public void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = $"{Mathf.Ceil(timer)}s";
        }
    }

    public void EndQuizTime()
    {
        isTimerRunning = false;
        timer = 0;
        questionScreen.SetActive(false);
        gameTime.DeductTime(10f); 
        StateController.ResumeGame();
    }
}
