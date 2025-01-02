using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public LoadingBarController loadingBarController;
    public GameObject loadingBarObject; // Reference to the Loading Bar GameObject

    // Manage the Scene
    public void GoToScene1()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToScene2()
    {
        LoadSceneWithLoadingBar(2);
        //SceneManager.LoadScene(1);
    }


    public void GoToScene4()
    {
         SceneManager.LoadScene(2);  //load to main game
        //SceneManager.LoadScene(1);
    }

    public void GoToScene3()
    {
        SceneManager.LoadScene(2);
    }
     public void InstrutionScene()
    {
        SceneManager.LoadScene(7);
    }

     public void GoToTeamMemberScene()
    {
        SceneManager.LoadScene(5);
    }

     public void GoToHistoryScene()
    {
        SceneManager.LoadScene(6);
    }

    // Loads a scene with the loading bar using the LoadingBarController
    private void LoadSceneWithLoadingBar(int sceneIndex)
    {
        if (loadingBarObject != null)
        {
            loadingBarObject.SetActive(true); // Activate loading bar UI
        }

        loadingBarController.gameObject.SetActive(true); // Ensure LoadingBarController is active
        loadingBarController.LoadScene(sceneIndex); // Start loading with progress bar
    }
}
