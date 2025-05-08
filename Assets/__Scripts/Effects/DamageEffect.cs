using UnityEngine;

public enum typeOfEffect { moveSlowing, none }
public class DamageEffect : MonoBehaviour
{
    public int damage = 1;
    public bool knockback = true;

    public bool effect = false;
    
    public typeOfEffect type = typeOfEffect.none;   
    public float effectPower = 0f;
    public float effectTime = 0f;
}
