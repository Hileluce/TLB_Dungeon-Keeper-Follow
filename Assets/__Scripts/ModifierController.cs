using UnityEngine;
using System.Collections;
public abstract class ModifierController : MonoBehaviour
{
    //eNum eModifier places in IModifier script
    IModifier iModOwner;
    
    public void ActivateMod(IModifier iMod, ModifierEffect modEf)
    {
        print("Mods activated");
        iModOwner = iMod;
        foreach (var mod in modEf.modifiers)
        {
            ApplyMod(mod);
        }
    }
    void ApplyMod(Modifier mod)
    {
        switch (mod.modifierName)
        {
            case eModifier.moveSpeedMod:
                StartCoroutine(SpeedModAction(mod.duration, mod.power));
                break;
            case eModifier.slowDown:
                //any slowdown action
                break;
            default:
                Debug.LogError("Modifier not accepter");
                break;
        }
    }
    IEnumerator SpeedModAction(float dur, float power)
    {
        iModOwner.moveSpeedMod *= power;
        yield return new WaitForSeconds(dur);
        iModOwner.moveSpeedMod /= power;
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    print(collision.name + " from ModifOCntroller");
    //}
}
