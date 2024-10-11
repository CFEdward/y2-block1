using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class dropdownMenuOptions : MonoBehaviour
{
    [SerializeField] private List<string> collectedSampleNames;
    [SerializeField] private Dropdown dropdown;

    private int currentOption;

    public void dropdownUpdate()
    {
        currentOption = dropdown.value;
        sampleSelector.Instance.showSample(currentOption);
    }

    public void showOptions(List<GameObject> collectedSamples)
    {
        collectedSampleNames.Clear();
        foreach (GameObject sample in collectedSamples)
        {
            collectedSampleNames.Add (sample.name);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(collectedSampleNames);
        dropdown.value = currentOption;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
