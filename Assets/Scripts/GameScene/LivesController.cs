using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LivesController : MonoBehaviour
{
    private int livesLeft;
    public Sprite emptyHeartSprite;
    public Sprite fullHeartSprite;


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

    public void AddLife()
    {
        if (livesLeft < transform.childCount && livesLeft < 3)
        {
            Image life = transform.GetChild(livesLeft).GetComponent<Image>();
            life.sprite = fullHeartSprite; // Restore to full heart (assumes default sprite is full)
            livesLeft++;
        }
    }

    public int GetLivesLeft()
    {
        return livesLeft;
    }
}
