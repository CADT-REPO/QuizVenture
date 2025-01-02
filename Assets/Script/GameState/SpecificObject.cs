using UnityEngine;

public class SpecificObject : MonoBehaviour, IPausable
{
    void OnEnable()
    {
        StateController.RegisterPausable(this);
    }

    void OnDisable()
    {
        StateController.UnregisterPausable(this);
    }

    public void ManualUpdate()
    {
        // Implement the update logic that should run even when the game is paused
        Debug.Log("ManualUpdate called");
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            // Implement the regular update logic
            Debug.Log("Regular Update called");
        }
    }
}
