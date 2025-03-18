using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LivesController : MonoBehaviour
{
    private int livesLeft;
    public Sprite emptyHeartSprite;

    void Awake()
    {
        livesLeft = transform.childCount;
    }

    public void RemoveLife()
    {
        if (livesLeft >= 1)
        {
            Image life = transform.GetChild(livesLeft-1).GetComponent<Image>();
            life.sprite = emptyHeartSprite;

            livesLeft--;
        }
    }

    public int GetLivesLeft()
    {
        return livesLeft;
    }
}
