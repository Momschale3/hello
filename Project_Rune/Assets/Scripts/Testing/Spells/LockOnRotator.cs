using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnRotator : MonoBehaviour
{

    public Transform playerPos;

    private void Update()
    {
        Vector3 targetVector = this.transform.position - playerPos.position;
        transform.rotation = Quaternion.LookRotation(targetVector, playerPos.transform.rotation * Vector3.up);
    }

    public void RotateVFX()
    {
        Vector3 targetVector = playerPos.position - transform.position;

        float newYAngle = Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, -1 * newYAngle, 0);
    }

    public void ClearVFX()
    {
        Destroy(this.gameObject);
        Debug.LogWarning("This is the GO: " + this.gameObject);
    }

}
