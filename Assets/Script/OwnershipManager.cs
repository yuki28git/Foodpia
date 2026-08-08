using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class OwnershipManager
{
    private static Dictionary<string, int> ownedCounts;

    public static void Load()
    {
        string saved = PlayerPrefs.GetString("OwnedCharacters", "");
        ownedCounts = new Dictionary<string, int>();

        if (string.IsNullOrEmpty(saved))
            return;

        string[] entries = saved.Split(',');
        foreach (var entry in entries)
        {
            if (string.IsNullOrWhiteSpace(entry)) continue;

            string[] pair = entry.Split(':');
            if (pair.Length != 2) continue;

            string name = pair[0];
            if (int.TryParse(pair[1], out int count))
            {
                ownedCounts[name] = count;
            }
        }
    }

    public static bool Has(string characterName)
    {
        if (ownedCounts == null) Load();
        return ownedCounts.ContainsKey(characterName) && ownedCounts[characterName] > 0;
    }

    public static int GetCount(string characterName)
    {
        if (ownedCounts == null) Load();
        return ownedCounts.TryGetValue(characterName, out int count) ? count : 0;
    }

    public static void Add(string characterName)
    {
        if (ownedCounts == null) Load();

        if (ownedCounts.ContainsKey(characterName))
            ownedCounts[characterName]++;
        else
            ownedCounts[characterName] = 1;

        Save();
    }

    private static void Save()
    {
        var entries = ownedCounts.Select(kvp => $"{kvp.Key}:{kvp.Value}");
        PlayerPrefs.SetString("OwnedCharacters", string.Join(",", entries));
        PlayerPrefs.Save();
    }

    public static void Clear()
    {
        ownedCounts = new Dictionary<string, int>();
        PlayerPrefs.DeleteKey("OwnedCharacters");
    }
}