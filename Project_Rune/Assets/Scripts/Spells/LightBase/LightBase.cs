using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBase : MonoBehaviour
{
    //===================================================================================================================================//

    //Light
    [Header("Light")]  
    [SerializeField, Tooltip("How much should the Basic Light Spell heal.")]
    private int lightHeal;
    [SerializeField]
    private GameObject L_Vfx;

    //Light Fire
    [Header("Light Fire")]
    [SerializeField, Tooltip("Reference the shield prefab here.")]
    private GameObject shieldPrefab;
    [SerializeField, Tooltip("How many projectiles can the shield reflect?")]
    private int shieldDurability;
    [SerializeField]
    private GameObject LF_Vfx;

    //Light Ice
    [Header("Light Ice")]
    [SerializeField, Tooltip("Duration of the AOE Blind.")]
    private int LI_duration;
    [SerializeField, Tooltip("Dont touch if the reference is already inside. Put the LI Range fomr the prefab in here.")]
    private LIRadius LI_range;
    [SerializeField]
    private GameObject LI_Vfx;

    //Light Air
    [Header("Light Air")]
    [SerializeField, Tooltip("Duration of the Eye Tracking Blessing.")]
    private int LA_duration;
    [SerializeField]
    private GameObject LA_Vfx;

    //LightLgiht
    [Header("Light Light")]
    [SerializeField, Tooltip("How much healing should doube Light Rune give?")]
    private int lightLightHeal;
    [SerializeField]
    private GameObject LL_Vfx;

    private int oldDamage;
    private PlayerHealth playerHealth;

    [HideInInspector]
    public LockOnSystem lockOnSystem;
    [HideInInspector]
    public EyeTrackingRay eyeTrackingRay;
    [HideInInspector]
    public LightBase thisLightBase;
    [HideInInspector]
    public Transform handPos;

    //===================================================================================================================================//

    private void Awake()
    {
        LI_range.OnEnemyEnter += StartBlindingEnemy;
        LI_range.OnEnemyExit += StopBlindingEnemy;
    }

    //===================================================================================================================================//

    public void isLight()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerHealth.Heal(lightHeal);      
    }

    //===================================================================================================================================//

    public void isLightFire()
    {
        LightShield shield = Instantiate(shieldPrefab, handPos).GetComponent<LightShield>();
        shield.shieldDurability = shieldDurability;
        shield.lightBase = this;
    }

    //===================================================================================================================================//

    public void isLightIce()
    {
        LI_range.gameObject.SetActive(true);
        StartCoroutine(LightIceTimer());
        Debug.LogWarning("ITS WAY TO BRIGHT!");
    }

    //===================================================================================================================================//

    public void isLightAir()
    {
        eyeTrackingRay  = lockOnSystem.eye_R;
        eyeTrackingRay.gameObject.SetActive(true);
        eyeTrackingRay.LightAir = true;

        Debug.LogWarning("RIGHT BACK TO THE SENDER 222222");

        StartCoroutine(LightAirTimer());
    }

    //===================================================================================================================================//

    public void isLightLight()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerHealth.Heal(lightLightHeal);

        Debug.LogWarning("BIG HEAL INCOMING!!!!!!!!!");
    }

    //===================================================================================================================================//

    public void StartBlindingEnemy(EnemyBehaviour enemyBehaviour)
    {
        oldDamage = enemyBehaviour.damage;
        enemyBehaviour.damage = 0;
    }

    //===================================================================================================================================//

    public void StopBlindingEnemy(EnemyBehaviour enemyBehaviour)
    {
        enemyBehaviour.damage = oldDamage;
        Resetter();
    }

    //===================================================================================================================================//

    IEnumerator LightIceTimer()
    {
        yield return new WaitForSeconds(LI_duration);
        LI_range.gameObject.SetActive(false);
        Resetter();
    }

    //===================================================================================================================================//

    IEnumerator LightAirTimer()
    {
        yield return new WaitForSeconds(LA_duration);
        eyeTrackingRay.LightAir = false;
        eyeTrackingRay.gameObject.SetActive(false);
        Resetter();
    }

    //===================================================================================================================================//

    public void Resetter()
    {
        Destroy(this.gameObject,0.5f);
    }

}
