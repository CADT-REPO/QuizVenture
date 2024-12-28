using UnityEngine;
using UnityEngine.UI;

public class BoxInteraction : MonoBehaviour
{
    public string playerTag = "Player"; // Tag to find the player
    public GameObject interactionButtonPrefab; // Prefab for the UI button
    public float interactionDistance = 3f; // Distance to trigger the button

    private GameObject player; // Player instance in the scene
    private GameObject interactionButton; // Instance of the interaction button
    private bool isPlayerInRange = false;

    void Start()
    {
        // Find the player in the scene by tag
        player = GameObject.FindWithTag(playerTag);
        if (player == null)
        {
            Debug.LogError("Player not found in the scene. Ensure the player prefab is instantiated and tagged correctly.");
        }

        // Create an interaction button dynamically if the prefab is assigned
        if (interactionButtonPrefab != null)
        {
            interactionButton = Instantiate(interactionButtonPrefab, FindObjectOfType<Canvas>().transform);
            interactionButton.SetActive(false); // Hide the button initially
        }
        else
        {
            Debug.LogError("Interaction button prefab is not assigned.");
        }
    }

    void Update()
    {
        if (player != null && interactionButton != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance <= interactionDistance)
            {
                if (!isPlayerInRange)
                {
                    isPlayerInRange = true;
                    interactionButton.SetActive(true); // Show the button
                    UpdateButtonPosition(); // Ensure the button is positioned correctly
                }
            }
            else
            {
                if (isPlayerInRange)
                {
                    isPlayerInRange = false;
                    interactionButton.SetActive(false); // Hide the button
                }
            }
        }
    }

    void UpdateButtonPosition()
    {
        if (interactionButton != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            interactionButton.transform.position = screenPosition + new Vector3(0, 50, 0); // Offset to position above the box
        }
    }

    public void OnInteract()
    {
        // Define what happens when the button is clicked
        Debug.Log("Interacted with the box!");
        interactionButton.SetActive(false); // Hide the button after interaction
    }

    private void OnDestroy()
    {
        // Clean up the button when the box is destroyed
        if (interactionButton != null)
        {
            Destroy(interactionButton);
        }
    }
}
