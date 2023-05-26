using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindProjectileOnHitEffect : MonoBehaviour
{
    //===================================================================================================================================//
    //[HideInInspector]
    public int damage;

    //Wind
    //[HideInInspector]
    public bool wind;
    [Header("Wind")]
    [SerializeField]
    private GameObject windImpact;

    //Fire
    //[HideInInspector]
    public bool burn = false;
    [HideInInspector]
    public int damagePerTick;
    [HideInInspector]
    public float burnDuration;
    [Header("Wind Fire")]
    [SerializeField]
    private GameObject windFireImpact;

    //Ice
    //[HideInInspector]
    public bool slow;
    [HideInInspector]
    public float slowAmount;
    [HideInInspector]
    public float slowDuration;
    [Header("Wind Ice")]

    [SerializeField]
    private GameObject windIceImpact;

    public float explosionRadius;
    
    
    
    //Light
    //[HideInInspector]
    public bool healNow;
    [HideInInspector]
    public int healStrength;
    [HideInInspector]
    public int  healDuration;
    [Header("WindLight")]
    [SerializeField]
    private GameObject windLightImpact;


    public Collider[] enemiesHit;
    [SerializeField]
    private float hitRadius;
    [SerializeField]
    private LayerMask enemyLayerMask;

    [Space(20)]

    [SerializeField]
    private GameObject vfx;
    [SerializeField]
    private GameObject curve;
    [SerializeField]
    private WindProjectile windProjectile;
    [SerializeField]
    private GameObject parentGO;

    //Privates
    public EnemyEffectManager enemyEffectManager;
    private LockOnRotator lockOnRotator;
    public EnemyBehaviour enemyBehaviour;
    private float speedSaver;
    private int damageSaver;
    public Collider[] slowSplash;

    //public GameObject A_Vfx;
    //public GameObject AF_Vfx;
    //public GameObject AI_Vfx;
    //public GameObject AA_Vfx;
    //public GameObject AL_Vfx;

    //===================================================================================================================================//
    public void OnHit()
    {
        enemiesHit = Physics.OverlapSphere(this.transform.position, hitRadius, enemyLayerMask);       

        foreach (Collider enemyCollider in enemiesHit)
        {
            enemyEffectManager = enemyCollider.GetComponent<EnemyEffectManager>();
            lockOnRotator = enemyCollider.GetComponentInChildren<LockOnRotator>();

            //Wind
            if(wind == true)
            {
                enemyEffectManager.TakeDamage(damage);
                Destroy(Instantiate(windImpact, enemyEffectManager.gameObject.transform), 5);
            }

            //Fire
            if (burn == true)
            {
                enemyEffectManager.StartBurn(damagePerTick, burnDuration);
                Destroy(Instantiate(windFireImpact, enemyEffectManager.gameObject.transform), 5);
            }     

            //Ice
            if (slow == true)
            {
                slowSplash = Physics.OverlapSphere(this.transform.position, explosionRadius, enemyLayerMask);

                foreach (Collider enemyColl in slowSplash)
                {
                    enemyEffectManager = enemyColl.GetComponent<EnemyEffectManager>();
                    enemyEffectManager.changeSpeed(slowAmount, false, slowDuration);
                    Debug.LogWarning(enemyColl + " HOW MANY IN THERE");
                    Debug.LogWarning(slowSplash.Length + " -- Length");

                    Destroy(Instantiate(windIceImpact, enemyEffectManager.gameObject.transform), 5);

                }
            }

            //Light
            if (healNow == true)
            {
                foreach (Collider enemyColl in enemiesHit)
                {
                    enemyEffectManager = enemyColl.GetComponent<EnemyEffectManager>();

                    damageSaver = enemyEffectManager.enemyBehaviour.damage;
                    //enemyEffectManager.enemyBehaviour.damage = healStrength * (-1);

                    enemyEffectManager.damage = healStrength * (-1);
                    enemyEffectManager.EnemyHealingCoroutine(healDuration, damageSaver);

                    Destroy(Instantiate(windLightImpact, enemyEffectManager.gameObject.transform), 5);
                }              

            }
            enemyEffectManager.TakeDamage(damage);
            lockOnRotator.ClearVFX();
        }

        Destroy(parentGO);
    }

    //===================================================================================================================================//

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, hitRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, explosionRadius);
    }

    //===================================================================================================================================//

}
