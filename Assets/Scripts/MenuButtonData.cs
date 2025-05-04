using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonData : ButtonData
{
    public void UpdateText() {
        switch(myState) {
            case ShopButtonState.NotPurchased:
                buttonText.SetText("Purchase");
                break;
            case ShopButtonState.NotActive:
            case ShopButtonState.Active:
                buttonText.SetText("Purchased");
                break;
        }
    }
}
