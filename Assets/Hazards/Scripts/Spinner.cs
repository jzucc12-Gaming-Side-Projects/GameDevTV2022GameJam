using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private float angularVelocity = 20;
    private Rigidbody2D rb = null;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.rotation += angularVelocity * Mathf.Sign(rb.velocity.x);
    }
}
