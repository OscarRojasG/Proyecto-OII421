using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public float speedX = 2f;
    public float rotationSpeed = 0f;

    private Rigidbody2D rb;
    private float startPos;
    private Vector3 rotationVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityX = speedX; // Mantiene la velocidad constante
        rotationVelocity = new Vector3(0f, 0f, rotationSpeed); // grados por segundo


        startPos = transform.position.x;
    }

    private void Update()
    {
        if (transform.position.x <= -startPos)
        {
            Destroy(gameObject);
        }

        transform.Rotate(rotationVelocity * Time.deltaTime);
    }
}
