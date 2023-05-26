using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;
public class PlayerVehicle : MonoBehaviour
{
    //===================================================================================================================================//

    //Accesibles
    [Tooltip("Speed which is not changed, just the normal speed value if no enemies are around.")]
    [SerializeField]
    public float speed = 5;

    [Tooltip("The radius which tests if enemies are near.")]
    [SerializeField]
    private float slowRadius;

    [Tooltip("The interval in which the List of enemies near the player gets updated.")]
    [SerializeField]
    private float slowIntervall;

    [Tooltip("How many enemies are needed to stop 100%. Formula: speed = (1 - (enemies in Range / maxEnemies) * startSpeed")]
    [SerializeField]
    private float maxEnemiesToStop;

    [SerializeField]
    private LayerMask enemy;

    [SerializeField]
    private PathCreator pathCreator;

    [SerializeField]
    private GameObject playerGO;

    [SerializeField]
    private GameObject playerOnVehicle;


    [SerializeField]
    private GameObject vehicle;

    public PathFollower pathFollower;

    //===================================================================================================================================//

    //Privates
    //[HideInInspector]
    public float startSpeed;   
    private float distanceTravelled;
    private bool drive = false;
    private Collider[] enemiesInRange;

    //===================================================================================================================================//

    private void Start()
    {
        
        StartPath();
        StartCoroutine(SlowChecker());
        StartCoroutine(startWaiter());

        pathFollower.enabled = true;

    }

    private void Update()
    {
        if (drive)
        {
            pathFollower.speed = speed;

            //distanceTravelled += speed * Time.deltaTime;
            //transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
            //vehicle.transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled); 
        }
    }

    //===================================================================================================================================//

    public void StartPath()
    {
        drive = true;
    }

    //===================================================================================================================================//

    private void measureDistance()
    {
        enemiesInRange = Physics.OverlapSphere(this.transform.position, slowRadius, enemy);
        if (enemiesInRange.Length > 0 && enemiesInRange.Length <= maxEnemiesToStop)
        {
            //Speed = 1- to reduce speed per enemy and not increase
            speed = (1 - ((float)enemiesInRange.Length / (float)maxEnemiesToStop)) * startSpeed;           
        }
    }

    //===================================================================================================================================//

    IEnumerator SlowChecker()
    {
        measureDistance();
        yield return new WaitForSeconds(slowIntervall);
        StartCoroutine(SlowChecker());
    }

    //===================================================================================================================================//

    IEnumerator startWaiter()
    {
        yield return new WaitForSeconds(1);
        playerGO.transform.parent = playerOnVehicle.transform;
        playerGO.transform.localPosition = new Vector3(0, 3.5f, 0);
    }

    //===================================================================================================================================//
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(this.transform.position, slowRadius);
    }

    //===================================================================================================================================//

}
