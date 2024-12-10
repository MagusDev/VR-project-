using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SphericalGridSpawner : MonoBehaviour
{
    [Header("Object to Spawn")]
    public GameObject objectPrefab;

    [Header("Grid Parameters")]
    public int rows = 10; // Number of latitude divisions
    public int columns = 10; // Number of longitude divisions
    public float radius = 5f; // Radius of the sphere

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
        StartCoroutine(ChangeRandomSphereColorsCoroutine()); // Start the color change coroutine
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

    IEnumerator ChangeRandomSphereColorsCoroutine()
    {
        while (true) // This will run indefinitely, you can modify the condition as needed
        {
            foreach (GameObject obj in spawnedObjects)
            {
                // Choose a random color from the list
                Color randomColor = colors[Random.Range(0, colors.Count)];
                Renderer renderer = obj.GetComponent<Renderer>();

                if (renderer != null)
                {
                    // Gradually change to the random color
                    StartCoroutine(ChangeColorOverTime(renderer, randomColor, 2f)); // 2 seconds to change color
                }
            }

            // Wait for a specific time before changing colors again
            yield return new WaitForSeconds(2f); // Change every 2 seconds (or adjust as needed)
        }
    }

    IEnumerator ChangeColorOverTime(Renderer renderer, Color targetColor, float duration)
    {
        Color initialColor = renderer.material.color;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            renderer.material.color = Color.Lerp(initialColor, targetColor, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is set
        renderer.material.color = targetColor;
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
