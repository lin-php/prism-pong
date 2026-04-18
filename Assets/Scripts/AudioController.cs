using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public static AudioController Instance;

    public void SoundOnHit(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
        
    }
}
