using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GachaResultUIScript : MonoBehaviour
{
    public Image singleImage;
    public Image singleFrameImage;

    public List<Image> multiImages;
    public List<Image> multiFrameImages;

    Dictionary<string, string> rarityFramePath = new Dictionary<string, string>() {
        { "Common",     "Charactors/Frames/normal_gray" },
        { "Rare",       "Charactors/Frames/rare_blue" },
        { "Super Rare", "Charactors/Frames/super_rare_gold" },
        { "Legendary",  "Charactors/Frames/legend_rainbow" }
    };

    void Start()
    {
        int count = GachaResultHolder.results.Count;

        for (int i = 0; i < count; i++)
        {
            CharacterData data = GachaResultHolder.results[i];
            OwnershipManager.Add(data.name);
        }

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