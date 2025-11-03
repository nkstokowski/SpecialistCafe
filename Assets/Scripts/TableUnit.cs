using System;
using UnityEngine;

public class TableUnit : MonoBehaviour
{
    public SpriteRenderer table;
    public SpriteRenderer chair;
    public SpriteRenderer guest;
    public SpriteRenderer drink;
    public GameObject coins;
    public MenuItem menuItem;

    public void SetMenu(MenuItem newMenuItem)
    {
        this.menuItem = newMenuItem;
        this.drink.sprite = this.menuItem.drinkSprite;
        this.guest.sprite = this.menuItem.guestSprite;
    }

    public void SetCoins(bool enable)
    {
        this.coins.GetComponent<SpriteRenderer>().sprite = this.menuItem.coinSprite;
        this.coins.GetComponent<CoinLogic>().value = this.menuItem.coinValue;
        this.coins.SetActive(enable);
    }

    public void SetTable(Sprite tableSprite)
    {
        this.table.sprite = tableSprite;
    }

    public void SetChair(Sprite chairSprite)
    {
        this.chair.sprite = chairSprite;
    }

    public String GetGuest()
    {
        return this.menuItem.guestName;
    }
}
