using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{

    public GameObject gameOver;
    public GameObject vitoria;
    public static GameController instance;

    public GameObject menu;
    /*
    public GameObject reset;
    public GameObject play;
    public GameObject quit;*/
    
    //public GameObject config;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }

    public void ShowVitoria()
    {
        vitoria.SetActive(true);
    }

    public void RestartGame(string lvlName)
    {
        SceneManager.LoadScene(lvlName);
    }

    public void MenuPrincipal(string menuP)
    {
        SceneManager.LoadScene(menuP);
    }

    public void Config()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resetar(string resetar)
    {
        SceneManager.LoadScene(resetar);
    }

    public void Play()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        //so funciona dentro da unity
        UnityEditor.EditorApplication.ExitPlaymode();
    }

   
}
