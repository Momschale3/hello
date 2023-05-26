using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyHealth : MonoBehaviour, IDamageable, IBurnable
{
    //===================================================================================================================================//

    [Tooltip("Health of the Enemy.")]
    [SerializeField]
    private int _health;

    //===================================================================================================================================//

    //Publics
    public delegate void DeathEvent(Enemy enemy);
    public event DeathEvent OnDeath;

    //===================================================================================================================================//

    //Privates
    private GameManager GM;
    private Coroutine burningCoroutine;

    //===================================================================================================================================//

    #region Getters & Setters 
    public int health { get => _health; set => _health = value; } //Get Health from IDamagable

    [SerializeField]
    private bool _isBurning;
    public bool isBurning { get => _isBurning; set => _isBurning = value; } //Get from IBurnable

    #endregion Getters & Setters

    //===================================================================================================================================//

    private void Start()
    {
        GM = FindAnyObjectByType<GameManager>();
    }

    //===================================================================================================================================//

    public void TakeDamage(int damage)
    {
        health -= damage;      
        if (health <= 0)
        {
            health = 0;
            //Kill Enemy
            OnDeath += killEnemy();
            OnDeath?.Invoke(GetComponent<Enemy>());

            StopBurning();
        }
    }

    //===================================================================================================================================//

    public void StartBurning(int damagePerSecond)
    {
        isBurning = true;
        if(burningCoroutine != null)
        {
            StopCoroutine(burningCoroutine);
        }

        burningCoroutine = StartCoroutine(Burn(damagePerSecond));
    }

    //===================================================================================================================================//

    private IEnumerator Burn(int damagPerSecond)
    {
        float minTimeToDamage = 1f / damagPerSecond;
        WaitForSeconds wait = new WaitForSeconds(minTimeToDamage);
        int damagePerTick = Mathf.FloorToInt(minTimeToDamage + 1);

        TakeDamage(damagePerTick);      

        while(isBurning)
        {
            yield return wait;
            TakeDamage(damagePerTick);
        }
    }

    //===================================================================================================================================//

    public void StopBurning()
    {
        isBurning = false;
        if(burningCoroutine != null)
        {
            StopCoroutine(burningCoroutine);
        }
    }

    //===================================================================================================================================//

    public DeathEvent killEnemy()
    {
        
        GM.RemoveThisEnemy(this.gameObject);
        Destroy(this.gameObject);

        return OnDeath;
    }

    //===================================================================================================================================//

}
