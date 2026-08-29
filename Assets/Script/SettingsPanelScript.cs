using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsUI : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject playSelectPanel;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            OpenSettings();
            ClosePlaySelect();
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

    // プレイ選択パネルを開くメソッド
    public void OpenPlaySelect()
    {
        playSelectPanel.SetActive(true);
    }

    // プレイ選択パネルを閉じるメソッド
    public void ClosePlaySelect()
    {
        playSelectPanel.SetActive(false);
    }
}