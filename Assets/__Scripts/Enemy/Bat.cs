using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Bat : Enemy, IFaceMover
{
    InRoom inRm;
    int facing = 0;
    int speed = 4;
    float timeStart;
    float timeEnd = -1;
    Vector2 p0, p1;
    Vector2 tVec;
    bool callCor = false;
    protected override void Awake()
    {
        base.Awake();
        inRm = GetComponent<InRoom>();
    }
    private void Start()
    {
        CalcPos();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (! (Dray.GET_ROOM_NUM() == inRm.roomNum)) return;
        base.Update();
        if (knockback) return;
        if (Time.time < timeEnd)
        {
            rigid.linearVelocity = tVec * speed;
            anim.speed = 1;
        }
        else
        {
            rigid.linearVelocity = Vector2.zero * speed;
            anim.speed = 0.5f;
            if (!callCor) StartCoroutine(WaitToMove());
        }
       
        
    }
    void CalcPos()
    {
        p0 = (Vector2)transform.position;
        p1 = new Vector2(
            (inRm.roomNum.x * InRoom.ROOM_W) + InRoom.WALL_T, 
            (inRm.roomNum.y * InRoom.ROOM_H) + InRoom.WALL_T);
        p1.x += Random.Range(0, (InRoom.ROOM_W - InRoom.WALL_T*2));
        p1.y += Random.Range(0, (InRoom.ROOM_H - InRoom.WALL_T*2));
        tVec = p1 - p0;
        
        float tMag = tVec.magnitude;
        tVec.Normalize();
        //print("magnit " + tMag);
        //print("sec veloc = " + Mathf.FloorToInt(tMag / speed));
        //print("Eime End is " + timeEnd);
        //timeEnd = Time.time + (Mathf.FloorToInt(tMag / speed));
        timeEnd = Time.time + (tMag / speed);
        callCor = false;
    }
    IEnumerator WaitToMove()
    {
        callCor = true;
        yield return new WaitForSeconds(3f);
        CalcPos();

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
