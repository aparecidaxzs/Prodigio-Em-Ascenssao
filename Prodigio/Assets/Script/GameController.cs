using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject gameOver;
    public static GameController instance;
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

}
