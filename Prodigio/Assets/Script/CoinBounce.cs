using UnityEngine;

public class CoinBounce : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float speed = 2f;
    private Vector3 startPos;

    public int Score = 1;

    

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
           
            if (CoinManager.instance != null)
                CoinManager.instance.AddCoin(Score);
            // 3) Destrói a moeda após a coleta
            Destroy(gameObject);
        }
    }
}