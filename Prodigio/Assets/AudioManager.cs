using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources Separados")]
    public AudioSource playerSource;      // pulo, dano, soco, tiro
    public AudioSource footstepSource;    // caminhadas
    public AudioSource collectSource;     // coleta de moeda, itens
    public AudioSource sfxSource;         // efeitos gerais

    public AudioSource[] sfxSources;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayPlayer(AudioClip c)
    {
        playerSource.PlayOneShot(c);
    }

    public void PlayFootstep(AudioClip c)
    {
        footstepSource.PlayOneShot(c);
    }

    public void PlayCollect(AudioClip c)
    {
        collectSource.PlayOneShot(c);
    }

    public void PlaySFX(AudioClip c)
    {
        sfxSource.PlayOneShot(c);
    }

    public void SetSFXVolume(float value)
{
    foreach (AudioSource sfx in sfxSources)
    {
        sfx.volume = value;
    }

    PlayerPrefs.SetFloat("sfxVolume", value);
}

}
