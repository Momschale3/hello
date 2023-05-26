using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnrageable
{
    public bool isEnraged { get; set; }

    public void enrageTimer(int duration);
}
