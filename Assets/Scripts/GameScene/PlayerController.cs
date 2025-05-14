using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f; // Fuerza del salto
    private Rigidbody2D rb;
    private bool isGrounded;

    private UnityAction<ObstacleController> collideObstacleAction;
    private UnityAction<CollectableController> collideCollectableAction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Detectar si el jugador presiona "espacio" o la pantalla táctil
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false; // Evita el doble salto
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Obstacle"))
        {
            collideObstacleAction(collider.GetComponent<ObstacleController>());
        }
        else if (collider.CompareTag("Collectable"))
        {
            collideCollectableAction(collider.GetComponent<CollectableController>());
        }
    }

    public void SetCollideObstacleAction(UnityAction<ObstacleController> action)
    {
        collideObstacleAction = action;
    }

    public void SetCollideCollectableAction(UnityAction<CollectableController> action)
    {
        collideCollectableAction = action;
    }
}