using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(ARPlaneManager))]

public class SceneController : MonoBehaviour
{
    private XRInteractionManager interactionManager;
    private List<Collider> colliders;
    private List<RaycastHit> raycastHits;

    //[SerializeField] private InputActionReference togglePlanesAction;
    [SerializeField] private InputActionReference activateAction;

    [SerializeField] private InputActionReference switchSceneAction;

    [SerializeField] private GameObject grabbableCube;
    [SerializeField] private NearFarInteractor nearFarInteractor;

    private ARPlaneManager planeManager;
    //private bool isVisible = false;
    //private int numPlanesAddedOccurred = 0;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("-> SceneController::Start()");
        interactionManager = FindFirstObjectByType<XRInteractionManager>();

        planeManager = GetComponent<ARPlaneManager>();

        if (planeManager == null)
        {
            Debug.LogError("-> Can't find 'ARPlaneManager'");
        }

        switchSceneAction.action.performed += OnSwitchSceneAction;
        //togglePlanesAction.action.performed += OnTogglePlanesAction;
        //planeManager.planesChanged += OnPlanesChanged;
        activateAction.action.performed += OnActivateAction;
    }

    private void OnActivateAction(InputAction.CallbackContext obj)
    {
        SpawnGrabbableCube();
    }

    public void OnSwitchSceneAction(InputAction.CallbackContext obj)
    {
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex == 0 ? 1 : 0);
        SceneLoader.Instance.LoadNewScene(SceneManager.GetActiveScene().buildIndex == 1 ? "PlanetScene" : "ShipScene");
    }

    public void SwitchSceneAction()
    {
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex == 0 ? 1 : 0);
        SceneLoader.Instance.LoadNewScene(SceneManager.GetActiveScene().buildIndex == 1 ? "PlanetScene" : "ShipScene");
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

    // Update is called once per frame
    private void Update()
    {
        if (nearFarInteractor.farInteractionCaster.TryGetColliderTargets(interactionManager, colliders, raycastHits))
        {
            foreach (var raycastHit in raycastHits)
            {
                Debug.Log("-> Hit detected! - name: " + raycastHit.transform.name);
            }
        }
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

    private void OnDestroy()
    {
        Debug.Log("-> SceneController::OnDestroy()");
        switchSceneAction.action.performed -= OnSwitchSceneAction;
        //togglePlanesAction.action.performed -= OnTogglePlanesAction;
        //planeManager.planesChanged -= OnPlanesChanged;
        activateAction.action.performed -= OnActivateAction;
    }
}
