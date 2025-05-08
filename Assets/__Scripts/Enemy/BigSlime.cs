using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class BigSlime : Enemy, IFaceMover
{
    private InRoom inRm;
    public float timeToMove = 0;
    public int facing = 0;
    public int speed = 6;
    public float movingTime = 0.4f;
    [SerializeField] AnimationCurve curve;
    protected override void Awake()
    {
        base.Awake();
        inRm = GetComponent<InRoom>();
       
    }
    void Start()
    {
        facing = 2;
        timeToMove = Time.time + 2f;
        //StartCoroutine(Waiting());
    }
    
    IEnumerator Waiting()
    {
        anim.Play("pre_jump");
        rigid.linearVelocity = Vector2.zero * speed;
        yield return new WaitForSeconds(0.3f);
        anim.Play("BigSlime");
        yield return new WaitForSeconds(3.5f);
        anim.Play("pre_jump");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(JumpMove());
    }
    IEnumerator JumpMove()
    {
        anim.Play("jump");
        rigid.linearVelocity = Vector2.left * speed;
        yield return new WaitForSeconds(movingTime);
        StartCoroutine(Waiting());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (knockback) return;
        //transform.position = new Vector3(transform.position.x, curve.Evaluate(Time.time), transform.position.y);
        print(curve.Evaluate(Time.time));
    }
    
    public Vector2 GetGridPosInRoom(float mult = -1)
    {
        return inRm.GetGridPosInRoom(mult);
    }
    public int GetFacing()
    {
        return facing % 4;
    }

    public float GetSpeed()
    {
        return speed;
    }
    public Vector2 roomNum { get { return inRm.roomNum; } set { inRm.roomNum = value; } }
    public Vector2 posInRoom { get { return inRm.posInRoom; } set { inRm.posInRoom = value; } }
    public bool moving { get { return (facing < 4); } }

    public float gridMult { get { return inRm.gridMult; } }

    public bool isInRoom { get { return inRm.isInRoom; } }
}
