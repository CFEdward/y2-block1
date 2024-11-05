using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactorPlacement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform interactorTF;
    private float distance;
    [SerializeField] private bool inDistance = false;
    [SerializeField] float distanceThreshold = 10f;
    [SerializeField] List<multipleLines> lines;



    // Start is called before the first frame update
    void Start()
    {
        player = playerManager.instance.transform;
        interactorTF = interactor.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);

        if (distance <= distanceThreshold)
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

    private void inRange()
    {
        interactorTF.position = transform.position;
        interactorTF.gameObject.SetActive(true);
        interactor.instance.lines = lines; 

    }

    private void outRange()
    {
        interactor.instance.lines.Clear();
    }
}
