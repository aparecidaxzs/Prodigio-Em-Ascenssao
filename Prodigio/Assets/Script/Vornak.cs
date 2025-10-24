using UnityEngine;
using System.Collections;
using UnityEngine.Scripting.APIUpdating;
using System.Collections.ObjectModel;
using UnityEngine.UI;
using System;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;

public class Vornak : MonoBehaviour
{
    public int vidaMax = 15;
    int vidaAtual;
    private bool morto = false;

    [Header("Movimento e Ataque")]
    public float velocidade = 2f;
    public float distanciaAtaque = 2f;
    public float distanciaPoder = 5f;
    public float distanciaDesefa = 2.5f;
    public float forcaPulo = 6f;
    public Transform player;
    public Transform ataquePeito;
    public GameObject prefabPoderPeito;
    public float tempoAtaques = 2f;

    [Header("Modo Raiva")]
    public bool modoRaiva = false;
    public float multVelocidade = 1.5f;
    [Range(0f, 1f)]
    public float danoRaiva = 0.3f;

    [Header("Defesa Reativa")]
    public bool defendendo = false;
    public float tempoDefesa = 1.2f; //duração da defesa
    public float chanceDefesa = 0.4f; // chance de defender quando o player se aproxima

    private float proximoAtaque;
    private Rigidbody2D rb;
    private Animator anim;
    private bool noChao = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is creat
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        vidaAtual = vidaMax;
    
    }

    // Update is called once per frame
    void Update()
    {
        if (morto) return;
        if (player == null) return;

        float distancia = Vector2.Distance(transform.position, player.position);
        Mover(distancia);

        if (Time.time >= proximoAtaque && distancia <= distanciaAtaque)
        {
            EscolherAtaque(distancia);
        }

        if (distancia <= distanciaDesefa && !defendendo && Random.value < chanceDefesa)
        {
            Defender();
        }
    }
    
    void Mover(float distancia)
    {
        if (player.position.x > transform.position.x)

            transform.eulerAngles = new Vector3(0, 0, 0);

        else
            transform.eulerAngles = new Vector3(0, 100, 0);

        if (distancia > distanciaAtaque)
        {
            float move = velocidade * (modoRaiva ? multVelocidade : 1f);
            transform.position = Vector2.MoveTowards(transform.position, player.position, move * Time.deltaTime);
            //aq coloca a animação Andando
            //anim.SetBool("andando", true);
        }

        else
        {
            //anim.SetBool("andando", false);
        }

        void EscolherAtaque(float distancia)
        {
            if (defendendo) return;

            proximoAtaque = Time.time + tempoAtaques;
            int tipoAtaque = Random.Range(0, 3);

            if(distancia < distanciaAtaque)
            {
                if (tipoAtaque == 0)
                {
                    //anim.SetTrigger("soco");
                }
                
            }
        }
        
    }
}
