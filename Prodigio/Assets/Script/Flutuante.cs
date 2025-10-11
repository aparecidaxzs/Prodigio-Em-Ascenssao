using UnityEngine;

public class Flutuante : MonoBehaviour
{
    public float fallingTime; //tempo até a plataforma cair depois do player pisar

    private TargetJoint2D target; //componente que segura a plataforma no lugar
    private BoxCollider2D boxColl; //colisor da plataforma

    // Start é chamado uma vez antes do primeiro frame
    void Start()
    {
        target = GetComponent<TargetJoint2D>(); //pega o TargetJoint2D da plataforma
        
        boxColl = GetComponent<BoxCollider2D>(); //pega o BoxCollider2D da plataforma
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") //se o jogador pisar na plataforma
        {
            Invoke("Falling", fallingTime); //chama a função Falling depois de X segundos
        }
    }

    private void OTriggerEnter2D(Collider2D other) //quando entra em um trigger
    {
        if (other.gameObject.tag == "GameOver") //se encostar no objeto de "GameOver"
        {
            Destroy(gameObject); //destroi a plataforma
        }
    }

    void Falling()
    {
        target.enabled = false; //desativa o TargetJoint (plataforma cai)
        boxColl.isTrigger = true; //colisor vira trigger (não segura mais o player)
    }
}
