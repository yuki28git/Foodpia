using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterButtonScript : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;

    // キャラクターデータと所持状況を受け取ってUIを更新するメソッド
    public void Setup(CharacterCollection.CharacterData data, bool isOwned)
    {
        // 所持している場合はアイコンと名前を表示
        if (isOwned)
        {
            iconImage.sprite = Resources.Load<Sprite>(data.imagePath);
            iconImage.color = Color.white;
            nameText.text = data.name;
            nameText.gameObject.SetActive(true);
        }
        // 未所持の場合はアイコンを消して名前も「?????」にする
        else
        {
            iconImage.sprite = null;
            iconImage.color = Color.black;
            nameText.text = "?????";
            nameText.gameObject.SetActive(true);
        }
    }
}