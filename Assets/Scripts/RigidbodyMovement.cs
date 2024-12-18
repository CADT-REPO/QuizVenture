using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMovement : MonoBehaviour
{
    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;
    private float xRotation ;

    public Transform playerCamera;
    public Rigidbody playerBody;
    [Space]
    public float playerSpeed;
    public float mouseSensitivity;
    public float jumpForce;

    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundRadius = 0.4f;
    bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //locks the cursor 
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MoveCamera(); 


    }
    void MovePlayer()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, whatIsGround);//checking is the player is touching the ground

        Vector3 move = transform.TransformDirection(PlayerMovementInput) * playerSpeed;
        playerBody.velocity = new Vector3(move.x, playerBody.velocity.y, move.z);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }   
    }

    void MoveCamera()
    {
        xRotation -= PlayerMouseInput.y * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.Rotate(0f, PlayerMouseInput.x * mouseSensitivity,0f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

}
