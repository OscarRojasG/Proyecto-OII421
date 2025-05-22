using UnityEngine;

public class LoadingCircleUI : MonoBehaviour
{
    public float speed = 90f; // degrees per second
    private RectTransform rectTransform;
    private float angle = 0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        angle += speed * Time.unscaledDeltaTime;
        angle %= 360f;
        rectTransform.rotation = Quaternion.Euler(0f, 0f, -angle); // Negative to rotate clockwise
    }
}
