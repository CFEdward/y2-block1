using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    [SerializeField] private List<Collider> pathColliders = new();
    [SerializeField] private Camera drawCamera;
    public GameObject markerUsed = null;

    public List<int> checkpoints = new();
    public bool shouldReset = false;

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
        if (shouldReset) ResetDrawing();

        if (checkpoints.Count == pathColliders.Count && CheckContinuity())
        {
            Debug.Log("-> Shape COMPLETE!");
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

    private void ResetDrawing()
    {
        Debug.Log("-> TODO: Reset Drawing");
        StartCoroutine(ResetMarker());
        drawCamera.clearFlags = CameraClearFlags.SolidColor;
        
        checkpoints.Clear();
        shouldReset = false;
    }

    private IEnumerator ResetMarker()
    {
        markerUsed.layer = 0;
        yield return new WaitForSeconds(2f);
        markerUsed.layer = 8;
    }
}
