using UnityEngine;
using System.Collections;
using UnityEngine.Scripting.APIUpdating;
using System.Collections.ObjectModel;
using UnityEngine.UI;



public class Player : MonoBehaviour
{
    public float velocidade;
    public float jumpForce;
    public bool isJump;
    public bool doubleJump;

    private Rigidbody2D rig;

    private GameObject coinColetar;

    
    public Image barrinhaFrente; //barra de vida que vai diminuir mais devagar 
    public Image barrinhaTras; //barra de vida que diminui mais rapido


    public int maxVida = 5; //maxima de vida do jogador 
    public int vidaAtual; //vai atualizar a barra de vida 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        vidaAtual = maxVida; //quando dá start, a vida atual é a quatidade do maximo de  vida que o jogador tem 
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        GameOver();
    }

    void Move()
    {
        float input = Input.GetAxisRaw("Horizontal");

        rig.linearVelocity = new Vector2(input * velocidade, rig.linearVelocity.y);

        if (input > 0f)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

        else if (input < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isJump == false)
            {
                rig.AddForce(new Vector3(0f, jumpForce), ForceMode2D.Impulse);
                doubleJump = true;
                isJump = true;
            }

            else
            {
                if (doubleJump)
                {
                    rig.AddForce(new Vector3(0f, jumpForce), ForceMode2D.Impulse);
                    doubleJump = false;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
        }
    }

    /*private void OTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Coin") && other.gameObject == coinColetar)
        {
            coinColetar = null;
        }
    }*/

    public void SetVida(int amount)
    {
        if (amount < 0)
        {
            vidaAtual = Mathf.Clamp(vidaAtual + amount, 0, maxVida);

            Vector3 frenteScale = barrinhaFrente.rectTransform.localScale;
            frenteScale.x = (float)vidaAtual / maxVida;
            barrinhaFrente.rectTransform.localScale = frenteScale;
            StartCoroutine(DecreasingTras(frenteScale));
        }


    }

    IEnumerator DecreasingTras(Vector3 newScale)
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 trasScale = barrinhaTras.transform.localScale;
        
        while (barrinhaTras.transform.localScale.x > newScale.x)
        {
            trasScale.x -= Time.deltaTime * 0.25f;
            barrinhaTras.transform.localScale = trasScale;

            yield return null;
        }
        barrinhaTras.transform.localScale = newScale;
        //Debug.Log(Time.time);
    }

    void GameOver()
    {
        if (vidaAtual == 0)
        {
            GameController.instance.ShowGameOver();
            Destroy(gameObject);
            //Destroy(barrinhaTras);
        }
        /*if (maxVida == 0)
        {
            GameController.instance.ShowGameOver();
            Destroy(gameObject);
        }*/
    }


}
