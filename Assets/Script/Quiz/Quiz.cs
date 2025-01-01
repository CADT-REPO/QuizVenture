using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include if using TextMeshPro
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;


public class Quiz : MonoBehaviour
{
    public GameObject questionScreen;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI correctCount;

    private List<Question> allQuestions = new List<Question>();
    private Question currentQuestion;
    private List<string> allAnswers;

    private TimerManager quizTimer; 

    private int score = 0;
    private int totalQuestionsAnswered = 0;
    private bool isTimerRunning = false;
    private bool answerSelected = false;
    void Start()
    {
        
        isGameRunning = true;
        questionScreen.SetActive(false);
    }
}