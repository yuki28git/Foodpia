using UnityEngine;
using TMPro;

public class MoneyDisplayScript : MonoBehaviour
{
    public TMP_Text moneyText;  // TMP_Textなら型・usingを修正

    public static int money = 25000; // 初期値/セーブする場合はPlayerPrefs利用を検討

    void Start()
    {
        RefreshMoney();
    }

    public void RefreshMoney()
    {
        if (moneyText != null)
            moneyText.text = money.ToString();
    }

    // お金を増減する共通API
    public bool TrySpendMoney(int cost)
    {
        if (money < cost) return false;
        money -= cost;
        RefreshMoney();
        return true;
    }

    public void AddMoney(int value)
    {
        money += value;
        RefreshMoney();
    }
}