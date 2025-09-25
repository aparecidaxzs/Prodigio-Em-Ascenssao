using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SairDaLogo : MonoBehaviour
{
    public float duracao = 4f;
    float tempo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tempo += Time.deltaTime;

        if (tempo >= duracao)
        {
            SceneManager.LoadScene("Menu_Principal");
        }
    }
}
