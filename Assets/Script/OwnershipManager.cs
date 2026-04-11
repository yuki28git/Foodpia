using System.Collections.Generic;
using UnityEngine;

public static class OwnershipManager
{
    private static HashSet<string> ownedNames;

    // 保存データのロード（最初や図鑑表示時に呼ぶ）
    // データは「OwnedCharacters」というキーで、キャラ名をカンマ区切りで保存している想定
    // 例: OwnedCharacters: "No1,No2,No3"
    public static void Load()
    {
        string saved = PlayerPrefs.GetString("OwnedCharacters", "");
        ownedNames = new HashSet<string>(
            saved.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)
        );
    }

    // 所持しているか確認
    public static bool Has(string characterName)
    {
        if (ownedNames == null) Load();
        return ownedNames.Contains(characterName);
    }

    // 所持キャラを追加＆保存
    public static void Add(string characterName)
    {
        if (ownedNames == null) Load();
        if (ownedNames.Add(characterName)) // 新規で追加時のみ保存
        {
            PlayerPrefs.SetString("OwnedCharacters", string.Join(",", ownedNames));
            PlayerPrefs.Save();
        }
    }

    // （テスト用）全データ消去
    public static void Clear()
    {
        ownedNames = new HashSet<string>();
        PlayerPrefs.DeleteKey("OwnedCharacters");
    }
}