using UnityEngine;

public class GachaConfirmScript : MonoBehaviour
{
    public GameObject gachaConfirmPanel;

    public void ShowConfirmPanel()
    {
        gachaConfirmPanel.SetActive(true);
    }
    public void HideConfirmPanel()
    {
        gachaConfirmPanel.SetActive(false);
    }
}