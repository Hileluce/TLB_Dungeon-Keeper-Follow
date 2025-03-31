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
        return gadget.GetComponent<IGadget>();

    }
}
