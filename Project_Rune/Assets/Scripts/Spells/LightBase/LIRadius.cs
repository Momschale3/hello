using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LIRadius : MonoBehaviour
{
    //===================================================================================================================================//

    //Delegates/Events
    [HideInInspector]
    public delegate void EnemyEnteredEvent(EnemyBehaviour enemyBehaviour);
    [HideInInspector]
    public delegate void EnemyExitedEvent(EnemyBehaviour enemyBehaviour);

    [HideInInspector]
    public EnemyEnteredEvent OnEnemyEnter;
    [HideInInspector]
    public EnemyExitedEvent OnEnemyExit;

    //===================================================================================================================================//

    //Privates
    private List<EnemyBehaviour> enemiesInRadius = new List<EnemyBehaviour>();

    //===================================================================================================================================//

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour enemyBehaviour))
        {
            enemiesInRadius.Add(enemyBehaviour);
            OnEnemyEnter?.Invoke(enemyBehaviour); // ? Checkt ob der delegate null ist
        }
    }

    //===================================================================================================================================//

    private void OnTriggerExit(Collider other)
    {

        if (other.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour enemyBehaviour))
        {
            enemiesInRadius.Remove(enemyBehaviour);
            OnEnemyExit?.Invoke(enemyBehaviour);
        }

    }

    //===================================================================================================================================//

    private void OnDisable()
    {
        foreach (EnemyBehaviour enemyBehaviours in enemiesInRadius)
        {
            OnEnemyExit?.Invoke(enemyBehaviours);
        }

        enemiesInRadius.Clear();
    }

    //===================================================================================================================================//
}
