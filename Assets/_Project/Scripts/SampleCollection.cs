using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCollection : MonoBehaviour
{
    [SerializeField] private bool badFruit;

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
            if (badFruit)
            {
                GameData.wrongFruitCollected = true;
            }
            else
            {
                GameData.fruitCollected = true;
            }
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
        if (collected)
        {
            transform.localScale = new Vector3(0.28372f, 0.28372f, 0.28372f);
        }
    }
}
