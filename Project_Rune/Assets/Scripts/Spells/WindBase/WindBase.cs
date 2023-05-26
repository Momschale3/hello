using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBase : MonoBehaviour
{
    //===================================================================================================================================//

    #region General

    [Header("General")]
    [SerializeField]
    private Transform projectileSpawn;

    #endregion General

    #region Wind Base

    [Header("Wind Base Spell")]
    [Tooltip("Please reference the Base Wind Projectile Prefab here.")]
    [SerializeField]
    private GameObject Wi_prefab;
    [Tooltip("How many projectiles should be spawned?")]
    [SerializeField]
    private int Wi_neededProjectiles;
    [Tooltip("Damage of the Wind Projectile.")]
    [SerializeField]
    private int Wi_damage;
    #endregion Wind Base

    #region Fire Wind 

    [Header("Wind Fire Spell")]
    [Tooltip("Please reference the Base Wind Projectile Prefab here.")]
    [SerializeField]
    private GameObject WiF_prefab;
    [Tooltip("How many projectiles should be spawned?")]
    [SerializeField]
    private int WiF_neededProjectiles;
    [Tooltip("Damage of the Wind Fire Projectile.")]
    [SerializeField]
    private int WiF_damageOnImpact;
    [Tooltip("Burn Damage.")]
    [SerializeField]
    private int WiF_burnDPS;
    [Tooltip("Duration of the burn on the enemy.")]
    [SerializeField]
    private int WiF_burnDuration;

    #endregion Fire Wind

    #region Fire Ice

    [Header("Wind Ice Spell")]
    [Tooltip("Please reference the Wind Ice Projectile Prefab here.")]
    [SerializeField]
    private GameObject WiI_prefab;
    [Tooltip("How many projectiles should be spawned?")]
    [SerializeField]
    private int WiI_neededProjectiles;
    [Tooltip("Damage of the Wind Ice Projectile.")]
    [SerializeField]
    private int WiI_damage;
    [Tooltip("Duration of the slow on the enemy.")]
    [SerializeField]
    private int WiI_slowDuration;
    [Tooltip("Strength of the slow on the enemy.")]
    [SerializeField]
    private int WiI_slowStrength;
    [Tooltip("Explosion Range of the projectile.")]
    [SerializeField]
    private int WiI_explosionRadius;

    #endregion Fire Ice

    #region Wind Wind

    [Tooltip("Please reference the Wind Wind Projectile Prefab here.")]
    [Header("Wind Wind Spell")]
    [SerializeField]
    private GameObject WiWi_prefab;
    [Tooltip("How many projectiles should be spawned?")]
    [SerializeField]
    private int WiWi_neededProjectiles;
    [Tooltip("Damage of the Wind Projectiles.")]
    [SerializeField]
    private int WiWi_damage;

    #endregion Wind Wind

    #region Wind Light
    [Tooltip("Please reference the Wind Light Projectile Prefab here.")]
    [Header("Wind Light Spell")]
    [SerializeField]
    private GameObject WiL_prefab;
    [Tooltip("How many projectiles should be spawned?")]
    [SerializeField]
    private int WiL_neededProjectiles;
    [Tooltip("Healing Amount of the Enemy who attacks the player")]
    [SerializeField]
    private int WiL_healingAmount;
    [Tooltip("Duration of the enchanted Enemy.")]
    [SerializeField]
    private int WiL_duration;
    #endregion Wind Light

    #region Not visible 

    [HideInInspector]
    public bool spawnNow = false;
    [HideInInspector]
    public LockOnSystem lockOnSystem;
    [HideInInspector]
    public int neededProjectiles;
    private int spawnedProjectiles;
    private GameObject projectileGO;
    public WindProjectileOnHitEffect windProjectileOnHitEffect;
    public Transform handRight;

    #endregion Not visible

    public bool A;
    public bool AF;
    public bool AI;
    public bool AA;
    public bool AL;

    //===================================================================================================================================//

    private void Update()
    {
        if(spawnNow == true) //Is true in LockOnSystem 
        {         
            spawnProjectile();
            spawnNow = false;
        }
    }

    //===================================================================================================================================//

    public void isWind()
    {
        neededProjectiles = Wi_neededProjectiles;
        projectileGO = Wi_prefab;

        A = true;      

        SetupLockOn();
    }

    //===================================================================================================================================//

    public void isWindFire()
    {
        neededProjectiles = WiF_neededProjectiles;
        projectileGO = WiF_prefab;

        AF = true;
       
        SetupLockOn();
    }

    //===================================================================================================================================//

    public void isWindIce()
    {
        neededProjectiles = WiI_neededProjectiles;
        projectileGO = WiI_prefab;

        AI = true;

        SetupLockOn();
    }

    //===================================================================================================================================//
    public void isWindWind()
    {
        neededProjectiles = WiWi_neededProjectiles;
        projectileGO = WiWi_prefab;

        AA = true;

        SetupLockOn();
    }

    //===================================================================================================================================//

    public void isWindLight()
    {
        neededProjectiles = WiL_neededProjectiles;
        projectileGO = WiL_prefab;

        AL = true;

        SetupLockOn();
    }

    //===================================================================================================================================//

    public void SetupLockOn()
    {
        
        lockOnSystem.enabled = true;
        lockOnSystem.neededTargets = neededProjectiles; //Tell lock on System how many targets are needed
        lockOnSystem.StartEyeTracking(); //Activate Lock On System      
    }

    //===================================================================================================================================//

    public void spawnProjectile()
    {
        spawnedProjectiles++; //Increase Index
 
        GameObject projectile = Instantiate(projectileGO, projectileSpawn.position, Quaternion.identity, handRight);
        projectile.GetComponentInChildren<WindProjectile>().lockOnSystem = lockOnSystem;
        //===================================================================================================================================//

        windProjectileOnHitEffect = projectileGO.GetComponentInChildren<WindProjectileOnHitEffect>();

        if (A == true)
        {
            A = false;           
            windProjectileOnHitEffect.damage = Wi_damage;

            //windProjectileOnHitEffect.A_Vfx.SetActive(true);
        }
        if(AF == true)
        {
            Debug.LogWarning("HEY ITS ME");
            AF = false;
            windProjectileOnHitEffect.damage = WiF_damageOnImpact;
            windProjectileOnHitEffect.damagePerTick = WiF_burnDPS;
            windProjectileOnHitEffect.burn = true;
            windProjectileOnHitEffect.burnDuration = WiF_burnDuration;

            //windProjectileOnHitEffect.AF_Vfx.SetActive(true);

        }
        if(AI == true)
        {
            AI = false;
            windProjectileOnHitEffect.damage = WiI_damage;
            windProjectileOnHitEffect.slow = true;
            windProjectileOnHitEffect.slowAmount = WiI_slowStrength;
            windProjectileOnHitEffect.slowDuration = WiI_slowDuration;
            windProjectileOnHitEffect.explosionRadius = WiI_explosionRadius;

            //windProjectileOnHitEffect.AI_Vfx.SetActive(true);
        }
        if (AA == true)
        {
            AA = false;
            windProjectileOnHitEffect.damage = WiWi_damage;

            //windProjectileOnHitEffect.AA_Vfx.SetActive(true);
        }
        if (AL == true)
        {
            AL = false;
            //windProjectileOnHitEffect.damage = 0;
            windProjectileOnHitEffect.healDuration = WiL_duration;
            windProjectileOnHitEffect.healNow = true;
            windProjectileOnHitEffect.healStrength = WiL_healingAmount;

            //windProjectileOnHitEffect.AL_Vfx.SetActive(true);
        }

        //===================================================================================================================================//
        if (neededProjectiles > spawnedProjectiles)
        {
            StartCoroutine(waiter());
        }
        else if(neededProjectiles == spawnedProjectiles || neededProjectiles < spawnedProjectiles)
        {
            Resetter();
        }
    }

    //===================================================================================================================================//

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(0.5f);
        spawnProjectile();
    }

    //===================================================================================================================================//

    public void Resetter()
    {
        //lockOnSystem.Resetter();
        spawnedProjectiles = 0;
        neededProjectiles = 0;
       // Destroy(this.gameObject);   
    }

    //===================================================================================================================================//

}
