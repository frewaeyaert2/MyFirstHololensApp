using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysManager : MonoBehaviour
{
    public Material whiteMaterial;
    public Material[] coloredMaterials;  // Blue, Green, Red, Yellow respectively


    public GameObject[] cubes; // Assign the four cubes here in the Unity editor
    private List<int> sequence = new List<int>();
    private int userSequenceIndex = 0;

    public void StartGame()
    {
        GenerateSequence();
        StartCoroutine(PlaySequence());
    }

    void GenerateSequence()
    {
        int nextCube = Random.Range(0, cubes.Length);
        sequence.Add(nextCube);
    }

    IEnumerator PlaySequence()
    {
        foreach (int cubeIndex in sequence)
        {
            cubes[cubeIndex].GetComponent<Renderer>().material = coloredMaterials[cubeIndex];
            yield return new WaitForSeconds(1);
            cubes[cubeIndex].GetComponent<Renderer>().material = whiteMaterial;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void CubeSelected(int cubeIndex)
    {
        if (cubeIndex == sequence[userSequenceIndex])
        {
            userSequenceIndex++;
            if (userSequenceIndex >= sequence.Count)
            {
                // Completed the sequence successfully
                userSequenceIndex = 0;
                StartGame();
            }
        }
        else
        {
            // Failed to match the sequence
            userSequenceIndex = 0;
            sequence.Clear();
            // Handle game over logic here, perhaps show a message to the user
        }
    }
}
