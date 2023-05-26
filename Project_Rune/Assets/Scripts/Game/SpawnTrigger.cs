using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    //===================================================================================================================================//
    [Tooltip("Spawnpoint of the Enemy.")]
    [SerializeField]
    private EnemySpawnPoint ESP;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ESP.SpawnEnemy();
            Destroy(this.gameObject.GetComponent<Collider>());
        }
    }

    //===================================================================================================================================//

}
