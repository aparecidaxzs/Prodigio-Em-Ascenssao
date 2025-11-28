using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject gameOver; //painel de Game Over
    public GameObject vitoria; //painel de Vitória
    public static GameController instance; //instância estática para acessar de outros scripts

    public GameObject menu; //menu de pausa

    public GameObject barraVida;
    public GameObject score;
    public GameObject pause;

    

    // Start é chamado antes do primeiro frame
    void Start()
    {
        instance = this; //define a instância do GameController
    }

    // Update é chamado a cada frame
    void Update()
    {
        //não está sendo usado (poderia ficar vazio mesmo)
    }

    public void ShowGameOver()
    {
        gameOver.SetActive(true); //ativa a tela de Game Over
        barraVida.SetActive(false);
        pause.SetActive(false);
        //score.SetActive(false);

    }

    public void ShowVitoria()
    {
        vitoria.SetActive(true); //ativa a tela de Vitória
        barraVida.SetActive(false);
        pause.SetActive(false);
        //score.SetActive(false);
    }

    public void RestartGame(string lvlName)
    {
        SceneManager.LoadScene(lvlName); //reinicia a cena recebida pelo nome
    }

    public void PassarGame(string lvlName)
    {
        SceneManager.LoadScene(lvlName); //reinicia a cena recebida pelo nome
    }

    public void MenuPrincipal(string menuP)
    {
        SceneManager.LoadScene(menuP); //carrega a cena do menu principal
    }

    public void Config()
    {
        menu.SetActive(true); //ativa o menu de pausa
        Time.timeScale = 0f; //pausa o tempo do jogo
    }

    public void Resetar(string resetar)
    {
        SceneManager.LoadScene("Lvl.Lua"); //carrega a cena recebida (resetar jogo)
        Time.timeScale = 1f;
    }

    public void Play()
    {
        menu.SetActive(false); //fecha o menu de pausa
        Time.timeScale = 1f; //retoma o tempo do jogo
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void Voltar()
    {
        menu.SetActive(false); //fecha o menu de pausa
        //Time.timeScale = 1f; //retoma o tempo do jogo
    }
}