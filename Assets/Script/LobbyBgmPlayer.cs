using UnityEngine;

public class LobbyBgmPlayer : MonoBehaviour
{
    private static LobbyBgmPlayer instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private float volume = 0.25f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.volume = volume;
    }

    private void Start()
    {
        audioSource.volume = VolumeController.GetSavedVolume(VolumeController.VolumeType.BGM);

        if (bgmClip == null)
        {
            Debug.LogWarning("BGM clip is not assigned.");
            return;
        }

        if (audioSource.clip != bgmClip)
            audioSource.clip = bgmClip;

        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void SetVolume(float v)
    {
        if (audioSource == null) return;
        audioSource.volume = Mathf.Clamp01(v);
    }
}