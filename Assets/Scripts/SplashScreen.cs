using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public Image logoImage; // Reference to the logo Image in the UI
    public string nextSceneName = "HomeScreen"; // Name of the scene to load after splash screen

    private void Awake()
    {
        // Ensure this object persists across scenes
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Check if logoImage is assigned
        if (logoImage == null)
        {
            Debug.LogError("Logo Image is not assigned! Please assign it in the Inspector.");
            return; // Exit to prevent further errors
        }

        // Start the splash screen coroutine
        StartCoroutine(PlaySplashScreen());
    }

    IEnumerator PlaySplashScreen()
    {
        // Ensure the logo starts invisible
        logoImage.canvasRenderer.SetAlpha(0.0f);

        // Fade in the logo
        FadeIn();
        yield return new WaitForSeconds(2.0f); // Duration for fade-in

        // Hold the logo visible
        yield return new WaitForSeconds(1.0f);

        // Fade out the logo
        FadeOut();
        yield return new WaitForSeconds(2.0f); // Duration for fade-out

        // Load the next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not specified!");
        }
    }

    void FadeIn()
    {
        // Fade in the logo to full opacity over 1.5 seconds
        logoImage.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    void FadeOut()
    {
        // Fade out the logo to zero opacity over 1.5 seconds
        logoImage.CrossFadeAlpha(0.0f, 1.5f, false);
    }
}
