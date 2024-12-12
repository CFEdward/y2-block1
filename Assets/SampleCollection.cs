using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCollection : MonoBehaviour
{
    [SerializeField] private bool collected;

    //this runs when you grab a sample, and then let go of it 
    public void collect()
    { 
        if (!collected)
        {
            collected = true;
            //check if the singleton sampleSelector exists
            if (sampleSelector.Instance != null)
            {
                collected = true;
                //make the sampleselector add this object to the list of samples
                sampleSelector.Instance.addObject(gameObject);
                //it should also be the parent object of the sample
                transform.SetParent(sampleSelector.Instance.transform, false);
            } else
            {
                Debug.LogWarning("sampleSelector not set");
            }
        }
    }
}
