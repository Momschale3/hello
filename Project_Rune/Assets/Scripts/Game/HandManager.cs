using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Struct = Class wihtout function
[System.Serializable]
public struct HandPose
{
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
}

public class HandManager : MonoBehaviour
{
    public float threshold = 0.1f;
    public OVRSkeleton skeleton;
    public List<HandPose> handPoses;
    public List<OVRBone> fingerBones;
    public bool debugMode;

    private bool startRecognizing;

    private HandPose previousHandPose;

    //in die Events einfach die Skripte reinziehen und richitge funktion auswählen
    // Posen bis jetzt:
    //1. GestureRecognizer.StartDrawing();
    //2. GestureRecognizer.EndDrawing();
    //3. SpellManager.CastSpell();


    private void Start()
    {
        StartCoroutine(myWaiter());
        
    }

    private void Update()
    {
        if(debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            Save();
        }

        if (startRecognizing)
        {

            HandPose currentPose = Recognize();
            bool hasRecognized = !currentPose.Equals(new HandPose());

            //Check if new Gesture
            if (hasRecognized && !currentPose.Equals(previousHandPose))
            {
                //New Gesure!
                Debug.Log("New Pose found : " + currentPose.name);
                previousHandPose = currentPose;
                currentPose.onRecognized?.Invoke();
            }
        }
    }

    void Save()
    {
        HandPose p = new HandPose();
        p.name = "New Pose";
        List<Vector3> data = new List<Vector3>();
        foreach(var bone in fingerBones)
        {
            Debug.LogError("HEY");
            //Finger Position related to root 
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }

        p.fingerDatas = data;
        handPoses.Add(p);
    }

    HandPose Recognize()
    {
        HandPose currentPose = new HandPose();
        float currentMin = Mathf.Infinity;

        foreach(var pose in handPoses)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            for(int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, pose.fingerDatas[i]);
                if(distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }
                sumDistance += distance;
            }

            if(!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentPose = pose;
            }

        }

        return currentPose;

    }

    IEnumerator myWaiter()
    {
        yield return new WaitForSeconds(2);
        fingerBones = new List<OVRBone>(skeleton.Bones);
        Debug.LogError(skeleton.Bones.Count);
        previousHandPose = new HandPose();
        startRecognizing = true;
    }

}
