using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentMoney;

    public String timeLastSaved;
    public List<string> unlockedMenuItems;
    public List<string> currentMenuItems;

    public GameData() {
        this.currentMoney = 0;
        timeLastSaved = DateTime.Now.ToString();
        unlockedMenuItems = new List<String>();
        currentMenuItems = new List<String>();
    }
}
