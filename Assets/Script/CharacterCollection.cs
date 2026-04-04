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
        characterList = LoadCharacterData();
        UpdatePage();
        prevButton.onClick.AddListener(PrevPage);
        nextButton.onClick.AddListener(NextPage);
    }

    void UpdatePage()
    {
        // 一度全削除
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        int start = currentPage * charsPerPage;
        int end = Mathf.Min(start + charsPerPage, characterList.Count);

        for (int i = start; i < end; ++i)
        {
            var obj = Instantiate(characterButtonPrefab, gridParent);
            obj.GetComponent<CharacterButtonScript>().Setup(characterList[i]);
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
                    rarity = vals[2]
                };
                list.Add(data);
            }
        }
        return list;
    }
}