using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentMoney;
    public int currentTickets;

    public String timeLastSaved;
    public String timeLastIncome;

    public List<String> unlockedMenuItems;

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
        this.currentTickets = 0;

        this.timeLastSaved = DateTime.Now.ToString();
        this.timeLastIncome = DateTime.Now.ToString();

        this.unlockedMenuItems = new List<String>();

        this.unlockedTables = new List<String>(){"Default"};
        this.unlockedChairs = new List<String>(){"Default"};
        this.unlockedWalls = new List<String>(){"Default"};
        this.unlockedFloors = new List<String>(){"Default"};
        this.unlockedCounters = new List<String>(){"Default"};

        this.currentTable = "Default";
        this.currentChair = "Default";
        this.currentWall = "Default";
        this.currentFloor = "Default";
        this.currentCounter = "Default";

    }
}
