using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsUI : MonoBehaviour
{
    public GameObject settingsPanel;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            OpenSettings();
        }
    }

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