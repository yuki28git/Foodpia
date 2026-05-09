using UnityEngine;

public class CollectionRootController : MonoBehaviour
{
    // Inspectorでセットできるようpublicにする
    public GameObject canvasObject;

    public static CollectionRootController Instance { get; private set; }
    void Awake() { Instance = this; }

    public void HideCanvas()
    {
        if (canvasObject != null)
            canvasObject.SetActive(false);
    }
    public void ShowCanvas()
    {
        if (canvasObject != null)
            canvasObject.SetActive(true);
    }
}