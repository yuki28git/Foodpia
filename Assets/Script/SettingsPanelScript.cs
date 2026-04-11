using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    public GameObject settingsPanel;

    // 設定パネルを開くメソッド
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // 設定パネルを閉じるメソッド
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}