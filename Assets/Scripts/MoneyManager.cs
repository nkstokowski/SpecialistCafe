using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour, IDataPersistence
{

    public int currentMoney = 0;
    public int currentTickets = 0;

    DateTime timeLastSaved;
    DateTime timeLastIncome;
    public CafeManager cafeManager;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI ticketText; 

    public void LoadData(GameData data)
    {
        this.currentMoney = data.currentMoney;
        this.currentTickets = data.currentTickets;

        this.moneyText.text = this.currentMoney.ToString();
        this.ticketText.text = this.currentTickets.ToString();

        this.timeLastSaved = DateTime.Parse(data.timeLastSaved);
        this.timeLastIncome = DateTime.Parse(data.timeLastIncome);
        CalculateIncome(timeLastSaved);
    }

    public void SaveData(ref GameData data)
    {
        data.timeLastSaved = DateTime.Now.ToString();

        data.currentMoney = this.currentMoney;
        data.currentTickets = this.currentTickets;
    }

    void CalculateIncome(DateTime startTime) {
        TimeSpan span = DateTime.Now.Subtract(startTime);
        Debug.Log("Hours passed: " + span.Hours);
    }

    public bool TryPurchaseFurniture(int cost) {
        if (this.currentMoney < cost) return false;

        this.currentMoney -= cost;
        this.moneyText.text = this.currentMoney.ToString();
        return true;
    }

    public bool TryAddMenuItem(int cost) {
        if (this.currentTickets < cost) return false;

        this.currentTickets -= cost;
        this.ticketText.text = this.currentTickets.ToString();
        return true;
    }

}
