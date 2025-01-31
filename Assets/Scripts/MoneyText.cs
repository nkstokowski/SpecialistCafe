using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyText : MonoBehaviour, IDataPersistence
{

    public int currentMoney = 10;
    public int moneyEarned = 0;

    private TextMeshProUGUI moneyText;

    private void Awake() {
        moneyText = this.GetComponent<TextMeshProUGUI>();
    }

    public void addMoney(int moneyToAdd) {
        this.moneyEarned += moneyToAdd;
    }

    private void Update() {
        if (moneyEarned > 0) {
            currentMoney += moneyEarned;
            moneyEarned = 0;
        }
        this.moneyText.text = "Money: " + currentMoney;
    }

    public void LoadData(GameData data)
    {
        this.currentMoney = data.currentMoney;
    }

    public void SaveData(ref GameData data)
    {
        data.currentMoney = this.currentMoney;
    }
}
