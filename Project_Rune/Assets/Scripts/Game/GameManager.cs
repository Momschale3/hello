using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;

public class GameManager : MonoBehaviour
{
    //===================================================================================================================================//

    [Tooltip("List of all enemies.")]
    [SerializeField]
    public List<GameObject> allEnemies;

    [Tooltip("Player GameObject")]
    [SerializeField]
    public Transform playerGO;

    [Space]
    [Tooltip("List of all checkpoints in the level.")]
    [SerializeField]
    List<Checkpoint> allCheckpoints;
    [Tooltip("Active checkpoint.")]
    [SerializeField]
    Checkpoint activeCheckpoint;

    [SerializeField]
    private float speedAfterRespawn;

    public PathCreator creator;
    public RuneCalibration runeCalibration;
    

    //Privates
    PlayerVehicle vehicle;
    bool deathFadeActive;

    bool continuedGame; // check if data has to be collected

    //===================================================================================================================================//

    private void Start()
    {
        PlayerHealth.onPlayerDeath += WaitForDeathFading;
        ScreenFade.onFadingEnded += MovePlayerToActiveCheckpoint;
        vehicle = playerGO.GetComponentInChildren<PlayerVehicle>();
        if (continuedGame)
        {
            // get saved checkpoint as activeCheckpoint -> either Checkpoint or its index
            //MovePlayerToActiveCheckpoint(false);
        }

        //Calibrate First Rune | 0 = F | 1 = I | 2 = A | 3 = L
        runeCalibration.CalibrateRune(0); 

    }

    //===================================================================================================================================//

    public void AddThisEnemy(GameObject enemy)
    {
        allEnemies.Add(enemy);
    }

    //===================================================================================================================================//

    public void RemoveThisEnemy(GameObject enemy)
    {
        allEnemies.Remove(enemy);
    }

    //===================================================================================================================================//

    public void AddCheckpoint(Checkpoint checkpoint)
    {
        allCheckpoints.Add(checkpoint);
    }

    //===================================================================================================================================//

    public void ChangeActiveCheckpoint(Checkpoint checkpoint)
    {
        if (activeCheckpoint != null) activeCheckpoint.isActive = false;
        activeCheckpoint = checkpoint;
    }

    //===================================================================================================================================//

    void MovePlayerToActiveCheckpoint(bool fadeTypeOut)
    {
        if (!deathFadeActive)
        {
            //playerGO.position = activeCheckpoint.transform.position;
            //return;
        }

        if (fadeTypeOut)
        {
            playerGO.position = activeCheckpoint.transform.position;
            creator.TriggerPathUpdate();
            Debug.LogWarning(activeCheckpoint.transform.position + " TEST");
            PlayerHealth.RevivePlayer();   
        }
        else
        {
            vehicle.speed = playerGO.GetComponent<PlayerVehicle>().speed;
            deathFadeActive = false;
        }
        
    }

    //===================================================================================================================================//

    void WaitForDeathFading()
    {
        vehicle.speed = 0;
        deathFadeActive = true;
    }

    //===================================================================================================================================//

}
