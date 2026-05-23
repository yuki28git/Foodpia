using UnityEngine;
using UnityEngine.UI;

public class CharacterDetailFavoriteButton : MonoBehaviour
{
    [SerializeField] private Image starImage;
    [SerializeField] private Color activeColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color inactiveColor = new Color(1f, 1f, 1f, 0.3f);

    private string characterId;

    private void Awake()
    {
        if (starImage == null)
        {
            starImage = GetComponent<Image>();
        }
    }

    public void Setup(string id)
    {
        characterId = id;
        Debug.Log("Setup characterId = " + characterId);
        Refresh();
    }

    public void OnClickFavorite()
    {
        Debug.Log("OnClickFavorite characterId = " + characterId);

        if (string.IsNullOrEmpty(characterId))
        {
            Debug.LogWarning("characterId が空です");
            return;
        }

        FavoriteCharacterService.SetFavorite(characterId);
        Debug.Log("保存後 favorite = " + FavoriteCharacterService.GetFavorite());

        Refresh();
    }

    public void Refresh()
    {
        if (starImage == null)
        {
            Debug.LogWarning("starImage が未設定です");
            return;
        }

        bool isFavorite = FavoriteCharacterService.IsFavorite(characterId);
        Debug.Log($"Refresh: characterId={characterId}, isFavorite={isFavorite}");

        starImage.color = isFavorite ? activeColor : inactiveColor;
    }
}