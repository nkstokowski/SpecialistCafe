using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CafeManager : MonoBehaviour, IDataPersistence
{

    // Ingridients & Menu
    private GameObject[] ingridientShelfObjects;
    public List<string> unlockedMenuItems;
    public List<MenuItem> allMenuItems;
    private Dictionary<String, MenuItem> menuItemsDict;
    public string upperItem;
    public string lowerItem;

    // Positions
    Vector3 upperGuestPos = new Vector3(-1.05999994f, -1.85000002f, -1f);
    Vector3 lowerGuestPos = new Vector3(0.829999983f, -3.81999993f, -1f);
    Vector3 upperDrinkPos = new Vector3(-1.06900001f, -1.14999998f, -1f);
    Vector3 lowerDrinkPos = new Vector3(0.779999971f, -3.1099999f, -2f);

    // Time & income
    int lastUpdateHour;
    public int updateCycleHours = 3;

    // Manager
    public MoneyManager moneyManager;
    public CafeLayoutManager cafeLayoutManager;


    void Awake()
    {
        EnforceMinUpdateCycle();

        ingridientShelfObjects = GameObject.FindGameObjectsWithTag("Ingridient");
        menuItemsDict = new Dictionary<string, MenuItem>();
        foreach (MenuItem item in allMenuItems)
        {
            //Debug.Log("Adding " + item.name + " to Menu Dictionary");
            menuItemsDict.Add(item.name, item);
        }
    }

    public void LoadData(GameData data)
    {
        this.unlockedMenuItems = data.unlockedMenuItems;

        this.lastUpdateHour = data.lastUpdateHour;

        this.upperItem = data.lastItemUpper;
        this.lowerItem = data.lastItemLower;

        ShowActiveIngridients();
        if (this.unlockedMenuItems.Count > 0)
        {
            PopulateCafe();
        }

        this.UpdateShopButtons();
    }

    public void SaveData(ref GameData data)
    {
        data.unlockedMenuItems = this.unlockedMenuItems;
        data.lastUpdateHour = this.lastUpdateHour;
        data.lastItemUpper = this.upperItem;
        data.lastItemLower = this.lowerItem;
    }

    public void UpdateShopButtons()
    {
        MenuButtonData[] allShopButtonDatas = GameObject.FindObjectsOfType<MenuButtonData>(true);
        foreach (MenuButtonData shopButtonData in allShopButtonDatas)
        {
            bool unlocked = this.unlockedMenuItems.Contains(shopButtonData.theme);
            shopButtonData.purchased = unlocked;
            shopButtonData.myState = unlocked ? ShopButtonState.Active : ShopButtonState.NotPurchased;
            shopButtonData.UpdateText();
        }
    }

    private void ShowActiveIngridients()
    {
        foreach (GameObject ingridient in ingridientShelfObjects)
        {
            ingridient.SetActive(unlockedMenuItems.Contains(ingridient.name));
        }
    }

    private void PopulateCafe()
    {
        if (GuestsShouldUpdate())
        {
            upperItem = GetRandomMenuItem();
            lowerItem = GetRandomMenuItem();
        }

        spawnGuest(upperItem, false);
        spawnGuest(lowerItem, true);
    }

    private bool GuestsShouldUpdate()
    {
        EnforceMinUpdateCycle();

        // Get the needed time of day values
        DateTime currentTime = DateTime.Now;
        Debug.Log("Current Time: " + currentTime);
        DateTime lastUpdateTime = DateTime.Today.AddHours(lastUpdateHour);
        Debug.Log("Last Time Updated: " + lastUpdateTime);
        TimeSpan difference = currentTime - lastUpdateTime;
        Debug.Log("Time since last update: " + difference);

        // Determine if we have passed an update cycle
        if (difference.TotalHours > updateCycleHours)
        {
            // Update our internal tracker of the most recent update hour
            int hour = currentTime.Hour;
            Debug.Log("Current hour of day: " + hour);
            int mostRecentUpdateCycle = hour / updateCycleHours;
            Debug.Log("Last update cycle of today: " + mostRecentUpdateCycle);
            lastUpdateHour = mostRecentUpdateCycle * updateCycleHours;
            Debug.Log("New Last Time Updated: " + DateTime.Today.AddHours(lastUpdateHour));

            return true;
        }

        return false;
    }

    private void EnforceMinUpdateCycle()
    {
        if (updateCycleHours <= 0)
        {
            Debug.Log("Update Cycle must be greater than 0. Setting to 1");
            updateCycleHours = 1;
        }
    }

    private void spawnGuest(string menuItemName, Boolean lowerGuest)
    {
        if (menuItemName == "None") return;

        MenuItem menuItem = menuItemsDict[menuItemName];

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

    public int CalculateIncome(DateTime startTime)
    {

        TimeSpan span = DateTime.Now.Subtract(startTime);
        int hoursPassed = span.Hours;
        //Debug.Log("Hours passed: " + hoursPassed);

        int income = 0;
        foreach (String item in unlockedMenuItems)
        {
            //Debug.Log("Calculating income from " + item);
            float incomeFromItem = menuItemsDict[item].rarity * hoursPassed * menuItemsDict[item].hourlyRate;
            //Debug.Log(item + " earned " + incomeFromItem);
            income += (int)incomeFromItem;
        }
        //Debug.Log("Total income earned over " + hoursPassed + " hours = " + income);
        return income;
    }

    public bool TryPurchase(ButtonData data)
    {
        if (!moneyManager.TryAddMenuItem(data.cost)) return false;

        this.unlockedMenuItems.Add(data.theme);

        return true;
    }

    string GetRandomMenuItem()
    {
        float weightedSum = 0f;
        Dictionary<string, float> menuWeights = new Dictionary<string, float>();
        Dictionary<string, float> menuOdds = new Dictionary<string, float>();

        foreach (string item in unlockedMenuItems)
        {
            float rarity = menuItemsDict[item].rarity;
            float themeMultiplier = cafeLayoutManager.GetThemeMultiplier(menuItemsDict[item].theme);
            float weightedRarity = themeMultiplier * rarity;

            menuWeights[item] = weightedRarity;
            weightedSum += weightedRarity;
            //Debug.Log("Menu item: " + item + ". Rarity: " + rarity + ". Theme Bonus: " + themeMultiplier + ". Weighted Rarity: " + weightedRarity + ".");
        }

        //Debug.Log("Sum of weighted menu rarities: " + weightedSum);

        foreach (string item in unlockedMenuItems)
        {
            menuOdds[item] = (menuWeights[item] / weightedSum) * 100f;
            //Debug.Log("Final Rarity for " + item + ": " + menuOdds[item] + ".");
        }

        float roll = UnityEngine.Random.Range(0f, 100f);
        //Debug.Log("Roll: " + roll);

        float cumulative = 0f;

        foreach (string item in unlockedMenuItems)
        {
            cumulative += menuOdds[item];
            if (roll < cumulative)
            {
                // i is the selected index
                //Debug.Log("Selected choice: " + item);
                return item;
            }
        }

        return "";
    }
}
