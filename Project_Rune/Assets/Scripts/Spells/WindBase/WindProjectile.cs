using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindProjectile : MonoBehaviour
{
    //===================================================================================================================================//

    //[HideInInspector]
    public LockOnSystem lockOnSystem;
    //[HideInInspector]
    public Transform target;

    //[SerializeField]
    //private float strength;

    private Rigidbody rigidbody;

    public QuadraticCurve curve;
    public float speed;

    private float sampleTime;

    public Transform startingPosition;

    public GameObject projectileMain;

    public bool start;

    public WindProjectileOnHitEffect windProjectileOnHitEffect;

    //===================================================================================================================================//

    void Start()
    {

        target = lockOnSystem.GiveTarget();

        sampleTime = 0f;

        curve.target = target;
        this.transform.position = startingPosition.position;
        curve.enabled = true;
        start = true;

    }

    //===================================================================================================================================//

    private void Update()
    {
        if (start)
        {
            sampleTime += Time.deltaTime * speed;
            transform.position = curve.evaluate(sampleTime);
            transform.forward = curve.evaluate(sampleTime + 0.001f) - transform.position;

            if (sampleTime >= 1f)
            {
                windProjectileOnHitEffect.OnHit();
                Debug.LogWarning("HIT");
                //Destroy(projectileMain);
            }
        }
    }
}
