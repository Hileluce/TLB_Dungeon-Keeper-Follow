using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Grappler : MonoBehaviour, IGadget
{
    [SerializeField] AudioClip[] sounds;

    public enum eMode { gIdle, gOut, gRetract, gPull }
    public float grappleSpd = 10;
    public float maxLength = 7.25f;
    public float minLength = 0.375f;
    private eMode _mode = eMode.gIdle;
    [SerializeField] Sprite _gadgetSprite;
    public Sprite gadgetSprite { get { return _gadgetSprite; } }
    public eMode mode
    {
        get { return _mode; }
        private set { _mode = value; }
    }
    private LineRenderer line;
    Rigidbody2D rigid;
    Collider2D colld;
    AudioSource aSource;
    Vector3 p0, p1;
    int facing;
    Dray dray;
    System.Func<IGadget, bool> gadgetDoneCallback;

    public int unsafeTileHealthPenalty = 2;

    
    Vector2[] directions = new Vector2[]
    {
        Vector2.right, Vector2.up, Vector2.left, Vector2.down };
    Vector3[] dirV3s = new Vector3[] { Vector3.right, Vector3.up, Vector3.left, Vector3.down };
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        colld = GetComponent<Collider2D>();
        aSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        
        gameObject.SetActive(false);
    }
    void SetGrappleMode(eMode newMode)
    {
        switch (newMode)
        {
            case eMode.gIdle:
                transform.DetachChildren();
                gameObject.SetActive(false);
                if(dray != null && dray.controlledBy == this as IGadget)
                {
                    dray.controlledBy = null;
                    dray.physicsEnabled = true;
                }
                break;
            case eMode.gOut:
                gameObject.SetActive(true);
                rigid.linearVelocity = directions[facing] * grappleSpd;
                break;
            case eMode.gRetract:
                rigid.linearVelocity = -directions[facing] * (grappleSpd * 2);
                break;
            case eMode.gPull:
                p1 = transform.position;
                rigid.linearVelocity = Vector2.zero;
                dray.controlledBy = this;
                dray.physicsEnabled = false;
                break;
        }
        mode = newMode;
    }
    void FixedUpdate()
    {
        p1 = transform.position;
        line.SetPosition(1, p1);
        switch (mode)
        {
            case eMode.gOut:
                if ((p1 - p0).magnitude >= maxLength)
                {
                    SetGrappleMode(eMode.gRetract);
                }
                break;
            case eMode.gRetract:
                if (Vector3.Dot((p1 - p0), dirV3s[facing]) < 0) GrappleDone();
                break;
            case eMode.gPull:
                if ((p1 - p0).magnitude > minLength)
                {
                    p0 += dirV3s[facing] * grappleSpd * Time.fixedDeltaTime;
                    dray.transform.position = p0;
                    line.SetPosition(0, p0);
                    transform.position = p1;
                }
                else
                {
                    p0 = p1 - (dirV3s[facing] * minLength);
                    dray.transform.position = p0;
                    Vector2 checkPos = (Vector2)p0 + new Vector2(0, -0.25f);
                    if (MapInfo.UNSAFE_TILE_AT_VECTOR2(checkPos))
                    {
                        dray.ResetInRoom(unsafeTileHealthPenalty);
                    }
                    GrappleDone();
                }
                break;
        }
    }
    void LateUpdate()
    {
        p1 = transform.position;
        line.SetPosition(1, p1);
    }
    void OnTriggerEnter2D(Collider2D colld)
    {
        string otherLayer = LayerMask.LayerToName(colld.gameObject.layer);
        switch (otherLayer)
        {
            case "Items":
                PickUp pup = colld.GetComponent<PickUp>();
                if (pup == null) return;
                pup.transform.SetParent(transform);
                pup.transform.localPosition = Vector3.zero;
                SetGrappleMode(eMode.gRetract);
                PlaySound(3);
                break;
            case "Enemies":
                Enemy e = colld.GetComponent<Enemy>();
                if (e != null) { PlaySound(2); SetGrappleMode(eMode.gRetract);  }
                break;
            case "GrapTiles":
                //SetGrappleMode(eMode.gRetract);
                PlaySound(1);
                SetGrappleMode(eMode.gPull);
                
                break;
            default:
                SetGrappleMode(eMode.gRetract);
                break;
        }
    }
    void GrappleDone()
    {
        SetGrappleMode(eMode.gIdle);
        gadgetDoneCallback(this);
    }
    #region IGadget_Implementation
    public eGadgetType type = eGadgetType.grappler;
    public bool GadgetUse(Dray tDray, System.Func<IGadget, bool> tCallback)
    {
        if (mode != eMode.gIdle) return false;
        dray = tDray;
        gadgetDoneCallback = tCallback;
        transform.localPosition = Vector3.zero;
        facing = dray.GetFacing();
        p0 = dray.transform.position;
        p1 = p0 + (dirV3s[facing] * minLength);
        gameObject.transform.position = p1;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 90 * facing);
        line.positionCount = 2;
        line.SetPosition(0, p0);
        line.SetPosition(1, p1);
        SetGrappleMode(eMode.gOut);
        PlaySound(0);
        return true;
    }
    public bool GadgetCancel()
    {
        if (mode == eMode.gOut) return false;
        SetGrappleMode(eMode.gIdle);
        gameObject.SetActive(false);
        return true;
    }

    #endregion
    void PlaySound(int i)
    {
        aSource.clip = sounds[i];
        aSource.Play();
    }
}
