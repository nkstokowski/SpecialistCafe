using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CafeLayoutManager : MonoBehaviour, IDataPersistence
{

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

    public Theme[] allThemes;
    Dictionary<string, Theme> themesDict;

    public SpriteRenderer[] tables;
    public SpriteRenderer[] chairs;
    public SpriteRenderer counter;
    public SpriteRenderer wall;
    public SpriteRenderer floor;

    public MoneyText moneyText;

    public void LoadData(GameData data)
    {
        this.unlockedTables = data.unlockedTables;
        this.unlockedChairs = data.unlockedChairs;
        this.unlockedWalls = data.unlockedWalls;
        this.unlockedFloors = data.unlockedFloors;
        this.unlockedCounters = data.unlockedCounters;

        this.currentTable = data.currentTable;
        this.currentChair = data.currentChair;
        this.currentWall = data.currentWall;
        this.currentFloor = data.currentFloor;
        this.currentCounter = data.currentCounter;

        UpdateCafe();
        UpdateShopButtons();
    }

    public void SaveData(ref GameData data)
    {
        data.unlockedTables = this.unlockedTables;
        data.unlockedChairs = this.unlockedChairs;
        data.unlockedWalls = this.unlockedWalls;
        data.unlockedFloors = this.unlockedFloors;
        data.unlockedCounters = this.unlockedCounters;

        data.currentTable = this.currentTable;
        data.currentChair = this.currentChair;
        data.currentWall = this.currentWall;
        data.currentFloor = this.currentFloor;
        data.currentCounter = this.currentCounter;
    }

    void Awake() {
        themesDict = new Dictionary<String, Theme>();
        foreach(Theme themeObj in allThemes) {
            themesDict.Add(themeObj.name, themeObj);
        }
    }

    void UpdateShopButtons() {
        ButtonData[] allShopButtonDatas = GameObject.FindObjectsOfType<ButtonData>(true);
        foreach(ButtonData shopButtonData in allShopButtonDatas) {
            TMP_Text shopButtonText = shopButtonData.GameObject().GetComponentInChildren<TMP_Text>();

            bool unlocked = false;

            switch(shopButtonData.category) {
            case "Table":
                unlocked = unlockedTables.Contains(shopButtonData.theme);
                break;
            case "Chair":
                unlocked = unlockedChairs.Contains(shopButtonData.theme);
                break;
            case "Wall":
                unlocked = unlockedWalls.Contains(shopButtonData.theme);
                break;
            case "Floor":
                unlocked = unlockedFloors.Contains(shopButtonData.theme);
                break;
            case "Counter":
                unlocked = unlockedCounters.Contains(shopButtonData.theme);
                break;
            }

            shopButtonText.text = unlocked ? "Use" : "Buy";
            shopButtonData.purchased = unlocked;
        }
    }

    void UpdateCafe() {
        SetTables(currentTable);
        SetChairs(currentChair);
        SetWall(currentWall);
        SetFloor(currentFloor);
        SetCounter(currentCounter);
    }

    public void SetTables(String theme) {
        tables[0].sprite = themesDict[theme].table;
        tables[1].sprite = themesDict[theme].table;
        this.currentTable = theme;
    }

    public void SetChairs(String theme) {
        chairs[0].sprite = themesDict[theme].chair;
        chairs[1].sprite = themesDict[theme].chair;
        this.currentChair = theme;
    }

    public void SetWall(String theme) {
        wall.sprite = themesDict[theme].wall;
        this.currentWall = theme;
    }

    public void SetFloor(String theme) {
        floor.sprite = themesDict[theme].floor;
        this.currentFloor = theme;
    }

    public void SetCounter(String theme) {
        counter.sprite = themesDict[theme].counter;
        this.currentCounter = theme;
    }

    internal bool TryPurchase(ButtonData data)
    {
        if(moneyText.currentMoney >= data.cost) {
            moneyText.currentMoney -= data.cost;

            switch(data.category) {
            case "Table":
                unlockedTables.Add(data.theme);
                break;
            case "Chair":
                unlockedChairs.Add(data.theme);
                break;
            case "Wall":
                unlockedWalls.Add(data.theme);
                break;
            case "Floor":
                unlockedFloors.Add(data.theme);
                break;
            case "Counter":
                unlockedCounters.Add(data.theme);
                break;
            }

            return true;
        }
        
        return false;
    }
}
