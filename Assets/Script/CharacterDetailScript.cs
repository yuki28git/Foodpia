using UnityEngine;
using TMPro; // or UnityEngine.UI;

public class CharacterDetailScript : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text nicknameText;
    public TMP_Text speciesText;
    public TMP_Text rarityText;
    public TMP_Text countText;
    public TMP_Text descriptionText;
    public Transform modelRoot;
    public CharacterDetailFavoriteButton favoriteButton;

    private GameObject currentModel;

    void Start()
    {
        var evs = FindObjectsOfType<UnityEngine.EventSystems.EventSystem>();
        for (int i = 1; i < evs.Length; ++i) Destroy(evs[i].gameObject);

        var als = FindObjectsOfType<AudioListener>();
        for (int i = 1; i < als.Length; ++i) Destroy(als[i].gameObject);

        var data = CharacterDetailHolder.SelectedData;
        bool isOwned = CharacterDetailHolder.IsOwned;

        nameText.text = data.name;
        nicknameText.text = data.nickname;
        speciesText.text = data.species;
        rarityText.text = data.rarity;

        int count = OwnershipManager.Has(data.name) ? 1 : 0;
        countText.text = count + "体";
        descriptionText.text = data.description;

        LoadAndDisplayModel(data.name);

        if (favoriteButton != null)
        {
            favoriteButton.Setup(data.name);
        }
    }

    void LoadAndDisplayModel(string characterName)
    {
        if (currentModel != null) Destroy(currentModel);

        string path = $"Charactors/3D/{characterName}";
        var prefab = Resources.Load<GameObject>(path);
        if (prefab != null && modelRoot != null)
        {
            currentModel = Instantiate(prefab, modelRoot, false);
            CharacterAnimationHelper.PlayIdle(currentModel);
        }
    }

    void OnDestroy()
    {
        if (currentModel != null)
            Destroy(currentModel);
    }
}