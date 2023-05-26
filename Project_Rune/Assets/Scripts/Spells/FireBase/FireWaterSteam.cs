using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWaterSteam : MonoBehaviour
{
    //===================================================================================================================================//
    [HideInInspector]
    public float duration;

    [Tooltip("The Steam VFX.")]
    [SerializeField]
    private ParticleSystem steamVFX;

    //===================================================================================================================================//

    private void Start()
    {

        StartCoroutine(destroyer());
        
    }

    //===================================================================================================================================//

    IEnumerator destroyer()
    {
        yield return new WaitForSeconds(duration);

        steamVFX.Stop();

        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

    //===================================================================================================================================//

}
