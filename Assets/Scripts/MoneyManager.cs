using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour, IDataPersistence
{

    DateTime timeLastSaved;
    public CafeManager cafeManager;

    public void LoadData(GameData data)
    {
        this.timeLastSaved = DateTime.Parse(data.timeLastSaved);
        CalculateIncome(timeLastSaved);
    }

    public void SaveData(ref GameData data)
    {
        data.timeLastSaved = DateTime.Now.ToString();
    }

    void CalculateIncome(DateTime startTime) {
        TimeSpan span = DateTime.Now.Subtract(startTime);
        Debug.Log("Hours passed: " + span.Hours);
    }

}
