using UnityEngine;

public class CoinBounce : MonoBehaviour
{

    //public Animator anim;
    public float amplitude = 0.5f; // altura do movimento
    public float speed = 2f;       // velocidade do movimento

    private Vector3 startPos;

    public int Score;

    public GameObject coletar;


    // Start é chamado uma vez antes do primeiro frame
    void Start()
    {
        startPos = transform.position; // salva a posição inicial

    }

    // Update é chamado a cada frame
    void Update()
    {
        //anim.SetBool("girando", true);
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {

            Destroy(gameObject);
        }
    }
}
