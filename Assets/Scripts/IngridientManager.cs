using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngridientManager : MonoBehaviour
{
    public List<GameObject> ingridients;

    public void setIngridient(int[] ingridientIndexes, Boolean state) {
        foreach(int i in ingridientIndexes) {
            ingridients[i].SetActive(state);
        }
    }
}
