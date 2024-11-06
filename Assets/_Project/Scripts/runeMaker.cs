using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class runeMaker : MonoBehaviour
{
    private GameObject edibleRune;
    private GameObject poisonRune;
    private Drawing test;
    //private Object[] objects;
    private int runeToSpawn = 0;

    public static runeMaker instance { get; private set; }

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
        //edibleRune = Resources.Load("DrawingEdible") as GameObject;
        //poisonRune = Resources.Load("DrawingPoisonous") as GameObject;
        //objects = FindObjectsOfType(typeof(Test), true);
        //Debug.Log(objects);
    }

    public void spawnRune()
    {
        Debug.Log("spawn rune");
        if (runeToSpawn == 0)
        {
            createEdibleRune();
            runeToSpawn = 1;
        }
        else
        {
            createPoisonRune();
        }
    }

    public void createEdibleRune()
    {
        //Instantiate(edibleRune, transform.forward, Quaternion.Euler(-90, 0, 0));
        //foreach (var obj in objects)
        //{
        //    if ((obj as GameObject).CompareTag("Edible"))
        //    {
        //        (obj as GameObject).SetActive(true);
        //    }
        //}
        var obj = GameObject.FindWithTag("Edible");
        var objRen = obj.GetComponent<Test>();
        objRen.meshRenderer.enabled = true;
        objRen.spriteRenderer.enabled = true;
    }
    public void createPoisonRune()
    {
        //Instantiate(poisonRune, transform.forward, Quaternion.Euler(-90, 0, 0));
        //foreach (var obj in objects)
        //{
        //    if ((obj as GameObject).CompareTag("Poisonous"))
        //    {
        //        (obj as GameObject).SetActive(true);
        //    }
        //}
        var obj = GameObject.FindWithTag("Poisonous");
        var objRen = obj.GetComponent<Test>();
        objRen.meshRenderer.enabled = true;
        objRen.spriteRenderer.enabled = true;
    }
}