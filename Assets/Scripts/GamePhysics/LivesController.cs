using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LivesController : MonoBehaviour
{
    private int livesLeft;

    void Awake()
    {
        livesLeft = transform.childCount;
    }

    public void RemoveLife()
    {
        if (livesLeft >= 1)
        {
            Image life = transform.GetChild(livesLeft-1).GetComponent<Image>();
            life.sprite = null;

            livesLeft--;
        }
    }

    public int GetLivesLeft()
    {
        return livesLeft;
    }
}
