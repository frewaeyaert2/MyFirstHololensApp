using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit;
using MixedReality.Toolkit.UX;

public class CubSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public int numberOfCubes = 10;
    public float spawnMaxX = 0.3f;
    public float spawnMaxY = 0.3f;
    public float spawnMaxZ = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        // Attach button click handler
        GetComponent<StatefulInteractable>().OnClicked.AddListener(SpawnCube);
    }

    private void SpawnCube()
    {
        Vector3 buttonPosition = transform.position;

        for (int i = 0; i < numberOfCubes; i++)
        {
            float randomX = Random.Range(-spawnMaxY, spawnMaxY);
            float randomY = Random.Range(-spawnMaxY, spawnMaxY);
            float randomZ = Random.Range(-spawnMaxZ, spawnMaxZ);

            Vector3 spawnPosition = new Vector3(buttonPosition.x + randomX, buttonPosition.y + randomY, buttonPosition.z + randomZ);
            Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
            cubePrefab.AddComponent<Rigidbody>(); // Add Rigidbody component for physics
            cubePrefab.AddComponent<CubeInteractable>(); // Add custom script for interaction
            //cubePrefab.GetComponent<StatefulInteractable>().OnClicked.AddListener(() => FallDownCube(cubePrefab));
        }

    }

    public void UpdateSpawnRate(float value)
    {
        // Update the spawn rate logic here
        // Update spawnRateText.text with value.ToString("F2")
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class CubeInteractable : StatefulInteractable
{
    private Rigidbody rb;

    void Start()
    {
        GetComponent<StatefulInteractable>().OnClicked.AddListener(FallDownCube);
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // Disable gravity initially
        }
    }

    public void FallDownCube()
    {
        if (rb != null)
        {
            rb.useGravity = true; // Enable gravity when clicked
        }
    }

    // Implement other IMixedRealityPointerHandler methods as needed
}