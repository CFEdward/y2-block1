using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class interactor : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private bool oneTimeInteraction;
    private bool hasInteracted;
    private float distance;
    [SerializeField] float distanceThreshold = 10f;
    [SerializeField] private bool inDistance = false;

    [SerializeField] private List<string> lines;


    [SerializeField] private InputActionReference interactAction;
    private bool interactActionPressed;


    public UnityEvent inRange;
    public UnityEvent outRange;
    public UnityEvent<List<string>> interact;


    void Start()
    {
        player = playerManager.instance.transform;


        interactAction.action.performed += i => interactActionPressed = true;
        //debug:
        onInteractInput();
    }


    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);

        if (distance <= distanceThreshold && !hasInteracted)
        {
            if (!inDistance)
            {
                inDistance = true;
                inRange.Invoke();
            }
        }
        else
        {
            if (inDistance)
            {
                inDistance = false;
                outRange.Invoke();
            }
        }
        if (interactActionPressed)
        {
            onInteractInput();
            
        }
    }

    public void onInteractInput()
    {

        interactActionPressed = false;
        if (inDistance && !hasInteracted)
        {
            Debug.Log(" jeeeeej");
            if (oneTimeInteraction)
            {
                hasInteracted = true;
            }
            interact.Invoke(lines);
        }
    }
}
