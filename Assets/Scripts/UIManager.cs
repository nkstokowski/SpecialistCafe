using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject shop;

    public void ShowShop()
    {
        shop.SetActive(true);
    }

    public void HideShop()
    {
        shop.SetActive(false);
    }
}
