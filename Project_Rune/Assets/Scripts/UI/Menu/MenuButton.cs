using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct ButtonEffect
{
    public UnityEvent onActivated;
}

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    private ButtonEffect buttonEffect;

    public void OnActivated()
    {
        Debug.LogWarning("LOLOLOL");
        buttonEffect.onActivated?.Invoke();
    }

   

}
