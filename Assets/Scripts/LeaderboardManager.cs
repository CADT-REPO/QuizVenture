using UnityEngine;
using UnityEngine.UI;

public class HistoryBoardManager : MonoBehaviour
{
    // Define the UI elements for the leaderboard row
    [System.Serializable]
    public class LeaderboardRow
    {
        public Text range;          // Text for the range (e.g., 1st, 2nd)
        public Text username;       // Text for the username
        public Text correctAnswers;  // Text for the number of correct answers
        public Text totalTime;          // Text for the time
        public Text totalQuest;     // Text for the total questions
    }

    // Row object containing all text fields
    public LeaderboardRow leaderboardRow;

    // Sample data for demonstration purposes
    public string playerRange = "1st";
    public string playerName = "Player1";
    public string correctAnswers = "8";
    public string totalTime = "01:20";
    public string totalQuest = "10";

    void Start()
    {
        // Update the UI elements with data
        // UpdateLeaderboardRow(playerRange, playerName, correctAnswers, totalTime, totalQuest);
    }

    public void UpdateLeaderboardRow(string range, string username, string correctAns, string timerValue, string totalQ)
    {
        if (leaderboardRow != null)
        {
            leaderboardRow.range.text = range ?? "N/A";   // Use null-coalescing operator to handle null values gracefully
            leaderboardRow.username.text = username ?? "N/A";
            leaderboardRow.correctAnswers.text = correctAns ?? "N/A";
            leaderboardRow.totalTime.text = timerValue ?? "N/A";
            leaderboardRow.totalQuest.text = totalQ ?? "N/A";
        }
        else
        {
            Debug.LogError("LeaderboardRow is not assigned in the inspector!");
        }
    }
}
