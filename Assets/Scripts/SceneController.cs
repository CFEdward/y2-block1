using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _togglePlanesAction;

    [SerializeField]
    private InputActionReference _activateAction;

    [SerializeField]
    private InputActionReference _switchSceneAction;

    [SerializeField]
    private GameObject _grabbableCube;

    private ARPlaneManager _planeManager;
    private bool _isVisible = true;
    private int _numPlanesAddedOccurred = 0;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("-> SceneController::Start()");

        _planeManager = GetComponent<ARPlaneManager>();

        if (_planeManager != null)
        {
            Debug.LogError("-> Can't find 'ARPlaneManager'");
        }

        _switchSceneAction.action.performed += OnSwitchSceneAction;
        _togglePlanesAction.action.performed += OnTogglePlanesAction;
        _planeManager.planesChanged += OnPlanesChanged;
        _activateAction.action.performed += OnActivateAction;
    }

    private void OnActivateAction(InputAction.CallbackContext obj)
    {
        SpawnGrabbableCube();
    }

    private void OnSwitchSceneAction(InputAction.CallbackContext obj)
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex == 0 ? 1 : 0);
    }

    private void SpawnGrabbableCube()
    {
        Debug.Log("-> SceneController::SpawnGrabbableCube()");

        Vector3 spawnPosition;

        // Iterate through each plane found in the scene
        foreach (var plane in _planeManager.trackables)
        {
            // Detect if the plane is a table, if so, spawn a cube on it
            if (plane.classification == PlaneClassification.Table)
            {
                spawnPosition = plane.transform.position;
                spawnPosition.y += 0.3f;
                Instantiate(_grabbableCube, spawnPosition, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnTogglePlanesAction(InputAction.CallbackContext obj)
    {
        _isVisible = !_isVisible;
        float fillAlpha = _isVisible ? 0.3f : 0f;
        float lineAlpha = _isVisible ? 1f : 0f;

        Debug.Log("-> OnTogglePlanesAction() - trackables.count: " + _planeManager.trackables.count);

        foreach (var plane in _planeManager.trackables)
        {
            SetPlaneAlpha(plane, fillAlpha, lineAlpha);
        }
    }

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

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (args.added.Count > 0)
        {
            _numPlanesAddedOccurred++;

            foreach (var plane in _planeManager.trackables)
            {
                PrintPlaneLabel(plane);
            }

            Debug.Log("-> Number of planes: " + _planeManager.trackables.count);
            Debug.Log("-> Num Planes Added Occurred: " + _numPlanesAddedOccurred);
        }
    }

    private void PrintPlaneLabel(ARPlane plane)
    {
        string label = plane.classification.ToString();
        string log = $"Plane ID: {plane.trackableId}, Label: {label}";
        Debug.Log(log);
    }

    private void OnDestroy()
    {
        Debug.Log("-> SceneController::OnDestroy()");
        _switchSceneAction.action.performed -= OnSwitchSceneAction;
        _togglePlanesAction.action.performed -= OnTogglePlanesAction;
        _planeManager.planesChanged -= OnPlanesChanged;
        _activateAction.action.performed -= OnActivateAction;
    }
}
