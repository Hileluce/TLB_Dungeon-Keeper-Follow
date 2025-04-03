using UnityEngine;
using UnityEngine.Events;

public class DrayEvents : MonoBehaviour
{
    public static UnityEvent RefreshHealthUI;
    public static UnityEvent DrayDeath;

    void Awake()
    {
        if (RefreshHealthUI == null) RefreshHealthUI = new UnityEvent();
        if(DrayDeath == null) DrayDeath = new UnityEvent();
    }
    
}
