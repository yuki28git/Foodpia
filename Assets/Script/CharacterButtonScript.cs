using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterButtonScript : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;

    CharacterCollection.CharacterData _data;
    bool _isOwned;

    // キャラクターデータと所持状況を受け取ってUIを更新するメソッド
    public void Setup(CharacterCollection.CharacterData data, bool isOwned)
    {
        _data = data;
        _isOwned = isOwned;
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
    public void OnClickDetail()
    {
        // 未所持は詳細を開かない
        if (!_isOwned)
        {
            Debug.Log("未所持キャラなので詳細画面は開けません");
            return;
        }

        CharacterDetailHolder.SelectedData = _data;
        CharacterDetailHolder.IsOwned = _isOwned;

        if (CollectionRootController.Instance != null)
            CollectionRootController.Instance.HideCanvas();

        SceneManager.LoadScene("FieldGuideDetailScene", LoadSceneMode.Additive);
    }
}