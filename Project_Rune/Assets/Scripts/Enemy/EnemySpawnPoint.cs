using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{

    //===================================================================================================================================//
    [Tooltip("Enemy prefab.")]
    [SerializeField]
    private GameObject enemyToSpawnPrefab;

    [Tooltip("How many enemies should spawn on this spawnpoint?")]
    [SerializeField]
    private int howManyEnemies = 1;

    [Tooltip("All spawnpoints")]
    [SerializeField]
    private Transform[] spawnPoints;

    //===================================================================================================================================//

    //Privates
    [HideInInspector]
    public GameManager GM;
    private GameObject holder;

    //===================================================================================================================================//

    private void Start()
    {
        GM = FindAnyObjectByType<GameManager>();
    }

    //===================================================================================================================================//

    public void SpawnEnemy()
    {
        for (int i = 0; i < howManyEnemies; i++)
        {
            holder = Instantiate(enemyToSpawnPrefab, spawnPoints[i]);
            GM.AddThisEnemy(holder);
        }
    }

    //===================================================================================================================================//

}
