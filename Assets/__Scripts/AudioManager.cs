using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager S;
    AudioSource aS => GetComponent<AudioSource>();
    [SerializeField] AudioClip[] globalSounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (S == null) S = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            print("Q");
            muteSound = !muteSound;
        }
    }

    public bool muteSound
    {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }    
    }
    public float volumeSound
    {
        get { return AudioListener.volume; }
        set { AudioListener.volume = value; }
    }
    void PlaySoundEff(GameObject go)
    {

    }
    public static void PlaySoundEffect(GameObject go)
    {
        S.PlaySoundEff(go);
    }
    public static AudioClip GET_GLOBAL_SOUND(int i)
    {
        return S.globalSounds[i];
    }
}
