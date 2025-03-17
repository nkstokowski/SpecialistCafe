using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CafeManager : MonoBehaviour, IDataPersistence
{
    private GameObject[] ingridientShelfObjects;
    public List<string> unlockedMenuItems;
    public List<string> currentMenuItems;

    public List<MenuItem> allMenuItems;
    private Dictionary<String, MenuItem> menuItemsDict;

    Vector3 upperGuestPos = new Vector3(-1.05999994f,-1.85000002f,-1f);
    Vector3 lowerGuestPos = new Vector3(0.829999983f,-3.81999993f,-1f);

    Vector3 upperDrinkPos = new Vector3(-1.06900001f,-1.14999998f,-1f);
    Vector3 lowerDrinkPos = new Vector3(0.779999971f,-3.1099999f,-2f);

    DateTime timeLastSaved;

    public MoneyText moneyText;


    void Start() {
        ingridientShelfObjects = GameObject.FindGameObjectsWithTag("Ingridient");
        menuItemsDict = new Dictionary<string, MenuItem>();
        foreach(MenuItem item in allMenuItems) {
            //Debug.Log("Adding " + item.name + " to Menu Dictionary");
            menuItemsDict.Add(item.name, item);
        }
    }

    public void LoadData(GameData data)
    {
        this.unlockedMenuItems = data.unlockedMenuItems;
        this.currentMenuItems = data.currentMenuItems;

        this.timeLastSaved = DateTime.Parse(data.timeLastSaved);
        CalculateIncome(timeLastSaved);

        ShowActiveIngridients();
        if(this.currentMenuItems.Count > 0) {
            PopulateCafe();
        }
    }

    public void SaveData(ref GameData data)
    {
        data.unlockedMenuItems = this.unlockedMenuItems;
        data.currentMenuItems = this.currentMenuItems;
        data.timeLastSaved = DateTime.Now.ToString();
    }

    private void ShowActiveIngridients() {
        foreach(GameObject ingridient in ingridientShelfObjects) {
            ingridient.SetActive(unlockedMenuItems.Contains(ingridient.name));
        }
    }

    private void PopulateCafe() {
        string upperMenuItem = currentMenuItems[UnityEngine.Random.Range(0, currentMenuItems.Count)];
        spawnGuest(menuItemsDict[upperMenuItem], false);
        string lowerMenuItem = currentMenuItems[UnityEngine.Random.Range(0, currentMenuItems.Count)];
        spawnGuest(menuItemsDict[lowerMenuItem], true);
    }

    private void spawnGuest(MenuItem menuItem, Boolean lowerGuest) {

        // Guest
        GameObject guest = new GameObject(menuItem.guestName, typeof(SpriteRenderer));
        guest.GetComponent<SpriteRenderer>().sprite = menuItem.guestSprite;
        guest.GetComponent<SpriteRenderer>().flipX = lowerGuest;
        guest.transform.position = lowerGuest ? lowerGuestPos : upperGuestPos;
        guest.transform.localScale = new Vector3(.5f, .5f, 1f);

        // Drink
        GameObject drink = new GameObject(menuItem.guestName + " Drink", typeof(SpriteRenderer));
        drink.GetComponent<SpriteRenderer>().sprite = menuItem.tableSprite;
        drink.GetComponent<SpriteRenderer>().flipX = lowerGuest;
        drink.transform.position = lowerGuest ? lowerDrinkPos : upperDrinkPos;
        drink.transform.localScale = new Vector3(.5f, .5f, 1f);
    }

    public int CalculateIncome(DateTime startTime) {

        TimeSpan span = DateTime.Now.Subtract(startTime);
        int hoursPassed = span.Hours;
        Debug.Log("Hours passed: " + hoursPassed);

        int income = 0;
        foreach( String item in currentMenuItems ) {
            Debug.Log("Calculating income from " + item);
            float incomeFromItem = menuItemsDict[item].rarity * hoursPassed * menuItemsDict[item].hourlyRate;
            Debug.Log(item + " earned " + incomeFromItem);
            income += (int) incomeFromItem;
        }
        Debug.Log("Total income earned over " + hoursPassed + " hours = " + income);
        return income;
    }
}
