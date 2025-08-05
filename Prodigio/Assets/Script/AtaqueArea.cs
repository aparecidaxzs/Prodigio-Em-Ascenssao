using UnityEngine;

public class AtaqueArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Inimigo"))
        {
                    
        }
    }
}
