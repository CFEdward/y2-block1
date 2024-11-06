using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Test : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        meshRenderer.enabled = false;
        spriteRenderer.enabled = false;
    }
}
