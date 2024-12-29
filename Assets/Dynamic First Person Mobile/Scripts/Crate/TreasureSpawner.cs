using UnityEngine;

public class TreasureSpawner : MonoBehaviour
{
    public GameObject treasureBoxPrefab; // Assign the treasure box prefab in the Inspector
    public int numberOfBoxes = 5; // Number of boxes to spawn
    public float yOffset = 0.5f; // Y-offset to ensure the crate is above the terrain

    private Vector3 spawnAreaMin;
    private Vector3 spawnAreaMax;

    void Start()
    {
        CalculateSpawnArea();
        SpawnTreasureBoxes();
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

        // Debug.Log($"Spawn Area Min: {spawnAreaMin}, Max: {spawnAreaMax}");
    }

    void SpawnTreasureBoxes()
    {
        for (int i = 0; i < numberOfBoxes; i++)
        {
            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomZ = Random.Range(spawnAreaMin.z, spawnAreaMax.z);
            float terrainY = GetTerrainHeightAtPosition(randomX, randomZ);

            if (terrainY != Mathf.NegativeInfinity)
            {
                // Apply Y-offset to position the crate properly above the terrain
                Vector3 spawnPosition = new Vector3(randomX, terrainY + yOffset, randomZ);

                // Instantiate with specific rotation
                Quaternion spawnRotation = Quaternion.Euler(-90, 0, 175.156f); // Set rotation as required
                Instantiate(treasureBoxPrefab, spawnPosition, spawnRotation);
                
                // Debug.Log($"Treasure box {i + 1} spawned at {spawnPosition} with rotation {spawnRotation.eulerAngles}");
            }
            else
            {
                Debug.LogWarning($"Failed to detect terrain height at X: {randomX}, Z: {randomZ}");
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
