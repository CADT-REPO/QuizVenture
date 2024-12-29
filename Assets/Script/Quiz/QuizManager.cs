using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include if using TextMeshPro
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
public class QuizManager : MonoBehaviour
{
    public static QuizManager quizManager;
    public GameObject questionScreen;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI timerText;
    public float timeLimit = 10f;

    private List<Question> allQuestions = new List<Question>();
    private Question currentQuestion;
    private float timeRemaining;
    private bool isTimerRunning = true;
    private bool answerSelected = false; // Track if an answer has been selected

    private float gameTime = 120f;
    private bool isGameRunning = false;
    public TextMeshProUGUI gameTimerText;
    public Button startButton;
    public TextMeshProUGUI correctCount;
    private int minutes;
    private int seconds;
    private int score = 0;
    private int totalQuestionsAnswered = 0;
    private List<string> allAnswers;



    void Awake()
    {
        if (quizManager == null)
        {
            quizManager = this;
            DontDestroyOnLoad(gameObject);
            LoadQuestion();
            Debug.Log("QuizManager initialized.");
        }
        else
        {
            quizManager = this;
            Debug.Log("Duplicate QuizManager destroyed.");
        }
    }
    void Start()
    {
        isGameRunning = true;
        gameTime = Mathf.Ceil(gameTime);
        gameTimerText.text = Mathf.Ceil(gameTime).ToString();
        startButton.onClick.AddListener(OnStartButtonClick);

        InvokeRepeating("UpdateGameTimer", 0f, 1f);
        questionScreen.SetActive(false);


    }

    void UpdateGameTimer()
    {
        if (isGameRunning)
        {
            gameTime -= 1f;
            minutes = Mathf.FloorToInt(gameTime / 60);
            seconds = Mathf.FloorToInt(gameTime % 60);

            gameTimerText.text = $"{minutes}mn {seconds:D2}s";

            if (gameTime <= 0)
            {
                isGameRunning = false;
                gameTime = 0;
                CancelInvoke("UpdateGameTimer");
                endGame();
            }
        }
    }

    void Update()
    {
        if (isTimerRunning && !answerSelected)
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
    public void OnStartButtonClick()
    {
        questionScreen.SetActive(true);

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

        timeRemaining = timeLimit;
        isTimerRunning = true;
        answerSelected = false;
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
            print("Answer: " + answer);
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(answer));


        }
        Debug.Log("Question loaded: " + allAnswers);
    }


    public void OnAnswerSelected(string selectedAnswer)
    {
        if (answerSelected) return; // Prevent multiple clicks

        answerSelected = true; // Mark answer as selected    
        // Debug.Log("Button Clicked: " + answerButtons[buttonIndex].name);
        // print("Button Index: " + buttonIndex);
        // string selectedAnswer = answerButtons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text;

        // Debug logs to verify selected answer and correct answers
        Debug.Log("Selected Answer: " + selectedAnswer);
        Debug.Log("Correct Answers: " + string.Join(", ", currentQuestion.answers.correct));

        // Check if the selected answer is one of the correct answers
        if (currentQuestion.answers.correct.Exists(answer => answer == selectedAnswer))
        {
            score++;
            correctCount.text = score.ToString();
            gameTime += 10f;
            Debug.Log("Correct Answer! +10s added. New Score: " + score);
            if (gameTime > 120f) gameTime = 120f;
            totalQuestionsAnswered++;

            if (totalQuestionsAnswered == 10 && gameTime > 0)
            {
                Debug.Log("You Win!");
                SceneManager.LoadScene("GameWinningScreen");
            }
        }
        else
        {
            gameTime -= 10f;
            Debug.Log("Wrong Answer! -10s deducted.");
            if (gameTime <= 0f)
            {
                SceneManager.LoadScene("GameOverScreen");
            }
        }
        // answerSelected = true;
        questionScreen.SetActive(false);
    }

    void OnTimeOut()
    {
        questionScreen.SetActive(false);
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
    private void UpdateScore()
    {
        correctCount.text = score.ToString();
        if (score == 10)
        {
            Debug.Log("You Win!");
            SceneManager.LoadScene("GameWinningScreen");
        }
    }

}
