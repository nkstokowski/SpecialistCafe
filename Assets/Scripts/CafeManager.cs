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
    float aspectScaler;
    float baseTableUnitY = -9f;

    // Time & income
    DateTime lastUpdateTime;
    public int updateCycleHours = 3;
    public bool upperCoinCollected;
    public bool lowerCoinCollected;

    // Manager
    public MoneyManager moneyManager;
    public CafeLayoutManager cafeLayoutManager;

    // Object management
    public Transform screensTransform;
    public Transform centerTransform;
    public TableUnit upperUnit;
    public TableUnit lowerUnit;


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

        this.lastUpdateTime = DateTime.Parse(data.lastUpdateTime);

        this.upperItem = data.lastItemUpper;
        this.lowerItem = data.lastItemLower;

        this.upperCoinCollected = data.upperCoinCollected;
        this.lowerCoinCollected = data.lowerCoinCollected;

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
        data.lastUpdateTime = this.lastUpdateTime.ToString();
        data.lastItemUpper = this.upperItem;
        data.lastItemLower = this.lowerItem;
        data.upperCoinCollected = this.upperCoinCollected;
        data.lowerCoinCollected = this.lowerCoinCollected;
    }

    public void UpdateShopButtons()
    {
        MenuButtonData[] allShopButtonDatas = GameObject.FindObjectsByType<MenuButtonData>(FindObjectsSortMode.None);
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

        bool spawnUpperCoins = !this.upperCoinCollected;
        bool spawnLowerCoins = !this.lowerCoinCollected;

        if (GuestsShouldUpdate())
        {
            upperItem = GetRandomMenuItem();
            lowerItem = GetRandomMenuItem();

            spawnUpperCoins = true;
            spawnLowerCoins = true;
        }

        upperUnit.SetMenu(menuItemsDict[upperItem]);
        upperUnit.SetCoins(spawnUpperCoins);
        

        lowerUnit.SetMenu(menuItemsDict[lowerItem]);
        lowerUnit.SetCoins(spawnLowerCoins);
    }

    private bool GuestsShouldUpdate()
    {
        EnforceMinUpdateCycle();

        // Get the needed time of day values
        DateTime currentTime = DateTime.Now;
        Debug.Log("Current Time: " + currentTime);
        Debug.Log("Last Time Updated: " + lastUpdateTime);
        TimeSpan difference = currentTime - lastUpdateTime;
        Debug.Log("Time since last update: " + difference);

        // Determine if we have passed an update cycle
        if (difference.TotalHours > updateCycleHours)
        {
            // Update our internal tracker of the most recent update hour
            lastUpdateTime = currentTime;
            Debug.Log("New Last Time Updated: " + lastUpdateTime);

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
        ShowActiveIngridients();

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

    public void CoinCollected(bool isLowerCoin)
    {
        if (isLowerCoin)
        {
            this.lowerCoinCollected = true;
        }
        else
        {
            this.upperCoinCollected = true;
        }
    }

    internal void SetScreensRatio(float aspectScaler)
    {
        screensTransform.localScale = new Vector3(aspectScaler, 1f, 1f);
        centerTransform.localScale = new Vector3(1f, aspectScaler, 1f);

        float scaledTableUnitY = baseTableUnitY / aspectScaler;
        //Debug.Log("Scaled table y unit: " + scaledTableUnitY);

        upperUnit.transform.localPosition = new Vector3(
            upperUnit.transform.position.x,
            scaledTableUnitY / 2,
            upperUnit.transform.position.z
        );

        lowerUnit.transform.localPosition = new Vector3(
            lowerUnit.transform.position.x,
            scaledTableUnitY,
            lowerUnit.transform.position.z
        );
    }
}
