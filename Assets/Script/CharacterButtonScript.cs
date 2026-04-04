using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterButtonScript : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;

    public void Setup(CharacterCollection.CharacterData data)
    {
        nameText.text = data.name;
        iconImage.sprite = Resources.Load<Sprite>(data.imagePath);
    }
}