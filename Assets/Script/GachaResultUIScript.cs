using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GachaResultUIScript : MonoBehaviour
{
    public Image singleImage;           // 単発用
    public List<Image> multiImages;     // 10連用（10個入れる）

    void Start()
    {
        int count = GachaResultHolder.results.Count;

        // 単発
        if (count == 1)
        {
            CharacterData data = GachaResultHolder.results[0];
            singleImage.sprite = Resources.Load<Sprite>(data.imagePath);
        }
        // 10連
        else
        {
            for (int i = 0; i < count; i++)
            {
                CharacterData data = GachaResultHolder.results[i];
                multiImages[i].sprite = Resources.Load<Sprite>(data.imagePath);
            }
        }
    }
}