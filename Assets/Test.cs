using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Test : MonoBehaviour
{
    private BoxCollider collider;
    [SerializeField] private GameObject attachPoint;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sample")
        {
            other.transform.position = attachPoint.transform.position;
            other.transform.rotation = attachPoint.transform.rotation;
        }
    }
}
