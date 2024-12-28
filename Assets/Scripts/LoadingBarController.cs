using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Defines a new class called LoadingBarController, which inherits from MonoBehaviour
// so it can be attached to GameObjects in a Unity scene.
public class LoadingBarController : MonoBehaviour
{
    // Public GameObject variable to hold the UI element that displays the loading screen.
    public GameObject LoaderUI;

    // Public Slider variable to control the visual loading bar UI component.
    public Slider LoadingBar;

    // Public method that initiates loading a scene with a specified index.
    public void LoadScene(int index)
    {
        // Starts the coroutine that will load the scene asynchronously.
        StartCoroutine(LoadScene_Coroutine(index));
    }

    // Coroutine that manages the asynchronous scene loading process.
    public IEnumerator LoadScene_Coroutine(int index)
    {
        // Sets the loading bar's value to 0 (empty) at the beginning.
        LoadingBar.value = 0;
        
        // Makes the loader UI visible to the player.
        LoaderUI.SetActive(true);

        // Begins loading the scene asynchronously. 
        // This allows the scene to load in the background without freezing the game.
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);

        // Prevents the scene from automatically switching to the loaded scene
        // when it reaches 90% completion, so we can control when it activates.
        asyncOperation.allowSceneActivation = false;

        // Initializes a float variable `progress` to track loading progress.
        float progress = 0;
        float loadSpeedFactor = 0.5f;

        // Creates a speed factor to control how fast the loading bar progresses visually.
        // float loadSpeedFactor = 0.1f;

        // Starts a loop that continues until the loading operation is complete.
        while (!asyncOperation.isDone)
        {
            // Gradually increases `progress` towards `asyncOperation.progress` over time
            // to slow down the visual filling of the loading bar.
             progress = Mathf.MoveTowards(progress, asyncOperation.progress, loadSpeedFactor * Time.deltaTime);

            // Sets the loading bar's value to the current `progress` value.
            LoadingBar.value = progress;

            // Checks if `progress` has reached or exceeded 0.9, the threshold for scene activation.
            if (progress >= 0.9f)
            {
                // When progress reaches 1, the loading bar is full, and we allow the scene to activate.
                LoadingBar.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            
            // Waits until the next frame before continuing the loop.
            yield return null;
        }
    }
}
