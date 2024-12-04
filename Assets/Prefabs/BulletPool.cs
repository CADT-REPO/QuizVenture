using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;  // Bullet prefab to instantiate
    public int poolSize = 10;  // Number of bullets in the pool
    private Queue<GameObject> bulletPool;

    void Start()
    {
        // Initialize the pool
        bulletPool = new Queue<GameObject>();

        // Preinstantiate the bullets and add them to the pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);  // Deactivate the bullet initially
            bulletPool.Enqueue(bullet);
        }
    }

    // Get a bullet from the pool
    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);  // Activate the bullet
            return bullet;
        }
        else
        {
            // If the pool is empty, you can instantiate a new bullet
            GameObject bullet = Instantiate(bulletPrefab);
            return bullet;
        }
    }

    // Return a bullet to the pool
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);  // Deactivate the bullet
        bulletPool.Enqueue(bullet);  // Add it back to the pool
    }
}
