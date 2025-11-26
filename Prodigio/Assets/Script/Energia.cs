using UnityEngine;

public class Energia : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float speed = 2f;


    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();

        if (player != null)
        {
        
            // adiciona 1 de vida
            player.AddVida(1);

            // destrói o item
            Destroy(gameObject);
        }
    }
}
