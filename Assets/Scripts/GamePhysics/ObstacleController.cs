using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float speedX = 2f; // Velocidad constante
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityX = speedX; // Mantiene la velocidad constante
    }
}
