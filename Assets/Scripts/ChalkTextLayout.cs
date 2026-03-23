using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChalkTextLayout : MonoBehaviour, IDataPersistence
{

    public List<string> unlockedMenu;
    public List<ChalkText> allChalkTexts;
    private Dictionary<string, ChalkText> chalkTextDict;

    public float startX;
    public float startY;


    void Awake()
    {
        chalkTextDict = new Dictionary<string, ChalkText>();
        foreach(ChalkText txt in allChalkTexts)
        {
            //Debug.Log("Adding chalk text for " + txt.menuItem + " to the dict.");
            chalkTextDict.Add(txt.menuItem, txt);
        }
    }

    public void AddMenuItem(string menuItem)
    {
        this.unlockedMenu.Add(menuItem);
        DrawText();
    }

    void DrawText()
    {
        float nextItemHeight = startY;
        //Debug.Log("DrawText: Starting y: " + nextItemHeight);

        foreach(String menuItem in this.unlockedMenu)
        {
            //Debug.Log("Draw Loop looking at " + menuItem);
            chalkTextDict[menuItem].gameObject.SetActive(true);
            //Debug.Log("Placing " + chalkTextDict[menuItem].menuItem + "at " + nextItemHeight);
            chalkTextDict[menuItem].gameObject.transform.localPosition = new Vector3(startX, nextItemHeight, 0);
            nextItemHeight -= (chalkTextDict[menuItem].height * 0.01f);
        }
    }

    public void LoadData(GameData data)
    {
        this.unlockedMenu = new List<string>(data.unlockedMenuItems);
        DrawText();
    }

    public void SaveData(ref GameData data)
    {

    }
}
