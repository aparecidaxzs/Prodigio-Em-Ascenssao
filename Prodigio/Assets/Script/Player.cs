using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movimentação")]
    public float velocidade;
    public float jumpForce;
    public bool isJump;
    public bool doubleJump;

    [Header("Vida do Player")]
    public int maxVida = 5;
    public int vidaAtual;
    private int amountt;

    [Header("Barra de Vida")]
    public GameObject barra;
    public GameObject barra0;
    public GameObject barra1;
    public GameObject barra2;
    public GameObject barra3;
    public GameObject barra4;

    [Header("Áudios do Player")]
    public AudioClip somPulo;
    
    public AudioClip somDano;

    private AudioSource audioPlayer;

    private float passoCooldown = 0.3f;
    private float proximoPasso = 0f;

    public static Player instance;
    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    private bool isTakingDamage = false;

    private ShieldAbility shield;


    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        audioPlayer = GetComponent<AudioSource>();
        vidaAtual = maxVida;
        instance = this;

        Atualizarbarra();

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        shield = GetComponent<ShieldAbility>();

    }

    void Update()
    {
        if (!isDead)
        {
            Move();
            Jump();
        }

        //GameOver();
    }

    // ===================== MOVIMENTO =====================
    void Move()
    {
        float input = Input.GetAxisRaw("Horizontal");
        rig.linearVelocity = new Vector2(input * velocidade, rig.linearVelocity.y);

        if (input != 0f)
        {
            transform.eulerAngles = new Vector3(0f, input > 0 ? 0f : 180f, 0f);
            anim.SetBool("Run", true);

            if (Time.time >= proximoPasso && !isJump)
            {
                
                proximoPasso = Time.time + passoCooldown;
            }
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    // ===================== PULO E DUPLO PULO =====================
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isJump)
            {
                rig.AddForce(new Vector3(0f, jumpForce), ForceMode2D.Impulse);
                doubleJump = true;
                isJump = true;
                anim.SetBool("Jump", true);
                audioPlayer.PlayOneShot(somPulo);
            }
            else if (doubleJump)
            {
                rig.AddForce(new Vector3(0f, jumpForce), ForceMode2D.Impulse);
                doubleJump = false;
                anim.SetBool("Jump", true);
                audioPlayer.PlayOneShot(somPulo);
            }
        }
    }

    // ===================== COLISÕES =====================
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chão") || collision.gameObject.CompareTag("Flutuante"))
        {
            isJump = false;
            doubleJump = false;
            anim.SetBool("Jump", true);
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

        if (collision.transform.CompareTag("Flutuante"))
    {
        transform.SetParent(collision.transform);
    }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chão") || collision.gameObject.CompareTag("Flutuante"))
        {
            isJump = true;
            
        }

        anim.SetBool("Jump", false);

        if (collision.transform.CompareTag("Flutuante"))
        {
        transform.SetParent(null);
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

    // ===================== VIDA / DANO =====================
    void GameOver()
{
    if (isDead) return;

    isDead = true;

    rig.linearVelocity = Vector2.zero;
    rig.bodyType = RigidbodyType2D.Kinematic;
    rig.gravityScale = 0f;
    GetComponent<Collider2D>().enabled = false;

    GameController.instance.ShowGameOver();

    Destroy(gameObject, 0.2f);
}


    public void AddVidaToda(int quantidade)
    {
        maxVida += quantidade;     // aumenta a vida máxima
        vidaAtual = maxVida;       // enche a vida do jogador
    }

   public void AddVida(int quantidade)
    {
    vidaAtual += quantidade;

    if (vidaAtual > maxVida)
        vidaAtual = maxVida;
        Atualizarbarra();
    }


    // ===================== CORRIGIDO =====================
    void Atualizarbarra()
    {
        if(vidaAtual == 4)
        {
            barra.SetActive(false);
            barra0.SetActive(true);
            barra1.SetActive(false);
            barra2.SetActive(false);
            barra3.SetActive(false);
            barra4.SetActive(false);
        }

        if(vidaAtual == 3)
        {
            barra.SetActive(false);
            barra0.SetActive(false);
            barra1.SetActive(true);
            barra2.SetActive(false);
            barra3.SetActive(false);
            barra4.SetActive(false);
        }

        if(vidaAtual == 2)
        {
            barra.SetActive(false);
            barra0.SetActive(false);
            barra1.SetActive(false);
            barra2.SetActive(true);
            barra3.SetActive(false);
            barra4.SetActive(false);
        }

        if(vidaAtual == 1)
        {
            barra.SetActive(false);
            barra0.SetActive(false);
            barra1.SetActive(false);
            barra2.SetActive(false);
            barra3.SetActive(true);
            barra4.SetActive(false);
        }

        if(vidaAtual == 0)
        {
            barra.SetActive(false);
            barra0.SetActive(false);
            barra1.SetActive(false);
            barra2.SetActive(false);
            barra3.SetActive(false);
            barra4.SetActive(true);
            GameOver();
        }

        if(vidaAtual == 5)
        {
            barra0.SetActive(false);
            barra1.SetActive(false);
            barra2.SetActive(false);
            barra3.SetActive(false);
            barra4.SetActive(false);
            barra.SetActive(true);
        }
    }

    IEnumerator BlinkEffect(float blinkSpeed, int times)
    {
        isTakingDamage = true;

        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(blinkSpeed);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(blinkSpeed);
        }

        isTakingDamage = false;
    }

public void TomarDano(int quantidade)
{
    if (isDead) return;

    // Primeiro verifica o escudo — se estiver ativo, não toma dano
    if (shield != null && shield.GetShieldState())
        return;

    // Agora aplica o dano
    vidaAtual = Mathf.Clamp(vidaAtual - quantidade, 0, maxVida);

    Atualizarbarra();

    audioPlayer.PlayOneShot(somDano);

    StartCoroutine(BlinkEffect(0.1f, 1));

    if (vidaAtual <= 0)
    {
        GameOver();
    }
}


}
