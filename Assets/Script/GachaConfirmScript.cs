using UnityEngine;

public class GachaConfirmScript : MonoBehaviour
{
    public GameObject gachaConfirmPanel;

    // ガチャ確認パネルを表示するメソッド
    public void ShowConfirmPanel()
    {
        gachaConfirmPanel.SetActive(true);
    }

    // ガチャ確認パネルを非表示にするメソッド
    public void HideConfirmPanel()
    {
        gachaConfirmPanel.SetActive(false);
    }
}