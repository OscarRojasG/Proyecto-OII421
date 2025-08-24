using UnityEngine;

public class ObstacleController : ObjectController
{
    public float speedY = 0f;  // Velocidad de movimiento vertical
    public float deltaY = 0f;  // Altura máxima de movimiento vertical

    private float startPosY;  // Para guardar la posición Y inicial

    public override void Start()
    {
        base.Start();
        startPosY = transform.position.y;
    }

    public override void Update()
    {
        base.Update();

        // Movimiento vertical (con un movimiento sinusoide entre startPosY y startPosY + deltaY)
        float newY = startPosY + Mathf.Sin(Time.time * speedY) * deltaY + deltaY;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
