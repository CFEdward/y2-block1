using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ARPlaneManager))]

public class SceneController : MonoBehaviour
{
    [SerializeField] private bool debugMode = true;

    private List<Collider> colliders = new();
    private List<RaycastHit> raycastHits = new();

    //[SerializeField] private InputActionReference togglePlanesAction;
    [SerializeField] private InputActionReference rightActivateAction;

    [SerializeField] private InputActionReference switchSceneAction;

    [SerializeField] private GameObject grabbableCube;

    private ARAnchorManager anchorManager;
    private List<ARAnchor> anchors = new();

    private ARPlaneManager planeManager;
    //private bool isVisible = false;
    //private int numPlanesAddedOccurred = 0;

    public static Vector3 playerShipPosition;
    public static Vector3 playerPlanetPosition;

    protected void OnEnable()
    {
        playerShipPosition = transform.position;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        Debug.Log("-> SceneController::Start()");

        planeManager = GetComponent<ARPlaneManager>();

        if (planeManager == null)
        {
            Debug.LogError("-> Can't find 'ARPlaneManager'");
        }

        anchorManager = GetComponent<ARAnchorManager>();

        if (anchorManager == null)
        {
            Debug.LogError("-> Can't find 'ARAnchorManager'");
        }

        switchSceneAction.action.performed += OnSwitchSceneAction;
        //togglePlanesAction.action.performed += OnTogglePlanesAction;
        //planeManager.planesChanged += OnPlanesChanged;
        anchorManager.anchorsChanged += OnAnchorsChanged;
        rightActivateAction.action.performed += OnRightActivateAction;

        SceneLoader.Instance.OnLoadBegin.AddListener(SaveLocation);
        SceneLoader.Instance.OnLoadEnd.AddListener(LoadLocation);
    }

    private void OnAnchorsChanged(ARAnchorsChangedEventArgs obj)
    {
        // remove any anchors that have been removed outside our control, such as during a session reset
        foreach (var removedAnchor in obj.removed)
        {
            anchors.Remove(removedAnchor);
            Destroy(removedAnchor.gameObject);
        }
    }

    private void OnRightActivateAction(InputAction.CallbackContext obj)
    {
        SpawnGrabbableCube();
    }

    public void OnSwitchSceneAction(InputAction.CallbackContext obj)
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            playerPlanetPosition = transform.position;
            SceneLoader.Instance.LoadNewScene("ShipScene");
        }
    }

    public void SwitchSceneOnControllerPickup()
    {
        if (!debugMode && SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(SwitchSceneDelay(3f));
        }
    }

    private IEnumerator SwitchSceneDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        playerShipPosition = transform.position;
        SceneLoader.Instance.LoadNewScene("PlanetScene");
    }

    private void SpawnGrabbableCube()
    {
        Debug.Log("-> SceneController::SpawnGrabbableCube()");

        Vector3 spawnPosition;

        // Iterate through each plane found in the scene
        foreach (var plane in planeManager.trackables)
        {
            // Detect if the plane is a table, if so, spawn a cube on it
            if (plane.classification == PlaneClassification.Table)
            {
                spawnPosition = plane.transform.position;
                spawnPosition.y += 0.3f;
                Instantiate(grabbableCube, spawnPosition, Quaternion.identity);
            }
        }
    }

    public void SaveLocation()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            GameData.playerShipPosition = this.transform.position;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            GameData.playerPlanetPosition = this.transform.position;
        }
    }

    public void LoadLocation()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            this.transform.position = GameData.playerShipPosition;
            Physics.SyncTransforms();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            this.transform.position = GameData.playerPlanetPosition;
            Physics.SyncTransforms();
        }
    }

    protected void OnDestroy()
    {
        Debug.Log("-> SceneController::OnDestroy()");
        switchSceneAction.action.performed -= OnSwitchSceneAction;
        //togglePlanesAction.action.performed -= OnTogglePlanesAction;
        //planeManager.planesChanged -= OnPlanesChanged;
        anchorManager.anchorsChanged -= OnAnchorsChanged;
        rightActivateAction.action.performed -= OnRightActivateAction;
    }

    /*
    private void OnTogglePlanesAction(InputAction.CallbackContext obj)
    {
        isVisible = !isVisible;
        float fillAlpha = isVisible ? 0f : 0f; // change these values for debug
        float lineAlpha = isVisible ? 0f : 0f;

        Debug.Log("-> OnTogglePlanesAction() - trackables.count: " + planeManager.trackables.count);

        foreach (var plane in planeManager.trackables)
        {
            SetPlaneAlpha(plane, fillAlpha, lineAlpha);
        }
    }
    */

    /*
    private void SetPlaneAlpha(ARPlane plane, float fillAlpha, float lineAlpha)
    {
        var meshRenderer = plane.GetComponentInChildren<MeshRenderer>();
        var lineRenderer = plane.GetComponentInChildren<LineRenderer>();

        if (meshRenderer != null)
        {
            Color color = meshRenderer.material.color;
            color.a = fillAlpha;
            meshRenderer.material.color = color;
        }

        if (lineRenderer != null)
        {
            // Get the current start and end colors
            Color startColor = lineRenderer.startColor;
            Color endColor = lineRenderer.endColor;

            // Set the alpha component
            startColor.a = lineAlpha;
            endColor.a = lineAlpha;

            // Apply the new colors with updated alpha
            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
        }
    }
    */

    /*
    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (args.added.Count > 0)
        {
            numPlanesAddedOccurred++;

            foreach (var plane in planeManager.trackables)
            {
                PrintPlaneLabel(plane);
            }

            Debug.Log("-> Number of planes: " + planeManager.trackables.count);
            Debug.Log("-> Num Planes Added Occurred: " + numPlanesAddedOccurred);
        }
    }
    */

    /*
    private void PrintPlaneLabel(ARPlane plane)
    {
        string label = plane.classification.ToString();
        string log = $"Plane ID: {plane.trackableId}, Label: {label}";
        Debug.Log(log);
    }
    */

}
