using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShield : MonoBehaviour
{
    //===================================================================================================================================//

    [SerializeField]
    public int shieldDurability;

    private int tankedShots;
    [HideInInspector]
    public LightBase lightBase;

    //===================================================================================================================================//

    public bool OnReflect()
    {
        if(tankedShots <= shieldDurability)
        {
            //Reflect
            tankedShots++;
            return true;
        }
        else
        {
            lightBase.Resetter();            
            Destroy(this.gameObject, 0.5f);
            return false;
        }
    }

    //===================================================================================================================================//

}
