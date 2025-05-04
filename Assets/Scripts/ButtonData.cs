using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ShopButtonState {
    NotPurchased,
    NotActive,
    Active
}

public class ButtonData : MonoBehaviour
{
    public string theme;
    public string category;

    public bool purchased;

    public int cost;

    public TMP_Text buttonText;
    public ShopButtonState myState;

}
