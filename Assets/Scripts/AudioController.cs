using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip menuTheme;
    [SerializeField] private AudioClip gameSong;
    [SerializeField] private AudioClip highScoreTheme;

    public static AudioController Instance;

    public void SoundOnHit(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusic(AudioClip clip, bool loop, float volume)
    {
        musicSource.loop = loop;
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void PlayMenuTheme()
    {
        PlayMusic(menuTheme, true, 0.04f);
    }

    public void PlayGameTheme()
    {
        PlayMusic(gameSong, true, 0.05f);
    }

    public void PlayHighScoreTheme()
    {
        PlayMusic(highScoreTheme, false, 0.03f);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            float volume = PlayerPrefs.GetFloat("Volume", 0.7f);
            AudioListener.volume = volume;
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
