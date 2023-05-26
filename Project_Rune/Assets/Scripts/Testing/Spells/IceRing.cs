using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRing : MonoBehaviour
{
    //===================================================================================================================================//

    [Tooltip("Strength/Speed of rotation")]
    [SerializeField]
    float rotationDegree;

    public IceProjectile[] iceProjectiles;

    public int iceProjectileDamage;
    public int iceProjectileSpeed;

    private void Start()
    {
        foreach(IceProjectile ip in iceProjectiles)
        {
            ip.damage = iceProjectileDamage;
            ip.speed = iceProjectileSpeed;
        }
    }

    //===================================================================================================================================//

    void Update()
    {
        transform.Rotate(0, rotationDegree, 0);
        // VFX
    }

    //===================================================================================================================================//
}
