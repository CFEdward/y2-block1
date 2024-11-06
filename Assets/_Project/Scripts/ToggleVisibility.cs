using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleVisibility : MonoBehaviour
{
    private bool shouldMove = false;
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (gameObject.CompareTag("ShipAlien"))
            {
                if (shouldMove)
                {
                    transform.position = GameData.alienShipPosition;
                    Physics.SyncTransforms();
                }
                gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<Renderer>().enabled = true;
            }
        }
        else
        {
            if (gameObject.CompareTag("ShipAlien"))
            {
                shouldMove = true;
                GameData.alienShipPosition = transform.position;
                gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                gameObject.GetComponent<Renderer>().enabled = false;
            }
        }
    }
}
