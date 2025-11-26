using UnityEngine;

public class EnergiaRara : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float speed = 2f;

    [Header("Som")]
    public AudioClip somColetavel;

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
            AudioManager.instance.PlaySFX(somColetavel);

            player.AddVidaToda(); // recupera vida total

            Destroy(gameObject);
        }
    }
}
