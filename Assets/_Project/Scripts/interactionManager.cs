using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class multipleLines
{
    public string linesName;
    public List<string> lines;
}

public class interactionManager: MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject inputIndicator;
    [SerializeField] private TextMeshProUGUI inputIndicatorText;
    [SerializeField] private textManager textManager;

    [SerializeField] private bool oneTimeInteraction;
    private bool hasInteracted;
    private float distance;
    [SerializeField] float distanceThreshold = 10f;
    [SerializeField] private bool inDistance = false;

    private int linesIndex = 0;

    [SerializeField] private List<multipleLines> allLines;


    [SerializeField] private InputActionReference interactAction;
    private bool interactActionPressed;
    public bool waitOneInteraction = false;


    public UnityEvent inRange;
    public UnityEvent outRange;
    public UnityEvent<List<string>> interact;
    public UnityEvent dialogueDone;



    public static interactionManager instance { get; private set; }
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

        interactAction.action.performed += i => interactActionPressed = true;

        inputIndicatorText.text = allLines[linesIndex].linesName;

        //optional: persist this instance between scenes
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        player = playerManager.instance.transform;
        textManager = GetComponentInChildren<textManager>();

        //debug:
        //onInteractInput();
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

        if (textManager.interactPossible && inDistance && !hasInteracted && linesIndex < allLines.Count)
        {
            inputIndicator.SetActive(true);
        } else
        {
            inputIndicator.SetActive(false);
        }

        if (interactActionPressed)
        {
            if (!waitOneInteraction)
            {
                onInteractInput();
            }
            else
            {
                waitOneInteraction = false;
                interactActionPressed = false;
            }
        }
    }

    public void onInteractInput()
    {
        interactActionPressed = false;
        if (inDistance && textManager.interactPossible && !hasInteracted && linesIndex < allLines.Count)
        {
            interact.Invoke(allLines[linesIndex].lines);
            linesIndex++;
            if (linesIndex > allLines.Count)
            {
                if (oneTimeInteraction)
                {
                    hasInteracted = true;
                }
            }
            else
            {
                if (linesIndex < allLines.Count)
                {

                    inputIndicatorText.text = allLines[linesIndex].linesName;
                } else
                {
                    dialogueDone.Invoke();
                }
            }
        }
    }
}
