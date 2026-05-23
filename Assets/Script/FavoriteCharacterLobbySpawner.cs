using UnityEngine;

public class FavoriteCharacterLobbySpawner : MonoBehaviour
{
    public Transform spawnRoot;
    private GameObject currentModel;

    void Start()
    {
        ShowFavoriteCharacter();
    }

    public void ShowFavoriteCharacter()
    {
        string favoriteId = FavoriteCharacterService.GetFavorite();

        if (string.IsNullOrEmpty(favoriteId))
        {
            Debug.Log("お気に入りキャラが未設定です");
            return;
        }

        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        string path = $"Charactors/3D/{favoriteId}";
        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab == null)
        {
            Debug.LogWarning($"Prefabが見つかりません: {path}");
            return;
        }

        if (spawnRoot == null)
        {
            Debug.LogWarning("spawnRoot が未設定です");
            return;
        }

        currentModel = Instantiate(prefab, spawnRoot, false);
    }

    private void OnDestroy()
    {
        if (currentModel != null)
        {
            Destroy(currentModel);
        }
    }
}