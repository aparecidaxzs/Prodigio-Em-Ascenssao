using UnityEngine;

public class Energia : MonoBehaviour
{

    public float amplitude = 0.5f; // altura do movimento
    public float speed = 2f;       // velocidade do movimento

    private Vector3 startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Player.instance.ResetVida();
        }
    }
}
