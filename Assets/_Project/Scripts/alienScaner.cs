using System.Collections;
using System.Collections.Generic;
using Unity.VRTemplate;
using UnityEngine;

public class alienScaner : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        //collect();
        //transform.SetParent(sampleSelector.Instance.transform, false);
    }


    public void collect()
    {
        GameData.alienScanned = true;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
