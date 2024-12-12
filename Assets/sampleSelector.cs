using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class sampleSelector : MonoBehaviour
{
    [SerializeField] private List<GameObject> collectedSamples;
    [SerializeField] private int currentSample;

    public UnityEvent<List<GameObject>> updateObjects;


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



    void Start()
    {
        //check if the sampleCollector object has children, if so, they should be collected
        foreach (Transform child in transform)
        {
            if (!collectedSamples.Contains(child.gameObject))
            {
                collectedSamples.Add(child.gameObject);
            }
        }
        //if it starts off with samples that are already collected, it'll update the dropdown menu to include these
        if (collectedSamples != null)
        {
            updateObjects.Invoke(collectedSamples);
        }
        showSample(currentSample);
    }


    //code for switching what gameobject it's showing in the samplecollector
    public void showSample(int sampleID)
    {
        int currentSampleID = 0;
        foreach (GameObject sample in collectedSamples)
        {
            //if it's already active, it's the old gameobject it was showing. if you had picked that up,
            ////it wont have the samplecollector as its parent anymore, so i fix that here 
            if (sample.activeSelf == true)
            {
                sample.SetActive(false);
                sample.transform.SetParent(transform);
            }
            //if the sample has a rigidbody, it might have fallen a bit before being turned off,
            ////or in the case of the sample it was showing before, the player might have moved it
            sample.transform.position = transform.position;

            //if it is the sample it should be showing:
            if (currentSampleID == sampleID)
            {
                //then it'll turn it on
                sample.SetActive(true);
            }
            else
            {
                //otherwise it'll turn it off
                sample.SetActive(false);
            }
            //make it itterate over all the samples
            currentSampleID++;
        }
        //keep track of what sample it is showing
        currentSample = sampleID;
    }


    //this collects a sample, this is mainly called from the sampleCollection script
    public void addObject(GameObject sampleToAdd)
    {
        //add a reference to it in the list of samples
        collectedSamples.Add(sampleToAdd);
        //the sampleCollector should be the parent of the object
        sampleToAdd.transform.SetParent(transform);
        //update the selection list
        updateObjects.Invoke(collectedSamples);
        //this should only be applicable if this is the first sample you collect, but i run it everytime as a failsafe
        showSample(currentSample);
    }


    //kind of self explanitory, if it switches scenes, turn the sample on if it switches to the shipscene,
    //and turn it off if it switches to the planet
    public void onSceneSwitch(string sceneName)
    {
        if (sceneName == "ShipScene")
        {
            showSample(currentSample);
        }else
        {
            showSample(-1);
        }
        
    }
}
