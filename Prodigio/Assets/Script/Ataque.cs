using System;
using System.Collections;
using UnityEngine;

public class Ataque : MonoBehaviour
{

    public KeyCode ataque = KeyCode.W;
    public float ataqueCool = 0.5f;
    public bool canAtaque = true;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(ataque) && canAtaque)
        {
            Coroutine coroutine = StartCoroutine(DoAttack);
        }        
    }

    private Coroutine StartCoroutine(Func<IEnumerator> doAttack)
    {
        throw new NotImplementedException();
    }

    private System.Collections.IEnumerator DoAttack()
    {
        canAtaque = false;

        transform.Find("AtaqueArea").gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        transform.Find("AtaqueArea").gameObject.SetActive(false);
       
        yield return new WaitForSeconds(ataqueCool);
        canAtaque = true;
    }
}
