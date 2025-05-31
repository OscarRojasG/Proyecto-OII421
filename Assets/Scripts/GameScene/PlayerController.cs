using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f; // Fuerza del salto
    private Rigidbody2D rb;
    private bool isGrounded;
    private PlayerData playerData;

    private UnityAction<ObstacleController> collideObstacleAction = (ObstacleController controller) => { };
    private UnityAction<CollectableController> collideCollectableAction = (CollectableController controller) => { };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerData = PlayerData.Instance;
    }


    void Update()
    {
        // Detectar si el jugador presiona "espacio" o la pantalla táctil
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isGrounded && !EventSystem.current.IsPointerOverGameObject())
        {
            if (playerData != null && playerData.data != null && playerData.data.jumpCount < 10)
            {
                playerData.data.jumpCount++;
            }

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

    public void Blink(float duration, float blinkInterval = 0.1f)
    {
        StartCoroutine(BlinkCoroutine(duration, blinkInterval));
    }

    private IEnumerator BlinkCoroutine(float duration, float blinkInterval)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float elapsed = 0f;
        while (elapsed < duration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSecondsRealtime(blinkInterval);
            elapsed += blinkInterval;
        }
        spriteRenderer.enabled = true;
    }

}