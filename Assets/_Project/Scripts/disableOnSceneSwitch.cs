using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableOnSceneSwitch : MonoBehaviour
{
    [SerializeField] private string sceneToTurnOn = "ShipScene";


    public void disable(string newScene)
    {
        if (newScene == sceneToTurnOn)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        
    }
}
