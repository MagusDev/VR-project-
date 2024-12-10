using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PeripheralVisionMotion : MonoBehaviour
{
    [Header("Object to Spawn")]
    public GameObject objectPrefab;

    [Header("Grid Parameters")]
    public int rows = 10; // Number of latitude divisions
    public int columns = 10; // Number of longitude divisions
    public float radius = 5f; // Radius of the sphere
    public float jiggleFrequency = 5f;
    public float jiggleIntensity = 5f;

    [Header("Player Reference")]
    public Transform player; // Reference to the player

    [Header("Colors")]
    public List<Color> colors; // List of colors to randomly assign

    private List<GameObject> spawnedObjects = new List<GameObject>(); // To store references to spawned objects

    void Start()
    {
        if (objectPrefab == null || player == null || colors.Count == 0)
        {
            Debug.LogError("Please assign the object prefab, player transform, and provide a list of colors in the Inspector.");
            return;
        }

        SpawnObjectsOnSphere();
        StartCoroutine(JiggleRandomObjectCoroutine(jiggleIntensity, jiggleFrequency, 2f)); // Start the jiggle effect coroutine
    }

    void SpawnObjectsOnSphere()
    {
        Vector3 poleDirection = player.forward.normalized; // Player's forward direction as the pole

        for (int i = 0; i < rows; i++)
        {
            // Latitude angle (from 0 to π)
            float theta = Mathf.PI * i / (rows - 1);

            for (int j = 0; j < columns; j++)
            {
                // Longitude angle (from 0 to 2π)
                float phi = 2 * Mathf.PI * j / columns;

                // Calculate position on the sphere's surface relative to the pole
                Vector3 offset = SphericalToCartesian(theta, phi, poleDirection);

                // Calculate spawn position relative to the player's position
                Vector3 spawnPosition = player.position + offset * radius;

                // Instantiate the object at the calculated position
                GameObject spawnedObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity, transform);

                // Add the spawned object to the list
                spawnedObjects.Add(spawnedObject);
            }
        }
    }

    IEnumerator JiggleRandomObjectCoroutine(float intensity, float frequency, float jiggleDuration)
    {
        Vector3[] originalPositions = new Vector3[spawnedObjects.Count];

        // Store original positions
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            originalPositions[i] = spawnedObjects[i].transform.position;
        }

        while (true)
        {
            // Select a random object in front of the player
            int randomIndex = GetRandomObjectIndex();
            GameObject targetObject = spawnedObjects[randomIndex];

            if (targetObject != null)
            {
                Vector3 originalPosition = originalPositions[randomIndex];

                // Jiggle the selected object for a duration
                float elapsedTime = 0f;
                while (elapsedTime < jiggleDuration)
                {
                    Vector3 jiggleOffset = new Vector3(
                        Mathf.Sin(Time.time * frequency) * intensity,
                        Mathf.Cos(Time.time * frequency) * intensity,
                        Mathf.Sin(Time.time * frequency * 0.5f) * intensity
                    );

                    targetObject.transform.position = originalPosition + jiggleOffset;
                    elapsedTime += Time.deltaTime;

                    yield return null; // Wait for the next frame
                }

                // Reset the object to its original position
                targetObject.transform.position = originalPosition;
            }

            // Wait before selecting the next object
            yield return new WaitForSeconds(0.2f); // Adjust as needed
        }
    }

    int GetRandomObjectIndex()
    {
        // Filter objects to only include those in front of the player
        List<int> frontIndices = new List<int>();
        Vector3 playerDirection = player.forward;

        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            Vector3 toObject = (spawnedObjects[i].transform.position - player.position).normalized;
            float dotProduct = Vector3.Dot(playerDirection, toObject); // Closer to 1 means directly in front

            if (dotProduct > 0f) // Only consider objects in front of the player
            {
                frontIndices.Add(i);
            }
        }

        if (frontIndices.Count > 0)
        {
            // Randomly select an index from the filtered list
            return frontIndices[Random.Range(0, frontIndices.Count)];
        }
        else
        {
            // Fallback in case no objects are in front (shouldn't happen unless objects are behind the player entirely)
            return Random.Range(0, spawnedObjects.Count);
        }
    }

    Vector3 SphericalToCartesian(float theta, float phi, Vector3 pole)
    {
        // Rotate spherical coordinates to align with the pole direction
        Vector3 defaultPole = Vector3.up; // Default pole is the y-axis
        Quaternion rotation = Quaternion.FromToRotation(defaultPole, pole);

        // Compute Cartesian coordinates
        Vector3 point = new Vector3(
            Mathf.Sin(theta) * Mathf.Cos(phi),
            Mathf.Cos(theta),
            Mathf.Sin(theta) * Mathf.Sin(phi)
        );

        return rotation * point;
    }
}
