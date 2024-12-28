using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    void Interact();
}

public class InteractiveTreasureBox : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Transform InteracterPlayerSource; // Source of the interaction (e.g., player camera or a point on the player)
    public float InteractRange = 3.0f; // Maximum distance to interact

    void Start()
{
    if (InteracterPlayerSource   == null)
    {
        InteracterPlayerSource   = Camera.main.transform; // Automatically use the main camera
    }
}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AttemptInteraction();
        }
    }

    private void AttemptInteraction()
    {
        if (InteracterPlayerSource   == null)
        {
            Debug.LogError("InteracterPlayerSource   is not assigned!");
            return;
        }

        Ray interactionRay = new Ray(InteracterPlayerSource .position, InteracterPlayerSource    .forward);
        if (Physics.Raycast(interactionRay, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out Interactable interactObj))
            {
                // Call the interact method on the hit object
                interactObj.Interact();
            }
            else
            {
                Debug.LogError("No interactable object found!");
            }
        }
        else
        {
            Debug.LogError("No object in interaction range!");
        }
    }
}

// Example implementation of the Interactable interface
public class TreasureBox : MonoBehaviour, Interactable
{
    private bool isOpen = false;

    public void Interact()
    {
        if (isOpen)
        {
            Debug.LogError("The treasure box is already open!");
        }
        else
        {
            Debug.LogError("Opening the treasure box...");
            isOpen = true;
            // Add animations, sounds, or logic for opening the treasure box
        }
    }
}
