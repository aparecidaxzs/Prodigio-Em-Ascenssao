using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public int dano = -1; //dano que o inimigo causa (valor negativo porque o SetVida do player espera isso)
    public float velocidade = 2f; //velocidade de movimento do inimigo
    public Transform[] pontosPatrulha; //lista de pontos que o inimigo vai patrulhar
    private int indiceAtual = 0; //índice do ponto de patrulha atual

    void Update()
    {
        Patrulhar(); //chama a função que faz o inimigo se mover entre os pontos
    }

    void Patrulhar()
    {
        if (pontosPatrulha.Length == 0) return; //se não tiver pontos, não faz nada

        Transform alvo = pontosPatrulha[indiceAtual]; //pega o ponto atual
        transform.position = Vector2.MoveTowards(transform.position, alvo.position, velocidade * Time.deltaTime); //move até o ponto

        if (Vector2.Distance(transform.position, alvo.position) < 0.1f) //quando chega bem perto do ponto
        {
            indiceAtual++; //passa para o próximo
            if (indiceAtual >= pontosPatrulha.Length) //se passar do último ponto
                indiceAtual = 0; //volta para o primeiro (loop)
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>(); //checa se colidiu com o Player
        if (player != null)
        {
            player.SetVida(dano); //aplica dano no player
        }
    }
}
