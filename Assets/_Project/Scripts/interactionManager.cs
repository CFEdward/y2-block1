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
    public bool spawnThing = false;
    public UnityEvent<int> endOfLines;
    public bool pauzeAfterLine = false;
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
    [SerializeField] private bool paused = false;


    private int linesIndex = 0;

    [SerializeField] private List<multipleLines> allLines;
    [SerializeField] private List<multipleLines> allLinesTheSecond;
    [SerializeField] private List<multipleLines> allLinesTheThird;


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

        GameData.endDraw.AddListener(onUnPauze);

        //debug:
        //onInteractInput();
    }


    void Update()
    {
        if (GameData.fruitCollected && allLines != allLinesTheSecond)
        {
            allLines = allLinesTheSecond;
            linesIndex = 0;
            paused = false;
        }
        else if (GameData.wrongFruitCollected && allLines != allLinesTheThird)
        {
            allLines = allLinesTheThird;
            linesIndex = 0;
            paused = false;
        }
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

        if (textManager.interactPossible && inDistance && !hasInteracted && linesIndex < allLines.Count && !paused)
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

    public void onLinesEnd()
    {
        if (allLines[linesIndex - 1].spawnThing)
        {

        }
        allLines[linesIndex - 1].endOfLines.Invoke(linesIndex);
        if (allLines[linesIndex - 1].pauzeAfterLine)
        {
            onPauze();
        }
    }

    public void onPauze()
    {
        paused = true;
    }
    public void onUnPauze()
    {
        paused = false;
    }

    public void onInteractInput()
    {
        interactActionPressed = false;
        if (inDistance && textManager.interactPossible && !hasInteracted && linesIndex < allLines.Count && !paused)
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
