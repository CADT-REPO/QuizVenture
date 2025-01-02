using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DisplayScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        int score = AnswerManager.instance.GetScore(); // Access the singleton instance of AnswerManager
        scoreText.text = score.ToString(); // Display the score in the UI
    }
}
