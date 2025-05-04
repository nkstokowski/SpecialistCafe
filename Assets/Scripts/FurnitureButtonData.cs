using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureButtonData : ButtonData
{

    void Update(){
        UpdateText();
    }
    public void UpdateText() {
        switch(myState) {
            case ShopButtonState.NotPurchased:
                buttonText.SetText("Purchase");
                break;
            case ShopButtonState.NotActive:
            case ShopButtonState.Active:
                buttonText.SetText("Use");
                break;
        }
    }
}
