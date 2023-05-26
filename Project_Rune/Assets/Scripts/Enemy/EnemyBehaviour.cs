using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour, IPushable, IEnrageable
{
    //===================================================================================================================================//

    #region General Enemy Stats

    [Header("General Enemy Stats")]
    [Tooltip("Attack Range.")]
    [SerializeField]
    public float attackRange;

    [Tooltip("Damage the enemy is dealing.")]
    [SerializeField]
    public int damage;

    [Tooltip("Range to notice the player.")]
    [SerializeField]
    public float noticeRange;

    #endregion General Enemy Stats

    #region Ranged Enemy

    //Ranged
    [Header("Ranged Enemy Stats")]

    [Tooltip("Is the enemy ranged?")]
    [SerializeField]
    public bool ranged;

    [Tooltip("Ranged Enemy Movement Speed")]
    [SerializeField]
    public float rangedEnemySpeed;

    [Tooltip("The prefab of the projectile.")]
    [SerializeField]
    private GameObject rangedProjectilePrefab;

    [Tooltip("Attacking cooldown in seconds")]
    [SerializeField]
    private float rangedAttackCooldown;

    [Tooltip("where should the projectile spawn? Y-Axis")]
    [SerializeField]
    private float projectileSpawnOffset = 2;

    [Tooltip("Is the view obstruced ray cast cooldown in seconds.")]
    [SerializeField]
    private float rayCastIntervalls = 5;

    [Tooltip("Is the view obstruced ray cast cooldown in seconds.")]
    [SerializeField]
    private float rayCastMaxDistance = 10;

    #endregion Ranged Enemy

    //===================================================================================================================================//

    #region Melee Enemy
    //Melee Enemy
    [Header("Melee Enemy Stats")]

    [Tooltip("Movement speed of the enemy.")]
    [SerializeField]
    public float meleeEnemySpeed;

    [Tooltip("How fast is the enraged enemy?")]
    [SerializeField]
    public float enragedEnemySpeed;

    [Tooltip("Melee Attack Cooldown in seconds.")]
    [SerializeField]
    public float meleeAttackCooldown;//Attacks per second

    [Tooltip("Attack Cooldown in enraged Status.")]
    [SerializeField]
    public float enragedmeleeAttackCooldown;

    #endregion Melee Enemy

    //===================================================================================================================================//

    [Tooltip("Player Layermask")]
    [SerializeField]
    public LayerMask player;

    [Tooltip("Enemy Layermask")]
    [SerializeField]
    public LayerMask enemy;

    [Tooltip("Illusion Layermask")]
    [SerializeField]
    public LayerMask illusion;

    //===================================================================================================================================//

    //Enrage
    [HideInInspector]
    public bool enraged = false;

    //===================================================================================================================================//

    //Privates
    private GameManager GM;
    private Rigidbody rb;
    private Transform playerGO;
    private NavMeshAgent agent;
    private GameObject holderProjectile;
    private bool targetInRange;
    private bool startAttackSequence;
    private bool readyToRayCast = true;
    private bool hitPlayer;

    //FireWindPushSpell
    private bool cooldown = false;

    //Enrage Stuff
    private float minDistance;
    private float newDistance;
    private GameObject shortestEnemy;
    private bool enemyCalculated;

    // Focus on ice illusion
    //[HideInInspector]
    public bool illusionFocused;
    [HideInInspector]
    public Transform illusion_object;

    public AnimationManager animationManager;

    //===================================================================================================================================//

    #region Getters & Setters

    //=========================================================================================//

    [SerializeField]
    [HideInInspector] private bool _beingPushed;
    [HideInInspector] public bool beingPushed { get => _beingPushed; set => _beingPushed = value; } //Get from IPushable

    [SerializeField]
    [HideInInspector] private Transform _sender;
    [HideInInspector] public Transform sender { get => _sender; set => _sender = value; } //Get from IPushable

    [SerializeField]
    [HideInInspector] private float _strength;
    [HideInInspector] public float strength { get => _strength; set => _strength = value; } //Get from IPushable

    [SerializeField]
    [HideInInspector] private float _intervals;
    [HideInInspector] public float intervals { get => _intervals; set => _intervals = value; } //Get from IPushable

    //=========================================================================================//

    [SerializeField]
    [HideInInspector] private bool _isEnraged;
    [HideInInspector] public bool isEnraged { get => _isEnraged; set => _isEnraged = value; } //Get from IEnrageable

    #endregion Getters & Setters

    //===================================================================================================================================//

    private void Start()
    {
        GM = FindObjectOfType<GameManager>();
        agent = GetComponentInChildren<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        playerGO = GM.playerGO;
        if (ranged)
        {
            agent.speed = rangedEnemySpeed;
        }
        else
        {
            agent.speed = meleeEnemySpeed;
        }
    }

    //===================================================================================================================================//

    private void Update()
    {
        //=========================================================================================//

        #region Fire Wind Push Spell
        if (_beingPushed && !cooldown)
        {
            cooldown = true;
            Vector3 direction = (transform.position - sender.transform.position).normalized;
            
            agent.velocity = direction * strength;           
            StartCoroutine(Reset());
        }
        #endregion Fire Wind Push Spell

        //=========================================================================================//

        #region Enraged Status 
        if (isEnraged)
        {

            if (!enemyCalculated)
            {              
                //Calculate neares Enemy
                CalculateNearestEnemy();
                enemyCalculated = true;
            }

            //Walks to nearest enemy
            if (Physics.CheckSphere(this.transform.position, noticeRange, enemy) && enemyCalculated == true)
            {
                if (shortestEnemy == null)
                {
                    enemyCalculated = false;
                }
                else
                {
                    agent.SetDestination(shortestEnemy.transform.position);
                }
            }
            
        }
        #endregion Enraged Status

        //=========================================================================================//

        #region Normal Status Player Finding
        //PlayerFinding
        else
        {
            if (!illusionFocused)
            {
                //Check for Player and walk to him
                if (Physics.CheckSphere(this.transform.position, noticeRange, player))
                {
                    animationManager.SetBool("Walking", true);
                    agent.SetDestination(playerGO.position);
                }
                else
                {
                    animationManager.SetBool("Walking", false);
                }
            }
            else
            {
                if (illusion_object != null) agent.SetDestination(illusion_object.position); // illusion should already spawn in notice range
                else illusionFocused = false;
            }
            
        }

        #endregion Normal Status Player Finding

        //=========================================================================================//

        #region Attacking 

        //Melee
        if (!ranged)
        {
            if (enraged) //Not fin
            {
                if (Physics.CheckSphere(this.transform.position, attackRange, enemy))
                {
                    targetInRange = true;
                    if (!startAttackSequence)
                    {
                        StartCoroutine(Attacker(enragedmeleeAttackCooldown, ranged));
                    }
                }
                else
                {
                    targetInRange = false;
                }
            }
            else if (!enraged && !illusionFocused)
            {
                if (Physics.CheckSphere(this.transform.position, attackRange, player))
                {
                    targetInRange = true;
                    if (!startAttackSequence)
                    {
                        StartCoroutine(Attacker(meleeAttackCooldown, ranged));
                    }
                }
                else
                {
                    targetInRange = false;
                }

            }
            else if (illusionFocused)
            {
                if (Physics.CheckSphere(this.transform.position, attackRange, illusion))
                {
                    targetInRange = true;
                    if (!startAttackSequence)
                    {
                        Debug.LogWarning("OY I FOUND TARGET");
                        StartCoroutine(Attacker(meleeAttackCooldown, ranged));
                    }
                }
                else
                {
                    targetInRange = false;
                }
            }
        }

        //Ranged
        else if(ranged == true)
        {
            if (readyToRayCast == true)
            {
                StartCoroutine(SteamRaycaster());
            }

            if (Physics.CheckSphere(this.transform.position, attackRange, player) && hitPlayer)
            {               
                agent.speed = 0;
                targetInRange = true;
                if (!startAttackSequence)
                {
                    StartCoroutine(Attacker(rangedAttackCooldown, ranged));
                }
            }
            else
            {
                agent.speed = rangedEnemySpeed;
                targetInRange = false;
            }
        }
        #endregion Attacking 

        //=========================================================================================//
    }

    #region Enrage Stuff

    //=========================================================================================//

    //Enrage Calculation
    private void CalculateNearestEnemy()
    {
        //Cycled through all Enemies, calculates the nearest one and assigns it to shortestenemy
        foreach (GameObject enemy in GM.allEnemies)
        {
            newDistance = Vector3.Distance(this.gameObject.transform.position, enemy.transform.position);

            if(minDistance == 0 && newDistance > 0)
            {
                minDistance = Vector3.Distance(this.gameObject.transform.position, enemy.transform.position);
                shortestEnemy = enemy;
            }

            if(newDistance > 0 && minDistance > newDistance)
            {
                shortestEnemy = enemy;
            }          
        }

        if (shortestEnemy != null)
        {
            enemyCalculated = true;
        }
    }

    //=========================================================================================//

    //Enrage Preparer
    public void enrageTimer(int duration)
    {
        StartCoroutine(enrageResetter(duration));
    }
    private IEnumerator enrageResetter(int duration)
    {
        enraged = true;
        agent.speed = enragedEnemySpeed;
        this.gameObject.layer = LayerMask.NameToLayer("EnragedEnemy");

        yield return new WaitForSeconds(duration);

        agent.speed = meleeEnemySpeed;
        this.gameObject.layer = LayerMask.NameToLayer("Enemy");
        isEnraged = false;
        enraged = false;
    }

    #endregion Enrage Stuff

    //=========================================================================================//

    //Attack Sequence with Cooldown
    private IEnumerator Attacker(float attackCooldown, bool isRanged)
    {
        if (!isRanged)
        {
            animationManager.SetTrigger("Attack");
            startAttackSequence = true;
            if (enraged && shortestEnemy != null)
            {
                shortestEnemy.GetComponent<EnemyHealth>().TakeDamage(damage);

            }
            else if (!enraged)
            {    
                if (!illusionFocused) // attack player
                {
                    playerGO.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
                }
                else // attack illusion
                {
                    illusion_object.gameObject.GetComponent<IceIllusion>().TakeDamage(damage);
                }
            }

            yield return new WaitForSeconds(attackCooldown);

            if (targetInRange == true)
            {
                StartCoroutine(Attacker(attackCooldown, isRanged));
            }
            else
            {
                startAttackSequence = false;
            }
        }
        else if (isRanged)
        {
            startAttackSequence = true;

            //Projectile Spawn
            SpawnRangedProjectile();

            yield return new WaitForSeconds(attackCooldown);

            if (targetInRange == true)
            {
                StartCoroutine(Attacker(attackCooldown, isRanged));
            }
            else
            {
                startAttackSequence = false;
            }
        }
    }

    //=========================================================================================//

    //Push Reseter
    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(intervals);
        cooldown = false;
    }

    //=========================================================================================//

    //Ranged Enemy Steam Raycast
    IEnumerator SteamRaycaster()
    {
        readyToRayCast = false;

        //target.position - sender.position;
        var direction = playerGO.position - this.transform.position;
        hitPlayer = Physics.Raycast(this.transform.position, direction, rayCastMaxDistance, player);
        Debug.DrawRay(this.transform.position, direction, Color.cyan, 4);     

        yield return new WaitForSeconds(rayCastIntervalls);
        readyToRayCast = true;
    }

    //=========================================================================================//
    public void SpawnRangedProjectile()
    {
        Quaternion test = new Quaternion(0, 0, 0, 0);
        Vector3 projectileSpawnPos = new Vector3(this.transform.position.x, this.transform.position.y + projectileSpawnOffset, this.transform.position.z);
        holderProjectile = Instantiate(rangedProjectilePrefab, projectileSpawnPos, Quaternion.identity);
        HomingProjectile projectile = holderProjectile.GetComponent<HomingProjectile>();
        projectile.playerGO = playerGO.gameObject;
        projectile.damage = damage;
        projectile.projectileSender = this.gameObject;
        projectile.target = playerGO.gameObject.GetComponent<Target>();
    }

    //=========================================================================================//

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, noticeRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }

    //=========================================================================================//

}
