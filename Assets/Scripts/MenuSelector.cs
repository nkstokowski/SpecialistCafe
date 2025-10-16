using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{

    public Button[] buttons;
    public MenuViewSelector viewSelector;
    public MenuViewSelector titleSelector;

    void SetAllButtonsInteractable() {
        foreach(Button b in buttons) {
            b.interactable = true;
        }
    }

    public void OnShopButtonSelected(Button clickedButton)
    {
        SetAllButtonsInteractable();
        clickedButton.interactable = false;
        viewSelector.ShowView(clickedButton.gameObject.name);
        if (titleSelector != null) {
            titleSelector.ShowView(clickedButton.gameObject.name);
        }
    }
}
