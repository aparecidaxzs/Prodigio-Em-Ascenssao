using UnityEngine;
using System.Collections;
using UnityEngine.Scripting.APIUpdating;
using System.Collections.ObjectModel;
using UnityEngine.UI;
using System;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;




public class Player : MonoBehaviour
{
    public float velocidade; //velocidade de movimento do jogador
    public float jumpForce; //força do pulo
    public bool isJump; //checa se o jogador está no ar
    public bool doubleJump; //checa se o jogador pode usar o segundo pulo

    private Rigidbody2D rig; //referência ao rigidbody do jogador

    private GameObject coinColetar; //referência para guardar a moeda coletada (ainda não está em uso)

    //public Image barrinhaFrente; //barra de vida que vai diminuir mais devagar 
    //public Image barrinhaTras; //barra de vida que diminui mais rapido


    public int maxVida = 5; //maxima de vida do jogador 
    int vidaAtual; //vai atualizar a barra de vida 
    public int amountt;




    public GameObject barra;
    public GameObject barra0;
    public GameObject barra1;
    public GameObject barra2;
    public GameObject barra3;
    public GameObject barra4;

    public static Player instance;


    public Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        vidaAtual = maxVida; //quando dá start, a vida atual é a quatidade do maximo de  vida que o jogador tem 
        instance = this;
        BarradeVida(vidaAtual + 1);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(); //chamando a função de movimento
        Jump(); //chamando a função de pulo
        GameOver(); //checando se o jogador perdeu

    }

    void Move()
    {
        float input = Input.GetAxisRaw("Horizontal"); //pegando input do jogador (esquerda/direita)

        rig.linearVelocity = new Vector2(input * velocidade, rig.linearVelocity.y); //movimentando no eixo X, mantendo Y

        if (input > 0f)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);

        }

        else if (input < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f); //vira para a esquerda

        }
        anim.SetBool("idle", false);

    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isJump == false) //se não está no ar
            {
                rig.AddForce(new Vector3(0f, jumpForce), ForceMode2D.Impulse); //aplica força para pular
                doubleJump = true; //habilita o segundo pulo
                isJump = true; //marca que está no ar

            }

            else
            {
                if (doubleJump) //se o segundo pulo está disponível
                {
                    rig.AddForce(new Vector3(0f, jumpForce), ForceMode2D.Impulse); //pula de novo
                    doubleJump = false; //consome o segundo pulo
                }
            }
            anim.SetBool("idle", false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chão") || collision.gameObject.CompareTag("Flutuante"))
        {
            isJump = false; //quando encosta no chão, pode pular de novo
            doubleJump = false; //reseta o segundo pulo

        }

        if (collision.gameObject.tag == "GameOver") //quando encosta no objeto de "GameOver"
        {
            GameController.instance.ShowGameOver(); //chama a tela de game over
            Destroy(gameObject); //destroi o jogador
        }

        if (collision.gameObject.tag == "Vitoria") //quando encosta no objeto de "Vitória"
        {
            GameController.instance.ShowVitoria(); //chama a tela de vitória
            Destroy(gameObject); //destroi o jogador
        }

        

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chão") || collision.gameObject.CompareTag("Flutuante"))
        {
            isJump = true; //se saiu do chão, marca que está no ar
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnergiaRara")
        {
            vidaAtual = maxVida;
            Atualizarbarra();
        }
    }


    void GameOver()
    {
        if (vidaAtual == 0) //se a vida chegar em zero
        {
            GameController.instance.ShowGameOver(); //mostra a tela de game over
            Destroy(gameObject); //destroi o jogador
            //Destroy(barrinhaTras); //destroi a barra de vida
        }
    }

    public void AddVidaToda()
    {
        vidaAtual = 5;
        Atualizarbarra();
    }

    public void AddVida(int quantidade)
    {
        maxVida += quantidade;
        vidaAtual = Mathf.Clamp(vidaAtual + quantidade, 0, maxVida); // soma só 1
        Debug.Log(maxVida);
        Atualizarbarra();
    }

    public void BarradeVida(int amount)
    {
        if (amount < 0) //se o valor recebido for negativo (dano)
        {
            vidaAtual = Mathf.Clamp(vidaAtual + amount, 0, maxVida); //diminui a vida, sem passar de 0 ou do máximo
            if (vidaAtual == 4)
            {
                barra0.SetActive(true);
                barra.SetActive(false);
            }

            if (vidaAtual == 3)
            {
                barra1.SetActive(true);
                barra0.SetActive(false);
            }

            if (vidaAtual == 2)
            {
                barra2.SetActive(true);
                barra1.SetActive(false);
            }

            if (vidaAtual == 1)
            {
                barra3.SetActive(true);
                barra2.SetActive(false);
            }

            if (vidaAtual == 0)
            {
                barra4.SetActive(true);
                barra3.SetActive(false);
            }
        }
    }

    void Atualizarbarra()
    {
        // Aplica dano ou cura
    vidaAtual = Mathf.Clamp(vidaAtual + amountt, 0, maxVida);

        // Atualiza visual conforme o valor da vida
        if (vidaAtual == 5)
        {
            barra.SetActive(true);
            barra0.SetActive(false);
            barra1.SetActive(false);
            barra2.SetActive(false);
            barra3.SetActive(false);
            barra4.SetActive(false);
        }
        else if (vidaAtual == 4)
        {
            barra0.SetActive(true);
            barra.SetActive(false);
            barra1.SetActive(false);
            barra2.SetActive(false);
            barra3.SetActive(false);
            barra4.SetActive(false);
        }
        else if (vidaAtual == 3)
        {
            barra1.SetActive(true);
            barra.SetActive(false);
            barra0.SetActive(false);
            barra2.SetActive(false);
            barra3.SetActive(false);
            barra4.SetActive(false);
        }
        else if (vidaAtual == 2)
        {
            barra2.SetActive(true);
            barra.SetActive(false);
            barra0.SetActive(false);
            barra1.SetActive(false);
            barra3.SetActive(false);
            barra4.SetActive(false);

        }
        else if (vidaAtual == 1)
        {
            barra3.SetActive(true);
            barra.SetActive(false);
            barra0.SetActive(false);
            barra1.SetActive(false);
            barra2.SetActive(false);
            barra4.SetActive(false);
        }
        else if (vidaAtual == 0)
        {
            barra4.SetActive(true);
            barra.SetActive(false);
            barra0.SetActive(false);
            barra1.SetActive(false);
            barra2.SetActive(false);
            barra3.SetActive(false);
        }
}
    }

