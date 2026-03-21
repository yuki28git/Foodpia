using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// キャラクターデータ構造
[System.Serializable]
public class CharacterData
{
    public string name;
    public string imagePath;
    public string rarity;
}

// 結果保持
public static class GachaResultHolder
{
    public static List<CharacterData> results = new List<CharacterData>();
}

// ガチャ管理
public class GachaManagerScript : MonoBehaviour
{
    public List<CharacterData> characterList = new List<CharacterData>();

    void Start()
    {
        LoadCSV();
    }

    void LoadCSV()
    {
        TextAsset csv = Resources.Load<TextAsset>("characters");
        string[] lines = csv.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] data = lines[i].Split(',');
            if (data.Length < 3) continue;

            CharacterData chara = new CharacterData();
            chara.name = data[0].Trim();
            chara.imagePath = data[1].Trim();
            chara.rarity = data[2].Trim();
            characterList.Add(chara);
        }
    }

    // ▼ レアリティ抽選
    string GetRandomRarity()
    {
        int rand = Random.Range(0, 100);

        if (rand < 60) return "Common";
        else if (rand < 94) return "Rare";
        else if (rand < 99) return "Super Rare";
        else return "Legendary";
    }

    CharacterData GetRandomCharacter()
    {
        string targetRarity = GetRandomRarity();

        // ログ出力
        Debug.Log($"抽選レアリティ: ★{targetRarity}");

        // 該当レアのキャラだけ抽出
        List<CharacterData> filtered = characterList.FindAll(c => c.rarity == targetRarity);

        // ★超重要：安全対策
        if (filtered.Count == 0)
        {
            Debug.LogWarning("該当レアなし → 全体から選ぶ");
            return characterList[Random.Range(0, characterList.Count)];
        }

        int index = Random.Range(0, filtered.Count);
        return filtered[index];
    }

    // ガチャボタンで呼ぶ
    public void OnClickGacha()
    {
        CharacterData result = GetRandomCharacter();

        if (result == null) return;

        GachaResultHolder.results.Clear();
        GachaResultHolder.results.Add(result);
    }
    public void OnClickGacha10()
    {
        GachaResultHolder.results.Clear();

        for (int i = 0; i < 10; i++)
        {
            CharacterData result = GetRandomCharacter();
            Debug.Log(result);
            if (result != null)
            {
                GachaResultHolder.results.Add(result);
            }
        }
    }
}
