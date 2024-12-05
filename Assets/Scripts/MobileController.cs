using UnityEngine;

public class MobileController : MonoBehaviour
{
    public SimplePlayer player; // Reference to the SimplePlayer script

    // Movement Methods
    public void MoveForward()
    {
        player.SetMovementInput(Vector3.forward);
    }

    public void MoveBackward()
    {
        player.SetMovementInput(Vector3.back);
    }

    public void MoveLeft()
    {
        player.SetMovementInput(Vector3.left);
    }

    public void MoveRight()
    {
        player.SetMovementInput(Vector3.right);
    }

    public void StopMovement()
    {
        player.SetMovementInput(Vector3.zero);
    }

    // Shooting Method
    public void Shoot()
    {
        player.Shoot();
    }
}
