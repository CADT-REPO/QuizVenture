using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    public float moveSpeed = 5f;             // Movement speed
    public float rotationSpeed = 720f;      // Rotation speed
    private Rigidbody rb;                   // Reference to Rigidbody component
    public BulletPool bulletPool;           // Reference to BulletPool
    public Transform firePoint;             // Fire point for shooting

    private Vector3 movementInput;          // Stores movement input

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Prevents unwanted tilting
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    public void SetMovementInput(Vector3 input)
    {
        movementInput = input.normalized;
    }

    void MovePlayer()
    {
        // If there's no input, skip movement
        if (movementInput.magnitude < 0.1f) return;

        // Calculate movement direction and apply translation
        Vector3 moveDirection = movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);

        // Calculate target rotation based on movement direction
        float targetAngle = Mathf.Atan2(movementInput.x, movementInput.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

        // Smoothly rotate towards the target
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
    }

    public void Shoot()
    {
        if (bulletPool != null && firePoint != null)
        {
            GameObject bullet = bulletPool.GetBullet();
            if (bullet != null)
            {
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;

                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.SetBulletPool(bulletPool); // Set the BulletPool reference
            }
        }
        else
        {
            Debug.LogWarning("BulletPool or FirePoint is not assigned!");
        }
    }
}
