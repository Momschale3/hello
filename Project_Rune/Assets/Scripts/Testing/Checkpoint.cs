using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //===================================================================================================================================//

    [SerializeField]
    public bool isActive;
    [SerializeField]
    GameManager GM;

    //===================================================================================================================================//

    private void Start()
    {
        //GM = FindObjectOfType<GameManager>();
        GM.AddCheckpoint(this);
        if (isActive)
        {
            GM.ChangeActiveCheckpoint(this);
        }
    }

    //===================================================================================================================================//

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other.gameObject.name + " COLLIDER");

        if (other.gameObject.tag == "Player")
        {
            isActive = true;
            GM.ChangeActiveCheckpoint(this);
        }
    }

    //===================================================================================================================================//

}
