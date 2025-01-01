using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float remainingTime;
    private bool isRunning = false;

    public void StartTimer(float duration)
    {
        remainingTime = duration;
        isRunning = true;
        UpdateTimerText();
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void DeductTime(float amount)
    {
        remainingTime -= amount;
        if (remainingTime < 0)
        {
            remainingTime = 0;
        }
        UpdateTimerText();
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    private void Update()
    {
        if (isRunning)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                isRunning = false;
            }
            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = $"{minutes}mn {seconds:D2}s";
    }
}
