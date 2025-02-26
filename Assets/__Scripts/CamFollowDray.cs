using UnityEngine;
[RequireComponent(typeof(InRoom))]
public class CamFollowDray : MonoBehaviour
{
    static public bool TRANSITIONING { get; private set; }
    public float transTime = 0.5f;
    private InRoom inRm;
    private Vector3 p0, p1;
    private float transStart;
    public bool showGizmoz = true;
    private void Awake()
    {
        inRm = GetComponent<InRoom>();
        TRANSITIONING = false;
    }
    private void Update()
    {
        if (TRANSITIONING)
        {
            float u = (Time.time - transStart) / transTime;
            if (u >= 1)
            {
                u = 1;
                TRANSITIONING = false;
            }
            transform.position = (1 - u) * p0 + u * p1;
        }
        else
        {
            if(Dray.IFM.roomNum != inRm.roomNum)
            {
                TransitionTo(Dray.IFM.roomNum);
            }
        }
    }
    void TransitionTo(Vector2 rm)
    {
        p0 = transform.position;
        inRm.roomNum = rm;
        p1 = transform.position + (Vector3.back * 10);
        transform.position = p0;
        transStart = Time.time;
        TRANSITIONING = true;
    }
    private void OnDrawGizmos()
    {
        for (int i = 1; i < 7; i++) 
        {
            Gizmos.DrawLine(new Vector3(i*16, 0, 0), new Vector3(i*16, 66, 0)); 
        }
        for (int i = 1; i < 7; i++)
        {
            Gizmos.DrawLine(new Vector3(0, i*11, 0), new Vector3(96, i*11, 0));
        }
    }
}
