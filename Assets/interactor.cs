using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class multipleLines
{
    public string linesListName;
    public List<string> linesList;
}

public class interactor : MonoBehaviour
{
<<<<<<< Updated upstream:Assets/interactor.cs
    private Transform player;
    [SerializeField] private bool oneTimeInteraction;
    private bool hasInteracted;
    private float distance;
    [SerializeField] float distanceThreshold = 10f;
    [SerializeField] private bool inDistance;
=======
    [SerializeField] private Transform player;
    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private TextMeshProUGUI interactName;
    [Space(20)]

    [SerializeField] private bool hasInteracted;
    private float distance;
    [SerializeField] private bool inDistance = false;
>>>>>>> Stashed changes:Assets/_Project/Scripts/interactor.cs

    [Space(20)]
    [SerializeField] float distanceThreshold = 10f;
    [SerializeField] private bool oneTimeInteraction;
    [SerializeField] private bool rambleMode = false;

    [SerializeField] private int linesIndex = 0;
    
    public List<multipleLines> lines;
    [Space(20)]

    
    private bool interactActionPressed;


    public UnityEvent inRange;
    public UnityEvent outRange;
    public UnityEvent<List<string>, bool> interact;


    public static interactor instance { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //optional: persist this instance between scenes
        //DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        player = playerManager.instance.transform;


        interactAction.action.performed += i => interactActionPressed = true;
        //debug:
        //onInteractInput();
        interactName.text = lines[linesIndex].linesListName;
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

        if (lines.Count > 0 && linesIndex < lines.Count)
        {
<<<<<<< Updated upstream:Assets/interactor.cs
            onInteractInput();
=======
            if (interactActionPressed)
            {
                onInteractInput();
            }
        } else
        {
            if (!textManager.instance.inDialogue)
            {
                gameObject.SetActive(false);
            }
>>>>>>> Stashed changes:Assets/_Project/Scripts/interactor.cs
        }
    }

    public void onInteractInput()
    {
        interactActionPressed = false;
        if (inDistance && !hasInteracted && textManager.instance.interactPossible)
        {
            if (oneTimeInteraction)
            {
                hasInteracted = true;
            }
            if (linesIndex < lines.Count)
            {
                interact.Invoke(lines[linesIndex].linesList, rambleMode);
                linesIndex++;
                if (linesIndex < lines.Count)
                {
                    interactName.text = lines[linesIndex].linesListName;
                }
            } 
        }
    }
}
