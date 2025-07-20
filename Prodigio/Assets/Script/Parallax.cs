using UnityEngine;

public class Parallax : MonoBehaviour
{

    private float lenght;
    private float StartPos;

    private Transform cam;

    public float ParallaxEffect;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //float Repos = cam.transform.position.x * (1 - ParallaxEffect);
        float Distance = -cam.transform.position.x * ParallaxEffect;

        transform.position = new Vector3(StartPos + Distance, transform.position.y, transform.position.z);

        /*if (Repos > StartPos + lenght)
        {
            StartPos += lenght;
        }
        else if (Repos < StartPos - lenght)
        {
            StartPos -= lenght;
        }*/
    }
}
