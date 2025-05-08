using System;
using UnityEngine;

public interface IModifier
{
    float moveSpeedMod {  get; set; }
}

public enum eModifier { moveSpeedMod, slowDown }
[Serializable]
public class Modifier
{
    public string modName;
    public eModifier modifierName;
    public float duration;
    public float power;
    public float end;
}
