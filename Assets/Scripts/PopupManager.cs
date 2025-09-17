using UnityEngine;

public static class PopupManager
{
    private static GameObject popupPrefab = Resources.Load<GameObject>("Prefabs/ErrorPopup");

    public static void Show(string message, System.Action onOk, System.Action onCancel = null)
    {
        Canvas canvas = GameObject.Find("PopupCanvas").GetComponent<Canvas>();
        GameObject popup = Object.Instantiate(popupPrefab, canvas.transform);
        popup.GetComponent<PopupController>().Setup(message, onOk, onCancel);
    }

    public static void LoadingShow(string message = "Cargando Datos...")
    {
        Canvas canvas = GameObject.Find("PopupCanvas").GetComponent<Canvas>();
        GameObject loadingPrefab = Resources.Load<GameObject>("Prefabs/CargandoDatosPopup");
        GameObject popup = Object.Instantiate(loadingPrefab);
    }

    public static void LoadingHide()
    {
        Canvas canvas = GameObject.Find("PopupCanvas").GetComponent<Canvas>();
        foreach (Transform child in canvas.transform)
        {
            if (child.name.Contains("CargandoDatos"))
                Object.Destroy(child.gameObject);
        }
    }


}
