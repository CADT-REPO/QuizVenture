using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBarController : MonoBehaviour
{
    // Public references to the Loader UI and the Slider
    public GameObject LoaderUI; // The GameObject containing the loading screen UI
    public Slider LoadingBar;  // The Slider component for the loading bar

    // Speed factor to slow down the loading bar
    public float speedFactor = 0.2f; // Lower value = slower loading bar

    // Method to start loading a scene by index
    public void LoadScene(int index)
    {
        if (LoaderUI != null && LoadingBar != null)
        {
            Debug.Log("Starting scene load...");
            LoaderUI.SetActive(true); // Display the loading screen
            StartCoroutine(LoadScene_Coroutine(2)); // Start loading the scene
        }
        else
        {
            Debug.LogError("LoaderUI or LoadingBar is not assigned in the inspector!");
        }
    }

    // Coroutine to handle asynchronous scene loading
    private IEnumerator LoadScene_Coroutine(int index)
    {
        // Reset the loading bar
        LoadingBar.value = 0;

        // Start loading the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.allowSceneActivation = false; // Prevent automatic scene activation

        float progress = 0;

        // Loop until the scene is fully loaded
        while (!asyncOperation.isDone)
        {
            // Gradually update the progress bar using the speed factor
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, speedFactor * Time.deltaTime);
            LoadingBar.value = progress;

            Debug.Log($"Loading progress: {progress}");

            // If loading is almost complete, fill the bar and activate the scene
            if (progress >= 0.9f)
            {
                // Pause briefly to simulate smooth transition
                yield return new WaitForSeconds(1f);

                // Fill the progress bar to 1 and activate the scene
                LoadingBar.value = 1;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null; // Wait for the next frame
        }

        Debug.Log("Scene load complete!");
    }
}
