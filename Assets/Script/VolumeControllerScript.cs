using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;

    // 最初の画面表示時の処理
    void Start()
    {
        volumeSlider.value = AudioListener.volume;
    }

    // ボリュームを変更するメソッド
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}