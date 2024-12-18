using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform spawnPoint; // Reference to the spawn point in the indoor scene
    public GameObject player;    // Reference to the player

    void Start()
    {
        // Check if the player object and spawn point are set
        if (player == null || spawnPoint == null)
        {
            Debug.LogError("Player or spawn point not assigned in the GameManager.");
            return;
        }

        // When the indoor scene loads, position the player at the spawn point
        player.transform.position = spawnPoint.position;
    }

    public void LoadIndoorScene(string sceneName)
    {
        // Load the indoor scene asynchronously
        SceneManager.LoadScene(sceneName);
    }
}
