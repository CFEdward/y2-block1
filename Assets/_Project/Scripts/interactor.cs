using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactor : MonoBehaviour
{ 
    private Transform player;

    private float distance;
    [SerializeField] float distanceThreshold = 10f;
    [SerializeField] private bool inDistance = false;
    private bool hasInteracted;

    [SerializeField] private List<string> lines;


    // Start is called before the first frame update
    void Start()
    {
        //player = playerManager.instance.Transform;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);

        if (distance <= distanceThreshold && !hasInteracted)
        {
            if (!inDistance)
            {
                inDistance = true;
                inRange();
            }
        }
        else
        {
            if (inDistance)
            {
                inDistance = false;
                outRange();
            }
        }
    }


    void inRange()
    {
        interactionManager.instance.gameObject.SetActive(true);
    }

    void outRange()
    {
        interactionManager.instance.gameObject.SetActive(false);
    }

}
