using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 20;
    private Queue<GameObject> bulletPool;

    private void Start()
    {
        bulletPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullet.GetComponent<Bullet>().SetBulletPool(this);
            bulletPool.Enqueue(bullet);
        }
        Debug.Log("BulletPool initialized with " + poolSize + " bullets.");
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
            Debug.Log("Bullet retrieved from pool.");
            return bullet;
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.GetComponent<Bullet>().SetBulletPool(this);
            Debug.LogWarning("Bullet pool empty. Instantiating new bullet.");
            return bullet;
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
        Debug.Log("Bullet returned to pool.");
    }
}