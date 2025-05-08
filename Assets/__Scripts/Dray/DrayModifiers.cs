using UnityEngine;

public class DrayModifiers : ModifierController
{
    Dray dray;
    private void Start()
    {
            dray = GetComponent<Dray>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ModifierEffect effect = collision.GetComponent<ModifierEffect>();
        if(effect != null)
        {
            print("has mod from collider");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ModifierEffect effect = collision.gameObject.GetComponent<ModifierEffect>();
        if (effect != null)
        {
            ActivateMod(dray, effect);
        }
    }
}
