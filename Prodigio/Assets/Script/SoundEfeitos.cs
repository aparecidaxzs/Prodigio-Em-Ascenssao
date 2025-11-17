using UnityEngine;
using UnityEngine.UI;

public class SoundEfeitos : MonoBehaviour
{
[SerializeField] Slider volumeSlider;

public GameObject somOn;
public GameObject somOff;

public AudioSource tiro, caminhada, pulo, soco, moeda;

    void Start()
    {
       if (!PlayerPrefs.HasKey("musicVolume"))
            // ...salva o volume como 1 (volume máximo) apenas na primeira vez que o jogador abrir o jogo
            PlayerPrefs.SetFloat("musicVolume", 1);

        // Carrega o valor salvo e aplica ao slider
        Load();
    }

    public void ChangeVolume()
    {
        // Aplica o volume global do jogo com base no valor atual do slider
        AudioListener.volume = volumeSlider.value;
        // Salva esse valor no PlayerPrefs
       Save();

        if(volumeSlider.value <= 0)
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
        tiro.volume = volumeSlider.value;
        caminhada.volume = volumeSlider.value;
        pulo.volume = volumeSlider.value;
        soco.volume = volumeSlider.value;
        moeda.volume = volumeSlider.value;
    }

    // Salva o valor atual do slider no PlayerPrefs
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
