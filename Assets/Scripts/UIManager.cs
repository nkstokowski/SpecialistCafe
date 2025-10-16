using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject shop;
    public GameObject ia;

    public void ShowShop()
    {
        shop.SetActive(true);
    }

    public void HideShop()
    {
        shop.SetActive(false);
    }

    public void ShowIA()
    {
        ia.SetActive(true);
    }

    public void HideIA()
    {
        ia.SetActive(false);
    }
}
