using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalButtonClickSE : MonoBehaviour
{
    public static GlobalButtonClickSE Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSE;

    private readonly HashSet<int> registeredButtonIds = new HashSet<int>();
    private float seVolume = 0.3f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        seVolume = VolumeController.GetSavedVolume(VolumeController.VolumeType.ClickSE);
    }

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void Start() => RegisterAllButtonsInScene();

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) => RegisterAllButtonsInScene();

    void RegisterAllButtonsInScene()
    {
        var buttons = FindObjectsOfType<Button>(true);
        foreach (var b in buttons) RegisterButton(b);
    }

    public void RegisterButton(Button button)
    {
        if (button == null) return;
        int id = button.GetInstanceID();
        if (registeredButtonIds.Contains(id)) return;

        button.onClick.RemoveListener(PlayClickSE);
        button.onClick.AddListener(PlayClickSE);
        registeredButtonIds.Add(id);
    }

    public void SetVolume(float v)
    {
        seVolume = Mathf.Clamp01(v);
    }

    public void PlayClickSE()
    {
        if (audioSource == null || clickSE == null) return;
        audioSource.PlayOneShot(clickSE, seVolume);
    }
}