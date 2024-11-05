using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class textManager : MonoBehaviour
{
    private GameObject inputIndicator;
    private GameObject dialogueBox;
    public bool inDialogue;
    [SerializeField] private TextMeshProUGUI textBox;
    private Transform player;
    [SerializeField] float turnSmoothSpeed;
    public float textSpeed;
    [SerializeField] private List<string> lines;
    private int index;
    [SerializeField] bool rambleMode = false;
    [SerializeField] bool startTalking = false;
    public bool interactPossible = true;

    [SerializeField] private InputActionReference nextLineAction;
    private bool nextLineActionPressed;


    public static textManager instance { get; private set; }

    //i'm using awake so any other script can already acces the textManager on start
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
        foreach (Transform child in transform)
        {
            if (child.CompareTag("inputIndicator"))
            {
                inputIndicator = child.gameObject;
            }
        }
        dialogueBox = textBox.transform.parent.gameObject;


        nextLineAction.action.performed += i => nextLineActionPressed = true;

        if (startTalking)
        {
            startDialogue(lines, rambleMode);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 direction = transform.position - player.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSmoothSpeed);
        }

        if (nextLineActionPressed)
        {
            onNextLineInput();
        }
        if (!inDialogue && rambleMode)
        {
            startDialogue(lines);
        }
    }


    private void onNextLineInput()
    {
        StopAllCoroutines();
        nextLineActionPressed = false;
        if (inDialogue)
        {
            if (index < lines.Count - 1)
            {
                index++;
                //StartCoroutine(typeOutLine());
                StartCoroutine(typeOutLine(lines[index]));
            }
            else
            {
                
                if (rambleMode)
                {
                    index = 0;
                    StartCoroutine(typeOutLine(lines[index]));
                } else
                {
                    stopDialogue();
                }
            }
        }
        else
        {
            interactPossible = true;
        }
    }

    public void inRange()
    {
        if (!interactPossible)
        {
            inputIndicator.SetActive(true);
        }
    }
    public void outRange()
    {
        inputIndicator.SetActive(false);
    }



    public void startDialogue(List<string> dialogueLines, bool goIntoRambleMode = false)
    {
        StopAllCoroutines();
        rambleMode = goIntoRambleMode;
        lines = dialogueLines;
        inputIndicator.SetActive(false);
        dialogueBox.SetActive(true);

        inDialogue = true;
        index = 0;
        StartCoroutine(typeOutLine(lines[0]));
    }

    public void stopDialogue()
    {
        //inputIndicator.SetActive(true);
        dialogueBox.SetActive(false);
        //interactPossible = true;
        inDialogue = false;
    }

    IEnumerator typeOutLine(string lineToType)
    {
        if (!rambleMode)
        {
            interactPossible = false;
        } else
        {
            interactPossible = true;
            inputIndicator.SetActive(true);
        }
        
        textBox.text = string.Empty;
        foreach (char character in lineToType.ToCharArray())
        {
            textBox.text += character;
            yield return new WaitForSeconds(textSpeed);
        }

        if (rambleMode)
        {
            yield return new WaitForSeconds(textSpeed * 5);
            onNextLineInput();
            //textSpeed = 0;
        }

        //debug:
        onNextLineInput();
    }


    /*
    private GameObject inputIndicator;
    private GameObject dialogueBox;
    private bool inDialogue;
    private TextMeshProUGUI textBox;
    private Transform player;
    [SerializeField] float turnSmoothSpeed;
    public float textSpeed;
    [SerializeField] private List<string> lines;

    [SerializeField] public List<optionBoxLogic> optionBoxes;



    //i made this myself using only this stackoverflow question: https://stackoverflow.com/questions/46595205/unity-arrays-with-string-int-and-sprites
    //and this microsoft article: https://learn.microsoft.com/en-us/dotnet/standard/collections/
    //and this is probably my proudest moment while coding yet, THIS WORKS WHILE I HADN'T EVEN THOUGHT OF USING SOMETHING LIKE THIS BEFORE THE STACKOVERFLOW QUESTION
    //(it was a lot of trial and error haha)
    [SerializeField] private List<dialogueAndOptions> dialogue = new List<dialogueAndOptions>();

    [SerializeField]private int index;

    public static textManager instance { get; private set; }

    //i'm using awake so any other script can already acces the textManager on start
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


    // Start is called before the first frame update
    void Start()
    {
        player = playerManager.instance.transform;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("inputIndicator"))
            {
                Debug.Log("dit werkt");
                inputIndicator = child.gameObject;
            }
        }
        textBox = GetComponentInChildren<TextMeshProUGUI>();
        dialogueBox = textBox.gameObject;


        //debug stuff:
        //textBox.text = "guuuuuuuuuhuhuhuhuhuhuh";
        //startDialogue(lines);
        Debug.Log(dialogue[1].line);
        startDialogue(dialogue);
        //dialogue[1].options[1].dialogueOption.Invoke();
    }

    public void debug0()
    {
        Debug.Log("dit fucking werkt, wtf");

    }

    public void debug1()
    {
        Debug.Log("numbero dos");

    }


    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 direction = transform.position - player.position;
            direction.y = 0; 
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSmoothSpeed); 
        }

        if (inDialogue)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (index < lines.Count - 1)
                {
                    Debug.Log("test");
                    index++;
                    //StartCoroutine(typeOutLine());
                    typeLine(index);

                }
                    
            }
        }
    }


    public void inRange()
    {
        if (!inDialogue)
        {
            inputIndicator.SetActive(true);
        }
    }
    public void outRange()
    {
        inputIndicator.SetActive(false);
    }


    public void startDialogue(List<dialogueAndOptions> dialogueAndOptions)
    {
        dialogue = dialogueAndOptions;
        
        inDialogue = true;
        index = 0;
        typeLine(0);
    }

    public void typeLine(int indexOfLine)
    {
        string line = dialogue[indexOfLine].line;

        if (dialogue[indexOfLine].options.Count > optionBoxes.Count)
        {
            Debug.Log("not eneugh option boxes");
        } else if (dialogue[indexOfLine].options.Count > 0)
        {
            int currentIndex = 0;

            //foreach(optionBoxLogic optionlogic in optionBoxes)
            //{
                //if (dialogue[indexOfLine].options[currentIndex] != null)
                //{
                    //optionlogic.gameObject.SetActive(true);
                    //optionlogic.typeLine(dialogue[indexOfLine].options[currentIndex].optionName);
                //}
                //currentIndex++;
            }
            
            foreach (dialogueOptions option in dialogue[indexOfLine].options)
            {
                optionBoxes[currentIndex].gameObject.SetActive(true);
                optionBoxes[currentIndex].gameObject.SetActive(true);
                //optionBoxes[currentIndex].gameObject.GetComponent<Button>().onClick.AddListener(optionPressed();

                optionBoxes[currentIndex].GetComponent<optionBoxLogic>().typeLine(option.optionName);
                currentIndex++;
            }
            if (dialogue[indexOfLine].options.Count < optionBoxes.Count)
            {
                while (currentIndex < optionBoxes.Count)
                {
                    optionBoxes[currentIndex].gameObject.SetActive(false);
                    currentIndex++;
                }
            }
        }
        else
        {
            foreach (optionBoxLogic logic in optionBoxes)
            {
                logic.gameObject.SetActive(false);
            }
        }

        StartCoroutine(typeOutLine(line));
    }

    public void buttonPressed(int optionBoxIndex)
    {

    }

    IEnumerator typeOutLine(string lineToType)
    {
        textBox.text = string.Empty;
        foreach (char character in lineToType.ToCharArray())
        {
            textBox.text += character;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    */
    /*
    public void startDialogue(List<string> Lines)
    {
        lines = Lines;
        inDialogue = true;
        index = 0;
        StartCoroutine(TypeLine());
    }


    */

}



