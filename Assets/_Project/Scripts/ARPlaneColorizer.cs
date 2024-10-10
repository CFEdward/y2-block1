using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlane))]
[RequireComponent(typeof(MeshRenderer))]

public class ARPlaneColorizer : MonoBehaviour
{
    private ARPlane ARPlane;
    private MeshRenderer planeMeshRenderer;

    private void Awake()
    {
        ARPlane = GetComponent<ARPlane>();
        planeMeshRenderer = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        UpdatePlaneColor();
    }

    private void UpdatePlaneColor()
    {
        Color planeMatColor = Color.gray;

        switch (ARPlane.classification)
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
        Color transparent = Color.white;
        transparent.a = 0f;
        planeMeshRenderer.material.color = planeMatColor;
        var lineRenderer = ARPlane.GetComponentInChildren<LineRenderer>();
        lineRenderer.startColor = transparent;
        lineRenderer.endColor = transparent;
    }
}
