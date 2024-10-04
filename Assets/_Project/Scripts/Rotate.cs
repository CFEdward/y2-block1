using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(Vector3.up, 10f * Time.deltaTime);
    }
}
