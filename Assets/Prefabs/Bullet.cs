using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 5f;

    private BulletPool bulletPool;

    void Start()
    {
        // Destroy the bullet after a set lifetime
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the bullet forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
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
