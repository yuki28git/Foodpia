using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class CharacterCollection : MonoBehaviour
{
    public GameObject characterButtonPrefab;
    public Transform gridParent;
    public Button prevButton;
    public Button nextButton;
    public TextMeshProUGUI pageLabel;

    List<CharacterData> characterList;
    int currentPage = 0;
    const int charsPerPage = 10;

    // 最初の図鑑画面表示時の処理
    void Start()
    {
        OwnershipManager.Load();
        characterList = LoadCharacterData();
        UpdatePage();
        prevButton.onClick.AddListener(PrevPage);
        nextButton.onClick.AddListener(NextPage);
    }

    // 現在のページに応じてキャラボタンを生成＆更新
    void UpdatePage()
    {
        // 既存のボタンを全て削除
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        // 表示するキャラの範囲を計算
        int start = currentPage * charsPerPage;
        int end = Mathf.Min(start + charsPerPage, characterList.Count);

        // 範囲内のキャラについて、所持状況を渡してボタンを生成
        for (int i = start; i < end; ++i)
        {
            var obj = Instantiate(characterButtonPrefab, gridParent);
            bool isOwned = OwnershipManager.Has(characterList[i].name);
            obj.GetComponent<CharacterButtonScript>().Setup(characterList[i], isOwned);
        }

        // ページ数表示とボタンの有効/無効設定
        int totalPage = Mathf.CeilToInt((float)characterList.Count / charsPerPage);
        pageLabel.text = $"{currentPage + 1} / {totalPage}";

        // Prevは0ページ目以外で有効、Nextは最後のページ以外で有効
        prevButton.interactable = (currentPage > 0);
        nextButton.interactable = ((currentPage + 1) * charsPerPage < characterList.Count);
    }

    // Nextボタンの処理
    public void NextPage()
    {
        currentPage++;
        UpdatePage();
    }

    // Prevボタンの処理
    public void PrevPage()
    {
        currentPage--;
        UpdatePage();
    }

    [System.Serializable]
    public class CharacterData
    {
        public string name;
        public string imagePath;
        public string rarity;
    }

    // CSVからキャラクターデータを読み込む処理
    List<CharacterData> LoadCharacterData()
    {
        TextAsset csv = Resources.Load<TextAsset>("characters");
        var list = new List<CharacterData>();
        using (var reader = new StringReader(csv.text))
        {
            bool header = true;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (header) { header = false; continue; }
                var vals = line.Split(',');
                var data = new CharacterData()
                {
                    name = vals[0],
                    imagePath = vals[1],
                    rarity = vals[2]
                };
                list.Add(data);
            }
        }
        return list;
    }
}