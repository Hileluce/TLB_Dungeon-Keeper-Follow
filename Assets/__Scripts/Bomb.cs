using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Bomb : MonoBehaviour, IGadget
{
    [SerializeField] AudioClip[] sounds;

    AudioSource aSource;
    public float animationTime = 0.15f;
    public float timer = 1f;
    public float boomAnim = 0.5f;
    public GameObject bombPref;
    public GameObject boomPref;
    [SerializeField] Sprite _gadgetSprite;
    public Sprite gadgetSprite { get { return _gadgetSprite; } }
    System.Func<IGadget, bool> gadgetDoneCallback;
    GameObject bomb;
    Dray dray;
    Vector3 posOfExplo;
    bool bombPlaced = false;
    public bool GadgetCancel()
    {
        //Debug.LogError("Bomb is cancel");
        return false;
    }
    private void Start()
    {
        aSource = GetComponent<AudioSource>();
        gameObject.SetActive(false);
    }
    public bool GadgetUse(Dray tDray, Func<IGadget, bool> tDoneCallback)
    {
        gameObject.SetActive(true);
        gadgetDoneCallback = tDoneCallback;
        dray = tDray;
        posOfExplo = dray.transform.position;
        if (!bombPlaced) { StartCoroutine(MakeBomb()); }
        else { StartCoroutine(WaitAnim()); }

        return true;
    }
    IEnumerator MakeBomb()
    {
        bombPlaced = true;
        bomb = Instantiate<GameObject>(bombPref);
        bomb.transform.position = posOfExplo;
        PlaySound(0);
        yield return new WaitForSeconds(animationTime);
        gadgetDoneCallback(this);
        PlaySound(1);
        yield return new WaitForSeconds(timer - animationTime);
        posOfExplo = bomb.transform.position;
        Destroy(bomb);
        bomb = Instantiate<GameObject>(boomPref);
        bomb.transform.position = posOfExplo;
        PlaySound(2);
        yield return new WaitForSeconds(boomAnim);
        Destroy(bomb);
        bombPlaced = false;
        aSource.Stop();
    }
    void PlaySound(int i)
    {
        aSource.clip = sounds[i];
        aSource.Play();
    }
    IEnumerator WaitAnim()
    {
        yield return new WaitForSeconds(animationTime);
        gadgetDoneCallback(this);
    }
}
