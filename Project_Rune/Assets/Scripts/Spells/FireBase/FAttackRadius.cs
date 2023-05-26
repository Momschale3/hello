using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FAttackRadius : MonoBehaviour
{ 
    //===================================================================================================================================//

    //Delegates/Events
    [HideInInspector]
    public delegate void EnemyEnteredEvent(Enemy enemy);
    [HideInInspector]
    public delegate void EnemyExitedEvent(Enemy enemy);

    [HideInInspector]
    public EnemyEnteredEvent OnEnemyEnter;
    [HideInInspector]
    public EnemyExitedEvent OnEnemyExit;

    //===================================================================================================================================//

    //Privates
    private List<Enemy> enemiesInRadius = new List<Enemy>();

    //===================================================================================================================================//

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemiesInRadius.Add(enemy);
            OnEnemyEnter?.Invoke(enemy); // ? Checkt ob der delegate null ist
        }
    }

    //===================================================================================================================================//

    private void OnTriggerExit(Collider other)
    {   

        if(other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemiesInRadius.Remove(enemy);
            OnEnemyExit?.Invoke(enemy);
        }

    }

    //===================================================================================================================================//

    private void OnDisable()
    {
        foreach(Enemy enemy in enemiesInRadius)
        {
            OnEnemyExit?.Invoke(enemy);
        }

        enemiesInRadius.Clear();
    }

    //===================================================================================================================================//

}
