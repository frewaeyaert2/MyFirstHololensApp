using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public int cubeIndex; // Set this manually in the inspector for each cube: 0 for Blue, 1 for Green, etc.
    private SimonSaysManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<SimonSaysManager>();
    }

    public void OnMouseDown()
    {
        gameManager.CubeSelected(cubeIndex);
    }

    public void Debugging()
    {
        Debug.Log("eerste");
    }
}
