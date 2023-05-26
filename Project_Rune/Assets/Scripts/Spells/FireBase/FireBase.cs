using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBase : MonoBehaviour
{
    //===================================================================================================================================//

    //Placeholder
    public bool isF;
    public bool isFF;
    public bool isFA;
    public bool isFI;
    public bool isFL;

    [SerializeField]
    private FAttackRadius attackRadius;

    //===================================================================================================================================//

    #region Fire Base

    [Space(20)]

    [Header("Fire Base Spell")]

    [Tooltip("Damage Per Second of the burn.")]
    [SerializeField]
    private int FburningDPS = 5;

    [Tooltip("How long should the target burn?")]
    [SerializeField]
    private float FburnDuration = 3f;

    [Tooltip("How long is the spell active?")]
    [SerializeField]
    private int Fduration;

    [Tooltip("VFX of Spell.")]
    [SerializeField]
    private GameObject fireBase;

    #endregion Fire Base

    //===================================================================================================================================//

    #region Fire Fire

    [Space(20)]

    [Header("Fire Fire Combi")]

    [Tooltip("Damage Per Second of the burn.")]
    [SerializeField]
    private int FFburningDPS = 5;

    [Tooltip("How long should the target burn?")]
    [SerializeField]
    private float FFburnDuration = 3f;

    [Tooltip("How long is the spell active?")]
    [SerializeField]
    private int FFduration;

    [Tooltip("VFX of Spell.")]
    [SerializeField]
    private ParticleSystem fireFireCombi;

    #endregion Fire Fire

    //===================================================================================================================================//

    #region Fire Water

    [Space(20)]

    [Header("Fire Water Combi")]
    [Tooltip("Damage Per Second of the burn.")]
    [SerializeField]
    private int FWaDPS = 0;

    [Tooltip("How long should the target burn?")]
    [SerializeField]
    private float FWaburnDuration = 0;

    [Tooltip("How long is the spell active?")]
    [SerializeField]
    private int FWaduration;

    [Tooltip("Duration of VFX")]
    [SerializeField]
    private int FWadurationVFX;

    [Tooltip("Steam prefab")]
    [SerializeField]
    private GameObject steamPrefab;

    [Tooltip("Where should the Steam prefab spawn?")]
    [SerializeField]
    private Transform spawnSteamPos;

    [Tooltip("VFX of Spell.")]
    [SerializeField]
    private ParticleSystem fireWaterCombi;

    #endregion Fire Water

    //===================================================================================================================================//

    #region Fire Wind

    [Space(20)]

    [Header("Fire Wind Combi")]

    [Tooltip("Damage Per Second of the burn.")]
    [SerializeField]
    private int FWiDPS = 5;

    [Tooltip("How long should the target burn?")]
    [SerializeField]
    private float FWiburnDuration = 0;

    [Tooltip("How long is the spell active?")]
    [SerializeField]
    public int FWiduration;

    [Tooltip("The strength of the push.")]
    [SerializeField]
    private float pushStrength = 0;

    [Tooltip("The interval in which pushing occurs.")]
    [SerializeField]
    private float pushInterval;

    [Tooltip("VFX of Spell.")]
    [SerializeField]
    private ParticleSystem fireWindCombi;

    #endregion Fire Wind

    //===================================================================================================================================//

    #region Fire Light

    [Space(20)] 

    [Header("Fire Light Combi")]
    [Tooltip("Damage Per Second of the burn.")]
    [SerializeField]
    public int FLduration;

    [Tooltip("How long should the enemy be in the enraged status?")]
    [SerializeField]
    private int FLEnrageDuration;

    [Tooltip("VFX of Spell.")]
    [SerializeField]
    private ParticleSystem fireLightCombi;

    #endregion Fire Light

    //===================================================================================================================================//

    //Privates
    private int burningDPS;
    private float burnDuration;

    //===================================================================================================================================//

    private void Awake()
    {      
        attackRadius.OnEnemyEnter += StartDamagingEnemy;
        attackRadius.OnEnemyExit += StopDamagingEnemy;
    }

    //===================================================================================================================================//

    private void Start()
    {
        if(pushInterval < 0.8)
        {
            Debug.LogError("push Intervall darf nicht 0.8 oder weniger sein!");
        }
    }

    //===================================================================================================================================//

    public void Fire()
    {
        isF = true;
        burningDPS = FburningDPS;
        burnDuration = FburnDuration;
        fireBase.SetActive(true);

        attackRadius.gameObject.SetActive(true);
        StartCoroutine(destructor(Fduration));
    }

    //===================================================================================================================================//

    public void FireFire()
    {
        isFF = true;
        burningDPS = FFburningDPS;
        burnDuration = FFburnDuration;
        fireFireCombi.gameObject.SetActive(true);

        attackRadius.gameObject.SetActive(true);
        StartCoroutine(destructor(FFduration));

    }

    //===================================================================================================================================//

    public void FireIce()
    {
        //Placeholder
        isFI = true;

        fireWaterCombi.gameObject.SetActive(true);

        //Instantiate block vor den spieler 
        //Raycast von fernkampf zu spieler schaueb ob block dazwischen
        GameObject spawnedSteam;

        spawnedSteam = Instantiate(steamPrefab, spawnSteamPos.position, Quaternion.identity);
        spawnedSteam.GetComponent<FireWaterSteam>().duration = FWaduration;
        StartCoroutine(destructor(FWadurationVFX));

    }

    //===================================================================================================================================//

    public void FireAir()
    {
        isFA = true;
        burningDPS = FWiDPS;
        burnDuration = FWiburnDuration;
        fireWindCombi.gameObject.SetActive(true);

        attackRadius.gameObject.SetActive(true);
        StartCoroutine(destructor(FWiduration));
    }

    //===================================================================================================================================//

    public void FireLight()
    {
        isFL = true;
        //Enraging Enemies
        burningDPS = 0;
        burnDuration = 0;
        fireLightCombi.gameObject.SetActive(true);

        attackRadius.gameObject.SetActive(true);
        StartCoroutine(destructor(FLduration));
    }

    //===================================================================================================================================//

    private void StartDamagingEnemy(Enemy enemy)
    {
        //Fire/Fire Fire/Fire Wind
        //BUG ?
        if (isF || isFF || isFA)
        {
            if (enemy.TryGetComponent<IBurnable>(out IBurnable burnable))
            {
                burnable.StartBurning(burningDPS);
                enemy.Health.OnDeath += HandleEnemyDeath;
            }
        }

        //Wind
        if (isFI)
        {
            if (enemy.TryGetComponent<IPushable>(out IPushable pushable))
            {

                pushable.beingPushed = true;
                pushable.sender = this.transform;
                pushable.strength = pushStrength;
                pushable.intervals = pushInterval;                             
            }
        }

        //Light
        if(isFL)
        {
            if(enemy.TryGetComponent<IEnrageable>(out IEnrageable enrageable))
            {
                enrageable.isEnraged = true;
                enrageable.enrageTimer(FLEnrageDuration);
            }
        }

    }

    //===================================================================================================================================//
    private void HandleEnemyDeath(Enemy enemy)
    {
        enemy.Health.OnDeath -= HandleEnemyDeath;

        if (isF || isFF) 
        {
            StartCoroutine(DelayedDisableBurn(enemy, burnDuration));
            
        }
    }

    //===================================================================================================================================//

    private IEnumerator DelayedDisableBurn(Enemy enemy, float duration)
    {
        yield return new WaitForSeconds(duration);
        if(enemy.TryGetComponent<IBurnable>(out IBurnable burnable))
        {
            burnable.StopBurning();
        }
    }

    //===================================================================================================================================//

    private void StopDamagingEnemy(Enemy enemy)
    {
        enemy.Health.OnDeath -= HandleEnemyDeath;
        StartCoroutine(DelayedDisableBurn(enemy, burnDuration));
       
    }

    //===================================================================================================================================//

    private IEnumerator destructor(int duration)
    {
        yield return new WaitForSeconds(duration);
        
        //Placeholder
        isF = false;
        isFF = false;
        isFI = false;
        isFA = false;
        isFL = false;

        StopVFX();

        attackRadius.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(burnDuration + 0.5f);
        Destroy(this.gameObject);
    }

    //===================================================================================================================================//

    private void StopVFX()
    {
        fireBase.SetActive(false);
        fireFireCombi.Stop();
        fireWaterCombi.Stop();
        fireWindCombi.Stop();
        fireLightCombi.Stop();
    }

    //===================================================================================================================================//

}
