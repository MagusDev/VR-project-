using UnityEngine;

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

    void Start()
    {
        if (objectPrefab == null || player == null)
        {
            Debug.LogError("Please assign the object prefab and player transform in the Inspector.");
            return;
        }

        SpawnObjectsOnSphere();
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
                Instantiate(objectPrefab, spawnPosition, Quaternion.identity, transform);
            }
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
