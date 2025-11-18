using UnityEngine;

public class SobeDesce : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 5f;

    private Vector3 startPosition;
    private Rigidbody2D rb;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Garante que seja kinematic
    }

    void FixedUpdate() // Muda para FixedUpdate para sincronizar com física
    {
        float movement = Mathf.PingPong(Time.time * speed, distance);
        Vector3 newPosition = startPosition + new Vector3(0f, movement, 0f);
        rb.MovePosition(newPosition); // Usa MovePosition para movimento suave
    }
}
