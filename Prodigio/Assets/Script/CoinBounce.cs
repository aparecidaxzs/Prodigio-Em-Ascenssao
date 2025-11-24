using UnityEngine;

public class CoinBounce : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float speed = 2f;
    private Vector3 startPos;

    public int Score = 1; // valor da moeda coletada

    [Header("Som")]
    public AudioClip somColetavel; // som da moeda/coletável

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
        if (collider.gameObject.CompareTag("Player"))
        {
            // Toca o som antes de destruir

            // Adiciona moedas ao CoinManager
            CoinManager.instance.AddCoin(Score);

            // Destrói a moeda
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(somColetavel);

        }
    }
}
