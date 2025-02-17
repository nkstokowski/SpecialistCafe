using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopViewSelector : MonoBehaviour
{
    public GameObject[] shopViews;

    public void showView(String viewName)
    {
        foreach(GameObject view in shopViews) {
            view.SetActive(view.name == viewName);
        }
    }
}
