using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentMoney;
    public int currentTickets;

    public String lastUpdateTime;

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

    public List<int> unlockedAchievements;

    public int currentLevel;
    public int expWithinCurrentLevel;


    /*public GameData()
    {
        this.currentMoney = 0;
        this.currentTickets = 0;
        this.lastUpdateTime = DateTime.Now.ToString();

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

        unlockedAchievements = new List<int>() {};

        this.currentLevel = 0;
        this.expWithinCurrentLevel = 0;

    }*/
    
    public GameData()
    {
        this.currentMoney = 100;
        this.currentTickets = 10;
        this.lastUpdateTime = DateTime.Now.ToString();

        this.unlockedMenuItems = new List<string>() { "Bamboo Boba", "Eucalyptus Tea", "Snowshoe Hare Pop", "Prairie Puppuccino" };
        this.unlockedTables = new List<string>() { "Default", "Airplane", "Book" };
        this.unlockedChairs = new List<string>() { "Default", "Airplane", "Book"  };
        this.unlockedWalls = new List<string>() { "Default", "Airplane", "Book"  };
        this.unlockedFloors = new List<string>() { "Default", "Airplane", "Book"  };
        this.unlockedCounters = new List<string>() { "Default", "Airplane", "Book"  };

        this.currentTable = "Airplane";
        this.currentChair = "Book";
        this.currentWall = "Book";
        this.currentFloor = "Airplane";
        this.currentCounter = "Book";

        this.lastItemUpper = "Bamboo Boba";
        this.lastItemLower = "Prairie Puppuccino";

        this.upperCoinCollected = false;
        this.lowerCoinCollected = false;

        unlockedAchievements = new List<int>() {};

        this.currentLevel = 0;
        this.expWithinCurrentLevel = 0;

    }
}
