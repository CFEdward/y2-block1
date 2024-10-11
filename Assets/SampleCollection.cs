using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCollection : MonoBehaviour
{

    [SerializeField] private bool collected;

    // Start is called before the first frame update
    void Start()
    {
        //collect();
        //transform.SetParent(sampleSelector.Instance.transform, false);
    }


    public void collect()
    {
        if (!collected)
        {
            collected = true;
            if (sampleSelector.Instance != null)
            {
                Debug.Log("minstens dit werkt");
                collected = true;
                sampleSelector.Instance.addObject(gameObject);
                transform.SetParent(sampleSelector.Instance.transform, false);
            } else
            {
                Debug.LogWarning("sampleSelector not set");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
