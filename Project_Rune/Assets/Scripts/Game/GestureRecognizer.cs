using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;

public class GestureRecognizer : MonoBehaviour
{

    //===================================================================================================================================//

    [Header("Gesture Creation Options")]
    public bool creationMode = true;
    public string newGestureName;

    [Header("Stats")]
    public float inputThreshold = 0.1f;
    public float newPositionThresholdDistance = 0.05f;      
    public float recognitionThreshold = 0.9f;

    [Header("References")]
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private SpellManager SM;
    [SerializeField]
    private Transform debugPrefabStorage;
    [SerializeField]
    private Transform movementSorce;
    [SerializeField]
    private Transform drawingPad;
    [SerializeField]
    private GameObject debugCubePrefab;
    [SerializeField]
    private RuneCalibration runeCalibration;

    //Privates
    private List<Gesture> trainingSet = new List<Gesture>();
    private bool isMoving;
    private bool startDrawing;
    private List<Vector3> positionList = new List<Vector3>();
    [HideInInspector]
    public bool draw = true;
    private float debugIntervall;
  

    //===================================================================================================================================//

    private void Start()
    {
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach(var item in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }
    }

    //===================================================================================================================================//

    private void Update()
    {
        //Start The Movement 
        if(!isMoving && startDrawing)
        {
            StartMovement();
        }
        //Endeing the Movement
        else if(isMoving && !startDrawing)
        {
            EndMovement();
        }
        //Updating the Movement
        else if (isMoving && startDrawing)
        {
            UpdateMovement();
        }
    }

    //===================================================================================================================================//

    void StartMovement()
    {
        //Debug.Log("Start Movement");
        isMoving = true;

        //Clear Positions
        positionList.Clear();
        positionList.Add(movementSorce.localPosition);
    }

    //===================================================================================================================================//

    void EndMovement()
    {
        //Debug.Log("End Movement");
        isMoving = false;

        //Create The Gesture from the Position List
        Point[] pointArray = new Point[positionList.Count];

        for(int i = 0; i < positionList.Count; i++)
        {          
            Vector2 screenPoint = cam.WorldToScreenPoint(positionList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }
        Gesture newGesture = new Gesture(pointArray);

        //Add a new Gestuer to training set
        if(creationMode)
        {
            creationMode = false;

            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);

           

            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);

            //Rune Calibration Communication
            runeCalibration.DrawingCompleted();

            Debug.LogError("CREATED: " + newGestureName);
        }
        //Recognize
        else
        {
            
            for (var i = 0; i < newGesture.Points.Length; i++)
            {

                Debug.LogWarning(newGesture.Points[i].X + newGesture.Points[i].Y);

            }

            for (var j = 0; j < trainingSet.Count; j++)
            {
                for (var z = 0; z < trainingSet[j].Points.Length; z++)
                {
                    //Debug.LogWarning(trainingSet[j].Points[z].X + trainingSet[j].Points[z].Y + " INDEX: --> " + j + " / " + z);
                }

            }
            
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());

            if(result.Score > recognitionThreshold)
            {
                Debug.LogWarning(result.GestureClass + ": " + result.Score);

                //Spell Manager F wurde gezeichnet 
                SM.drewRune(result.GestureClass);
            }
        }
    }

    //===================================================================================================================================//

    void UpdateMovement()
    {      
        Vector3 lastPosition = positionList[positionList.Count - 1];

        if(Vector3.Distance(movementSorce.localPosition, lastPosition) > newPositionThresholdDistance)
        {
            //Add Positions
            positionList.Add(movementSorce.localPosition);
            

            if (debugCubePrefab && draw == true)
            {
                draw = false;
                StartCoroutine(drawer());                
                Destroy(Instantiate(debugCubePrefab, movementSorce.position, Quaternion.identity, debugPrefabStorage), 3);
            }
        }
    }

    //===================================================================================================================================//

    public void StartDrawing()
    {
        startDrawing = true;

        if (creationMode == false)
        {
            //Activate Drawing Pad
            drawingPad.gameObject.SetActive(true);
        }
    }

    //===================================================================================================================================//

    public void EndDrawing()
    {
        startDrawing = false;
        //Deactivate Drawing Pad
        drawingPad.gameObject.SetActive(false);
    }

    //===================================================================================================================================//

    IEnumerator drawer()
    {
        yield return new WaitForSeconds(debugIntervall);
        draw = true;
    }

    //===================================================================================================================================//

    public void CalibrateRunes(Point[] points, Gesture newGesture)
    {
        //aktiviere Gameobject was die Rune anzeigt 

        //Zeig dem SPieler wie er dir rune zeichnet 

        //Speicher die neue gezeichneten Runen 

        newGesture.Name = newGestureName;
        trainingSet.Add(newGesture);

        string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
        GestureIO.WriteGesture(points, newGestureName, fileName);

        creationMode = false;
        Debug.LogError("CREATED: " + newGestureName);
    }
    
}
