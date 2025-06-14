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
    Vector3 upperCoinPos = new Vector3(0.620000005f,-1.03100002f,-2f);
    Vector3 lowerCoinPos = new Vector3(-0.741999984f,-2.95700002f,-1f);

    // Time & income
    DateTime lastUpdateTime;
    public int updateCycleHours = 3;
    public bool upperCoinCollected;
    public bool lowerCoinCollected;

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

        bool spawnUpperCoins = !this.upperCoinCollected;
        bool spawnLowerCoins = !this.lowerCoinCollected;

        if (GuestsShouldUpdate())
        {
            upperItem = GetRandomMenuItem();
            lowerItem = GetRandomMenuItem();

            spawnUpperCoins = true;
            spawnLowerCoins = true;
        }

        spawnGuest(upperItem, false, spawnUpperCoins);
        spawnGuest(lowerItem, true, spawnLowerCoins);
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

    private void spawnGuest(string menuItemName, bool lowerGuest, bool spawnCoins)
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

        // Coins
        if (spawnCoins)
        {
            GameObject coins = Instantiate(menuItem.coinsPrefab);
            coins.transform.position = lowerGuest ? lowerCoinPos : upperCoinPos;
            coins.GetComponent<SpriteRenderer>().flipX = lowerGuest;
            coins.GetComponent<CoinLogic>().lowerCoin = lowerGuest;
            if (lowerGuest) this.lowerCoinCollected = false;
            if (!lowerGuest) this.upperCoinCollected = false;
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
}
