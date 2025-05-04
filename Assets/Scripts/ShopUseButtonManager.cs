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

        switch (data.myState) {
            case ShopButtonState.NotPurchased:
                bool itemPurchased = myCafeLayoutManager.TryPurchase(data);
                if (!itemPurchased) return;
                data.myState = ShopButtonState.Active;
                break;
            case ShopButtonState.NotActive:
                data.myState = ShopButtonState.Active;
                break;
            case ShopButtonState.Active:
                return;
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
        }

        data.UpdateText();
    }
}
