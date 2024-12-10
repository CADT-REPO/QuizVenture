using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 5f;

    private BulletPool bulletPool;
    private float timeSinceSpawn;

    void OnEnable()
    {
        timeSinceSpawn = 0f;
    }

    void Update()
    {
        // Move the bullet forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Check if the bullet has exceeded its lifetime
        timeSinceSpawn += Time.deltaTime;
        if (timeSinceSpawn >= lifetime)
        {
            bulletPool.ReturnBullet(gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Destroy(col.gameObject);  
        }

        bulletPool.ReturnBullet(gameObject);
    }

    public void SetBulletPool(BulletPool pool)
    {
        bulletPool = pool;  
    }
}