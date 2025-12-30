using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour, IDataPersistence
{

    public int currentTickets = 0;
    public int currentExp = 0;
    public int currentLevel = 1;

    List<int> expPerLevel;

    // UI Objects
    public TMP_Text expText;
    public Slider expSlider;
    public TMP_Text ticketText;


    public void LoadData(GameData data)
    {
        this.currentTickets = data.currentTickets;
        this.currentExp = data.expWithinCurrentLevel;
        this.currentLevel = data.currentLevel;
        this.expPerLevel = data.expPerLevel;

        this.ticketText.text = this.currentTickets.ToString();
        SetExpBar();
    }

    public void SaveData(ref GameData data)
    {
        data.currentTickets = this.currentTickets;
        data.expWithinCurrentLevel = this.currentExp;
        data.currentLevel = this.currentLevel;
    }

    void SetExpBar()
    {
        String expBarText = "Max Level Reached!";
        
        if (this.currentLevel < this.expPerLevel.Count)
        {
            int expToNextLevel = this.expPerLevel[this.currentLevel - 1];
            int expRemaining = expToNextLevel - this.currentExp;
            expBarText = expRemaining + " EXP to next level";

            expSlider.maxValue = expToNextLevel;
            expSlider.value = this.currentExp;
        }
        else
        {
            expSlider.maxValue = 1;
            expSlider.value = 1;
        }

        expText.text = expBarText;
    }

    public bool TryAddMenuItem(int cost)
    {
        if (this.currentTickets < cost) return false;

        this.currentTickets -= cost;
        this.ticketText.text = this.currentTickets.ToString();
        return true;
    }
}
