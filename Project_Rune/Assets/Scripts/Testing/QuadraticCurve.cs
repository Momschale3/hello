using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticCurve : MonoBehaviour
{

    public Transform target;
    public Transform startPoint;
    public Transform Control;
    public int yHeight;

    public WindProjectile windProjectile;

    private void Start()
    {
        //startPoint = windProjectile.transform;
    }

    private void Update()
    {
        //Control.position = Vector3.Lerp(target.position, startPoint.position, 0.5f);
        //Control.position = new Vector3(Control.position.x, yHeight, Control.position.z) ;
    }

    public Vector3 evaluate(float t)
    {
        Vector3 ac = Vector3.Lerp(startPoint.position,  Control.position, t);
        Vector3 cb = Vector3.Lerp(Control.position, target.position, t);
        return Vector3.Lerp(ac, cb, t);
    }

    //private void OnDrawGizmos()
    //{
    //    if(target == null || startPoint == null || Control == null)
    //    {
    //        return;
    //    }
    //    for(int i = 0; 0 < 3; i++)
    //    {
    //        //Gizmos.DrawWireSphere(evaluate(i/3f),0.1f);
    //    }
    //}

}
