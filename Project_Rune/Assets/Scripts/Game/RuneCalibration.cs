using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneCalibration : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GestureRecognizer gestureRecognizer;
    [SerializeField, Tooltip("The Pad the player sees when he draws.")]
    private MeshRenderer drawPad;
    [SerializeField, Tooltip("The materials which describe the drawing process of a rune. DISCLAIMER: 0 = F | 1 = I | 2 = A | 3 = L")]
    private Material[] runeInstruction;

    [SerializeField, Tooltip("0 = F | 1 = I | 2 = A | 3 = L ")]
    private List<string> runeNames; //0 = F | 1 = I | 2 = A | 3 = L

    [SerializeField, Tooltip("How many saves should be made? Can go up to 99.")]
    private int numberOfRuneSaves = 5;

    //Privates
    private int index = 0;
    private int runeIndexWorkingOn; //On what rune are we currently working on?

    //Calibrate First Rune | 0 = F | 1 = I | 2 = A | 3 = L
    //runeCalibration.CalibrateRune(0); 
    //Paste this command anywhere you want to Create new Runes
    public int indexer;

    public void CalibrateRune(int runeIndex)//RuneIndex for what rune F? I? A? L?
    {
        if (index < numberOfRuneSaves)
        {
            drawPad.gameObject.SetActive(true);
            runeIndexWorkingOn = runeIndex;

            gestureRecognizer.creationMode = true;
            gestureRecognizer.newGestureName = runeNames[runeIndex] + index;

            drawPad.material = runeInstruction[runeIndex];
            index++;
        }
        else
        {
            //Only for Calibrating all Runes
            indexer++;

            drawPad.gameObject.SetActive(false);
            Debug.LogWarning("You completed your drawing calibration!");

            if (indexer < 4)
            {
                index = 0;
                
                CalibrateRune(indexer);

            }
        }
    }

    public void DrawingCompleted()
    {
        CalibrateRune(runeIndexWorkingOn);
    }

}
