using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopUseButtonManager : MonoBehaviour
{
    public CafeLayoutManager myCafeLayoutManager;
    public CafeManager myCafeManager;
    public ExpManager myExpManager;
    public const int EXP_PER_SHOP_PURCHASE = 3;

    public void OnShopMenuUseButtonSelected(Button button) {
        MenuButtonData data = button.gameObject.GetComponent<MenuButtonData>();
        Debug.Log("Menu button pressed: " + data.theme);

        switch (data.myState) {
            case ShopButtonState.NotPurchased:
                bool itemPurchased = myCafeManager.TryPurchase(data);
                if (!itemPurchased) return;
                data.purchased = true;
                data.myState = ShopButtonState.Active;
                break;
            case ShopButtonState.NotActive:
            case ShopButtonState.Active:
                return;
        }
        
        data.UpdateText();
    }

    public void OnShopUseButtonSelected(Button button) {
        FurnitureButtonData data = button.gameObject.GetComponent<FurnitureButtonData>();
        //Debug.Log("Button pushed: " + data.theme);

        switch (data.myState) {
            case ShopButtonState.NotPurchased:
                //Debug.Log("Attempting to purchase");
                bool itemPurchased = myCafeLayoutManager.TryPurchase(data);
                if (!itemPurchased) return;
                //Debug.Log("Purchasing");
                myExpManager.ModifyExp(EXP_PER_SHOP_PURCHASE);
                data.myState = ShopButtonState.Active;
                break;
            case ShopButtonState.NotActive:
                //Debug.Log("Already owned, setting to active");
                data.myState = ShopButtonState.Active;
                break;
            case ShopButtonState.Active:
                //Debug.Log("Already active");
                break;
        }

        switch (data.category) {
            case "Table":
                myCafeLayoutManager.SetTables(data.theme);
                break;
            case "Chair":
                myCafeLayoutManager.SetChairs(data.theme);
                break;
            case "Wall":
                myCafeLayoutManager.SetWall(data.theme);
                break;
            case "Floor":
                myCafeLayoutManager.SetFloor(data.theme);
                break;
            case "Counter":
                myCafeLayoutManager.SetCounter(data.theme);
                break;
            case "Hat":
                myCafeLayoutManager.SetHat(data.theme);
                break;
        }

        data.UpdateText();
    }
}
