using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Casters;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public enum ObjectToSpawn
{
    Analyzer,
    Console,
    Sign
}

public class PlaceObject : MonoBehaviour
{
    private XRInteractionManager interactionManager;

    [SerializeField] private InputActionReference rotateAction;
    private bool rotateActionInput;
    [SerializeField] private InputActionReference placeAction;
    private bool placeActionInput;

    //private bool alreadySpawned = false;

    private ARAnchorManager anchorManager;
    private List<ARAnchor> anchors = new();

    private CurveInteractionCaster curveInteractionCaster;
    private List<Collider> colliders = new();
    private List<RaycastHit> raycastHits = new();
    private ARPlane planeHit = null;
    private Vector3 positionHit;
    private bool shouldCast = true;

    private GameObject objectToSpawn;
    private GameObject analyzerPrefab;
    private GameObject consolePrefab;
    private GameObject signPrefab;
    private MeshRenderer objectToSpawnRenderer;
    private Material originalMaterial;
    [SerializeField] private Material placingMaterial;

    private bool successfullySpawned = false;
    private bool canPlace = false;

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

        placeAction.action.performed += i => placeActionInput = true;
        rotateAction.action.started += i => rotateActionInput = true;
        rotateAction.action.canceled += i => rotateActionInput = false;
    }

    protected void Start()
    {
        analyzerPrefab = Resources.Load("Analyzer") as GameObject;
        consolePrefab = Resources.Load("Console") as GameObject;
        signPrefab = Resources.Load("Sign") as GameObject;
    }

    public void OnButtonPressed(int objectSelected)
    {
        //objectToSpawnRenderer = objectToSpawn.GetComponent<MeshRenderer>();

        Quaternion spawnRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        GameObject obj;
        switch ((ObjectToSpawn)objectSelected)
        {
            case ObjectToSpawn.Analyzer:
                if ((obj = GameObject.FindWithTag("Analyzer")) != null)
                {
                    Destroy(obj);
                }
                objectToSpawn = Instantiate(analyzerPrefab, SimpleRay.hit.transform);
                successfullySpawned = true;
                break;
            case ObjectToSpawn.Console:
                if ((obj = GameObject.FindWithTag("Console")) != null)
                {
                    Destroy(obj);
                }
                objectToSpawn = Instantiate(consolePrefab, SimpleRay.hit.transform);
                successfullySpawned = true;
                break;
            case ObjectToSpawn.Sign:
                if ((obj = GameObject.FindWithTag("Sign")) != null)
                {
                    Destroy(obj);
                }
                objectToSpawn = Instantiate(signPrefab, SimpleRay.hit.transform);
                successfullySpawned = true;
                break;
            default:
                successfullySpawned = false;
                break;
        }

        objectToSpawn.transform.rotation = spawnRotation;
        StartCoroutine(WaitBeforePlacing());
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (objectToSpawn && successfullySpawned)
        {
            objectToSpawn.transform.position = SimpleRay.hit.point + new Vector3(0f, objectToSpawn.GetComponent<DetectSurface>().adjustedPos, 0f);
            Debug.Log(SimpleRay.hit.point);
            Physics.SyncTransforms();

            if (rotateActionInput)
            {
                objectToSpawn.transform.Rotate(Vector3.up, 50f * Time.deltaTime);
            }

            Place();
        }
    }

    private IEnumerator WaitBeforePlacing()
    {
        yield return new WaitForSeconds(2f);

        canPlace = true;
    }

    private void Place()
    {
        if (canPlace && placeActionInput)
        {
            canPlace = false;
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

            //objectToSpawnRenderer.material = originalMaterial;
            objectToSpawn.GetComponent<DetectSurface>().enabled = false;
            successfullySpawned = false;
            objectToSpawn = null;
        }
        else if (!canPlace && placeActionInput)
        {
            placeActionInput = false;
        }
    }

    /*
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
                objectToSpawn = Instantiate(objectToSpawn, positionHit, rotation);
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
    */
}
