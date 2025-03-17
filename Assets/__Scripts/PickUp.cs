using UnityEngine;

public class PickUp : MonoBehaviour, ISwappable
{
    public enum eType { none, key, health, gadget, healthContainer, heartArmor }
    public eType itemType;
    
    public GameObject gadgetLink; // items allready in Dray GO, just trigger to activate on  pickup
    Collider2D colld;
    private const float colliderEnableDelay = 0.5f;
    private void Awake()
    {
        colld = GetComponent<Collider2D>();
        colld.enabled = false;
        Invoke(nameof(EnableCollider), colliderEnableDelay);
    }
    void EnableCollider()
    {
        colld.enabled = true;
    }
    public GameObject guaranteedDrop { get; set; }
    public int tileNum { get; private set; }
    public virtual void Init (int fromTileNum, int tileX, int tileY)
    {
        tileNum = fromTileNum;
        transform.position = new Vector3(tileX, tileY, 0) + MapInfo.OFFSET;
        
    }
}
