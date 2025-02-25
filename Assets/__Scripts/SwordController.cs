using UnityEngine;

public class SwordController : MonoBehaviour
{
    
    GameObject sword;
    Dray dray;
    private void Start()
    {
        Transform swordT = transform.Find("Sword");
        if (swordT == null) { Debug.LogError("Errrrr"); return; }
        sword = swordT.gameObject;
        dray = GetComponentInParent<Dray>();
        if (dray == null) { Debug.LogError("Errrr"); return; }
        sword.SetActive(false);
    }
    private void Update()
    {
       transform.rotation = Quaternion.Euler(0f,  0f, dray.facing * 90f);
       sword.SetActive(dray.mode == Dray.eMode.attack);
    }
}
