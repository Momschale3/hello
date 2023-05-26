using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    //===================================================================================================================================//

    float freezeTime = 5;

    //===================================================================================================================================//

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // open UI / win effects
            StartCoroutine(TimeFreeze());
        }
    }

    //===================================================================================================================================//

    IEnumerator TimeFreeze()
    {
        float timer = 0;
        while (timer <= freezeTime)
        {
            Time.timeScale = Mathf.Lerp(1, 0, timer / freezeTime);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 0;
    }

    //===================================================================================================================================//

}
