using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    [SerializeField] private List<Collider> pathColliders = new();
    [SerializeField] private Camera drawCamera;
    public GameObject markerUsed = null;

    public List<int> checkpoints = new();
    public bool shouldReset = false;

    [SerializeField] private TextMeshPro text;
    private int timesCompleted = 0;

    private void Awake()
    {
        checkpoints.Clear();
    }

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < pathColliders.Count; i++)
        {
            pathColliders[i].AddComponent<CheckPath>();
            pathColliders[i].GetComponent<CheckPath>().id = i;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        drawCamera.clearFlags = CameraClearFlags.Nothing;
        if (shouldReset) ResetDrawing(false);

        if (checkpoints.Count == pathColliders.Count && CheckContinuity())
        {
            Debug.Log("-> Shape COMPLETE!");
            timesCompleted++;
            //checkpoints.Clear();
            checkpoints = new();
            ResetDrawing(true);
        }

        if (timesCompleted >= 2)
        {
            StartCoroutine(DisplayText(true));
        }
    }

    private bool CheckContinuity()
    {
        for (int i = 0; i < checkpoints.Count; i++)
        {
            if (checkpoints[i] == i)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetDrawing(bool correct)
    {
        Debug.Log("-> TODO: Reset Drawing");
        drawCamera.clearFlags = CameraClearFlags.SolidColor;
        StartCoroutine(ResetMarker());
        StartCoroutine(DisplayText(correct));
        
        
        //checkpoints.Clear();
        checkpoints = new();
        shouldReset = false;
    }

    private IEnumerator DisplayText(bool correct)
    {
        switch (correct)
        {
            case true:
                text.text = "Good!";
                text.color = Color.green;
                text.gameObject.SetActive(true);
                yield return new WaitForSeconds(2f);
                if (timesCompleted >= 2)
                {
                    GameData.endDraw.Invoke();
                    Destroy(gameObject.transform.parent.gameObject);
                }
                else
                {
                    text.gameObject.SetActive(false);
                }
                break;
            case false:
                text.text = "Wrong!";
                text.color = Color.red;
                text.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                text.gameObject.SetActive(false);
                break;
            default: break;
        }
    }

    private IEnumerator ResetMarker()
    {
        markerUsed.layer = 0;
        yield return new WaitForSeconds(2f);
        markerUsed.layer = 8;
    }
}
