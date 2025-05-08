using UnityEngine;

public class ItemButton : MonoBehaviour
{
    GameObject gadget;

    public void SetGadget(GameObject g)
    {
        gadget = g;
    }
    public IGadget GiveIGadget()
    {
        if (gadget == null) { return null; }
        return gadget.GetComponent<IGadget>();

    }
}
