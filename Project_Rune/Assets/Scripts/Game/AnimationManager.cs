using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    public Animator anim;

    public void SetBool(string parameterName, bool boolean)
    {
        anim.SetBool(parameterName, boolean);
    }

    public void SetTrigger(string parameterName)
    {
        anim.SetTrigger(parameterName);
    }

    public void SetFloat(string parameterName, float newValue)
    {
        anim.SetFloat(parameterName, newValue);
    }

}
