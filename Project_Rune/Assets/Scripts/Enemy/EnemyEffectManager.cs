using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectManager : MonoBehaviour
{
    //===================================================================================================================================//

    public EnemyBehaviour enemyBehaviour;

    public EnemyHealth enemyHealth;

    public int slow;
    public int damage;

    private float oldSpeed;

    //===================================================================================================================================//

    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    //===================================================================================================================================//

    #region Fire Effect

    public void StartBurn(int burnDPS, float duration)
    {
        //Debug.LogWarning(burnDPS + " < burn dPS ---- duration > " + duration);
        enemyHealth.StartBurning(burnDPS);
        StartCoroutine(StopBurn(duration));
    }   

    IEnumerator StopBurn(float duration)
    {
        yield return new WaitForSeconds(duration);
        enemyHealth.StopBurning();
    }

    #endregion Fire Effect

    //===================================================================================================================================//

    #region Slow Effect

    public void changeSpeed(float newSpeed, bool reset, float duration)
    {
        if (!reset)
        {
            if (enemyBehaviour.ranged)
            {
                oldSpeed = enemyBehaviour.rangedEnemySpeed;
                enemyBehaviour.rangedEnemySpeed = newSpeed;
            }
            else
            {
                oldSpeed = enemyBehaviour.meleeEnemySpeed;
                enemyBehaviour.meleeEnemySpeed = newSpeed;
            }

            StartCoroutine(SpeedResetter(duration, newSpeed, true));

        }
        else if(reset)
        {
            enemyBehaviour.meleeEnemySpeed = oldSpeed;
            enemyBehaviour.rangedEnemySpeed = oldSpeed;
        }


    }
    
    IEnumerator SpeedResetter(float duration, float newSpeed, bool reset)
    {
        yield return new WaitForSeconds(duration);
        changeSpeed(newSpeed, reset, duration);
    }

    #endregion SlowEffect

    //===================================================================================================================================//

    #region Enemy heals now

    public void EnemyHealingCoroutine(int healDuration, int oldDamage)
    {
        StartCoroutine(EnemyHealingResetter(healDuration, oldDamage));      
    }

    public IEnumerator EnemyHealingResetter(int duration, int oldDamage)
    {
        enemyBehaviour.damage = damage;
        yield return new WaitForSeconds(duration);
        enemyBehaviour.damage = oldDamage;
    }

    #endregion Enemy heals now

    //===================================================================================================================================//

    public void TakeDamage(int damage)
    {
        enemyHealth.TakeDamage(damage);
    }

    //===================================================================================================================================//

}
