using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EyeTrackingRay : MonoBehaviour
{
    //===================================================================================================================================//

    [Header("General Ray Stats")]
    [SerializeField]
    private float rayDistance = 1.0f;
    [SerializeField]
    private float rayWidth = 0.01f;
    [SerializeField]
    private float maxDistance = 10f;
    [SerializeField]
    private LayerMask enemyLayerMask;


    [Header("Debug Colors")]
    [SerializeField]
    private bool debugMode;
    [SerializeField]
    private Color rayColorDefaultState = Color.yellow;
    [SerializeField]
    private Color rayColorHoverState = Color.red;


    [HideInInspector]
    public LockOnSystem lockOnSystem;  
    //[HideInInspector]
    public int neededTargets;
    //[HideInInspector]
    //public GameObject[] lockOnVFXs;
    public List<GameObject> lockOnVFXs;
    private int lockedOnTargets;
    [HideInInspector]
    public bool addTarget;
    private LineRenderer lineRenderer;
    public bool start;

    //Light Air
    [HideInInspector]
    public bool LightAir = false;
    public LayerMask enemyProjectile;

    //===================================================================================================================================//

    void Start()
    {
        addTarget = true;
        if (debugMode)
        {
            lineRenderer = GetComponent<LineRenderer>();
            SetupRay();
        }
    }

    //===================================================================================================================================//

    void SetupRay()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;
        lineRenderer.startColor = rayColorDefaultState;
        lineRenderer.endColor = rayColorDefaultState;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, transform.position.z + rayDistance));
    }

    //===================================================================================================================================//

    private void FixedUpdate()
    {
        if (neededTargets > lockedOnTargets && start && !LightAir) //Do you have enough targets?
        {
            RaycastHit hit;
            Vector3 rayCastDirection = transform.TransformDirection(Vector3.forward) * rayDistance; //Ray Direction

            if (Physics.Raycast(transform.position, rayCastDirection, out hit, maxDistance, enemyLayerMask)) //Cast Ray and if enemy was hit
            {
                if (addTarget) //Check if you already added the target
                {
                    addTarget = false; //We dont want to hit the same enemy every frame
                    lockedOnTargets++;
                    lockOnSystem.targets.Add(hit.transform); //Add Enemy we hit to List in LockOnSystems        

                    AddLockOnVFX(hit);

                    if (debugMode) //Debugging
                    {
                        lineRenderer.startColor = rayColorHoverState;
                        lineRenderer.endColor = rayColorHoverState;
                    }
                }
            }
            else
            {
                if (debugMode) //Debugging
                {
                    lineRenderer.startColor = rayColorDefaultState;
                    lineRenderer.endColor = rayColorDefaultState;
                }
                
                addTarget = true; //Allow Adding target again since we looked at something else for a second (Multi-Targeting on 1 enemy)
            }
        }
        else if(LightAir == true)
        {
            RaycastHit hit;
            Vector3 rayCastDirection = transform.TransformDirection(Vector3.forward) * rayDistance; //Ray Direction

            if (Physics.Raycast(transform.position, rayCastDirection, out hit, maxDistance, enemyProjectile)) //Cast Ray and if enemy projectile was hit
            {
                HomingProjectile projectile = hit.transform.gameObject.GetComponent<HomingProjectile>();
                projectile.target = projectile.projectileSender.GetComponent<Target>();


                Debug.LogWarning("RIGHT BACK TO THE SENDER");
            }
        }
        
    }

    //===================================================================================================================================//

    private void AddLockOnVFX(RaycastHit hit)
    {
        //Spawn VFX
        GameObject lockOn = Instantiate(lockOnVFXs[lockedOnTargets - 1], hit.transform);
        lockOn.GetComponent<LockOnRotator>().playerPos = lockOnSystem.eye_R.gameObject.transform;
        lockOnSystem.lockOnVFXs.Add(lockOn.transform);
    }

    //===================================================================================================================================//

    public void Resetter()
    {
        lockedOnTargets = 0;
        neededTargets = 0;
    }

    //===================================================================================================================================//

}
