using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeripheralColorTest : MonoBehaviour
{
    [Header("Grid Settings")]
    public GameObject spherePrefab;   // Prefab for the test object
    public float gridRadius = 5f;     // Radius of the spherical grid
    public int gridResolution = 10;  // Number of objects per hemisphere
    public float displayTime = 2f;   // Time to show each stimulus

    [Header("Colors for Testing")]
    public Color[] testColors;       // Colors to assign randomly

    [Header("Player Settings")]
    public Transform playerHead;     // Reference to the player's head (VR headset or main camera)

    private List<GameObject> gridObjects = new List<GameObject>();
    private bool isTestRunning = false;

    void Start()
    {
        CreateSphericalGrid();
        StartCoroutine(RunPeripheralTest());
    }

    void Update()
    {
        // Keep the grid centered on the player's forward direction
        transform.position = playerHead.position + playerHead.forward * gridRadius;

        // Ensure the grid faces the player
        transform.rotation = Quaternion.LookRotation(playerHead.position - transform.position);
    }

    // Creates the spherical grid of objects in front of the player's vision
    void CreateSphericalGrid()
    {
        for (int i = 0; i < gridResolution; i++)
        {
            for (int j = 0; j < gridResolution; j++)
            {
                // Generate spherical coordinates
                float theta = Mathf.PI * i / gridResolution; // Vertical angle
                float phi = 2 * Mathf.PI * j / gridResolution; // Horizontal angle

                // Convert to Cartesian coordinates
                Vector3 localPosition = new Vector3(
                    gridRadius * Mathf.Sin(theta) * Mathf.Cos(phi),
                    gridRadius * Mathf.Sin(theta) * Mathf.Sin(phi),
                    gridRadius * Mathf.Cos(theta)
                );

                // Offset so spheres are distributed around the player's forward direction
                localPosition = Quaternion.LookRotation(playerHead.forward) * localPosition;

                // Instantiate sphere and parent it to the grid object
                GameObject sphere = Instantiate(spherePrefab, transform);
                sphere.transform.localPosition = localPosition;
                sphere.SetActive(false); // Start with objects disabled
                gridObjects.Add(sphere);
            }
        }
    }

    // Coroutine to run the peripheral color test
    IEnumerator RunPeripheralTest()
    {
        isTestRunning = true;

        foreach (GameObject obj in gridObjects)
        {
            // Randomly assign a color to the object
            Renderer renderer = obj.GetComponent<Renderer>();
            renderer.material.color = testColors[Random.Range(0, testColors.Length)];

            // Display the object
            obj.SetActive(true);

            // Wait for the display time
            yield return new WaitForSeconds(displayTime);

            // Hide the object
            obj.SetActive(false);
        }

        isTestRunning = false;
    }
}
