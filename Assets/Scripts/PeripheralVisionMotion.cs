using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PeripheralVisionMotion : MonoBehaviour
{
    [Header("Object to Spawn")]
    public GameObject objectPrefab;

    [Header("Grid Parameters")]
    public int rows = 10;
    public int columns = 10;
    public float radius = 5f;
    public float pulseDuration = 0.5f;
    public float delayBetweenPulses = 0.1f;
    public int simultaneousPulses = 5;

    [Header("Player Reference")]
    public Transform player;

    [Header("Field Definitions")]
    [Range(0.0f, 1.0f)] public float centerThreshold = 0.6f; // Center threshold
    [Range(-1.0f, 1.0f)] public float edgeThreshold = 0.3f;   // Edge threshold

    [Header("Selection Parameters")]
    public int startRow = 5; // Only select objects starting from this row (0-based index)

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();
    private bool selectCenter = true; // Toggle for alternating selection

    void Start()
    {
        if (objectPrefab == null || player == null)
        {
            Debug.LogError("Please assign the object prefab and player transform in the Inspector.");
            return;
        }

        SpawnObjectsOnSphere();
        StartCoroutine(JiggleObjectsCoroutine());
    }

    void SpawnObjectsOnSphere()
    {
        Vector3 poleDirection = player.forward.normalized;

        for (int i = 0; i < rows; i++)
        {
            float theta = Mathf.PI * i / (rows - 1);

            for (int j = 0; j < columns; j++)
            {
                float phi = 2 * Mathf.PI * j / columns;
                Vector3 offset = SphericalToCartesian(theta, phi, poleDirection);
                Vector3 spawnPosition = player.position + offset * radius;
                GameObject spawnedObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity, transform);
                spawnedObjects.Add(spawnedObject);
                originalPositions[spawnedObject] = spawnPosition;
            }
        }
    }

    IEnumerator JiggleObjectsCoroutine()
    {
        while (true)
        {
            // Alternate between center and edge groups
            List<int> randomIndices = GetRandomObjectIndices(simultaneousPulses, selectCenter);

            foreach (int index in randomIndices)
            {
                StartCoroutine(JiggleObject(spawnedObjects[index]));
            }

            selectCenter = !selectCenter; // Toggle selection group
            yield return new WaitForSeconds(delayBetweenPulses);
        }
    }

    IEnumerator JiggleObject(GameObject targetObject)
    {
        if (targetObject == null) yield break;

        Vector3 relativeOriginalPosition;
        if (!originalPositions.TryGetValue(targetObject, out relativeOriginalPosition)) yield break;

        float elapsedTime = 0f;

        while (elapsedTime < pulseDuration)
        {
            float jiggleAmount = Mathf.Sin(elapsedTime * Mathf.PI * 4) * 0.1f; // Adjust frequency and amplitude as needed
            Vector3 currentBasePosition = player.position + (relativeOriginalPosition - player.position).normalized * radius;
            targetObject.transform.position = currentBasePosition + Random.insideUnitSphere * jiggleAmount;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Vector3 resetBasePosition = player.position + (relativeOriginalPosition - player.position).normalized * radius;
        targetObject.transform.position = resetBasePosition; // Reset to the recalculated position
    }

    List<int> GetRandomObjectIndices(int count, bool selectCenter)
    {
        List<int> validIndices = new List<int>();
        int startIndex = startRow * columns; // Calculate the first object index in the start row

        for (int i = startIndex; i < spawnedObjects.Count; i++)
        {
            Vector3 toObject = (spawnedObjects[i].transform.position - player.position).normalized;
            float dotProduct = Vector3.Dot(player.forward, toObject);

            // Group objects based on dot product thresholds
            if (selectCenter && dotProduct >= centerThreshold)
            {
                validIndices.Add(i);
            }
            else if (!selectCenter && dotProduct < edgeThreshold)
            {
                validIndices.Add(i);
            }
        }

        // Shuffle and limit to `count` indices
        for (int i = 0; i < validIndices.Count; i++)
        {
            int randomIndex = Random.Range(0, validIndices.Count);
            int temp = validIndices[i];
            validIndices[i] = validIndices[randomIndex];
            validIndices[randomIndex] = temp;
        }

        return validIndices.GetRange(0, Mathf.Min(count, validIndices.Count));
    }

    Vector3 SphericalToCartesian(float theta, float phi, Vector3 pole)
    {
        Vector3 defaultPole = Vector3.up;
        Quaternion rotation = Quaternion.FromToRotation(defaultPole, pole);
        Vector3 point = new Vector3(
            Mathf.Sin(theta) * Mathf.Cos(phi),
            Mathf.Cos(theta),
            Mathf.Sin(theta) * Mathf.Sin(phi)
        );
        return rotation * point;
    }
}
