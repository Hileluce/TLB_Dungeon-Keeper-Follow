using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static List<IGadget> gadgets;
    public static Inventory S;
    Dray dray;
    public IGadget currentGadget {  get; private set; }  
    public IGadget secondGadget { get; private set; }
    public GameObject curIcon;
    public GameObject secIcon;
    SpriteRenderer curRend;
    SpriteRenderer secRend;
    //public GameObject fi;
    private void Awake()
    {
        if (S == null) S = this;
        dray = GetComponent<Dray>();
        gadgets = new List<IGadget>();
        curRend = curIcon.GetComponent<SpriteRenderer>();
        secRend = secIcon.GetComponent<SpriteRenderer>();
    }

    public void SetGadget(IGadget gadget)
    {
        if (gadget == null) return;
        currentGadget = gadget;

        //curRend.sprite = currentGadget.gameObject.GetComponent<SpriteRenderer>().sprite;
        
        SetCurGadgetIcon();
        
    }
    public void SetSecondGadget(IGadget gadget)
    {
        if (gadget == null) return;
        secondGadget = gadget;
        SetSecGadgetIcon();
    }
    public void SetCurGadgetIcon() 
    { 
        curRend.sprite = currentGadget.gadgetSprite; 
    }
    public void SetSecGadgetIcon() 
    { 
        secRend.sprite = secondGadget.gadgetSprite; 
    }
    public void SetGadgetIcon(int slot, IGadget gadget)
    {
        //maybe for change upper 2 methods
    }
    public void SwapGadgets()
    {
        
        if (currentGadget == null) return;
        IGadget tempGadg = currentGadget;
        currentGadget = secondGadget;
        secondGadget = tempGadg;
        SetSecGadgetIcon();
        SetCurGadgetIcon();
    }
    public void CurToSecGadget()
    {
        if(currentGadget == null) return;
        secondGadget = currentGadget;
        SetSecGadgetIcon();
    }
    
    public void InstantiateAndSetGadget(GameObject gadget)
    {
        GameObject GO = Instantiate<GameObject>(gadget, dray.transform);
        IGadget iGgo = GO.GetComponent<IGadget>();
        Inventory.gadgets.Add(iGgo);
        SetGadget(iGgo);
        UI_Behaiver.InstallGadgetInCell(GO);
    }

}
