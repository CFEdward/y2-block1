using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sampleSelector : MonoBehaviour
{
    [SerializeField] private List<GameObject> collectedSamples;
    [SerializeField] private int currentSample;


    public static sampleSelector Instance { get; private set; }

    void Awake()
    {
        // Ensure that there's only one instance of this class
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Optional: Persist this instance between scenes
        // DontDestroyOnLoad(gameObject);
    }



    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (!collectedSamples.Contains(child.gameObject))
            {
                collectedSamples.Add(child.gameObject);
            }
        }
        showSample(currentSample);
    }




    public void showSample(int sampleID)
    {
        int currentSampleID = 0;
        foreach (GameObject sample in collectedSamples)
        {
            if (sample.activeSelf == true)
            {
                sample.SetActive(false);
                sample.transform.SetParent(transform);
            }
            sample.transform.position = transform.position;

            if (currentSampleID == sampleID)
            {
                sample.SetActive(true);
            }
            else
            {
                sample.SetActive(false);
            }
            currentSampleID++;
        }
        currentSample = sampleID;
    }

    public void addObject(GameObject sampleToAdd)
    {
        Debug.Log("dit werkt ook");
        collectedSamples.Add(sampleToAdd);
        sampleToAdd.transform.SetParent(transform);
        showSample(currentSample);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
