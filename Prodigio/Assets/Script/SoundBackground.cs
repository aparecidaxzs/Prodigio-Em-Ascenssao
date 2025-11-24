using UnityEngine;
using UnityEngine.UI;

public class SoundBackground : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    public GameObject somOn;
    public GameObject somOff;
    public AudioSource mscFundo;

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
            PlayerPrefs.SetFloat("musicVolume", 1);

        Load();
    }

    public void ChangeVolume()
    {
        // Aplica o volume apenas ao AudioSource do background
        mscFundo.volume = volumeSlider.value;

        // Salva o valor
        Save();

        // Lógica para ícones on/off
        if (volumeSlider.value <= 0)
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
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        mscFundo.volume = volumeSlider.value;
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
