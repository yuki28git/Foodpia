using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public enum VolumeType
    {
        BGM,
        ClickSE
    }

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private VolumeType volumeType;

    private const string BgmKey = "BGMVolume";
    private const string ClickKey = "ClickSEVolume";

    private void Awake()
    {
        EnsureDefaultValue();
        ApplySavedVolumeToSlider();
    }

    private void OnEnable()
    {
        ApplySavedVolumeToSlider();
    }

    private const float DefaultBgmVolume = 0.3f;
    private const float DefaultSeVolume = 0.3f;

    private void EnsureDefaultValue()
    {
        string key = GetKey(volumeType);
        if (!PlayerPrefs.HasKey(key))
        {
            float def = (volumeType == VolumeType.BGM) ? DefaultBgmVolume : DefaultSeVolume;
            PlayerPrefs.SetFloat(key, def);
            PlayerPrefs.Save();
        }
    }

    private void ApplySavedVolumeToSlider()
    {
        if (volumeSlider == null) return;

        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.wholeNumbers = false; // ← 連続値

        float savedVolume = GetSavedVolume(volumeType); // 0.0～1.0
        volumeSlider.SetValueWithoutNotify(savedVolume);
    }

    // Slider OnValueChanged に登録
    public void SetVolume(float sliderValue)
    {
        float normalizedVolume = Mathf.Clamp01(sliderValue);

        string key = GetKey(volumeType);
        PlayerPrefs.SetFloat(key, normalizedVolume);
        PlayerPrefs.Save();

        ApplyVolumeImmediately(normalizedVolume);
    }
    private void ApplyVolumeImmediately(float volume)
    {
        if (volumeType == VolumeType.BGM)
        {
            var bgmPlayers = FindObjectsOfType<LobbyBgmPlayer>(true);
            foreach (var p in bgmPlayers)
            {
                p.SetVolume(volume);
            }
        }
        else
        {
            if (GlobalButtonClickSE.Instance != null)
                GlobalButtonClickSE.Instance.SetVolume(volume);
        }
    }

    private string GetKey(VolumeType type)
    {
        return type == VolumeType.BGM ? BgmKey : ClickKey;
    }

    public static float GetSavedVolume(VolumeType type)
    {
        string key = type == VolumeType.BGM ? BgmKey : ClickKey;
        float def = type == VolumeType.BGM ? DefaultBgmVolume : DefaultSeVolume;
        return PlayerPrefs.GetFloat(key, def);
    }
}