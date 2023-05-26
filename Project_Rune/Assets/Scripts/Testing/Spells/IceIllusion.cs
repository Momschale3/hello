using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceIllusion : MonoBehaviour
{
    //===================================================================================================================================//

    [SerializeField]
    public int maxHealth;
    [SerializeField]
    public int health;

    public IceBase iceBase;

    //===================================================================================================================================//

    private void Start()
    {
        health = maxHealth;
    }

    public void SetIceBase(IceBase Ibase)
    {
        iceBase = Ibase;
    }

    //===================================================================================================================================//

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            if(iceBase != null)
            {
                iceBase.allIllusions.Remove(gameObject);
            }
            Destroy(gameObject);
        }
    }

    //===================================================================================================================================//
}
