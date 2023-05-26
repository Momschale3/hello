using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{

    public bool secondRune = false;
    public bool runeCombiFull;
    public string spellToCast;
    public Transform myHand;
    public GameObject playerGO;
    public GameManager gameManager;

    [Header("Materials")]

    public MeshRenderer handMesh;

    public Material Normal;

    [Header("Fire Materials")]

    public Material Fire;
    public Material FireFire;
    public Material FireIce;
    public Material FireAir;
    public Material FireLight;

    [Header("Ice Materials")]

    public Material Ice;
    public Material IceFire;
    public Material IceIce;
    public Material IceWind;
    public Material IceLight;

    [Header("Wind Materials")]

    public Material Wind;
    public Material WindFire;
    public Material WindIce;
    public Material WindWind;
    public Material WindLight;

    [Header("Light Materials")]

    public Material Light;
    public Material LightFire;
    public Material LightIce;
    public Material LightWind;
    public Material LightLight;

    public GameObject[] allSpells;

    public LockOnSystem lockOnSystem;

    public int index;
    private bool drew;  

    //Fire Index = 100 - 199
    //Ice Index = 200 - 299
    //Wind Index = 300 - 399
    //Light Index = 400 - 499
    private int spellIndex;

    private Material spellMat;


    //===================================================================================================================================//

    private void Start()
    {
        //spellIndex = 300;
        //GameObject SpawnedSpell = Instantiate(allSpells[1], myHand.transform.position, Quaternion.identity, myHand);
        //isAir(SpawnedSpell);
    }

    //===================================================================================================================================//

    //Von Gesture Recognition das hier callen --> mit string F/Wi/Wa/L --> SpellManager.drewRune(F) / SpellManager.drewRune(Wi);
    public void drewRune(string rune)
    {
        if (secondRune == false &&!runeCombiFull) //Wenn das die erste Rune ist 
        {
            secondRune = true;
            spellToCast = rune;
        }
        else if (secondRune == true && runeCombiFull == false) //Wenn das die zweite Rune ist
        {
            spellToCast = spellToCast + rune;
            secondRune = false;
            runeCombiFull = true;
        }
        else if(runeCombiFull) //Wenn schon zwei gecastet wurden
        {
            runeCombiFull = false;
            return;
        }
        drew = true;

        IdentifySpell();
    }

    //===================================================================================================================================//

    //On Hand Release Gesture --> Wenn Zauber gewirkt werden soll call das hier
    public void CastSpell()
    {
        if (spellToCast == "")
        {
            Debug.LogError("No rune drawn.");
        }
        else
        {           
            foreach (GameObject spell in allSpells)
            {       
                if (spellToCast[0].ToString() == spell.tag)
                {
                    GameObject SpawnedSpell = Instantiate(spell,myHand);

                    if (spellIndex < 200 && spellIndex > 0)
                    {
                        isFire(SpawnedSpell);
                        Resetter();
                    }
                    else if(spellIndex < 300 && spellIndex > 199)
                    {
                        isIce(SpawnedSpell);
                        Resetter();
                    }
                    else if(spellIndex < 400 && spellIndex > 299)
                    {
                        isAir(SpawnedSpell);
                        Resetter();
                    }
                    else if(spellIndex < 500 && spellIndex > 399)
                    {
                        isLight(SpawnedSpell);
                        Resetter();
                    }
                    break;
                }            
            }

            Resetter();

        }
    }

    //===================================================================================================================================//

    public void IdentifySpell()
    {       
        if (spellToCast[0] == 'F')
        {
            IdentifyFire();
        }
        else if(spellToCast[0] == 'I')
        {
            IdentifyIce();
        }
        else if (spellToCast[0] == 'A')
        {
            IdentifyAir();
        }
        else if (spellToCast[0] == 'L')
        {
            IdentifyLight();
        }

        ChangeHandMaterial(spellMat);
        

    }

    private void IdentifyFire()
    {
        if (spellToCast.Length == 2) //F1 -> 0 = F | 1 = 1
        {
            spellIndex = 100;
            spellMat = Fire;
        }
        else
        {
            if (spellToCast[2] == 'F') //F1F2 -> 0 = F | 1 = 1 | 2 = F
            {
                spellIndex = 110;
                spellMat = FireFire;
            }
            else if (spellToCast[2] == 'I')//F5I9 -> 0 = F | 1 = 1 | 2 = I
            {
                spellIndex = 120;
                spellMat = FireIce;
            }
            else if (spellToCast[2] == 'A')
            {
                spellIndex = 130;
                spellMat = FireAir;
            }
            else if (spellToCast[2] == 'L')
            {
                spellIndex = 140;
                spellMat = FireLight;
            }
        }
    }

    //===================================================================================================================================//
    private void IdentifyIce()
    {
        if (spellToCast.Length == 2) //F1 -> 0 = F | 1 = 1
        {
            spellIndex = 200;
            spellMat = Ice;
        }
        else
        {
            if (spellToCast[2] == 'F') //F1F2 -> 0 = F | 1 = 1 | 2 = F
            {
                spellIndex = 210;
                spellMat = IceFire;
            }
            else if (spellToCast[2] == 'I')//F5I9 -> 0 = F | 1 = 1 | 2 = I
            {
                spellIndex = 220;
                spellMat = IceIce;
            }
            else if (spellToCast[2] == 'A')
            {
                spellIndex = 230;
                spellMat = IceWind;
            }
            else if (spellToCast[2] == 'L')
            {
                spellIndex = 240;
                spellMat = IceLight;
            }
        }
    }

    //===================================================================================================================================//
    private void IdentifyAir()
    {
        if (spellToCast.Length == 2) //F1 -> 0 = F | 1 = 1
        {
            spellIndex = 300;
            spellMat = Wind;
        }
        else
        {
            if (spellToCast[2] == 'F') //F1F2 -> 0 = F | 1 = 1 | 2 = F
            {
                spellIndex = 310;
                spellMat = WindFire;
            }
            else if (spellToCast[2] == 'I')//F5I9 -> 0 = F | 1 = 1 | 2 = I
            {
                spellIndex = 320;
                spellMat = WindIce;
            }
            else if (spellToCast[2] == 'A')
            {
                spellIndex = 330;
                spellMat = WindWind;
            }
            else if (spellToCast[2] == 'L')
            {
                spellIndex = 340;
                spellMat = WindLight;
            }
        }
    }

    //===================================================================================================================================//
    private void IdentifyLight()
    {
        if (spellToCast.Length == 2) //F1 -> 0 = F | 1 = 1
        {
            spellIndex = 400;
            spellMat = Light;
        }
        else
        {
            if (spellToCast[2] == 'F') //F1F2 -> 0 = F | 1 = 1 | 2 = F
            {
                spellIndex = 410;
                spellMat = LightFire;
            }
            else if (spellToCast[2] == 'I')//F5I9 -> 0 = F | 1 = 1 | 2 = I
            {
                spellIndex = 420;
                spellMat = LightIce;
            }
            else if (spellToCast[2] == 'A')
            {
                spellIndex = 430;
                spellMat = LightWind;
            }
            else if (spellToCast[2] == 'L')
            {
                spellIndex = 440;
                spellMat = LightLight;
            }
        }
    }

    //===================================================================================================================================//

    private void ChangeHandMaterial(Material mat)
    {
        handMesh.material = mat;
    }

    //===================================================================================================================================//
    private void isFire(GameObject spawnedSpell)
    {
        if(spellIndex == 100)
        {
            spawnedSpell.GetComponent<FireBase>().Fire();           
            Debug.LogWarning("Im spawning fire base");
        }
        else if(spellIndex == 110)
        {
            spawnedSpell.GetComponent<FireBase>().FireFire();
            Debug.LogWarning("Im spawning Fire Fire");
        }
        else if(spellIndex == 120)
        {
            spawnedSpell.GetComponent<FireBase>().FireIce();
            Debug.LogWarning("Im spawning Fire Ice");
        }
        else if(spellIndex == 130)
        {
            spawnedSpell.GetComponent<FireBase>().FireAir();
            Debug.LogWarning("Im spawning Fire Air");
        }
        else if(spellIndex == 140)
        {
            spawnedSpell.GetComponent<FireBase>().FireLight();
            Debug.LogWarning("Im spawning Fire Light");
        }
        Resetter();
    }

    //===================================================================================================================================//

    private void isIce(GameObject spell)
    {

        IceBase icy = spell.GetComponent<IceBase>();
        icy.playerGO = playerGO;

        if (spellIndex == 200)
        {
            icy.Ice();
            Debug.LogWarning("Im spawning Ice base");
        }
        else if (spellIndex == 210)
        {
            icy.IceFire();
            Debug.LogWarning("Im spawning Ice Fire");
        }
        else if (spellIndex == 220)
        {
            icy.IceIce();
            Debug.LogWarning("Im spawning Ice Ice");
        }
        else if (spellIndex == 230)
        {
            icy.IceAir();
            Debug.LogWarning("Im spawning Ice Air");
        }
        else if (spellIndex == 240)
        {
            icy.IceLight();
            Debug.LogWarning("Im spawning Ice Light");
        }
        Resetter();
    }

    //===================================================================================================================================//

    private void isAir(GameObject spell)
    {

        lockOnSystem.windBase = spell.GetComponent<WindBase>();
        spell.GetComponent<WindBase>().handRight = myHand;
        spell.GetComponent<WindBase>().lockOnSystem = lockOnSystem;

        if (spellIndex == 300)
        {                     
            spell.GetComponent<WindBase>().isWind();
            Debug.LogWarning("Im spawning Air base");           
        }
        else if (spellIndex == 310)
        {
            spell.GetComponent<WindBase>().isWindFire();
            Debug.LogWarning("Im spawning Air Fire");          
        }
        else if (spellIndex == 320)
        {
            spell.GetComponent<WindBase>().isWindIce();
            Debug.LogWarning("Im spawning Air Ice");         
        }
        else if (spellIndex == 330)
        {
            spell.GetComponent<WindBase>().isWindWind();
            Debug.LogWarning("Im spawning Air Air");
           
        }
        else if (spellIndex == 340)
        {
            spell.GetComponent<WindBase>().isWindLight();
            Debug.LogWarning("Im spawning Air Light");
        }
        Resetter();
    }

    //===================================================================================================================================//

    private void isLight(GameObject spell)
    {
        LightBase LB = spell.GetComponent<LightBase>();
        LB.handPos = myHand;
        LB.lockOnSystem = lockOnSystem;

        if (spellIndex == 400)
        {
            LB.isLight();
            Debug.LogWarning("Im spawning Light base");
        }
        else if (spellIndex == 410)
        {
            LB.isLightFire();
            Debug.LogWarning("Im spawning Light Fire");
        }
        else if (spellIndex == 420)
        {
            LB.isLightIce();
            Debug.LogWarning("Im spawning Light Ice");
        }
        else if (spellIndex == 430)
        {
            LB.isLightAir();
            Debug.LogWarning("Im spawning Light Air");
        }
        else if (spellIndex == 440)
        {
            LB.isLightLight();
            Debug.LogWarning("Im spawning Light Light");
        }
        Resetter();
    }

    //===================================================================================================================================//
    private void Resetter()
    {
        handMesh.material = Normal;
        drew = false;
        spellToCast = "";                           //reset 
        runeCombiFull = false;                      //reset
        secondRune = false;
        index = 0;
    }

    //===================================================================================================================================//

}
