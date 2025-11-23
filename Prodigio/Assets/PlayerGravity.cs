using UnityEngine;
using System.Collections;

public class PlayerGravity : MonoBehaviour
{
    private Rigidbody2D rb;
    private float originalGravity;
    private Coroutine gravityRoutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
    }

    public void ApplyLowGravity(float newGravity, float time)
    {
        if (gravityRoutine != null)
            StopCoroutine(gravityRoutine);

        gravityRoutine = StartCoroutine(LowGravityRoutine(newGravity, time));
    }

    private IEnumerator LowGravityRoutine(float newGravity, float time)
    {
        rb.gravityScale = newGravity;

        yield return new WaitForSeconds(time);

        rb.gravityScale = originalGravity;
    }
}
