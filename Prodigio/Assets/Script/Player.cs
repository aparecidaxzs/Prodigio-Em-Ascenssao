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
[Header("Movimentação")]
public float velocidade; //velocidade de movimento do jogador
public float jumpForce; //força do pulo
public bool isJump; //checa se o jogador está no ar
public bool doubleJump; //checa se o jogador pode usar o segundo pulo

[Header("Vida do Player")]
public int maxVida = 5; //maxima de vida do jogador
int vidaAtual; //vai atualizar a barra de vida
private int amountt;

[Header("Barra de Vida")]
public GameObject barra;
public GameObject barra0;
public GameObject barra1;
public GameObject barra2;
public GameObject barra3;
public GameObject barra4;

public static Player instance;
private Animator anim;
private Rigidbody2D rig; 
private SpriteRenderer spriteRenderer; 
private bool isTakingDamage = false;
private bool isDead = false;

private GameObject coinColetar;

// Start
void Start()
{
    rig = GetComponent<Rigidbody2D>();
    vidaAtual = maxVida;
    instance = this;
    BarradeVida(vidaAtual + 1);
    anim = GetComponent<Animator>();
    spriteRenderer = GetComponent<SpriteRenderer>();
}

// Update
void Update()
{
    if (!isDead)
    {
        Move();
        Jump();
    }
    GameOver();
}

void Move()
{
    float input = Input.GetAxisRaw("Horizontal");
    rig.linearVelocity = new Vector2(input * velocidade, rig.linearVelocity.y);

    if (input > 0f)
    {
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        anim.SetBool("Run", true);
    }
    else if (input < 0f)
    {
        transform.eulerAngles = new Vector3(0f, 180f, 0f);
        anim.SetBool("Run", true);
    }
    if (input == 0f)
    {
        anim.SetBool("Run", false);
    }
}

void Jump()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        if (!isJump)
        {
            rig.AddForce(new Vector3(0f, jumpForce), ForceMode2D.Impulse);
            doubleJump = true;
            isJump = true;
            anim.SetBool("Jump", true);
        }
        else
        {
            if (doubleJump)
            {
                rig.AddForce(new Vector3(0f, jumpForce), ForceMode2D.Impulse);
                doubleJump = false;
                anim.SetBool("Jump", true);
            }
        }
    }
}

void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Chão") || collision.gameObject.CompareTag("Flutuante"))
    {
        isJump = false;
        doubleJump = false;
        anim.SetBool("Jump", false);
    }

    if (collision.gameObject.tag == "GameOver")
    {
        GameController.instance.ShowGameOver();
        Destroy(gameObject);
    }

    if (collision.gameObject.tag == "Vitoria")
    {
        GameController.instance.ShowVitoria();
        Destroy(gameObject);
    }
}

void OnCollisionExit2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Chão") || collision.gameObject.CompareTag("Flutuante"))
    {
        isJump = true;
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
    if (vidaAtual == 0 && !isDead)
    {
        isDead = true;

        rig.linearVelocity = Vector2.zero;
        rig.bodyType = RigidbodyType2D.Kinematic;
        rig.gravityScale = 0f;
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(DieBlink());
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
    vidaAtual = Mathf.Clamp(vidaAtual + quantidade, 0, maxVida);
    Atualizarbarra();
}

public void BarradeVida(int amount)
{
    if (amount < 0)
    {
        vidaAtual = Mathf.Clamp(vidaAtual + amount, 0, maxVida);

        if (!isDead && !isTakingDamage)
        {
            StartCoroutine(BlinkEffect(0.1f, 1));
        }

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
    vidaAtual = Mathf.Clamp(vidaAtual + amountt, 0, maxVida);

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

IEnumerator BlinkEffect(float blinkSpeed, int times)
{
    isTakingDamage = true;
    for (int i = 0; i < times; i++)
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(blinkSpeed);
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(blinkSpeed);
    }
    isTakingDamage = false;
}

IEnumerator DieBlink()
{
    for (int i = 0; i < 3; i++)
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
    }
    GameController.instance.ShowGameOver();
    Destroy(gameObject);
}
}
