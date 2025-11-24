using UnityEngine;

public class EnergiaRara : MonoBehaviour
{
    public float amplitude = 0.5f; // altura do movimento
    public float speed = 2f;       // velocidade do movimento
    private Vector3 startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Som")]
    public AudioClip somColetavel; // som da moeda/coletável

    
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
         Player player = collider.GetComponent<Player>();
        if (player != null)
        {
                        AudioManager.instance.PlaySFX(somColetavel);

            player.AddVidaToda(); // Dá +1 de vida máxima
            Destroy(gameObject); // Destroi o coração
        }
    }
}
