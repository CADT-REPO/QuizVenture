using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include if using TextMeshPro

// update logic for randomize the question set
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText; // Text component to display the question
    public Button[] answerButtons; // Array of answer buttons
    public TextMeshProUGUI timerText; // Text component for the timer
    public float timeLimit = 10f; // Time limit per question in seconds

    private List<Question> allQuestions = new List<Question>();
    private Question currentQuestion;
    private float timeRemaining;
    private bool isTimerRunning = true;
    private bool correctAnswerSelected = false;

    void Start()
    {
        LoadQuestion();
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

    public void LoadQuestion()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "questionSet.JSON");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            allQuestions = JsonConvert.DeserializeObject<List<Question>>(json);
        }
        else
        {
            Debug.LogError("Questions JSON file not found.");
        }
    }

    public void StartNewQuestion()
    {
        if(allQuestions.Count == 0){
            Debug.LogError("No question available to display");
            return;
        }

        //  Randomly select a question
        int randomIndex = Random.Range(0, allQuestions.Count);
        currentQuestion = allQuestions[randomIndex];
        allQuestions.RemoveAt(randomIndex); //Optionally remove questions to avoid repeats

        // reset timer and start it

        timeRemaining = timeLimit;
        isTimerRunning = true;
        correctAnswerSelected = false;

        // update the question text
        questionText.text = currentQuestion.question;

        // randomize and assign answer button
        List<string> allAnswers = new List<string>(currentQuestion.answers.correct);
        allAnswers.AddRange(currentQuestion.answers.wrong);
        shuffleList(allAnswers);

        for (int i=0; i < answerButtons.Length; i++){
            if(i < allAnswers.Count){
                int index = i;
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = allAnswers[i];
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index, allAnswers[i]));
            }
            else {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnAnswerButtonClicked(int buttonIndex, string selectedAnswer)
    {
        OnAnswerSelected(buttonIndex, selectedAnswer);
    }
    
    public void OnAnswerSelected(int buttonIndex, string selectedAnswer)
    {
        Debug.Log("Button clicked: " + buttonIndex + " - " + selectedAnswer);

       if(currentQuestion.answers.correct.Contains(selectedAnswer)){
            Debug.Log("correct answer!!");
            correctAnswerSelected = true; 
            isTimerRunning = false;
       }
       else {
            Debug.Log("wrong answer. try again");
       }
    }

    void OnTimeOut()
    {
        Debug.Log("Time's up!");
        // Handle time-out logic, e.g., proceed to the next question
    }

    void shuffleList<T>(List<T> list){
        for (int i = list.Count -1; i > 0; i--){
            int j = Random.Range(0, i+1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
