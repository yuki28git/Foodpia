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

    void Start()
    {
        OwnershipManager.Load();
        characterList = LoadCharacterData();
        UpdatePage();
        prevButton.onClick.AddListener(PrevPage);
        nextButton.onClick.AddListener(NextPage);
    }

    void UpdatePage()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        int start = currentPage * charsPerPage;
        int end = Mathf.Min(start + charsPerPage, characterList.Count);

        for (int i = start; i < end; ++i)
        {
            var obj = Instantiate(characterButtonPrefab, gridParent);
            bool isOwned = OwnershipManager.Has(characterList[i].name);

            var script = obj.GetComponent<CharacterButtonScript>();
            script.Setup(characterList[i], isOwned);

            var button = obj.GetComponent<Button>();
            if (button != null)
            {
                // 1回目クリックから確実に鳴らすため、その場で登録
                if (GlobalButtonClickSE.Instance != null)
                    GlobalButtonClickSE.Instance.RegisterButton(button);

                // 詳細遷移
                button.onClick.AddListener(script.OnClickDetail);
            }
        }

        int totalPage = Mathf.CeilToInt((float)characterList.Count / charsPerPage);
        pageLabel.text = $"{currentPage + 1} / {totalPage}";

        prevButton.interactable = (currentPage > 0);
        nextButton.interactable = ((currentPage + 1) * charsPerPage < characterList.Count);
    }

    public void NextPage()
    {
        currentPage++;
        UpdatePage();
    }

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
        public string nickname;
        public string species;
        public string description;
    }

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
                    rarity = vals[2],
                    nickname = vals[3],
                    species = vals[4],
                    description = vals[5],
                };
                list.Add(data);
            }
        }
        return list;
    }
}