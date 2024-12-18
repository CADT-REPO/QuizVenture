using UnityEngine;

public class TreasureSpawner : MonoBehaviour
{
    public GameObject treasureBoxPrefab; // Assign the treasure box prefab in the Inspector
    public Vector3 spawnAreaMin; // Bottom-left corner of the spawn area
    public Vector3 spawnAreaMax; // Top-right corner of the spawn area
    public float fixedYPosition = 0f; // Fixed y-coordinate for the boxes
    public int numberOfBoxes = 5; // Number of boxes to spawn

    void Start()
    {
        SpawnTreasureBoxes();
    }

    void SpawnTreasureBoxes()
    {
        for (int i = 0; i < numberOfBoxes; i++)
        {
            // Generate random X and Z within the bounds
            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomZ = Random.Range(spawnAreaMin.z, spawnAreaMax.z);

            // Keep Y fixed
            Vector3 spawnPosition = new Vector3(randomX, fixedYPosition, randomZ);

            // Spawn with the prefab's original rotation
            Instantiate(treasureBoxPrefab, spawnPosition, treasureBoxPrefab.transform.rotation);

            Debug.Log($"Treasure box {i + 1} spawned at {spawnPosition}");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Draw the spawn area
        Vector3 center = (spawnAreaMin + spawnAreaMax) / 2;
        Vector3 size = spawnAreaMax - spawnAreaMin;

        Gizmos.DrawWireCube(center, size);
    }
}
 