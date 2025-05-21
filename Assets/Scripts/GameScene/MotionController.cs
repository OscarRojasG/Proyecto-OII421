using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class MotionController : MonoBehaviour
{
    public float speed = -5f;

    private float cloneStartX;
    private bool cloned = false;

    void Start()
    {
            // Debug.Log($"{gameObject.name} MotionController initialized at position {transform.localPosition}");
        Tilemap tilemap = GetComponentInChildren<Tilemap>();
        tilemap.CompressBounds(); // Eliminar tiles vacíos

        float width = tilemap.cellBounds.size.x * tilemap.layoutGrid.cellSize.x * transform.localScale.x;
        cloneStartX = width;
    }

    void Update()
    {
        // Debug.Log($"{gameObject.name} MotionController updating position {speed} {Time.deltaTime} {Vector3.right}");
        transform.localPosition += speed * Time.deltaTime * Vector3.right;

        if (transform.localPosition.x <= 0 && cloned == false)
        {
            GameObject clone = Instantiate(gameObject, transform.parent);
            clone.name = gameObject.name;
            clone.transform.localPosition = new Vector3((float)Math.Floor(transform.localPosition.x) + cloneStartX, 0, 0);
            cloned = true;
        }
        if (transform.localPosition.x <= -cloneStartX)
        {
            Destroy(gameObject);
        }
    }
}
