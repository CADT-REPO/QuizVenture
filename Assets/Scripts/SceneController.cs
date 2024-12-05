using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public LoadingBarController loadingBarController; // Reference to the LoadingBarController
    public GameObject loadingBarObject;              // Reference to the Loader UI object

    // Method to load Scene 1 directly
    public void GoToScene1()
    {
        SceneManager.LoadScene(0); // Load the scene with index 0
    }

    // Method to load Scene 2 with the loading bar
    public void GoToScene2()
    {
        LoadSceneWithLoadingBar(2); // Pass the index of the scene to load
    }

    // Method to load Scene 3 directly
    public void GoToScene3()
    {
        SceneManager.LoadScene(3); // Load the scene with index 3
    }

    // Private method to load a scene using the loading bar
    private void LoadSceneWithLoadingBar(int sceneIndex)
    {
        if (loadingBarObject != null)
        {
            loadingBarObject.SetActive(true); // Activate the loading UI
        }

        if (loadingBarController != null)
        {
            loadingBarController.gameObject.SetActive(true); // Ensure the LoadingBarController is active
            loadingBarController.LoadScene(sceneIndex); // Start loading with the loading bar
        }
        else
        {
            Debug.LogError("LoadingBarController is not assigned in the inspector!");
        }
    }
}
