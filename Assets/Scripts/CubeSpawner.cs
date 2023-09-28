using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit;
using MixedReality.Toolkit.UX;
using UnityEngine.XR.Interaction.Toolkit;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public Camera hololensCamera;
    public int numberOfCubes = 10;


    //public GameObject parentObject;
    //public float spawnMaxX = 0.3f;
    //public float spawnMaxY = 0.3f;
    //public float spawnMaxZ = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<StatefulInteractable>().selectEntered.AddListener(SpawnMultipleCubes);
        GameObject buttonObject = GameObject.Find("ButtonSpawnCubes");

        if (buttonObject != null)
        {
            StatefulInteractable interactable = buttonObject.GetComponent<StatefulInteractable>();

            if (interactable != null)
            {
                interactable.selectEntered.AddListener(SpawnMultipleCubes);
            }
            else
            {
                Debug.LogError("StatefulInteractable component not found on the button.");
            }
        }
        else
        {
            Debug.LogError("ButtonSpawnCubes not found in the scene.");
        }
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
            //Vector3 spawnPosition = GenerateRandomPositionNearButton();
            Vector3 spawnPosition = GeneratePositionInFrontOfCamera();

            if (!IsPositionOccupied(spawnPosition))
            {
                GameObject newCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
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

    Vector3 GeneratePositionInFrontOfCamera()
    {
        if (!hololensCamera)
        {
            Debug.LogError("Hololens camera not set.");
            return Vector3.zero;
        }

        // Random vertical offset between 0 and 40 cm above the camera's position for eye height
        float yOffset = Random.Range(-0.4f, 0.4f);

        // Random horizontal offset, either to the left or right, between 0 and 2m
        float randomXOffset = Random.Range(-2f, 2f);

        // Random distance in front of the camera between 0 and 2m
        float zOffset = Random.Range(0f, 2f);

        Vector3 spawnPosition = hololensCamera.transform.position + hololensCamera.transform.forward * zOffset;
        spawnPosition.y += yOffset;
        spawnPosition.x += randomXOffset;

        return spawnPosition;
    }

    /*
    Vector3 GenerateRandomPositionNearButton()
    {
        Vector3 buttonPosition = transform.position;

        float randomX = Random.Range(-spawnMaxY, spawnMaxY);
        float randomY = Random.Range(-spawnMaxY, spawnMaxY);
        float randomZ = Random.Range(-spawnMaxZ, spawnMaxZ);

        return new Vector3(buttonPosition.x + randomX, buttonPosition.y + randomY, buttonPosition.z + randomZ);
    }
    */

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
