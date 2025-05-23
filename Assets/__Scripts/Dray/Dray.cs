using UnityEngine;
using UnityEngine.UI;

public class Dray : MonoBehaviour, IFaceMover, IKeyMaster, IModifier
{
    // ---------------------*******__COMPONNENTS__******-------------------
    static private Dray S;
    static public IFaceMover IFM;
    Rigidbody2D rig;
    Animator anim;
    private SpriteRenderer sRend;
    private Inventory inventory;
    private DrayAudio audioDray;
    private InRoom inRm;
    Collider2D colld;

    // ---------------------*******__STATS__*********-------------------
    public float speed = 5;
    public float attackDuration = 0.25f;
    public float attackDelay = 0.5f;
    private float timeAtkDone = 0;
    float timeAtkNext = 0;
    const int highestAmountOfHealth = 20;
    [SerializeField] // maximum for UI hearts in Panel for Grid Layout Group is 20. More of that will brake grid
    int _maxHealth = 8;
    public int maxHealth
        {   get { return _maxHealth; }    set { _maxHealth = Mathf.Min(value, highestAmountOfHealth); }}
    [SerializeField] [Range(0, highestAmountOfHealth)] 
    private int _health;
    public int health
        {   get { return _health; }       set { _health = value; } }

    const int highestArmoredHeart = highestAmountOfHealth / 2;
    int _armoredHeart = 0;
    public int armoredHeart
    { get { return _armoredHeart; } set { _armoredHeart = Mathf.Min(value, highestArmoredHeart); } }
    public int healthPickupAmount = 2;
    public float knockbackSpeed = 10;
    public float knockbackDuration = 0.25f;
    public float invincibleDuration = 0.25f;


    // ---------------------*******__MODS__*********-------------------
    float _moveSpeedMod = 1;
    public float moveSpeedMod { set { _moveSpeedMod = Mathf.Clamp(value, 0.2f, 1.4f); } get { return _moveSpeedMod; } }

    // ---------------------*******__CONTROL__**********-------------------
    public int dirHeld = -1;
    Vector2[] dir = new Vector2[] { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
    KeyCode[] keys = new KeyCode[] { KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.D, KeyCode.W, KeyCode.A, KeyCode.S };
    public enum eMode { idle, move, attack, roomTrans, knockback, gadget }
    public bool moving { get { return (mode == eMode.move); } }
    public int facing = 1;
    public eMode mode = eMode.idle;
        [SerializeField] [Range(0, 20)] 
    private int _numKeys = 0;
    public KeyCode keyAttack = KeyCode.Z;
    public KeyCode keyMainAttack = KeyCode.K;
    public KeyCode keyGadget = KeyCode.X;
    public KeyCode keyMainGadget = KeyCode.J;
    public KeyCode keySwapGadget = KeyCode.I;

    // ---------------------*******__INFO__**********-------------------
    bool _alive = true;
    public bool alive { get { return _alive; }  set { _alive = value; } }
    public bool invincible = false;
    private float knockbackDone = 0;
    private float invincibleDone = 0;
    private Vector2 knockbackVel;
   
     // ---------------------*******__SOUNDS__**********-------------------
    GameObject walkingSound;

    // ---------------------*******__MAP__**********-------------------
    public float roomTransDelay = 0.5f;
    private float roomTransDone = 0;
    Vector3 lastSafeLoc;
    int lastSafeFacing;
    public float gridMult { get { return inRm.gridMult; } }
    public bool isInRoom { get { return inRm.isInRoom; } }
    public Vector2 roomNum { get { return inRm.roomNum; } set { inRm.roomNum = value; } }
    public Vector2 posInRoom { get { return inRm.posInRoom; } set { inRm.posInRoom = value; } }
    Vector2 roomTransPos;
    bool changingDirect;

    // ---------------------*******__CODE__**********-------------------
    // ---------------------*******__CODE__**********-------------------
    private void Awake()
    {
        maxHealth = 8;

        S = this;
        IFM = this;

        sRend = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inRm = GetComponent<InRoom>();
        health = maxHealth;
        colld = GetComponent<Collider2D>();
        inventory = GetComponent<Inventory>();
        audioDray = GetComponent<DrayAudio>();
        walkingSound = transform.Find("Walking").gameObject;
        changingDirect = false;
    }
    private void Start()
    {
        lastSafeLoc = transform.position;
        lastSafeFacing = facing;
    }
   
    private void Update()
    {
        if (GameMenu.isGamePaused) return; //check pause settings
        if (isControlled) return;
        if (!alive) return;
        
        if(mode == eMode.move ) { walkingSound.SetActive(true); } else { walkingSound.SetActive(false); }

        if (invincible && Time.time > invincibleDone) invincible = false;
        sRend.color = invincible ? Color.red : Color.white;
        if(mode == eMode.knockback)
        {
            rig.linearVelocity = knockbackVel;
            if (Time.time < knockbackDone) return;
            mode = eMode.idle;
        }
        if(mode == eMode.roomTrans)
        {
            rig.linearVelocity = Vector3.zero;
            anim.speed = 0;
            posInRoom = roomTransPos;
            if (Time.time < roomTransDone) return;
            mode = eMode.idle;
        }
        if(mode == eMode.attack && Time.time >= timeAtkDone)
        {
            mode = eMode.idle;
            if(!Input.GetKeyDown(keyMainAttack))
            {
                changingDirect = false;
            }
        }
        if(mode == eMode.idle || mode == eMode.move)
        {
            dirHeld = -1;
            if (Input.GetKey(keyMainAttack) && mode == eMode.idle)
            {
                changingDirect = true;
                //changing direction without moving
                for (int i = 0; i < keys.Length; i++)
                {
                    if (Input.GetKey(keys[i]))
                    {
                        facing = i % 4;
                    }
                }
                
            }
            if(changingDirect && Input.GetKeyUp(keyMainAttack)) { changingDirect = false; }


            for (int i = 0; i < keys.Length; i++)
            {
                if (!changingDirect && Input.GetKey(keys[i])) dirHeld = i % 4;
            }
            if (dirHeld == -1)
            {
                mode = eMode.idle;
            }
            else
            {
                facing = dirHeld;
                mode = eMode.move;
                //audioDray.PlaySound(0);
            }
            if (Input.GetKeyDown(keyGadget) || Input.GetKeyDown(keyMainGadget))
            {
                if(inventory.currentGadget != null)
                {
                    if(inventory.currentGadget.GadgetUse(this, GadgetIsDone))
                    {
                        mode = eMode.gadget;
                        rig.linearVelocity = Vector2.zero;
                    }
                }
            }
            if((Input.GetKeyDown(keyAttack) || Input.GetKeyDown(keyMainAttack)) && Time.time >= timeAtkNext)
            {
                mode = eMode.attack;
                timeAtkDone = Time.time + attackDuration;
                timeAtkNext = Time.time + attackDelay;
                audioDray.PlaySound(0, true);
                
            }
            
            if (Input.GetKeyDown(keySwapGadget))
            {
                
                if (inventory.secondGadget == null)
                {
                    print("sec gad not exist");
                    //TO DO blink secondary icon for gadget
                }
                else
                {
                    inventory.SwapGadgets();
                }
            }
        }
        Vector2 vel = Vector2.zero;
        switch (mode)
        {
            case eMode.attack:
                anim.Play("Dray_Attack_" + facing);
                anim.speed = 0;
                break;
            case eMode.idle:
                anim.Play("Dray_Walk_" + facing);
                anim.speed = 0;
                break;
            case eMode.move:
                vel = dir[dirHeld];
                anim.Play("Dray_Walk_" + facing);
                anim.speed = 1;
                break;
            case eMode.gadget:
                anim.Play("Dray_Attack_" + facing);
                anim.speed = 0;
                break;
        }
        rig.linearVelocity = vel * speed * moveSpeedMod;
    }
    void LateUpdate()
    {
        if (isControlled) return;
        Vector2 gridPosIR = GetGridPosInRoom(0.25f);
        int doorNum;
        for (doorNum =0; doorNum < 4; doorNum++)
        {
            if(gridPosIR == InRoom.DOORS[doorNum])
            {
                break;
            }
        }
        if (doorNum > 3 || doorNum != facing) return;

        Vector2 rm = roomNum;
        switch (doorNum)
        {
            case 0: rm.x += 1;
                break;
            case 1: rm.y += 1;
                break;
            case 2:
                rm.x -= 1;
                break;
            case 3:
                rm.y -= 1;
                break;
        }
        if(0 <= rm.x && rm.x <= InRoom.MAX_RM_X)
        {
            if(0 <= rm.y && rm.y <= InRoom.MAX_RM_Y)
            {
                roomNum = rm;
                roomTransPos = InRoom.DOORS[(doorNum + 2) % 4];
                posInRoom = roomTransPos;
                mode = eMode.roomTrans;
                roomTransDone = Time.time + roomTransDelay;
                lastSafeLoc = transform.position;
                lastSafeFacing = facing;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (isControlled) return;
        if (invincible) return;
        DamageEffect dEf = coll.gameObject.GetComponent<DamageEffect>();
        if (dEf == null) return;

        //check for protection
        if ((armoredHeart - dEf.damage)>=0) 
        { 
            armoredHeart -= dEf.damage; 
        }
        else
        {
            health += (armoredHeart - dEf.damage);
            if (health <= 0) { alive = false; DrayEvents.DrayDeath.Invoke(); }
        }
        //play sound of hit with random pitch
        audioDray.PlaySound(1, rand: true, pLow:0.8f, pHigh:1.15f);

        DrayEvents.RefreshHealthUI.Invoke();
        invincible = true;
        invincibleDone = Time.time + invincibleDuration;
        if (dEf.knockback)
        {
            Vector2 delta = transform.position - coll.transform.position;
            if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
            {
                delta.x = (delta.x > 0) ? 1 : -1;
                delta.y = 0;
            }
            else
            {
                delta.x = 0;
                delta.y = (delta.y > 0) ? 1 : -1;
            }
            knockbackVel = delta * knockbackSpeed;
            rig.linearVelocity = knockbackVel;
            if(mode != eMode.gadget || inventory.currentGadget.GadgetCancel())
            {
                mode = eMode.knockback;
                knockbackDone = Time.time + knockbackDuration;
            }
        }
        changingDirect = false;
        // experimental 
        //if(Input.GetKey(keyMainAttack){ changingDirect = false;}

    }
    void OnTriggerEnter2D(Collider2D colld)
    {
        if (isControlled) return;
        PickUp pup = colld.GetComponent<PickUp>();
        if (pup == null) return;
        switch (pup.itemType)
        {
            case PickUp.eType.health:
                health = Mathf.Min(health + healthPickupAmount, maxHealth);
                DrayEvents.RefreshHealthUI.Invoke();
                break;
            case PickUp.eType.key:
                _numKeys++;
                break;
            case PickUp.eType.gadget:
                if (pup.gadgetLink != null)
                {
                    inventory.CurToSecGadget();
                    inventory.InstantiateAndSetGadget(pup.gadgetLink);
                }
                else { Debug.LogError("Link to gadget not exist "); }
                break;
            case PickUp.eType.healthContainer:
                maxHealth += 2;
                health = maxHealth;
                DrayEvents.RefreshHealthUI.Invoke();
                break;
            case PickUp.eType.heartArmor:
                armoredHeart += 1;
                DrayEvents.RefreshHealthUI.Invoke();
                break;
             
            default:
                Debug.LogError("No case for PickUp type " + pup.itemType);
                break;
        }
        audioDray.PlaySound(0);
        Destroy(pup.gameObject);   
    }
    
    public int GetFacing()
    {
        return facing;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public Vector2 GetGridPosInRoom(float mult = -1)
    {
        return inRm.GetGridPosInRoom(mult);
    }

    public int keyCount
    {
        get { return _numKeys; }
        set { _numKeys = value; }
    }
    public Vector2 pos
    {
        get { return (Vector2)transform.position; }
    }
    static public int HEALTH { get { return S._health; } }
    static public int ARMORED_HEALTH { get {  return S.armoredHeart; } }
    static public int MAX_HEALTH { get { return S.maxHealth; } }    
    static public int NUM_KEYS { get { return S._numKeys; } }
    public void ResetInRoom(int healthLoss = 0)
    {
        transform.position = lastSafeLoc;
        facing = lastSafeFacing;
        health -= healthLoss;
        invincible = true;
        invincibleDone = Time.time + invincibleDuration;
        DrayEvents.RefreshHealthUI.Invoke();
    }

    #region IGadget_Affordances
    
    public bool GadgetIsDone(IGadget gadget)
    {
        //if (gadget != inventory.currentGadget)
        //{
        //    Debug.LogError("A..... non-current Gadget called GadgetDone" + "\ncurrentGadget: " + inventory.currentGadget.name + "\tcalled by: " + gadget.name); 
            
        //}
        controlledBy = null;
        physicsEnabled = true;
        mode = eMode.idle;
        return true;
    }
    public IGadget controlledBy { get; set; }
    public bool isControlled
    {
        get { return (controlledBy != null); }
    }
    [SerializeField]
    private bool _physicsEnabled = true;
    public bool physicsEnabled
    {
        get { return _physicsEnabled; }
        set
        {
            if(_physicsEnabled!= value)
            {
                _physicsEnabled = value;
                colld.enabled = _physicsEnabled;
                rig.simulated = _physicsEnabled;
            }
        }
    }
    #endregion
    public static Vector2 GET_ROOM_NUM()
    {
        return S.inRm.roomNum;
    }
    
}
