using UnityEngine;

public class MagicSword : MonoBehaviour
{
    Rigidbody2D rigid;
    int speed = 8;
    Transform parent;
    Vector3 posOnStart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        parent = transform.parent;
        posOnStart = new Vector3(0.75f, 0,0);
    }

    // Update is called once per frame
    void Update()
    {
        rigid.linearVelocity = transform.right * speed;
        transform.parent = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //gameObject.SetActive(false);
        transform.parent = parent;
        transform.localPosition = posOnStart;
        parent.GetComponent<SwordController>().MagSRet();
    }
}
