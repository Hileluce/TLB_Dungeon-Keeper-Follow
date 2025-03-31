using UnityEngine;
using System.Collections.Generic;
public class Enemy : MonoBehaviour, ISwappable
{
    [SerializeField] protected AudioClip[] sounds;
    protected static Vector2[] directions = new Vector2[] { Vector2.right, Vector2.up, Vector2.left, Vector2.down, Vector2.zero };
    public float maxHealth = 1;
    public float health;
    protected Animator anim;
    protected Rigidbody2D rigid;
    protected SpriteRenderer sRend;
    protected AudioSource aSource;

    public float knockbackSpeed = 10;
    public float knockbackDuration = 0.25f;
    public float invincibleDuration = 0.25f;
    public bool invincible = false;
    public bool knockback = false;
    private float invincibleDone = 0;
    float knockbackDone = 0;
    Vector2 knockbackVel;

    //public GameObject guaranteedDrop = null;
    public List<GameObject> randomItems;

    [SerializeField] GameObject _guaranteedDrop = null;
    protected virtual void Awake()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        sRend = GetComponent<SpriteRenderer>();
        aSource = GetComponent<AudioSource>();
    }
    protected virtual void Update()
    {
        if (invincible && Time.time > invincibleDone) invincible = false;
        sRend.color = invincible ? Color.red : Color.white;
        if (knockback)
        {
            rigid.linearVelocity = knockbackVel;
            if (Time.time < knockbackDone) return;
        }
        anim.speed = 1;
        knockback = false;
    }
    void OnTriggerEnter2D(Collider2D colld)
    {
        if (invincible) return;
        DamageEffect dEf = colld.gameObject.GetComponent<DamageEffect>();
        if (dEf == null) return;
        health -= dEf.damage;
        if (health <= 0) { Die(); return; }

       //Play sound of sword hit
        PlaySound(1);
        
        invincible = true;
        invincibleDone = Time.time + invincibleDuration;
        if (dEf.knockback)
        {
            Vector2 delta;
            IFaceMover iFM = colld.GetComponentInParent<IFaceMover>();
            if (iFM != null)
            {
                delta = directions[iFM.GetFacing()];
            }
            else
            {
                delta = transform.position - colld.transform.position;
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
            }
            knockbackVel = delta * knockbackSpeed;
            rigid.linearVelocity = knockbackVel;
            knockback = true;
            knockbackDone = Time.time + knockbackDuration;
            anim.speed = 0;
        }
    }
    void Die()
    {
        GameObject go;
        if(guaranteedDrop != null)
        {
            go = Instantiate<GameObject>(guaranteedDrop);
            go.transform.position = transform.position;
            AudioSource.PlayClipAtPoint(AudioManager.GET_GLOBAL_SOUND(0), transform.position, 0.9f);
        }
        else if ( randomItems.Count > 0)
        {
            int n = Random.Range(0, randomItems.Count);
            GameObject prefab = randomItems[n];
            if(prefab != null)
            {
                go = Instantiate<GameObject>(prefab);
                go.transform.position = transform.position;
            }
        }
        
        AudioSource.PlayClipAtPoint(sounds[0], transform.position);
        Destroy(gameObject);
    }
    public GameObject guaranteedDrop
    {
        get { return _guaranteedDrop; }
        set { _guaranteedDrop = value; }
    }
    public int tileNum { get; private set; }
    public virtual void Init(int fromTileNum, int tileX, int tileY)
    {
        tileNum = fromTileNum;
        transform.position = new Vector3(tileX, tileY, 0) + MapInfo.OFFSET;
    }
    void PlaySound(int i)
    {
        if (sounds[i] == null) { Debug.LogError("SOUNDS " + i + " DIDNT LINKED"); }
        aSource.clip = sounds[i];
        aSource.Play();
    }
}
