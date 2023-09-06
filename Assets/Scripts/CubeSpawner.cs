using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit;
using MixedReality.Toolkit.UX;
using UnityEngine.XR.Interaction.Toolkit;

public class CubSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject parentObject;
    public int numberOfCubes = 10;
    public float spawnMaxX = 0.3f;
    public float spawnMaxY = 0.3f;
    public float spawnMaxZ = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<StatefulInteractable>().selectEntered.AddListener(SpawnMultipleCubes);
    }

    private void SpawnMultipleCubes(SelectEnterEventArgs arg0)
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            SpawnCubeWithRetry();
        }
    }

    void SpawnCubeWithRetry()
    {
        int attempts = 0;
        while (attempts < 10)
        {
            Vector3 spawnPosition = GenerateRandomPositionNearButton();

            if (!IsPositionOccupied(spawnPosition))
            {
                GameObject newCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity, parentObject.transform);
                newCube.GetComponent<StatefulInteractable>().OnClicked.AddListener(() => FallDownCube(newCube));
                break;
            }

            attempts++;
        }

        if (attempts == 10)
        {
            Debug.LogWarning("Failed to spawn cube after 10 attempts.");
        }
    }

    Vector3 GenerateRandomPositionNearButton()
    {
        Vector3 buttonPosition = transform.position;

        float randomX = Random.Range(-spawnMaxY, spawnMaxY);
        float randomY = Random.Range(-spawnMaxY, spawnMaxY);
        float randomZ = Random.Range(-spawnMaxZ, spawnMaxZ);

        return new Vector3(buttonPosition.x + randomX, buttonPosition.y + randomY, buttonPosition.z + randomZ);
    }

    bool IsPositionOccupied(Vector3 position)
    {
        float cubeSize = 0.1f;
        Collider[] hitColliders = Physics.OverlapBox(position, new Vector3(cubeSize / 2, cubeSize / 2, cubeSize / 2));
        return hitColliders.Length > 0;
    }

    public void FallDownCube(GameObject cube)
    {
        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
        }
    }
}
