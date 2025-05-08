using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class DrayAudio : MonoBehaviour
{
    [SerializeField] AudioStock[] randomClips;
    [SerializeField]
    AudioClip[] clips;
    AudioSource aSource;
    private void Awake()
    {
            aSource = GetComponent<AudioSource>();
        
    }
    public void PlaySound(int i, bool rand = false, float pLow = 0.85f, float pHigh = 1.15f)
    {
        aSource.pitch = Random.Range(pLow, pHigh);
        AudioClip clipForPlay = rand ? randomClips[i].clipGroup[Random.Range(0, randomClips[i].clipGroup.Length)] : clips[i];
        if (rand) aSource.PlayOneShot(clipForPlay);
        else { aSource.clip = clipForPlay; aSource.Play(); }
                
    }
    
    
    [System.Serializable] public class AudioStock
    {
        public AudioClip[] clipGroup;
    }
}
