using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include if using TextMeshPro
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Collections;
public class QuizManager : MonoBehaviour, IPausable
{
    public GameObject timerObject;
    public GameObject questionScreen;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    // public TextMeshProUGUI timerText;
    public float timeLimit = 10f;

    private List<Question> allQuestions = new List<Question>();
    private Question currentQuestion;
    private float timeRemaining = 10f;
    private bool isTimerRunning = false;
    private bool answerSelected = false; // Track if an answer has been selected

    public GameTimeManager gameTime;
    public QuizTime quizTimer;
    public Button startButton;
    private int minutes;
    private int seconds;
    public AnswerManager answerManager;
    private int totalQuestionsAnswered = 0;
    private List<string> allAnswers;


    public void ManualUpdate()
    {
        print("ManualUpdate");
    }

    void Start()
    {

        quizTimer.StartTimer();
        startButton.onClick.AddListener(OnStartButtonClick);

        questionScreen.SetActive(false);
    }



    void Update()
    {   
        print("Update" + gameTime.timer);
        if (gameTime.timer <= 0)
        {
            endGame();
        }
    }


    public void OnStartButtonClick()
    {
        timerObject.SetActive(true); // Ensure Timer GameObject is active
        quizTimer.StartTimer();
        questionScreen.SetActive(true);
        StateController.PauseGame();

        LoadQuestion();
        StartNewQuestion();
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
        if (allQuestions.Count == 0)
        {
            Debug.LogError("No question available to display");
            return;
        }

        int randomIndex = Random.Range(0, allQuestions.Count);
        currentQuestion = allQuestions[randomIndex];
        allQuestions.RemoveAt(randomIndex);

        answerSelected = false;

        quizTimer.timer = quizTimer.timeLimit; // Reset the timer here
        quizTimer.isTimerRunning = true; // Ensure the timer starts fresh
        quizTimer.StartTimer();

        questionText.text = currentQuestion.question;

        allAnswers = new List<string>();

        if (currentQuestion.answers.correct.Count > 0)
        {
            int correctIndex = Random.Range(0, currentQuestion.answers.correct.Count);
            allAnswers.Add(currentQuestion.answers.correct[correctIndex]);
        }

        if (currentQuestion.answers.wrong.Count > 0)
        {
            List<string> wrongAnswers = new List<string>(currentQuestion.answers.wrong);
            for (int i = 0; i < 3 && wrongAnswers.Count > 0; i++)
            {
                int wrongIndex = Random.Range(0, wrongAnswers.Count);
                allAnswers.Add(wrongAnswers[wrongIndex]);
                wrongAnswers.RemoveAt(wrongIndex);
            }
        }
        shuffleList(allAnswers);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(true);
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = allAnswers[i];

            string answer = allAnswers[i];
            Debug.Log("Answer: " + answer);
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(answer));
        }
        Debug.Log("Question loaded: " + allAnswers);
    }

    public void OnAnswerSelected(string selectedAnswer)
    {
        if (answerSelected) return; // Prevent multiple clicks

        answerSelected = true; // Mark answer as selected    
        quizTimer.EndQuizTime(); // Stop the timer

        Debug.Log("Selected Answer: " + selectedAnswer);
        Debug.Log("Correct Answers: " + string.Join(", ", currentQuestion.answers.correct));

        if (currentQuestion.answers.correct.Exists(answer => answer == selectedAnswer))
        {
            answerManager.AddScore(1);
            gameTime.AddTime(10f);
            Debug.Log("Correct Answer! +10s added. New Score: " + answerManager.GetScore());
            totalQuestionsAnswered++;

            if (totalQuestionsAnswered == 10 && gameTime.timer > 0)
            {
                Debug.Log("You Win!");
                SceneManager.LoadScene("GameWinningScreen");
            }
        }
        else
        {
            // gameTime -= 10f;
            gameTime.DeductTime(10f);
            Debug.Log("Wrong Answer! -10s deducted.");
            if (gameTime.timer <= 0f)
            {
                SceneManager.LoadScene("GameOverScreen");
            }
        }
        questionScreen.SetActive(false);
        StateController.ResumeGame(); // Resume the game after an answer is selected
    }



    void OnTimeOut()
    {
        Debug.Log("Timer expired, entering OnTimeOut()");
        Debug.Log("Timer Value: " + quizTimer.timer);
        questionScreen.SetActive(false);
        StateController.ResumeGame(); // Resume the game if time runs out
    }

    void shuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }


    void endGame()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene("GameOverScreen");
    }


}