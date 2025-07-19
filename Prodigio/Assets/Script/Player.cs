using UnityEngine;
using System.Collections;
using UnityEngine.Scripting.APIUpdating;
using System.Collections.ObjectModel;


public class Player : MonoBehaviour
{
    public float velocidade;
    public float jumpForce;
    public bool isJump;
    public bool doubleJump;

    private Rigidbody2D rig;

    private GameObject coinColetar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        if (Input.GetKeyDown(KeyCode.W) && coinColetar != null)
        {
            CollectCoin();
        }
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
        if (collision.gameObject.CompareTag("Chão"))
        {
            isJump = false;
            doubleJump = false;
        }

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chão"))
        {
            isJump = true;
        }
    }

    void CollectCoin()
    {
        Destroy(coinColetar);
        coinColetar = null;
    }

    private void OTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coinColetar = other.gameObject;
        }
    }

    private void OTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Coin") && other.gameObject == coinColetar)
        {
            coinColetar = null;
        }
    }



}
