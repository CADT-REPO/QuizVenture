using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public Animator playerAnim;
    public Rigidbody playerRigid;
    public float walkSpeed, backwardSpeed, originalWalkSpeed, runSpeed, rotationSpeed;
    private bool isWalking;
    public Transform playerTrans;

    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement += transform.forward * walkSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement -= transform.forward * backwardSpeed;
        }

        playerRigid.MovePosition(playerRigid.position + movement * Time.deltaTime);
    }

    void Update()
    {
        HandleMovementAnimations();
        HandleRotation();
        HandleRunToggle();
    }

    void HandleMovementAnimations()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerAnim.SetTrigger("walk");
            playerAnim.ResetTrigger("idle");
            isWalking = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnim.ResetTrigger("walk");
            playerAnim.SetTrigger("idle");
            isWalking = false;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerAnim.SetTrigger("walkback");
            playerAnim.ResetTrigger("idle");
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnim.ResetTrigger("walkback");
            playerAnim.SetTrigger("idle");
        }
    }

    void HandleRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            playerTrans.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerTrans.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }

    void HandleRunToggle()
    {
        if (isWalking)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                walkSpeed += runSpeed;
                playerAnim.SetTrigger("run");
                playerAnim.ResetTrigger("walk");
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                walkSpeed = originalWalkSpeed;
                playerAnim.ResetTrigger("run");
                playerAnim.SetTrigger("walk");
            }
        }
    }
}
