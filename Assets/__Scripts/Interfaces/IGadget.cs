using UnityEngine;
public enum eGadgetType { none, grappler, bomb, boomerang }
public interface IGadget
{
    bool GadgetUse(Dray tDray, System.Func<IGadget, bool> tDoneCallback);
    bool GadgetCancel();
    string name { get; }
    GameObject gameObject { get; }
    Sprite gadgetSprite { get; }
}
