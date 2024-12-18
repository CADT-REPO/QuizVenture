using UnityEngine;

public class CrateController : MonoBehaviour
{
    private bool mIsOpen = false; 
    private Animator animator; 

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch
            Ray ray = Camera.main.ScreenPointToRay(touch.position); // Create a ray from the touch position

            if (Physics.Raycast(ray, out RaycastHit hit, 100f)) // Raycast to detect the crate
            {
                if (hit.transform == transform && touch.phase == TouchPhase.Began && !mIsOpen)
                {
                    Open_Crate(); // Call the Open_Crate function if the player touches the crate
                }
            }
        }
    }

    private void Open_Crate()
    {
        mIsOpen = true; 
        animator.SetBool("open", mIsOpen); // Use SetBool to control the animator state
        Debug.Log("Opened the crate!"); // Log a message to the console
    }

    public void Close_Crate()
    {
        mIsOpen = false; 
        animator.SetBool("open", mIsOpen); // Use SetBool to control the animator state
    }
}
