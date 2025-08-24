using UnityEngine;

public class Parallax : MonoBehaviour
{

    private float lenght; //comprimento do sprite (largura da imagem usada no fundo)
    private float StartPos; //posição inicial do sprite no eixo X

    private Transform cam; //referência para a câmera

    public float ParallaxEffect; //intensidade do efeito de parallax (quanto menor, mais devagar o fundo anda)


      void Start()
    {
        StartPos = transform.position.x; //guarda a posição inicial do fundo
        lenght = GetComponent<SpriteRenderer>().bounds.size.x; //pega a largura do sprite (em unidades do mundo)
        cam = Camera.main.transform; //pega a câmera principal
    }

    // Update é chamado a cada frame
    void Update()
    {
        //float Repos = cam.transform.position.x * (1 - ParallaxEffect); //posição que seria usada para looping (desativado por enquanto)
        float Distance = -cam.transform.position.x * ParallaxEffect; //distância proporcional ao movimento da câmera

        transform.position = new Vector3(StartPos + Distance, transform.position.y, transform.position.z); //atualiza a posição do fundo para dar o efeito

        /* código que faria o looping infinito do fundo (está comentado):
        if (Repos > StartPos + lenght) //se a câmera passou da largura do sprite
        {
            StartPos += lenght; //move o fundo para frente e reaproveita
        }
        else if (Repos < StartPos - lenght) //se a câmera foi para o outro lado
        {
            StartPos -= lenght; //move o fundo para trás e reaproveita
        }*/
    }
}