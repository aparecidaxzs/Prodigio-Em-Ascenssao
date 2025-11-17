using UnityEngine;
using UnityEngine.UI;

public class SoundBackground : MonoBehaviour
{
    // Referência ao Slider no UI que controla o volume
    [SerializeField] Slider volumeSlider;

    public GameObject somOn;
    public GameObject somOff;

    public AudioSource mscFundo;

    // Start é chamado antes do primeiro frame
    void Start()
    {
        // Se NÃO existir um valor salvo para o volume...
        if (!PlayerPrefs.HasKey("musicVolume"))
            // ...salva o volume como 1 (volume máximo) apenas na primeira vez que o jogador abrir o jogo
            PlayerPrefs.SetFloat("musicVolume", 1);

        // Carrega o valor salvo e aplica ao slider
        Load();
    }

    // Método chamado quando o jogador mexe no slider
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

    // Carrega o volume salvo e coloca no slider
    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        mscFundo.volume = volumeSlider.value;
    }

    // Salva o valor atual do slider no PlayerPrefs
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
