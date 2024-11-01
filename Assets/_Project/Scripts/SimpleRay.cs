using UnityEngine;

public class SimpleRay : MonoBehaviour
{
    public static RaycastHit hit;

    // Update is called once per frame
    protected void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
            //Debug.Log(hit.collider);
        }
        else
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }
    }
}
