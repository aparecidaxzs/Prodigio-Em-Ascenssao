using UnityEngine;
using UnityEngine.UI;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] Slider sfxVolumeSlider;
    // Lista de AudioSources para efeitos sonoros (adicione quantos precisar)
    public AudioSource[] sfxSources;  // Arraste os AudioSources aqui no Inspector

    public GameObject somOn;
    public GameObject somOff;

    void Start()
    {
        if (!PlayerPrefs.HasKey("sfxVolume"))
            PlayerPrefs.SetFloat("sfxVolume", 1);

        Load();
    }

    public void ChangeSFXVolume()
    {
        // Aplica o volume a todos os AudioSources de SFX
        foreach (AudioSource source in sfxSources)
        {
            source.volume = sfxVolumeSlider.value;
        }
        Save();

        if(sfxVolumeSlider.value <= 0)
        {
            somOn.SetActive(false);
            somOff.SetActive(true);
        }
        else
        {
            somOn.SetActive(true);
            somOff.SetActive(false);
        }
    }

    private void Load()
    {
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        foreach (AudioSource source in sfxSources)
        {
            source.volume = sfxVolumeSlider.value;
        }
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("sfxVolume", sfxVolumeSlider.value);
    }
}