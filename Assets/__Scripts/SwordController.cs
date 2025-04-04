using UnityEngine;

public class SwordController : MonoBehaviour
{
    
    GameObject sword;
    GameObject magSword;
    Dray dray;
    bool _firingSword;

    float endFire;
    float attackDuration = 0.5f;

    float startFire;
    public float delayFire = 0.15f;
    

    public bool firingSword { get { return _firingSword; } set { _firingSword = value; } }
    private void Start()
    {
        Transform swordT = transform.Find("Sword");
        if (swordT == null) { Debug.LogError("Errrrr"); return; }
        sword = swordT.gameObject;
        Transform magSwordT = transform.Find("MagicSword");
        if (magSwordT == null) { Debug.LogError("Errrrr"); return; }
        magSword = magSwordT.gameObject;
        dray = GetComponentInParent<Dray>();
        if (dray == null) { Debug.LogError("Errrr"); return; }
        sword.SetActive(false);
        magSword.SetActive(false);
        firingSword = false;
        
    }
    private void Update()
    {
       transform.rotation = Quaternion.Euler(0f,  0f, dray.facing * 90f);
        if(!firingSword) magSword.SetActive(false);
        if (dray.health == dray.maxHealth)
        {
            if (!firingSword && dray.mode == Dray.eMode.attack)
            {
                if (Time.time > endFire)
                {
                    startFire = Time.time + delayFire;
                    endFire = Time.time + attackDuration;
                    firingSword = true;
                }
            }
            if(firingSword && Time.time > startFire)
            {
                magSword.SetActive(true);
            }
        }
            //    if (!wasFire && !firingSword && dray.mode == Dray.eMode.attack)
            //    {
            //        wasFire = true;
            //        startFire = Time.time + delayFire;
            //        print("has fire if " + Time.time);
            //    }
            //}
            //if(wasFire && !firingSword && Time.time > startFire)
            //{
            //    
            //    wasFire = false;
            //    print("aaaaaaaand it go in " + Time.time);
            //}
            sword.SetActive(dray.mode == Dray.eMode.attack);
    }
    public void MagSRet()
    {
        firingSword = false;
        magSword.transform.localRotation = Quaternion.Euler(Vector3.zero);

    }
}
