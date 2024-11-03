using UnityEngine;

public class DetectSurface : MonoBehaviour
{
    private RaycastHit hit;
    public float adjustedPos;

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit))
        {
            var distanceToGround = hit.distance;
            var currentPos = transform.position;
            adjustedPos = currentPos.y - distanceToGround;
        }
    }
}
