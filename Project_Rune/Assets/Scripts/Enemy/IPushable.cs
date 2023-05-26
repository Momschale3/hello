using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable 
{
    
    public bool beingPushed   { get; set; } 

    public Transform sender { get; set; }

    public float strength { get; set; }

    public float intervals { get; set; }


}
