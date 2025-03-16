using UnityEngine;
using UnityEngine.UI;

public class CollectableBarController : MonoBehaviour
{
    public Image collectableIcon;

    public void AddCollectable()
    {
        Instantiate(collectableIcon, transform);
    }
}
