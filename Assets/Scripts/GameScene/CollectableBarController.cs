using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableBarController : MonoBehaviour
{
    public Image collectableIconPrefab;
    private Dictionary<Sprite, Image> spriteMap = new Dictionary<Sprite, Image>();

    public void AddSlot(Sprite spriteEmpty)
    {
        Image collectableIcon = Instantiate(collectableIconPrefab, transform);
        collectableIcon.sprite = spriteEmpty;

        spriteMap[spriteEmpty] = collectableIcon;
    }

    public void Fill(Sprite spriteEmpty, Sprite spriteFilled)
    {
        spriteMap[spriteEmpty].sprite = spriteFilled;
    }
}
