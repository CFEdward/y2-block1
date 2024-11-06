using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    // Update is called once per frame
    void Start()
    {
        //transform.position = new Vector3(0f, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        Physics.SyncTransforms();
    }
}
