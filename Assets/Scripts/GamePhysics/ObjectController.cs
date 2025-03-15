using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public float speedX = 2f; // Velocidad constante
    private Rigidbody2D rb;

    private float startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityX = speedX; // Mantiene la velocidad constante

        startPos = transform.position.x;
    }

    private void Update()
    {
        if (transform.position.x <= -startPos)
        {
            Destroy(gameObject);
        }
    }
}
