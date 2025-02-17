using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSelector : MonoBehaviour
{

    public Button[] buttons;
    public ShopViewSelector viewSelector;

    void SetAllButtonsInteractable() {
        foreach(Button b in buttons) {
            b.interactable = true;
        }
    }

    public void OnShopButtonSelected(Button clickedButton) {
        SetAllButtonsInteractable();
        clickedButton.interactable = false;
        viewSelector.showView(clickedButton.gameObject.name);
    }
}
