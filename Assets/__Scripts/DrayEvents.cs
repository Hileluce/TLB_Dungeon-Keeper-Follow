using UnityEngine;
using UnityEngine.Events;

public class DrayEvents : MonoBehaviour
{
    public static UnityEvent RefreshHealthUI;
    public static UnityEvent Healing;

    void Awake()
    {
        if (RefreshHealthUI == null) RefreshHealthUI = new UnityEvent();
        if(Healing == null) Healing = new UnityEvent();

       
    }
   
}
