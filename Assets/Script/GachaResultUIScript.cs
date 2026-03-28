using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GachaResultUIScript : MonoBehaviour
{
    public Image singleImage;          // 単発アイコン
    public Image singleFrameImage;     // 単発の枠 ←追加

    public List<Image> multiImages;          // 10連アイコン
    public List<Image> multiFrameImages;     // 10連の枠 ←追加

    // レアリティ→フレームSpriteのパス
    Dictionary<string, string> rarityFramePath = new Dictionary<string, string>() {
        { "Common",     "Charactors/Frames/normal_gray" },
        { "Rare",       "Charactors/Frames/rare_blue" },
        { "Super Rare", "Charactors/Frames/super_rare_gold" },
        { "Legendary",  "Charactors/Frames/legend_rainbow" }
    };

    void Start()
    {
        int count = GachaResultHolder.results.Count;

        if (count == 1)
        {
            CharacterData data = GachaResultHolder.results[0];
            singleImage.sprite = Resources.Load<Sprite>(data.imagePath);

            if (rarityFramePath.ContainsKey(data.rarity))
            {
                singleFrameImage.sprite = Resources.Load<Sprite>(rarityFramePath[data.rarity]);
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                CharacterData data = GachaResultHolder.results[i];
                multiImages[i].sprite = Resources.Load<Sprite>(data.imagePath);

                if (rarityFramePath.ContainsKey(data.rarity))
                {
                    multiFrameImages[i].sprite = Resources.Load<Sprite>(rarityFramePath[data.rarity]);
                }
            }
        }
    }
}