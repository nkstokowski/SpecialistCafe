using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUseButtonManager : MonoBehaviour
{
    public CafeLayoutManager myCafeLayoutManager;

    public void OnShopUseButtonSelected(Button button) {
        ButtonData data = button.gameObject.GetComponent<ButtonData>();
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
    }
}
