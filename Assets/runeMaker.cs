using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runeMaker : MonoBehaviour
{
    [SerializeField] private GameObject edibleRune;
    [SerializeField] private GameObject poisonRune;
    private int runeToSpawn = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnRune()
    {
        if (runeToSpawn == 0)
        {
            createEdibleRune();
            runeToSpawn = 1;
        } else
        {
            createPoisonRune();
        }
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
