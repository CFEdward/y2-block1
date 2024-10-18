using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    [SerializeField] private List<Collider> pathColliders = new();

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
        checkpoints.Clear();
        shouldReset = false;
    }
}
