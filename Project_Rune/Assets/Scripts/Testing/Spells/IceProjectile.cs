using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    //===================================================================================================================================//

    [SerializeField]
    public int damage;
    [SerializeField]
    public int speed;

    //===================================================================================================================================//

    void Update()
    {       
        // VFX
    }

    //===================================================================================================================================//

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            // VFX
        }
    }

    //===================================================================================================================================//
}
