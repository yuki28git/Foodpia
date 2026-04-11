using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GachaResultUIScript : MonoBehaviour
{
    public Image singleImage;          // 単発アイコン
    public Image singleFrameImage;     // 単発の枠

    public List<Image> multiImages;       // 10連用アイコン
    public List<Image> multiFrameImages;  // 10連用フレーム

    // レアリティに応じたフレームのパスを定義
    Dictionary<string, string> rarityFramePath = new Dictionary<string, string>() {
        { "Common",     "Charactors/Frames/normal_gray" },
        { "Rare",       "Charactors/Frames/rare_blue" },
        { "Super Rare", "Charactors/Frames/super_rare_gold" },
        { "Legendary",  "Charactors/Frames/legend_rainbow" }
    };

    // ガチャ結果画面表示時の処理
    void Start()
    {
        int count = GachaResultHolder.results.Count;

        // ガチャ結果を所持リストに登録
        for (int i = 0; i < count; i++)
        {
            CharacterData data = GachaResultHolder.results[i];
            OwnershipManager.Add(data.name);
        }

        // 結果に応じてUIを更新(キャラクターアイコンとレアリティフレームの表示)
        if (count == 1)
        {
            CharacterData data = GachaResultHolder.results[0];
            singleImage.sprite = Resources.Load<Sprite>(data.imagePath);

            if (rarityFramePath.ContainsKey(data.rarity))
                singleFrameImage.sprite = Resources.Load<Sprite>(rarityFramePath[data.rarity]);
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                CharacterData data = GachaResultHolder.results[i];
                multiImages[i].sprite = Resources.Load<Sprite>(data.imagePath);

                if (rarityFramePath.ContainsKey(data.rarity))
                    multiFrameImages[i].sprite = Resources.Load<Sprite>(rarityFramePath[data.rarity]);
            }
        }
    }
}