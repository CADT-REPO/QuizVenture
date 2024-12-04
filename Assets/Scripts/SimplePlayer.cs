using UnityEngine;

public class SimplePlayer    : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 720f;
    private Rigidbody rb;
    public BulletPool bulletPool;
    public Transform firePoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool fire = Input.GetKeyDown(KeyCode.Space);  // Fire with spacebar

        MovePlayer(h, v);

        if (fire)
        {
            Shoot();
        }
    }

    void MovePlayer(float h, float v)
    {
        Vector3 move = new Vector3(h, 0, v).normalized;
        if (move.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rb.MovePosition(transform.position + move * moveSpeed * Time.deltaTime);
        }
    }

    void Shoot()
    {
        if (bulletPool != null && firePoint != null)
        {
            GameObject bullet = bulletPool.GetBullet();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetBulletPool(bulletPool);  // Set the BulletPool reference
        }
    }
}
