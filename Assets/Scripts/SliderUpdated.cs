using MixedReality.Toolkit.UX;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SliderUpdated : MonoBehaviour
{
    // Start is called before the first frame update

    private Slider mySlider;

    [SerializeField]
    private TextMeshPro textObject;

    private void Awake()
    {
        mySlider = GetComponent<Slider>();

        if (mySlider == null)
        {
            Debug.LogError("Slider component not found!");
            return;
        }

        if (textObject == null)
        {
            Debug.LogError("TextObject is not set.");
            return;
        }

        // Add a listener to the slider value changed event
        mySlider.OnValueUpdated.AddListener(UpdateTextValue);
    }

    private void UpdateTextValue(SliderEventData eventData)
    {
        // Update the text object with the new slider value
        textObject.text = mySlider.Value.ToString("F0") + " cubes";  // "F2" rounds to 2 decimal places
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
