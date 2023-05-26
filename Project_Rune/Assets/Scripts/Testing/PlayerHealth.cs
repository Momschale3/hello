using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //===================================================================================================================================//
    
    [SerializeField]
    int maxHealth = 100;
    [SerializeField]
    int health = 100;

    bool fadeIsActive = false;

    public delegate void OnPlayerDeath();
    public static event OnPlayerDeath onPlayerDeath;

    public delegate void OnPlayerRevival();
    public static event OnPlayerRevival onPlayerRevival;

    float timer = 0;
    bool timerUsed = false;

    //===================================================================================================================================//

    private void Start()
    {
        onPlayerRevival += RestorePlayer;
    }

    public void Heal(int restoredHealth)
    {
        health += restoredHealth;
        if (maxHealth < health) health = maxHealth;
        Debug.LogWarning("RESTORING!");
    }

    //===================================================================================================================================//

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !fadeIsActive)
        {
            fadeIsActive = true;
            health = 0;
            onPlayerDeath += PlayerDeath;
            onPlayerDeath?.Invoke();
        }
    }

    //===================================================================================================================================//

    public static void RevivePlayer() // only meant for activating delegate event, revival effects are added in RestorePlayer() method
    {
        onPlayerRevival?.Invoke();
    }

    //===================================================================================================================================//

    void PlayerDeath() // dying effects can be added here
    {
        // can be removed from delegate container and called normally instead, if order doesn't fit anymore
    }

    //===================================================================================================================================//

    void RestorePlayer() // revival effects are added here
    {
        health = maxHealth;
    }

    //===================================================================================================================================//

}
