using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    //===================================================================================================================================//

    [SerializeField]
    public EyeTrackingRay eye_R; 

    [SerializeField]
    public List<Transform> targets = new List<Transform>();

    [SerializeField]
    private GameObject[] lockOnVFXPrefabs;

    //===================================================================================================================================//

    [HideInInspector]
    public WindBase windBase;

    //[HideInInspector]
    public int neededTargets;

    public int targetIndex;

    //[HideInInspector]
    public bool shootHandPose;

    //[HideInInspector]
    public List<Transform> lockOnVFXs = new List<Transform>();

    //===================================================================================================================================//

    private void Start()
    {
        StartEyeTracking();
    }

    private void Update()
    {
        if (shootHandPose == true && targets.Count > 0) //Check if targets are in List and if Shoot Pose was made
        {
            shootHandPose = false;
            windBase.spawnNow = true; 
        }
        else if(shootHandPose) //If no List entries just do nothing and return
        {
            shootHandPose = false; 
        }
    }

    //===================================================================================================================================//

    public void StartEyeTracking() //If we shoot rays from both we get 2 Transforms --> Dominant Eye change in Settings
    {             
        //eye_R.gameObject.SetActive(true);
        eye_R.addTarget = true;
        eye_R.start = true;
        //eye_R.lockOnVFX = lockOnVFXPrefab;
        
        foreach(GameObject vfx in lockOnVFXPrefabs)
        {
            eye_R.lockOnVFXs.Add(vfx);
        }

        eye_R.neededTargets = neededTargets; //Tell Eye How many Entries in List we need  
    }

    //===================================================================================================================================//

    public Transform GiveTarget() //Called by the Instantiated Wind Projectile --> needs targets
    {
        Transform thisTarget = targets[targetIndex]; 
       
        //lockOnVFXs[targetIndex].GetComponent<LockOnRotator>().ClearVFX();
        //lockOnVFXs.Remove(thisTarget);

        targetIndex++;

        if(targetIndex >= targets.Count)
        {
            Resetter();
        }

        return thisTarget;
    }

    //===================================================================================================================================//

    public void ShootProjectile() //Called by Hand Pose Manager Script
    {
        shootHandPose = true;
    }

    //===================================================================================================================================//

    public void Resetter() //Resets back to standard values
    {
        neededTargets = 0;
        targetIndex = 0;      
        targets.Clear();
        lockOnVFXs.Clear();
        eye_R.Resetter();
        //eye_R.gameObject.SetActive(false);
        this.enabled = false;
    }

    //===================================================================================================================================//
}
