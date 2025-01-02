using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StateController.PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StateController.ResumeGame();
        }
    }
}
