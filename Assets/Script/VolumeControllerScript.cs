/*
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

    void Awake()
    {
        EnsureDefaultValue();
        ApplySavedVolumeToSlider();
    }

    void OnEnable()
    {
        ApplySavedVolumeToSlider();
    }

    void EnsureDefaultValue()
    {
        string key = GetKey(volumeType);

        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetFloat(key, 1f);
            PlayerPrefs.Save();
        }
    }

    void ApplySavedVolumeToSlider()
    {
        if (volumeSlider == null) return;

        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 10f;
        volumeSlider.wholeNumbers = true;

        float savedVolume = GetSavedVolume(volumeType);
        int sliderValue = Mathf.RoundToInt(savedVolume * 10f);
        volumeSlider.SetValueWithoutNotify(sliderValue);
    }

    public void SetVolume(float sliderValue)
    {
        int step = Mathf.RoundToInt(sliderValue);
        float normalizedVolume = step / 10f;

        string key = GetKey(volumeType);
        PlayerPrefs.SetFloat(key, normalizedVolume);
        PlayerPrefs.Save();

        Debug.Log($"{volumeType} Volume = {normalizedVolume} (step {step})");
    }

    string GetKey(VolumeType type)
    {
        switch (type)
        {
            case VolumeType.BGM:
                return "BGMVolume";
            case VolumeType.ClickSE:
                return "ClickSEVolume";
            default:
                return "ClickSEVolume";
        }
    }

    public static float GetSavedVolume(VolumeType type)
    {
        string key = type == VolumeType.BGM ? "BGMVolume" : "ClickSEVolume";
        return PlayerPrefs.GetFloat(key, 1f);
    }
}
*/