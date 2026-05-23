using UnityEngine;

public static class FavoriteCharacterService
{
    private const string Key = "FavoriteCharacterId";

    public static void SetFavorite(string characterId)
    {
        PlayerPrefs.SetString(Key, characterId);
        PlayerPrefs.Save();
    }

    public static string GetFavorite()
    {
        return PlayerPrefs.GetString(Key, "");
    }

    public static bool IsFavorite(string characterId)
    {
        return GetFavorite() == characterId;
    }

    public static void ClearFavorite()
    {
        PlayerPrefs.DeleteKey(Key);
        PlayerPrefs.Save();
    }
}
