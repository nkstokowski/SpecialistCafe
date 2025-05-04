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

    public MoneyManager moneyManager;

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
        FurnitureButtonData[] allShopButtonDatas = GameObject.FindObjectsOfType<FurnitureButtonData>(true);
        foreach(FurnitureButtonData shopButtonData in allShopButtonDatas) {

            bool unlocked = false;
            bool current = false;

            switch(shopButtonData.category) {
                case "Table":
                    unlocked = unlockedTables.Contains(shopButtonData.theme);
                    current = currentTable == shopButtonData.name;
                    break;
                case "Chair":
                    unlocked = unlockedChairs.Contains(shopButtonData.theme);
                    current = currentChair == shopButtonData.name;
                    break;
                case "Wall":
                    unlocked = unlockedWalls.Contains(shopButtonData.theme);
                    current = currentWall == shopButtonData.name;
                    break;
                case "Floor":
                    unlocked = unlockedFloors.Contains(shopButtonData.theme);
                    current = currentFloor == shopButtonData.name;
                    break;
                case "Counter":
                    unlocked = unlockedCounters.Contains(shopButtonData.theme);
                    current = currentCounter == shopButtonData.name;
                    break;
            }

            shopButtonData.purchased = unlocked;
            if (unlocked) {
                shopButtonData.myState = current ? ShopButtonState.Active : ShopButtonState.NotActive;
            } else {
                shopButtonData.myState = ShopButtonState.NotPurchased;
            }

            shopButtonData.UpdateText();
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

    public bool TryPurchase(ButtonData data) {
        if (!moneyManager.TryPurchaseFurniture(data.cost)) return false;

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
}
