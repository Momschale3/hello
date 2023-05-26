using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GesturePlaceHolder : MonoBehaviour
{

    public bool RuneF;
    public bool RuneWi;
    public bool RuneWa;
    public bool RuneL;

    public bool attackGeste;

    public SpellManager SM;

    private void Update()
    {
        if(RuneF)
        {
            SM.drewRune("F"); //Fire
            RuneF = false;
        }

        if (RuneWi)
        {
            SM.drewRune("A"); //Air
            RuneWi = false;
        }

        if (RuneWa)
        {
            SM.drewRune("I"); //Water
            RuneWa = false;
        }

        if (RuneL)
        {
            SM.drewRune("L"); //Light
            RuneL = false;
        }

        if(attackGeste)
        {
            attackGeste = false;
            SM.CastSpell();
        }

    }

}
