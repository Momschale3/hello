using Oculus.Interaction.Surfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IceBase : MonoBehaviour
{
    //===================================================================================================================================//

    [SerializeField]
    private NavMeshSurface navMesh;
    [SerializeField]
    public IceAreaCheck areaCheck;

    public delegate void IceSpellActivated(float radiusSize);
    public static event IceSpellActivated iceSpellActivated;

    //===================================================================================================================================//

    #region Ice Base

    float duration;
    int effectStrength; // I || II
    int radius;

    #endregion Ice Base

    //===================================================================================================================================//

    [Space]

    #region Ice
    [Header("Ice Rune")]

    [Tooltip("Duration of the spell.")]
    [SerializeField]
    float IDuration;

    [Tooltip("Range of the spell. Yellow Gizmos")]
    [SerializeField]
    int IRadius;

    [Tooltip("Strength factor of the effect.")]
    [SerializeField]
    int IEffectStrength;

    [Tooltip("VFX of Spell.")]
    [SerializeField]
    ParticleSystem IParticle;

    #endregion

    //===================================================================================================================================//

    [Space(10)]

    #region IceIce
    [Header("Ice Ice Combination")]

    [Tooltip("Duration of the spell.")]
    [SerializeField]
    float IIDuration;
    [Tooltip("Range of the spell. Green Gizmos")]
    [SerializeField]
    int IIRadius;

    [Tooltip("Strength factor of the effect.")]
    [SerializeField]
    int IIEffectStrength;
    [Tooltip("VFX of Spell.")]
    [SerializeField]
    ParticleSystem IIParticle;

    #endregion

    //===================================================================================================================================//

    [Space]

    #region IceFire
    [Header("Ice Fire Combination")]

    [Tooltip("Duration of the spell.")]
    [SerializeField]
    float IFDuration;
    [Tooltip("Damage to enemies of the effect.")]
    [SerializeField] 
    int IFEffectStrength;
    [Tooltip("Range of the spell. Red Gizmos")]
    [SerializeField] 
    int IFRadius;
    [Tooltip("VFX of Spell.")]
    [SerializeField] 
    ParticleSystem IFParticle;

    #endregion

    //===================================================================================================================================//

    [Space]

    #region IceAir
    [Header("Ice Air Combination")]

    [Tooltip("Duration of the spell.")]
    [SerializeField]
    float IADuration;
    [Tooltip("Range of the spell. Blue Gizmos")]
    [SerializeField]
    int IARadius;
    [Tooltip("VFX of Spell.")]
    [SerializeField]
    ParticleSystem IAParticle;
    [Tooltip("Ice projectiles in ring form as prefab.")]
    [SerializeField]
    GameObject IceRing_prefab;
    GameObject activeIceRing;

    [Tooltip("Ice Projectile Damage")]
    [SerializeField]
    private int iceProjectileDamage;
    [Tooltip("Ice Projectile Speed.")]
    [SerializeField]
    private int iceProjectileSpeed;

    #endregion

    //===================================================================================================================================//

    [Space]

    #region IceLight
    [Header("Ice Light Combination")]

    [Tooltip("Duration of the spell.")]
    [SerializeField]
    float ILDuration;
    [Tooltip("Range of the spell. White Gizmos")]
    [SerializeField]
    int ILRadius;
    [Tooltip("VFX of Spell.")]
    [SerializeField]
    ParticleSystem ILParticle;
    [Tooltip("Illusion prefab.")]
    [SerializeField]
    GameObject Illusion_prefab;
    [Tooltip("HP of the Illusions.")]
    [SerializeField]
    private int iceIllusionHP;

    GameObject currentIllusion;
    List<Vector3> spawnPoints = new List<Vector3>();
    [Tooltip("All active illusion objects.")]
    public List<GameObject> allIllusions = new List<GameObject>();

    #endregion

    //Privates
    //[HideInInspector]
    public GameObject playerGO;
   
    private bool isI;
    private bool isIF;
    private bool isIA;
    private bool isII;
    private bool isIL;

    //===================================================================================================================================//

    private void Awake()
    {
        IceAreaCheck.enemyEnter += StartEffectOnEnemy;
        IceAreaCheck.enemyExit += StopEffectOnEnemy;
    }

    private void Start()
    {
        navMesh = FindObjectOfType<NavMeshSurface>();
    }

    //===================================================================================================================================//

    void StartEffectOnEnemy(Enemy enemy)
    {
        if (isI || isII)
        {
            SlowDownEnemies(enemy, effectStrength);
            isI = false;
            isII = false;
        }
        else if (isIF)
        {
            enemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth);
            enemyHealth.TakeDamage(effectStrength);
            isIF = false;
            
            //VFX

        }
        else if (isIA)
        {
            // nothing, deleted when finished
        }
        else if (isIL) // change priority of enemy attacks
        {      
            enemy.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour enemyBehaviour);
            enemyBehaviour.illusion_object = currentIllusion.transform;
            enemyBehaviour.illusionFocused = true;
           
        }
    }

    //===================================================================================================================================//

    void StopEffectOnEnemy(Enemy enemy)
    {
        // stop VFX on enemy

        if(isI || isII) 
        {
            StopSlowDownEffect(enemy, effectStrength);
        }
    }

    //===================================================================================================================================//

    IEnumerator SpellDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        
        if (isIL)
        {
            foreach (GameObject illusion in allIllusions)
            {
                if (illusion != null) Destroy(illusion.gameObject);
            }
            allIllusions.Clear();
        }
        else if (isIA)
        {
            isIA = false;
            Destroy(activeIceRing);
        }
        else if (isI || isII) // IF not needed since one time effect
        {
            foreach (Enemy enemy in areaCheck.enemiesInRadius)
            {
                StopEffectOnEnemy(enemy);
            }
        }
        

        isI = false;
        isIF = false;
        isIA = false;
        isII = false;
        isIL = false;
        areaCheck.enabled = false;
        //StopVFX();  
        Destroy(this.gameObject);
    }

    //===================================================================================================================================//

    void StopVFX()
    {
        IParticle.Stop();
        IIParticle.Stop();
        IFParticle.Stop();
        IAParticle.Stop();
        ILParticle.Stop();
    }

    //===================================================================================================================================//

    public void Ice() // ice fog that slows enemies
    {
        isI = true;
        
        duration = IDuration;
        effectStrength = IEffectStrength;
        areaCheck.enabled = true;
        //iceSpellActivated?.Invoke(IRadius);
        areaCheck.AreaRadius((float)IRadius);
        //IParticle.gameObject.SetActive(true);

        foreach (Enemy enemy in areaCheck.enemiesInRadius)
        {
            StartEffectOnEnemy(enemy);
        }

        StartCoroutine(SpellDuration(duration));

    }

    //===================================================================================================================================//

    public void IceIce() // ice fog that slows/freezes enemies
    {
        isII = true;
        duration = IIDuration;
        effectStrength = IIEffectStrength;
        areaCheck.enabled = true;
        //iceSpellActivated?.Invoke(IIRadius);
        areaCheck.AreaRadius((float)IRadius);
        //IIParticle.gameObject.SetActive(true);

        foreach (Enemy enemy in areaCheck.enemiesInRadius)
        {
            StartEffectOnEnemy(enemy);
        }

        StartCoroutine(SpellDuration(duration));
    }

    //===================================================================================================================================//

    public void IceFire() // ice ring that hurts enemies
    {
        isIF = true;
        duration = IFDuration;
        effectStrength = IFEffectStrength;
        areaCheck.enabled = true;
        //iceSpellActivated?.Invoke(IFRadius);
        areaCheck.AreaRadius((float)IFRadius);
        //IFParticle.gameObject.SetActive(true);
        foreach (Enemy enemy in areaCheck.enemiesInRadius)
        {
            StartEffectOnEnemy(enemy);
        }
        StartCoroutine(SpellDuration(duration));
    }

    //===================================================================================================================================//

    public void IceAir() // ice projectiles around player
    {
        isIA = true;
        duration = IADuration;
        areaCheck.enabled = true;
        //iceSpellActivated?.Invoke(IARadius);
        areaCheck.AreaRadius((float)IARadius);

        activeIceRing = Instantiate(IceRing_prefab, playerGO.transform);

        IceRing iceRing = activeIceRing.GetComponent<IceRing>();
        iceRing.iceProjectileDamage = iceProjectileDamage;
        iceRing.iceProjectileSpeed = iceProjectileSpeed;

        //IAParticle.gameObject.SetActive(true);
        StartCoroutine(SpellDuration(duration));
    }

    //===================================================================================================================================//

    public void IceLight()
    {
        isIL = true;
        duration = ILDuration;
        areaCheck.enabled = true;
        //iceSpellActivated?.Invoke(ILRadius);
        areaCheck.AreaRadius((float)ILRadius);
        //ILParticle.gameObject.SetActive(true); // illusions could get their own system instead
        StartCoroutine(IllusionSpawnWaiter());
        
        StartCoroutine(SpellDuration(duration));
    }

    //===================================================================================================================================//

    void SpawnIceIllusions()
    {
      
        //IceAreaCheck iceAreaCheck = areaCheck.GetComponent<IceAreaCheck>();
        int numberOfIllusions = areaCheck.enemiesInRadius.Count;
        numberOfIllusions = (int)Mathf.Ceil(numberOfIllusions - (numberOfIllusions/3)); // check number

        // Spawn illusions
        for (int illusions = 0; illusions < numberOfIllusions; illusions++)
        {
            Transform currentEnemy = areaCheck.enemiesInRadius[illusions].transform;
            Vector3 point = (Random.insideUnitSphere * 3) + currentEnemy.position;
            point.y = 0; // needs to be changed

            float maxDistance = 0;
            navMesh.ClosestSurfacePoint(point, out SurfaceHit hit, maxDistance);
            
            if (maxDistance == 0)
            {
                foreach(Vector3 spawn in spawnPoints)
                {
                    if (point == spawn)
                    {
                        point = (Random.insideUnitSphere * 3) + currentEnemy.position;
                        point.y = 0; // needs to be changed
                        break;
                    }
                }
                float lookDirection = point.x - currentEnemy.position.x; // check where object looks
                currentIllusion = Instantiate(Illusion_prefab, point, Quaternion.Euler(new Vector3(0f, lookDirection, 0f)));
                IceIllusion iceIllu = currentIllusion.GetComponent<IceIllusion>();
                iceIllu.SetIceBase(this);
                iceIllu.maxHealth = iceIllusionHP;             

                StartEffectOnEnemy(areaCheck.enemiesInRadius[illusions]);
                allIllusions.Add(currentIllusion);
            }
            else // not on nav mesh
            {
                point = (Random.insideUnitSphere * 3) + currentEnemy.position;
                point.y = 0; // needs to be changed
                // get closest point instead
            }
        }
        spawnPoints.Clear();
    }

    //===================================================================================================================================//

    void SlowDownEnemies(Enemy enemy, float slowDownStrength) // only working on melee enemy type
    {
        enemy.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour enemyBehaviour);
        
        enemyBehaviour.meleeEnemySpeed /= slowDownStrength;
        enemyBehaviour.enragedEnemySpeed /= slowDownStrength;

        //slow down animations
    }

    //===================================================================================================================================//

    void StopSlowDownEffect(Enemy enemy, float slowDownStrength) // only working on melee enemy type
    {
        enemy.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour enemyBehaviour);

        enemyBehaviour.meleeEnemySpeed *= slowDownStrength;
        enemyBehaviour.enragedEnemySpeed *= slowDownStrength;

        //return animations to normal speed
    }

    //===================================================================================================================================//
    
    IEnumerator IllusionSpawnWaiter()
    {
        yield return new WaitForSeconds(0.2f);
        SpawnIceIllusions();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, IRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, IIRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, IFRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, IARadius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, ILRadius);
    }
}
