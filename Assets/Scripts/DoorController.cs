using UnityEngine;

public class DoorController : MonoBehaviour
{
    // private Animator _doorAnim;

    // void Start()
    // {
    //     // Initialize the Animator component
    //     _doorAnim = this.transform.parent.GetComponent<Animator>();

    //     // Check if the Animator component is found
    //     if (_doorAnim == null)
    //     {
    //         Debug.LogError("Animator component not found on parent object.");
    //     }
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     // Check if the Animator component is not null before setting parameters
    //     if (_doorAnim != null)
    //     {
    //         _doorAnim.SetBool("Isopening", true);
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     // Check if the Animator component is not null before setting parameters
    //     if (_doorAnim != null)
    //     {
    //         _doorAnim.SetBool("Isopening", false);
    //     }
    // }

    // void Update()
    // {
    //     // Update logic if needed
    // }


     // Reference to the player
    public GameObject player;

    // Trigger when player enters door area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Move the player inside the door
            MovePlayerThroughDoor();
        }
    }

    // Move the player inside the door (adjust based on your scene setup)
    private void MovePlayerThroughDoor()
    {
        // Example: Move player a few steps inside
        Vector3 newPlayerPosition = transform.position + transform.forward * 2f; // Adjust the 2f distance based on your scene scale
        player.transform.position = newPlayerPosition;
        
        // Optionally, you can trigger a scene change or do any other action here
        // Example: SceneManager.LoadScene("NextSceneName");
    }
}
