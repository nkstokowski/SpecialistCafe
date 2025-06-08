using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentMoney;
    public int currentTickets;

    public int lastUpdateHour;

    public List<string> unlockedMenuItems;

    public List<string> unlockedTables;
    public List<string> unlockedChairs;
    public List<string> unlockedWalls;
    public List<string> unlockedFloors;
    public List<string> unlockedCounters;

    public string currentTable;
    public string currentChair;
    public string currentWall;
    public string currentFloor;
    public string currentCounter;
    
    public string lastItemUpper;
    public string lastItemLower;

    public bool upperCoinCollected;
    public bool lowerCoinCollected;


    public GameData()
    {
        this.currentMoney = 0;
        this.currentTickets = 0;
        this.lastUpdateHour = 0;

        this.unlockedMenuItems = new List<string>();
        this.unlockedTables = new List<string>() { "Default" };
        this.unlockedChairs = new List<string>() { "Default" };
        this.unlockedWalls = new List<string>() { "Default" };
        this.unlockedFloors = new List<string>() { "Default" };
        this.unlockedCounters = new List<string>() { "Default" };

        this.currentTable = "Default";
        this.currentChair = "Default";
        this.currentWall = "Default";
        this.currentFloor = "Default";
        this.currentCounter = "Default";

        this.lastItemUpper = "None";
        this.lastItemLower = "None";

        this.upperCoinCollected = true;
        this.lowerCoinCollected = true;

    }
}
