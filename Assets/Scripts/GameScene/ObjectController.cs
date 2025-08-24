using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public float speedX = 2f;
    public float rotationSpeed = 0f;

    private Rigidbody2D rb;
    private float startPos;
    private float width;
    private Vector3 rotationVelocity;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityX = -speedX; // Mantiene la velocidad constante
        rotationVelocity = new Vector3(0f, 0f, rotationSpeed); // grados por segundo

        startPos = transform.position.x;

        // Obtén el componente Collider (suponiendo que hay solo uno)
        BoxCollider2D col = GetComponent<BoxCollider2D>();

        if (col != null)
        {
            // Usamos los bounds del collider para obtener el ancho
            width = col.bounds.size.x;
        }
        else
        {
            Debug.LogWarning("El objeto no tiene un Collider.");
        }
    }

    public virtual void Update()
    {
        if (transform.position.x <= -startPos - width)
        {
            Destroy(gameObject);
        }

        transform.Rotate(rotationVelocity * Time.deltaTime);
    }

    public void SetSprite(Sprite sprite)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }
}
