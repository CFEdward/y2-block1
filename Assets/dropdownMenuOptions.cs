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
        //make the sampleSelector show the sample you just selected in the dropdown menu
        sampleSelector.Instance.showSample(currentOption);
    }

    public void showOptions(List<GameObject> collectedSamples)
    { 
        collectedSampleNames.Clear();
        foreach (GameObject sample in collectedSamples)
        {
            //get the name of the sample, and add it to a list of names
            collectedSampleNames.Add(sample.name);
        }
        dropdown.ClearOptions();
        //add all the names in the lists to the dropdown menu
        dropdown.AddOptions(collectedSampleNames);
        //make it select the correct sample again
        dropdown.value = currentOption;
    }
}
