using UnityEngine;
using System.Collections.Generic;

public class TreasureSpawner : MonoBehaviour
{
    public GameObject treasureBoxPrefab; // Assign the treasure box prefab in the Inspector
    public int poolSize = 10; // Number of objects in the pool
    public int numberOfBoxes = 5; // Number of boxes to spawn
    public float yOffset = 0.5f; // Y-offset to ensure the crate is above the terrain

    private List<GameObject> treasureBoxPool;
    private Vector3 spawnAreaMin;
    private Vector3 spawnAreaMax;

    void Start()
    {
        InitializeObjectPool();
        CalculateSpawnArea();
        SpawnTreasureBoxes();
    }

    void InitializeObjectPool()
    {
        // Initialize the pool
        treasureBoxPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(treasureBoxPrefab);
            obj.SetActive(false); // Start inactive
            treasureBoxPool.Add(obj);
        }
    }

    GameObject GetPooledObject()
    {
        foreach (GameObject obj in treasureBoxPool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj; // Return the first inactive object
            }
        }
        return null; // Return null if no objects are available
    }

    void CalculateSpawnArea()
    {
        // Initialize the spawn area bounds
        spawnAreaMin = new Vector3(float.MaxValue, 0, float.MaxValue);
        spawnAreaMax = new Vector3(float.MinValue, 0, float.MinValue);

        // Iterate through all active terrains
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            Vector3 terrainPos = terrain.transform.position;
            Vector3 terrainSize = terrain.terrainData.size;

            // Update spawn area bounds
            spawnAreaMin = Vector3.Min(spawnAreaMin, terrainPos);
            spawnAreaMax = Vector3.Max(spawnAreaMax, terrainPos + new Vector3(terrainSize.x, 0, terrainSize.z));
        }
    }

    void SpawnTreasureBoxes()
    {
        for (int i = 0; i < numberOfBoxes; i++)
        {
            GameObject treasureBox = GetPooledObject();

            if (treasureBox != null)
            {
                float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
                float randomZ = Random.Range(spawnAreaMin.z, spawnAreaMax.z);
                float terrainY = GetTerrainHeightAtPosition(randomX, randomZ);

                if (terrainY != Mathf.NegativeInfinity)
                {
                    // Apply Y-offset to position the crate properly above the terrain
                    Vector3 spawnPosition = new Vector3(randomX, terrainY + yOffset, randomZ);

                    // Set the position, rotation, and activate the object
                    Quaternion spawnRotation = Quaternion.Euler(-90, 0, 175.156f); // Set rotation as required
                    treasureBox.transform.position = spawnPosition;
                    treasureBox.transform.rotation = spawnRotation;
                    treasureBox.SetActive(true);
                }
                else
                {
                    Debug.LogWarning($"Failed to detect terrain height at X: {randomX}, Z: {randomZ}");
                }
            }
            else
            {
                Debug.LogWarning("No available objects in the pool to spawn.");
            }
        }
    }

    float GetTerrainHeightAtPosition(float x, float z)
    {
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            Vector3 terrainPos = terrain.transform.position;
            TerrainData terrainData = terrain.terrainData;

            if (x >= terrainPos.x && x <= terrainPos.x + terrainData.size.x &&
                z >= terrainPos.z && z <= terrainPos.z + terrainData.size.z)
            {
                float localX = (x - terrainPos.x) / terrainData.size.x;
                float localZ = (z - terrainPos.z) / terrainData.size.z;
                return terrainPos.y + terrainData.GetInterpolatedHeight(localX, localZ);
            }
        }

        return Mathf.NegativeInfinity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (spawnAreaMax != Vector3.zero && spawnAreaMin != Vector3.zero)
        {
            Vector3 center = (spawnAreaMin + spawnAreaMax) / 2;
            Vector3 size = spawnAreaMax - spawnAreaMin;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
