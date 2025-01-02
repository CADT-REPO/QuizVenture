using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include if using TextMeshPro
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;

public class QuizManager : MonoBehaviour, IPausable
{
    private AudioSource audioSource;

    public GameObject timerObject;
    // public AudioSource audioSource;
    public GameObject questionScreen;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public float timeLimit = 10f;
    public int correctToWin = 2;

    private List<Question> allQuestions = new List<Question>();
    private Question currentQuestion;
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
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing from this GameObject!");
            return;
        }

        // Optionally, ensure an audio clip is assigned
        if (audioSource.clip == null)
        {
            Debug.LogWarning("No audio clip assigned to the AudioSource.");
        }

        quizTimer.StartTimer();
        startButton.onClick.AddListener(OnStartButtonClick);

        questionScreen.SetActive(false);
    }



    void Update()
    {
        if (!gameTime.isGameRunning)
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

    // public void LoadQuestion()
    // {
    //     string path = Path.Combine(Application.streamingAssetsPath, "khmer_questionSet.JSON");
    //     if (File.Exists(path))
    //     {
    //         string json = File.ReadAllText(path);
    //         allQuestions = JsonConvert.DeserializeObject<List<Question>>(json);
    //     }
    //     else
    //     {
    //         Debug.LogError("Questions JSON file not found.");
    //     }
    // }
    public void LoadQuestion()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "khmer_questionSet.JSON");

        if (path.Contains("://") || path.Contains(":///"))
        {
            // For Android or WebGL
            StartCoroutine(LoadJsonFromStreamingAssets(path));
        }
        else
        {
            // For desktop and other platforms
            if (System.IO.File.Exists(path))
            {
                string json = System.IO.File.ReadAllText(path);
                allQuestions = JsonConvert.DeserializeObject<List<Question>>(json);
            }
            else
            {
                Debug.LogError("Questions JSON file not found.");
            }
        }
    }

    private IEnumerator LoadJsonFromStreamingAssets(string path)
    {
        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            allQuestions = JsonConvert.DeserializeObject<List<Question>>(json);
        }
        else
        {
            Debug.LogError("Failed to load JSON file from StreamingAssets: " + www.error);
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
            PlayCorrectAnswerAudio();
            answerManager.AddScore(1);
            gameTime.AddTime(10f);
            Debug.Log("Correct Answer! +10s added. New Score: " + answerManager.GetScore());
            totalQuestionsAnswered++;

            if (answerManager.GetScore() == correctToWin && gameTime.timer > 0)
            {
                Debug.Log("You Win!");
                SceneManager.LoadScene(4);
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


    public static void endGame()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene("GameOverScreen");
    }
    public void PlayCorrectAnswerAudio()
    {
        // Play the audio
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Cannot play audio: AudioSource or audio clip is missing.");
        }
    }


}