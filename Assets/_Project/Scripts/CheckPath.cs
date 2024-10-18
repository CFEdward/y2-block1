using UnityEngine;

public class CheckPath : MonoBehaviour
{
    private Drawing drawingParent;
    public int id;

    private void Awake()
    {
        drawingParent = GetComponentInParent<Drawing>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Marker" && drawingParent.checkpoints.IndexOf(id) == -1)
        {
            drawingParent.checkpoints.Add(id);

            if (drawingParent.checkpoints.IndexOf(id) != id)
                drawingParent.shouldReset = true;
        }
        else
        {
            drawingParent.shouldReset = true;
        }
    }
}
