using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAreaCheck : MonoBehaviour
{
    public SphereCollider spellCollider;

    public delegate void NewEnemyEnter(Enemy enemy);
    public static event NewEnemyEnter enemyEnter;

    public delegate void NewEnemyExit(Enemy enemy);
    public static event NewEnemyExit enemyExit;

    [SerializeField]
    public List<Enemy> enemiesInRadius = new List<Enemy>();

    private void Start()
    {
        spellCollider = GetComponent<SphereCollider>();
        IceBase.iceSpellActivated += AreaRadius; 
    }

    public void AreaRadius(float radiusSize)
    {        
        float originalRadius = spellCollider.radius;
        spellCollider.radius += radiusSize;
        spellCollider.radius -= originalRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemiesInRadius.Add(other.GetComponent<Enemy>());
            enemyEnter?.Invoke(enemy);          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemiesInRadius.Remove(other.GetComponent<Enemy>());
            enemyExit?.Invoke(enemy);         
        }
    }

    void OnDisable()
    {
        foreach (Enemy enemy in enemiesInRadius)
        {
            enemyExit?.Invoke(enemy);
        }
        enemiesInRadius.Clear();
    }
}
