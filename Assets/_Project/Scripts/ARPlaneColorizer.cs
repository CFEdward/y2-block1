using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlane))]
[RequireComponent(typeof(MeshRenderer))]

public class ARPlaneColorizer : MonoBehaviour
{
    private ARPlane _ARPlane;
    private MeshRenderer _planeMeshRenderer;

    private void Awake()
    {
        _ARPlane = GetComponent<ARPlane>();
        _planeMeshRenderer = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        UpdatePlaneColor();
    }

    private void UpdatePlaneColor()
    {
        Color planeMatColor = Color.gray;

        switch (_ARPlane.classification)
        {
            case PlaneClassification.Floor:
                planeMatColor = Color.green;
                break;

            case PlaneClassification.Wall:
                planeMatColor = Color.white;
                break;
            
            case PlaneClassification.Ceiling:
                planeMatColor = Color.red;
                break;

            case PlaneClassification.Table:
                planeMatColor = Color.yellow;
                break;

            case PlaneClassification.Seat:
                planeMatColor = Color.blue;
                break;

            case PlaneClassification.Door:
                planeMatColor = Color.magenta;
                break;

            case PlaneClassification.Window:
                planeMatColor = Color.cyan;
                break;
        }

        planeMatColor.a = 0f;
        _planeMeshRenderer.material.color = planeMatColor;
    }
}
