using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include if using TextMeshPro

// update logic for randomize the question set
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject questionScreen;
    public TextMeshProUGUI questionText; // Text component to display the question
    public Button[] answerButtons; // Array of answer buttons
    public TextMeshProUGUI timerText; // Text component for the timer
    public float timeLimit = 10f; // Time limit per question in seconds

    private List<Question> allQuestions = new List<Question>();
    private Question currentQuestion;
    private float timeRemaining;
    private bool isTimerRunning = true;
    private bool correctAnswerSelected = false;

    // global main screen
    private float gameTime = 120f;
    private float gameTimeRemaining;
    private bool isGameRunning = false; // Track game state
    public TextMeshProUGUI gameTimerText; // Game timer text (total game time)
    public Button startButton; // Button to start the game
    public TextMeshProUGUI correctCount; // Text to display the correct count
    // for game timer
    private int minutes;
    private int seconds;
    private int score = 0;

    // to random position of treasure button
    public Button targetButton;
    public RectTransform canvasRect;

    void Start()
    {
        // LoadQuestion();
        // StartNewQuestion();
        isGameRunning = true;
        gameTime = Mathf.Ceil(gameTime);
        gameTimerText.text = Mathf.Ceil(gameTime).ToString();
        startButton.onClick.AddListener(OnStartButtonClick);

        // start the game timer as sson as unity plays
        InvokeRepeating("UpdateGameTimer", 0f, 1f);
        mainScreen.SetActive(true);
        questionScreen.SetActive(false);

        if (targetButton != null && canvasRect != null)
        {
            RandomizeTreasure();
        }
        else
        {
            Debug.LogError("Button or Canvas not assigned in the Inspector.");
        }

    }

    void UpdateGameTimer()
    {
        if (isGameRunning)
        {
            gameTime -= 1f;

            Debug.Log($"Game Time: {gameTime}");

            minutes = Mathf.FloorToInt(gameTime / 60);
            seconds = Mathf.FloorToInt(gameTime % 60);
            // Debug.Log($"Minutes: {minutes}, Seconds: {seconds}");

            // Update the game timer text
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

        // if (isGameRunning)
        // {
        //     // Update the overall game timer

        //     gameTime -= Time.deltaTime;
        //     // gameTimeRemaining = gameTime;
        //     gameTimerText.text = Mathf.Ceil(gameTime).ToString();

        //     if (gameTime <= 0)
        //     {
        //         // If game time is up, end the game
        //         isGameRunning = false;
        //         gameTime = 0;
        //         endGame();
        //     }
        // }

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

    public void OnStartButtonClick()
    {
        // // Start the game
        // isGameRunning = true;
        mainScreen.SetActive(false);
        questionScreen.SetActive(true);
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
        List<string> allAnswers = new List<string>();
        // allAnswers.AddRange(currentQuestion.answers.wrong);

        // randomly choose 1 correct answer
        if (currentQuestion.answers.correct.Count > 0)
        {
            int correctIndex = Random.Range(0, currentQuestion.answers.correct.Count);
            allAnswers.Add(currentQuestion.answers.correct[correctIndex]);
        }

        // randomly choose 3 wrong answer
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
            if (i < allAnswers.Count)
            {
                // int index = i;
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = allAnswers[i];

                string answer = allAnswers[i];
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(i, answer));
            }
            else
            {
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

        if (currentQuestion.answers.correct.Contains(selectedAnswer))
        {
            Debug.Log("correct answer!!");
            correctAnswerSelected = true;
            isTimerRunning = false;
            correctCount.text = (score++).ToString();


            // isGameRunning = false;
            gameTime += 10f;
            if (gameTime > 120f)
            {
                gameTime = 120f;
            }

            minutes = Mathf.FloorToInt(gameTime / 60);
            seconds = Mathf.FloorToInt(gameTime % 60);

            gameTimerText.text = $"{minutes}mn {seconds:D2}s";

            questionScreen.SetActive(false);
            mainScreen.SetActive(true);
            RandomizeTreasure();
            // isGameRunning = true;

        }
        else
        {
            gameTime -= 10f;
            if (gameTime <= 0f)
            {
                // gameTime = 120f;
                SceneManager.LoadScene("GameOverScreen");
            }

            minutes = Mathf.FloorToInt(gameTime / 60);
            seconds = Mathf.FloorToInt(gameTime % 60);

            gameTimerText.text = $"{minutes}mn {seconds:D2}s";

            Debug.Log("wrong answer. try again");
            mainScreen.SetActive(true);
            questionScreen.SetActive(false);
        }
    }

    void OnTimeOut()
    {
        Debug.Log("Time's up!");
        mainScreen.SetActive(true);
        questionScreen.SetActive(false);
        // Handle time-out logic, e.g., proceed to the next question
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
        Debug.Log("game over");
        mainScreen.SetActive(true);
    }
    void RandomizeTreasure()
    {
        RectTransform buttonRect = targetButton.GetComponent<RectTransform>();

        // get the size of the canvas
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // calculate random x and y posisition within the canvas bound
        float randomX = Random.Range(-canvasWidth / 2 + buttonRect.rect.width / 2, canvasWidth / 2 - buttonRect.rect.width / 2);
        float randomY = Random.Range(-canvasHeight / 2 + buttonRect.rect.height / 2, canvasHeight / 2 - buttonRect.rect.height / 2);

        // Set the button position
        buttonRect.anchoredPosition = new Vector2(randomX, randomY);
    }
}
