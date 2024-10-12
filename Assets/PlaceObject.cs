using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Casters;

public class PlaceObject : MonoBehaviour
{
    private XRInteractionManager interactionManager;

    [SerializeField] private InputActionReference placeAction;
    private bool placeActionInput;

    [SerializeField] private GameObject objectToSpawn;
    private bool alreadySpawned = false;

    private ARAnchorManager anchorManager;
    private List<ARAnchor> anchors = new();

    private CurveInteractionCaster curveInteractionCaster;
    private List<Collider> colliders = new();
    private List<RaycastHit> raycastHits = new();
    private ARPlane planeHit = null;
    private Vector3 positionHit;
    private bool shouldCast = true;

    private MeshRenderer objectToSpawnRenderer;
    private Material originalMaterial;
    [SerializeField] private Material placingMaterial;

    protected void Awake()
    {
        interactionManager = FindFirstObjectByType<XRInteractionManager>();

        anchorManager = GetComponentInParent<ARAnchorManager>();

        if (anchorManager == null)
        {
            Debug.LogError("-> Can't find 'ARAnchorManager'");
        }

        curveInteractionCaster = GetComponentInChildren<CurveInteractionCaster>();
        if (curveInteractionCaster == null)
        {
            Debug.LogError("-> Can't find 'CurveInteractionCaster'");
        }

        objectToSpawnRenderer = objectToSpawn.GetComponent<MeshRenderer>();

        //anchorManager.anchorsChanged += OnAnchorsChanged;
        placeAction.action.performed += i => placeActionInput = true;
    }

    /*
    private void OnAnchorsChanged(ARAnchorsChangedEventArgs obj)
    {
        // remove any anchors that have been removed outside our control, such as during a session reset
        foreach (var removedAnchor in obj.removed)
        {
            anchors.Remove(removedAnchor);
            Destroy(removedAnchor.gameObject);
        }
    }*/

    protected void Update()
    {
        if (placeActionInput && objectToSpawn)
        {
            placeActionInput = false;

            if (objectToSpawn.GetComponent<ARAnchor>() == null)
            {
                ARAnchor anchor = objectToSpawn.AddComponent<ARAnchor>();

                if (anchor != null)
                {
                    Debug.Log("-> CreateAnchoredObject() - anchor added!");
                    anchors.Add(anchor);
                }
                else
                {
                    Debug.LogError("-> CreateAnchoredObject() - anchor is null!");
                }
            }
            objectToSpawnRenderer.material = originalMaterial;
            objectToSpawn = null;
        }
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (objectToSpawn)
        {
            CheckIfRayHitsFloor();
        }
    }

    private void CheckIfRayHitsFloor()
    {
        if (curveInteractionCaster.TryGetColliderTargets(interactionManager, colliders, raycastHits) &&
            (planeHit = colliders[0].gameObject.GetComponent<ARPlane>()) != null &&
            planeHit.classification == PlaneClassification.Floor)
        {
            Debug.Log("-> Hit detected! - name: " + raycastHits[0].transform.name);

            Quaternion rotation = Quaternion.LookRotation(raycastHits[0].normal, Vector3.up);
            positionHit = raycastHits[0].point + new Vector3(0f, 0.95f, 0f);

            if (!alreadySpawned)
            {
                objectToSpawn = Instantiate(objectToSpawn, raycastHits[0].point, rotation);
                originalMaterial = objectToSpawnRenderer.material;
                objectToSpawnRenderer.material = placingMaterial;

                alreadySpawned = !alreadySpawned;
            }
            else
            {
                objectToSpawn.transform.position = positionHit;
            }

            colliders.Clear();
            raycastHits.Clear();
        }
        else
        {
            Debug.LogFormat("-> No valid hit detected! (floor)");
            //position = Vector3.zero;
        }
    }

    protected void OnDestroy()
    {
        //anchorManager.anchorsChanged -= OnAnchorsChanged;
    }
}
