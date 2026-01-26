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

    public List<int> expPerLevel;

    // UI Objects
    public TMP_Text expText;
    public Slider expSlider;
    public TMP_Text ticketText;


    public void LoadData(GameData data)
    {
        this.currentTickets = data.currentTickets;
        this.currentExp = data.currentExp;
        this.currentLevel = data.currentLevel;

        this.ticketText.text = this.currentTickets.ToString();
        SetExpBar();
    }

    public void SaveData(ref GameData data)
    {
        data.currentTickets = this.currentTickets;
        data.currentExp = this.currentExp;
        data.currentLevel = this.currentLevel;
    }

    void SetExpBar()
    {
        String expBarText = "All tickets earned!";
        
        if (this.currentLevel < this.expPerLevel.Count)
        {
            int expRemaining = this.expPerLevel[this.currentLevel - 1] - this.currentExp;
            expBarText = expRemaining + " EXP to next ticket";

            expSlider.minValue = (this.currentLevel <= 1) ? 0 : this.expPerLevel[this.currentLevel - 2];
            expSlider.maxValue = this.expPerLevel[this.currentLevel - 1];
            expSlider.value = this.currentExp;
        }
        else
        {
            expSlider.minValue = 0;
            expSlider.maxValue = 1;
            expSlider.value = 1;
        }

        expText.text = expBarText;
    }

    public void ModifyExp(int amount)
    {
        int levelBeforeModify = this.currentLevel;
        
        this.currentExp += amount;
        for(int i=0; i<this.expPerLevel.Count; i++)
        {
            if (this.currentExp < this.expPerLevel[i])
            {
                this.currentLevel = (i + 1);
                Debug.Log("Setting player level to: " + this.currentLevel);
                break;
            }
        }

        // Award Tickets
        if(this.currentLevel > levelBeforeModify)
        {
            this.currentTickets += this.currentLevel - levelBeforeModify;
            this.ticketText.text = this.currentTickets.ToString();
        }

        SetExpBar();
    }


    public bool TryAddMenuItem(int cost)
    {
        if (this.currentTickets < cost) return false;

        this.currentTickets -= cost;
        this.ticketText.text = this.currentTickets.ToString();
        return true;
    }
}
