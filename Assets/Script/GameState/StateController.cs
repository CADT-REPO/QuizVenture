using UnityEngine;
using System.Collections.Generic;

public class StateController : MonoBehaviour
{
    private static List<IPausable> pausableObjects = new List<IPausable>();
    public static bool isPause = false;

    static public void PauseGame()
    {
        Time.timeScale = 0;
        isPause = true;
        Debug.Log("Game Paused");
    }

    static public void ResumeGame()
    {
        Time.timeScale = 1;
        isPause = false;
        Debug.Log("Game Resumed");
    }

    public static void RegisterPausable(IPausable pausable)
    {
        if (!pausableObjects.Contains(pausable))
        {
            pausableObjects.Add(pausable);
        }
    }

    public static void UnregisterPausable(IPausable pausable)
    {
        if (pausableObjects.Contains(pausable))
        {
            pausableObjects.Remove(pausable);
        }
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            foreach (var pausable in pausableObjects)
            {
                pausable.ManualUpdate();
                Debug.Log("ManualUpdate called for: " + pausable);
            }
        }
    }
}
