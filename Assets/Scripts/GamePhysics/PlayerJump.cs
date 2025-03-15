using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 5f; // Fuerza del salto
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Detectar si el jugador presiona "espacio" o la pantalla táctil
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false; // Evita el doble salto
        }
    }

    // Detectar si toca el suelo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}