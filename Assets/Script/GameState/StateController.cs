using UnityEngine;
using System.Collections.Generic;

public class StateController : MonoBehaviour
{
    private static List<IPausable> pausableObjects = new List<IPausable>();

    static public void PauseGame()
    {
        Time.timeScale = 0;
    }

    static public void ResumeGame()
    {
        Time.timeScale = 1;
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
            }
        }
    }
}
