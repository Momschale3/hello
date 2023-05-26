using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    //===================================================================================================================================//
       
    [Header("References")]
    
    [Tooltip("Prefab of the impact.")]
    [SerializeField] 
    private GameObject impactPrefab;

    [Header("Movement")]
    [Tooltip("Projectile Speed")]
    [SerializeField] 
    private float speed = 15;

    [Tooltip("Speed of the rotation.")]
    [SerializeField] 
    private float rotateSpeed = 95;

    [Tooltip("Strength of the spawn.")]
    [SerializeField] 
    private float spawnStrength = 1.5f;

    //===================================================================================================================================//

    [Header("Prediction")]

    [Tooltip("Max Distance to predict")]
    [SerializeField] 
    private float maxDistancePredict;

    [Tooltip("Minimum Distance to Predict.")]
    [SerializeField] 
    private float minDistancePredict;

    [Tooltip("Max prediction time.")]
    [SerializeField] 
    private float maxTimePrediction;

    //===================================================================================================================================//

    [Header("Deviation")]

    [Tooltip("How much Deviation do u want?")]
    [SerializeField] 
    private float deviationAmount;

    [Tooltip("Speed of the Deviation.")]
    [SerializeField] 
    private float deviationSpeed;


    [Tooltip("How long should the projectile fly upward?")]
    [SerializeField]
    private float upDuration;

    [Tooltip("Cooldown which speeds up the projectile.")]
    [SerializeField] 
    private float cooldown;

    //===================================================================================================================================//

    //Privates
    [HideInInspector]
    public Target target;
    [HideInInspector]
    public GameObject projectileSender;
    [HideInInspector]
    public GameObject playerGO;
    [HideInInspector]
    public int damage;
    private Rigidbody rb;
    private Vector3 standardPrediction, deviatedPrediction;
    private float timer;
    private bool seek;
    private bool increaseSpeed = true;
    private float oldSpeed;

    //===================================================================================================================================//

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(upLiftDuration(upDuration));
        var rotation = Quaternion.LookRotation(transform.up);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, 200));

        oldSpeed = speed;
    }

    //===================================================================================================================================//

    private void FixedUpdate()
    {
        if (increaseSpeed)
        {         
            if (timer < Time.time)
            {              
                rb.velocity += transform.forward * spawnStrength;
                timer += cooldown;
            }
        }

        if (seek)
        {
            rb.velocity = transform.forward * speed;

            var leadTimePercentage = Mathf.InverseLerp(minDistancePredict, maxDistancePredict, Vector3.Distance(transform.position, target.transform.position));
            PredictMovement(leadTimePercentage);
            AddDeviation(leadTimePercentage);
            RotateRocket();
        }
    }

    //===================================================================================================================================//

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, maxTimePrediction, leadTimePercentage);
        standardPrediction = target.Rb.position + target.Rb.velocity * predictionTime;
    }

    //===================================================================================================================================//

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);
        var predictionOffset = transform.TransformDirection(deviation) * deviationAmount * leadTimePercentage;
        deviatedPrediction = standardPrediction + predictionOffset;
    }

    //===================================================================================================================================//

    private void RotateRocket()
    {
        var heading = deviatedPrediction - transform.position;
        var rotation = Quaternion.LookRotation(heading);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime));
    }

    //===================================================================================================================================//

    private void OnCollisionEnter(Collision collision)
    {
        if (impactPrefab)
        {
            Instantiate(impactPrefab, transform.position, Quaternion.identity);
        }

        if (collision.collider.CompareTag("Player"))
        {
            //Player DMG
            Debug.LogWarning("Hit Player");
            playerGO.GetComponent<PlayerHealth>().TakeDamage(damage);

            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("Enemy"))
        {
            Debug.LogWarning("Hit Enemy");
            projectileSender.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if(collision.collider.CompareTag("Shield"))
        {
            LightShield lightShield = collision.collider.GetComponent<LightShield>();
            if (lightShield.OnReflect())
            {
                ReflectedProjectile(collision, lightShield);
             
            }
            else
            {
                Debug.LogWarning("SHIT");
                Destroy(this.gameObject);
            }
        }
        else
        {
            Debug.Log("Hit Environment");
            Destroy(gameObject);
        }

    }

    //===================================================================================================================================//

    public void ReflectedProjectile(Collision collision, LightShield lightShield)
    {
        target = projectileSender.GetComponent<Target>(); //Get Target

        speed = 0;
        Vector3 newDirection = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
        this.transform.rotation = Quaternion.LookRotation(newDirection);
        this.rb.angularVelocity = Vector3.zero;
        speed = oldSpeed;      
    }

    //===================================================================================================================================//

    private IEnumerator upLiftDuration(float upDuration)
    {
        increaseSpeed = true;
        yield return new WaitForSeconds(upDuration);
        increaseSpeed = false;
        seek = true;
    }

    //===================================================================================================================================//
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, standardPrediction);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(standardPrediction, deviatedPrediction);

    }

    //===================================================================================================================================//

}

