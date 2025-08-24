using UnityEngine;

public class CoinBounce : MonoBehaviour
{
    public float speedY = 0.25f; //altura do movimento (quanto a moeda vai "subir e descer")
    public float speed = 2f; //velocidade da animação de sobe e desce

    public Vector3 startPos; //posição inicial da moeda

    // Start é chamado uma vez antes do primeiro frame
    void Start()
    {
        startPos = transform.position; //guarda a posição inicial da moeda
    }

    // Update é chamado a cada frame
    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * speedY; //calcula a nova posição no eixo Y usando seno (movimento suave)

        transform.position = new Vector3(startPos.x, newY, startPos.z); //atualiza a posição da moeda (só muda o Y)
    }
}
