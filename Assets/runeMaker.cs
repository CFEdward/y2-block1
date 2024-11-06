using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runeMaker : MonoBehaviour
{
    [SerializeField] private GameObject edibleRune;
    [SerializeField] private GameObject poisonRune;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createEdibleRune()
    {
        Instantiate(edibleRune, transform.forward, Quaternion.Euler(-90, 0, 0));
    }
    public void createPoisonRune()
    {
        Instantiate(poisonRune, transform.forward, Quaternion.Euler(-90, 0, 0));
    }
}
