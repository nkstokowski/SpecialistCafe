using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentMoney;

    public String timeLastSaved;
    public List<String> unlockedMenuItems;
    public List<String> currentMenuItems;

    public List<String> unlockedTables;
    public List<String> unlockedChairs;
    public List<String> unlockedWalls;
    public List<String> unlockedFloors;
    public List<String> unlockedCounters;

    public String currentTable;
    public String currentChair;
    public String currentWall;
    public String currentFloor;
    public String currentCounter;

    public GameData() {
        this.currentMoney = 0;
        timeLastSaved = DateTime.Now.ToString();
        unlockedMenuItems = new List<String>();
        currentMenuItems = new List<String>();

        unlockedTables = new List<String>(){"Default"};
        unlockedChairs = new List<String>(){"Default"};
        unlockedWalls = new List<String>(){"Default"};
        unlockedFloors = new List<String>(){"Default"};
        unlockedCounters = new List<String>(){"Default"};

        currentTable = "Default";
        currentChair = "Default";
        currentWall = "Default";
        currentFloor = "Default";
        currentCounter = "Default";

    }
}
