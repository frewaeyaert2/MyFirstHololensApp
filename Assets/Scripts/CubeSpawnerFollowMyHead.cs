using System.Collections;
using UnityEngine;
using MixedReality.Toolkit;
using MixedReality.Toolkit.UX;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class CubeSpawnerFollowMyHead : MonoBehaviour
{
    [Header("Prefabs and Objects")]
    public GameObject cubePrefab;
    public Camera hololensCamera;
    public Slider mySlider;
    private TextMeshPro textObject;

    [Header("Spawn Settings")]
    private int numberOfCubes = 5;
    private Coroutine spawnRoutine;

    private void Awake()
    {
        SetupSlider();
        SetupTextObject();
    }

    private void SetupSlider()
    {
        if (mySlider == null)
        {
            Debug.LogError("Slider component not found!");
            return;
        }
        mySlider.OnValueUpdated.AddListener(UpdateNumberOfCubes);
    }

    private void SetupTextObject()
    {
        textObject = GameObject.Find("txtNumberOfCubes")?.GetComponent<TextMeshPro>();

        if (textObject == null)
        {
            Debug.LogError("TextMeshPro object not found!");
            return;
        }
        UpdateTextObject(mySlider.Value);
    }

    private void UpdateNumberOfCubes(SliderEventData eventData)
    {
        numberOfCubes = Mathf.RoundToInt(eventData.NewValue);
        UpdateTextObject(eventData.NewValue);
    }

    private void UpdateTextObject(float value)
    {
        textObject.text = value.ToString("F0") + " cubes";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleSpawning(true);
        }
    }

    public void ToggleSpawning(bool isToggled)
    {
        if (!isToggled)
        {
            StopCurrentSpawningRoutine();
        }
        else
        {
            spawnRoutine = StartCoroutine(SpawnCubesContinuously());
        }
    }

    private void StopCurrentSpawningRoutine()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }
    }

    IEnumerator SpawnCubesContinuously()
    {
        while (true)
        {
            for (int i = 0; i < numberOfCubes; i++)
            {
                SpawnCubeWithRetry(GeneratePositionInFrontOfCamera());
            }
            yield return new WaitForSeconds(3f);
        }
    }

    void SpawnCubeWithRetry(Vector3 spawnPosition)
    {
        int maxAttempts = 10;
        for (int i = 0; i < maxAttempts; i++)
        {
            if (!IsPositionOccupied(spawnPosition))
            {
                SpawnCube(spawnPosition);
                return;
            }
        }
        Debug.LogWarning("Failed to spawn cube after 10 attempts.");
    }

    void SpawnCube(Vector3 position)
    {
        GameObject newCube = Instantiate(cubePrefab, position, Quaternion.identity);
        newCube.GetComponent<StatefulInteractable>().OnClicked.AddListener(() => FallDownCube(newCube));
    }

    Vector3 GeneratePositionInFrontOfCamera()
    {
        if (!hololensCamera)
        {
            Debug.LogError("Hololens camera not set.");
            return Vector3.zero;
        }

        // Random vertical offset between -40 and 40 cm above the camera's position for eye height
        float yOffset = Random.Range(-0.4f, 0.4f);
        // Random horizontal offset, either to the left or right, between -2 and 2m
        float xOffset = Random.Range(-2f, 2f);
        // Random distance in front of the camera between 0 and 2m
        float zOffset = Random.Range(0f, 2f);

        Vector3 spawnPosition = hololensCamera.transform.position + hololensCamera.transform.forward * zOffset;
        spawnPosition.y += yOffset;
        spawnPosition.x += xOffset;

        return spawnPosition;
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
