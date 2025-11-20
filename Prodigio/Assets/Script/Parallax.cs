using UnityEngine;

public class Parallax : MonoBehaviour
{    public float Lenght;

    public new Transform camera;

    public Vector3 InitPos;



    // Start is called before the first frame update
    void Start()
    {
        InitPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"Posição inicial Parallax = {InitPos}, Posição camera = {camera.position}");
        transform.position = new Vector3(InitPos.x - camera.position.x * Lenght, InitPos.y - camera.position.y * Lenght, transform.position.z);
    }
}
