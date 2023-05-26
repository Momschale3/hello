using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class EyeInteractible : MonoBehaviour
{

    public GameObject test;
    public bool IsHovered;

    [SerializeField]
    private UnityEvent<GameObject> OnObjectHover;

    [SerializeField]
    private Material OnHoverActiveMaterial;


    [SerializeField]
    private Material OnHoverInactiveMaterial;

    private MeshRenderer meshRenderer;


    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if(IsHovered)
        {
            meshRenderer.material = OnHoverActiveMaterial;
            test.SetActive(true);
            OnObjectHover?.Invoke(this.gameObject);
        }
        else
        {
            test.SetActive(false);
            meshRenderer.material = OnHoverInactiveMaterial;
        }
    }

}

